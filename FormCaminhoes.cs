namespace CrudExcelApp;

public class FormCaminhoes : Form
{
    private TextBox txtPlaca, txtMotorista, txtKmSaida, txtKmEntrada, txtDestino, txtNotasFiscais;
    private DateTimePicker dtpDataSaida, dtpDataEntrada;
    private MaskedTextBox txtHoraSaida, txtHoraEntrada;
    private DataGridView dgv;
    private int? selectedId = null;

    public FormCaminhoes()
    {
        Text = "Caminhões SVP";
        Size = new Size(1100, 760);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1100, 760);
        BackColor = Color.WhiteSmoke;

        var fieldsPanel = new TableLayoutPanel
        {
            ColumnCount = 4,
            Location = new Point(20, 20),
            Size = new Size(1040, 240)
        };
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

        AddField(fieldsPanel, 0, 0, "Placa *:", txtPlaca = CreateTextBox());
        AddField(fieldsPanel, 0, 2, "Motorista:", txtMotorista = CreateTextBox());
        AddField(fieldsPanel, 1, 0, "Data Saída:", dtpDataSaida = CreateDatePicker());
        AddField(fieldsPanel, 1, 2, "Hora Saída:", txtHoraSaida = CreateTimeTextBox());
        AddField(fieldsPanel, 2, 0, "Km Saída:", txtKmSaida = CreateTextBox());
        AddField(fieldsPanel, 2, 2, "Data Entrada:", dtpDataEntrada = CreateDatePicker());
        AddField(fieldsPanel, 3, 0, "Hora Entrada:", txtHoraEntrada = CreateTimeTextBox());
        AddField(fieldsPanel, 3, 2, "Km Entrada:", txtKmEntrada = CreateTextBox());
        AddField(fieldsPanel, 4, 0, "Destino:", txtDestino = CreateTextBox(260));
        AddField(fieldsPanel, 5, 0, "Notas Fiscais:", txtNotasFiscais = CreateMultilineTextBox(), 3);
        Controls.Add(fieldsPanel);

        var buttonsPanel = new FlowLayoutPanel
        {
            Location = new Point(20, 275),
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

        dgv = CreateGrid(new Point(20, 330), new Size(1040, 390));
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
        var rows = ExcelDatabase.ListRows("CaminhoesCasa");
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
        txtMotorista.Text = row.Cells["Motorista"].Value?.ToString() ?? "";
        txtHoraSaida.Text = row.Cells["HoraSaida"].Value?.ToString() ?? "";
        txtKmSaida.Text = row.Cells["KmSaida"].Value?.ToString() ?? "";
        txtHoraEntrada.Text = row.Cells["HoraEntrada"].Value?.ToString() ?? "";
        txtKmEntrada.Text = row.Cells["KmEntrada"].Value?.ToString() ?? "";
        txtDestino.Text = row.Cells["Destino"].Value?.ToString() ?? "";
        txtNotasFiscais.Text = row.Cells["NotasFiscais"].Value?.ToString() ?? "";

        var dataSaida = row.Cells["DataSaida"].Value?.ToString() ?? "";
        if (DateTime.TryParseExact(dataSaida, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dtSaida))
            dtpDataSaida.Value = dtSaida;

        var dataEntrada = row.Cells["DataEntrada"].Value?.ToString() ?? "";
        if (DateTime.TryParseExact(dataEntrada, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dtEntrada))
            dtpDataEntrada.Value = dtEntrada;
    }

    private void BtnIncluir_Click(object? sender, EventArgs e)
    {
        if (!ValidateForm()) return;
        var data = new Dictionary<string, string>
        {
            ["Placa"] = txtPlaca.Text.Trim(),
            ["Motorista"] = txtMotorista.Text.Trim(),
            ["DataSaida"] = dtpDataSaida.Value.ToString("dd/MM/yyyy"),
            ["HoraSaida"] = txtHoraSaida.Text.Trim(),
            ["KmSaida"] = txtKmSaida.Text.Trim(),
            ["DataEntrada"] = dtpDataEntrada.Value.ToString("dd/MM/yyyy"),
            ["HoraEntrada"] = txtHoraEntrada.Text.Trim(),
            ["KmEntrada"] = txtKmEntrada.Text.Trim(),
            ["Destino"] = txtDestino.Text.Trim(),
            ["NotasFiscais"] = txtNotasFiscais.Text.Trim()
        };
        ExcelDatabase.InsertRow("CaminhoesCasa", data);
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
            ["Motorista"] = txtMotorista.Text.Trim(),
            ["DataSaida"] = dtpDataSaida.Value.ToString("dd/MM/yyyy"),
            ["HoraSaida"] = txtHoraSaida.Text.Trim(),
            ["KmSaida"] = txtKmSaida.Text.Trim(),
            ["DataEntrada"] = dtpDataEntrada.Value.ToString("dd/MM/yyyy"),
            ["HoraEntrada"] = txtHoraEntrada.Text.Trim(),
            ["KmEntrada"] = txtKmEntrada.Text.Trim(),
            ["Destino"] = txtDestino.Text.Trim(),
            ["NotasFiscais"] = txtNotasFiscais.Text.Trim()
        };
        ExcelDatabase.UpdateRow("CaminhoesCasa", selectedId.Value, data);
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
            ExcelDatabase.DeleteRow("CaminhoesCasa", selectedId.Value);
            MessageBox.Show("Registro apagado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            LoadData();
        }
    }

    private void LimparCampos()
    {
        txtPlaca.Text = "";
        txtMotorista.Text = "";
        dtpDataSaida.Value = DateTime.Today;
        txtHoraSaida.Text = "";
        txtKmSaida.Text = "";
        dtpDataEntrada.Value = DateTime.Today;
        txtHoraEntrada.Text = "";
        txtKmEntrada.Text = "";
        txtDestino.Text = "";
        txtNotasFiscais.Text = "";
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

        foreach (var field in new[] { txtHoraSaida, txtHoraEntrada })
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

    private void AddField(TableLayoutPanel panel, int row, int column, string labelText, Control input, int columnSpan = 1)
    {
        while (panel.RowStyles.Count <= row)
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, row == 5 ? 86 : 36));

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
        if (columnSpan > 1)
            panel.SetColumnSpan(input, columnSpan);
    }

    private TextBox CreateTextBox(int width = 220)
    {
        return new TextBox { Width = width };
    }

    private TextBox CreateMultilineTextBox()
    {
        return new TextBox
        {
            Width = 650,
            Height = 72,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
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
