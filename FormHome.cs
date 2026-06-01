namespace CrudExcelApp;

public class FormHome : Form
{
    public FormHome()
    {
        Text = "Controle de Entrada SVP";
        Size = new Size(700, 650);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.WhiteSmoke;

        var lblTitle = new Label
        {
            Text = "Controle de Entrada SVP",
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = Color.FromArgb(40, 60, 100),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(660, 55),
            Location = new Point(20, 20)
        };

        var separator = new Panel
        {
            BackColor = Color.FromArgb(180, 190, 210),
            Size = new Size(640, 2),
            Location = new Point(30, 85)
        };

        var btnCaminhoes = CreateButton("Caminhões SVP", 135);
        btnCaminhoes.Click += (s, e) => { new FormCaminhoes().ShowDialog(); };

        var btnAgregados = CreateButton("Agregados", 255);
        btnAgregados.Click += (s, e) => { new FormVeiculos().ShowDialog(); };

        var btnChaves = CreateButton("Controle de Chaves", 375);
        btnChaves.Click += (s, e) => { new FormChaves().ShowDialog(); };

        var btnFornecedores = CreateButton("Fornecedores", 495);
        btnFornecedores.Click += (s, e) => { new FormFornecedores().ShowDialog(); };

        Controls.AddRange(new Control[] { lblTitle, separator, btnCaminhoes, btnAgregados, btnChaves, btnFornecedores });
    }

    private Button CreateButton(string text, int top)
    {
        return new Button
        {
            Text = text,
            Font = new Font("Segoe UI", 15, FontStyle.Regular),
            Size = new Size(400, 90),
            Location = new Point(150, top),
            BackColor = Color.FromArgb(50, 90, 160),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            FlatAppearance = { BorderSize = 0 }
        };
    }
}
