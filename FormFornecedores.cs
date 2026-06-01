namespace CrudExcelApp;

public class FormFornecedores : Form
{
    private DateTimePicker dtpData;
    private TextBox txtNome, txtRg, txtPlaca, txtNomeEmpresa, txtNotaFiscal, txtSetor;
    private MaskedTextBox txtHoraChegada, txtHoraEntrada, txtHoraSaida;
    private DataGridView dgv;
    private int? selectedId = null;

    public FormFornecedores()
    {
        Text = "Fornecedores";
        Size = new Size(1100, 720);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1100, 720);
        BackColor = Color.WhiteSmoke;

        var fieldsPanel = new TableLayoutPanel
        {
            ColumnCount = 4,
            Location = new Point(20, 20),
            Size = new Size(1040, 200)
        };
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

        AddField(fieldsPanel, 0, 0, "Data:", dtpData = CreateDatePicker());
        AddField(fieldsPanel, 0, 2, "Nome *:", txtNome = CreateTextBox());
        AddField(fieldsPanel, 1, 0, "RG:", txtRg = CreateTextBox());
        AddField(fieldsPanel, 1, 2, "Placa:", txtPlaca = CreateTextBox());
        AddField(fieldsPanel, 2, 0, "Nome Empresa:", txtNomeEmpresa = CreateTextBox(260));
        AddField(fieldsPanel, 2, 2, "Nota Fiscal:", txtNotaFiscal = CreateTextBox());
        AddField(fieldsPanel, 3, 0, "Hora Chegada:", txtHoraChegada = CreateTimeTextBox());
        AddField(fieldsPanel, 3, 2, "Hora Entrada:", txtHoraEntrada = CreateTimeTextBox());
        AddField(fieldsPanel, 4, 0, "Hora Saída:", txtHoraSaida = CreateTimeTextBox());
        AddField(fieldsPanel, 4, 2, "Setor:", txtSetor = CreateTextBox());
        Controls.Add(fieldsPanel);

        var buttonsPanel = new FlowLayoutPanel
        {
            Location = new Point(20, 235),
            Size = new Size(1040, 40),
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        var btnIncluir = AddButton("Incluir");
        var btnEditar = AddButton("Editar");
        var btnApagar = AddButton("Apagar");
        var btnLimpar = AddButton("Limpar");
        var btnVoltar = AddButton("Voltar");
        buttonsPanel.Controls.AddRange(new Control[] { btnIncluir, btnEditar, btnApagar, btnLimpar, btnVoltar });
        Controls.Add(buttonsPanel);

        dgv = CreateGrid(new Point(20, 290), new Size(1040, 370));
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
        var rows = ExcelDatabase.ListRows("Fornecedores");
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
        var dataStr = row.Cells["Data"].Value?.ToString() ?? "";
        if (DateTime.TryParseExact(dataStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dt))
            dtpData.Value = dt;
        txtNome.Text = row.Cells["Nome"].Value?.ToString() ?? "";
        txtRg.Text = row.Cells["RG"].Value?.ToString() ?? "";
        txtPlaca.Text = row.Cells["Placa"].Value?.ToString() ?? "";
        txtNomeEmpresa.Text = row.Cells["NomeEmpresa"].Value?.ToString() ?? "";
        txtNotaFiscal.Text = row.Cells["NotaFiscal"].Value?.ToString() ?? "";
        txtHoraChegada.Text = row.Cells["HoraChegada"].Value?.ToString() ?? "";
        txtHoraEntrada.Text = row.Cells["HoraEntrada"].Value?.ToString() ?? "";
        txtHoraSaida.Text = row.Cells["HoraSaida"].Value?.ToString() ?? "";
        txtSetor.Text = row.Cells["Setor"].Value?.ToString() ?? "";
    }

