namespace CrudExcelApp;

public class FormVeiculos : Form
{
    private DateTimePicker txtData;
    private TextBox txtPlaca;
    private MaskedTextBox txtHorario;
    private DataGridView dgv;
    private int? selectedId = null;

    public FormVeiculos()
    {
        Text = "Agregados";
        Size = new Size(760, 520);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(760, 520);
        BackColor = Color.WhiteSmoke;

        var fieldsPanel = new TableLayoutPanel
        {
            ColumnCount = 3,
            RowCount = 3,
            Location = new Point(20, 20),
            Size = new Size(700, 80),
            AutoSize = false
        };
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230));

        AddField(fieldsPanel, 0, "Data *:", txtData = new DateTimePicker { Width = 220, Format = DateTimePickerFormat.Short });
        AddField(fieldsPanel, 1, "Placa *:", txtPlaca = new TextBox { Width = 220 });
        AddField(fieldsPanel, 2, "Horário:", txtHorario = CreateTimeTextBox());
        Controls.Add(fieldsPanel);

        var buttonsPanel = new FlowLayoutPanel
        {
            Location = new Point(20, 115),
            Size = new Size(700, 40),
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

        dgv = CreateGrid(new Point(20, 170), new Size(700, 290));
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
        var rows = ExcelDatabase.ListRows("Veiculos");
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
        txtPlaca.Text = row.Cells["Placa"].Value?.ToString() ?? "";
        txtHorario.Text = row.Cells["Horario"].Value?.ToString() ?? "";
    }

    private void BtnIncluir_Click(object? sender, EventArgs e)
    {
        if (!ValidateForm())
        {
            return;
        }
        var data = new Dictionary<string, string>
        {
            ["Placa"] = txtPlaca.Text.Trim(),
            ["Horario"] = txtHorario.Text.Trim()
        };
        ExcelDatabase.InsertRow("Veiculos", data);
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
            ["Placa"] = txtPlaca.Text.Trim(),
            ["Horario"] = txtHorario.Text.Trim()
        };
        ExcelDatabase.UpdateRow("Veiculos", selectedId.Value, data);
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
            ExcelDatabase.DeleteRow("Veiculos", selectedId.Value);
            MessageBox.Show("Registro apagado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            LoadData();
        }
    }

    private void LimparCampos()
    {
        txtPlaca.Text = "";
        txtHorario.Text = "";
        selectedId = null;
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(txtPlaca.Text))
        {
            MessageBox.Show("O campo Placa é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPlaca.Focus();
            return false;
        }

        if (!IsValidTime(txtHorario))
        {
            MessageBox.Show("Informe o horário no formato HH:mm.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtHorario.Focus();
            return false;
        }

        return true;
    }

    private void AddField(TableLayoutPanel panel, int row, string labelText, Control input)
    {
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
        panel.Controls.Add(label, 0, row);
        panel.Controls.Add(input, 1, row);
    }

    private MaskedTextBox CreateTimeTextBox()
    {
        return new MaskedTextBox("00:00")
        {
            Width = 100,
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
