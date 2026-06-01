namespace CrudExcelApp;

public class FormChaves : Form
{
    private TextBox txtIdentificacao, txtResponsavel, txtStatus;
    private DateTimePicker dtpRetirada, dtpDevolucao;
    private DataGridView dgv;
    private int? selectedId = null;

    public FormChaves()
    {
        Text = "Controle de Chaves";
        Size = new Size(850, 600);
        StartPosition = FormStartPosition.CenterScreen;

        int y = 20;
        AddLabel("Identificação *:", 20, y); txtIdentificacao = AddTextBox(140, y, 250); y += 35;
        AddLabel("Responsável:", 20, y); txtResponsavel = AddTextBox(140, y, 250); y += 35;
        AddLabel("Data Retirada:", 20, y); dtpRetirada = AddDatePicker(140, y); y += 35;
        AddLabel("Data Devolução:", 20, y); dtpDevolucao = AddDatePicker(140, y); y += 35;
        AddLabel("Status:", 20, y); txtStatus = AddTextBox(140, y); y += 35;

        int bx = 20;
        var btnIncluir = AddButton("Incluir", bx, y); bx += 110;
        var btnEditar = AddButton("Editar", bx, y); bx += 110;
        var btnApagar = AddButton("Apagar", bx, y); bx += 110;
        var btnLimpar = AddButton("Limpar", bx, y); bx += 110;
        var btnVoltar = AddButton("Voltar", bx, y);
        y += 45;

        dgv = new DataGridView
        {
            Location = new Point(20, y),
            Size = new Size(790, 250),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        dgv.CellClick += Dgv_CellClick;
        Controls.Add(dgv);

        btnIncluir.Click += BtnIncluir_Click;
        btnEditar.Click += BtnEditar_Click;
        btnApagar.Click += BtnApagar_Click;
        btnLimpar.Click += (s, e) => LimparCampos();
        btnVoltar.Click += (s, e) => Close();

        LoadData();
    }

    private void LoadData()
    {
        var rows = ExcelDatabase.ListRows("Chaves");
        dgv.DataSource = null;
        if (rows.Count > 0)
        {
            var dt = new System.Data.DataTable();
            foreach (var key in rows[0].Keys) dt.Columns.Add(key);
            foreach (var row in rows)
            {
                var dr = dt.NewRow();
                foreach (var kvp in row) dr[kvp.Key] = kvp.Value;
                dt.Rows.Add(dr);
            }
            dgv.DataSource = dt;
        }
    }

    private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = dgv.Rows[e.RowIndex];
        selectedId = int.Parse(row.Cells["ID"].Value?.ToString() ?? "0");
        txtIdentificacao.Text = row.Cells["Identificacao"].Value?.ToString() ?? "";
        txtResponsavel.Text = row.Cells["Responsavel"].Value?.ToString() ?? "";
        txtStatus.Text = row.Cells["Status"].Value?.ToString() ?? "";
        var dataRet = row.Cells["DataRetirada"].Value?.ToString() ?? "";
        if (DateTime.TryParseExact(dataRet, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dt1))
            dtpRetirada.Value = dt1;
        var dataDev = row.Cells["DataDevolucao"].Value?.ToString() ?? "";
        if (DateTime.TryParseExact(dataDev, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dt2))
            dtpDevolucao.Value = dt2;
    }

    private void BtnIncluir_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtIdentificacao.Text))
        {
            MessageBox.Show("O campo Identificação é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        var data = new Dictionary<string, string>
        {
            ["Identificacao"] = txtIdentificacao.Text.Trim(),
            ["Responsavel"] = txtResponsavel.Text.Trim(),
            ["DataRetirada"] = dtpRetirada.Value.ToString("dd/MM/yyyy"),
            ["DataDevolucao"] = dtpDevolucao.Value.ToString("dd/MM/yyyy"),
            ["Status"] = txtStatus.Text.Trim()
        };
        ExcelDatabase.InsertRow("Chaves", data);
        MessageBox.Show("Registro incluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LimparCampos();
        LoadData();
    }

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        if (selectedId == null) { MessageBox.Show("Selecione um registro para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (string.IsNullOrWhiteSpace(txtIdentificacao.Text)) { MessageBox.Show("O campo Identificação é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        var data = new Dictionary<string, string>
        {
            ["Identificacao"] = txtIdentificacao.Text.Trim(),
            ["Responsavel"] = txtResponsavel.Text.Trim(),
            ["DataRetirada"] = dtpRetirada.Value.ToString("dd/MM/yyyy"),
            ["DataDevolucao"] = dtpDevolucao.Value.ToString("dd/MM/yyyy"),
            ["Status"] = txtStatus.Text.Trim()
        };
        ExcelDatabase.UpdateRow("Chaves", selectedId.Value, data);
        MessageBox.Show("Registro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LimparCampos();
        LoadData();
    }

    private void BtnApagar_Click(object? sender, EventArgs e)
    {
        if (selectedId == null) { MessageBox.Show("Selecione um registro para apagar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        var result = MessageBox.Show("Deseja realmente apagar este registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            ExcelDatabase.DeleteRow("Chaves", selectedId.Value);
            MessageBox.Show("Registro apagado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            LoadData();
        }
    }

    private void LimparCampos()
    {
        txtIdentificacao.Text = "";
        txtResponsavel.Text = "";
        txtStatus.Text = "";
        dtpRetirada.Value = DateTime.Now;
        dtpDevolucao.Value = DateTime.Now;
        selectedId = null;
    }

    private Label AddLabel(string text, int x, int y)
    {
        var lbl = new Label { Text = text, Location = new Point(x, y + 3), AutoSize = true };
        Controls.Add(lbl);
        return lbl;
    }

    private TextBox AddTextBox(int x, int y, int width = 200)
    {
        var txt = new TextBox { Location = new Point(x, y), Width = width };
        Controls.Add(txt);
        return txt;
    }

    private DateTimePicker AddDatePicker(int x, int y)
    {
        var dtp = new DateTimePicker { Location = new Point(x, y), Width = 200, Format = DateTimePickerFormat.Short };
        Controls.Add(dtp);
        return dtp;
    }

    private Button AddButton(string text, int x, int y)
    {
        var btn = new Button { Text = text, Location = new Point(x, y), Width = 100 };
        Controls.Add(btn);
        return btn;
    }
}