    private void BtnIncluir_Click(object? sender, EventArgs e)
    {
        if (!ValidateForm()) return;
        var data = new Dictionary<string, string>
        {
            ["Data"] = dtpData.Value.ToString("dd/MM/yyyy"),
            ["Nome"] = txtNome.Text.Trim(),
            ["RG"] = txtRg.Text.Trim(),
            ["Placa"] = txtPlaca.Text.Trim(),
            ["NomeEmpresa"] = txtNomeEmpresa.Text.Trim(),
            ["NotaFiscal"] = txtNotaFiscal.Text.Trim(),
            ["HoraChegada"] = txtHoraChegada.Text.Trim(),
            ["HoraEntrada"] = txtHoraEntrada.Text.Trim(),
            ["HoraSaida"] = txtHoraSaida.Text.Trim(),
            ["Setor"] = txtSetor.Text.Trim()
        };
        ExcelDatabase.InsertRow("Fornecedores", data);
        MessageBox.Show("Registro incluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LimparCampos();
        LoadData();
    }

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        if (selectedId == null) { MessageBox.Show("Selecione um registro para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (!ValidateForm()) return;
        var data = new Dictionary<string, string>
        {
            ["Data"] = dtpData.Value.ToString("dd/MM/yyyy"),
            ["Nome"] = txtNome.Text.Trim(),
            ["RG"] = txtRg.Text.Trim(),
            ["Placa"] = txtPlaca.Text.Trim(),
            ["NomeEmpresa"] = txtNomeEmpresa.Text.Trim(),
            ["NotaFiscal"] = txtNotaFiscal.Text.Trim(),
            ["HoraChegada"] = txtHoraChegada.Text.Trim(),
            ["HoraEntrada"] = txtHoraEntrada.Text.Trim(),
            ["HoraSaida"] = txtHoraSaida.Text.Trim(),
            ["Setor"] = txtSetor.Text.Trim()
        };
        ExcelDatabase.UpdateRow("Fornecedores", selectedId.Value, data);
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
            ExcelDatabase.DeleteRow("Fornecedores", selectedId.Value);
            MessageBox.Show("Registro apagado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            LoadData();
        }
    }

    private void LimparCampos()
    {
        dtpData.Value = DateTime.Today;
        txtNome.Text = "";
        txtRg.Text = "";
        txtPlaca.Text = "";
        txtNomeEmpresa.Text = "";
        txtNotaFiscal.Text = "";
        txtHoraChegada.Text = "";
        txtHoraEntrada.Text = "";
        txtHoraSaida.Text = "";
        txtSetor.Text = "";
        selectedId = null;
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(txtNome.Text))
        {
            MessageBox.Show("O campo Nome é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtNome.Focus();
            return false;
        }

        foreach (var field in new[] { txtHoraChegada, txtHoraEntrada, txtHoraSaida })
        {
            if (!IsValidTime(field))
            {
                MessageBox.Show("Informe os horários no formato HH:mm.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                field.Focus();
                return false;
            }
        }

        return true;
    }

    private void AddField(TableLayoutPanel panel, int row, int column, string labelText, Control input)
    {
        while (panel.RowStyles.Count <= row)
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

        var label = new Label
        {
            Text = labelText,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 8, 10, 0)
        };
        input.Anchor = AnchorStyles.Left;
        input.Margin = new Padding(0, 4, 0, 0);
        panel.Controls.Add(label, column, row);
        panel.Controls.Add(input, column + 1, row);
    }

    private TextBox CreateTextBox(int width = 220)
    {
        return new TextBox { Width = width };
    }

    private DateTimePicker CreateDatePicker()
    {
        return new DateTimePicker
        {
            Width = 130,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy"
        };
    }

    private MaskedTextBox CreateTimeTextBox()
    {
        return new MaskedTextBox("00:00")
        {
            Width = 90,
            TextMaskFormat = MaskFormat.IncludePromptAndLiterals
        };
    }

    private bool IsValidTime(MaskedTextBox field)
    {
        var value = field.Text.Trim();
        if (string.IsNullOrWhiteSpace(value) || value.Contains('_'))
            return string.IsNullOrWhiteSpace(value.Replace(":", string.Empty).Trim('_'));

        return TimeSpan.TryParseExact(value, "hh\\:mm", null, out _);
    }

    private DataGridView CreateGrid(Point location, Size size)
    {
        var grid = new DataGridView
        {
            Location = location,
            Size = size,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AllowUserToAddRows = false,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            RowHeadersVisible = false
        };
        grid.CellClick += Dgv_CellClick;
        return grid;
    }

    private Button AddButton(string text)
    {
        return new Button
        {
            Text = text,
            Width = 110,
            Height = 32,
            Margin = new Padding(0, 0, 10, 0)
        };
    }
}
