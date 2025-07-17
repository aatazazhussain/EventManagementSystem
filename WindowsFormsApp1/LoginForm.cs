using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        string xmlPath = Path.Combine(Application.StartupPath, "Users.xml");

        public LoginForm()
        {
            this.Text = "Login - Event Management System";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.SkyBlue;

            InitializeComponent();

            // ===== Logo =====
            PictureBox logo = new PictureBox()
            {
                Image = Image.FromFile("logo.png"), // 🔁 Replace with actual path if needed
                Location = new Point((this.ClientSize.Width - 100) / 2, 20),
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            this.Controls.Add(logo);

            // ===== Heading =====
            Label lblHeading = new Label()
            {
                Text = "Welcome to Event Management System",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Navy,
                Location = new Point((this.ClientSize.Width - 350) / 2, 130)
            };
            this.Controls.Add(lblHeading);

            // ===== Panel for form =====
            Panel loginPanel = new Panel()
            {
                Size = new Size(300, 150),
                Location = new Point((this.ClientSize.Width - 300) / 2, 170),
                BackColor = Color.FromArgb(240, 248, 255) // Light background inside
            };
            this.Controls.Add(loginPanel);

            // ===== Username Label =====
            Label lblUsername = new Label() { Text = "Username:", Location = new Point(20, 20), AutoSize = true };
            loginPanel.Controls.Add(lblUsername);

            // ===== Username TextBox =====
            TextBox txtUsername = new TextBox() { Name = "txtUsername", Location = new Point(100, 20), Width = 170 };
            loginPanel.Controls.Add(txtUsername);

            // ===== Password Label =====
            Label lblPassword = new Label() { Text = "Password:", Location = new Point(20, 60), AutoSize = true };
            loginPanel.Controls.Add(lblPassword);

            // ===== Password TextBox =====
            TextBox txtPassword = new TextBox() { Name = "txtPassword", Location = new Point(100, 60), Width = 170, PasswordChar = '*' };
            loginPanel.Controls.Add(txtPassword);

            // ===== Login Button =====
            Button btnLogin = new Button()
            {
                Text = "Login",
                Location = new Point(100, 100),
                Width = 100,
                BackColor = Color.Navy,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.Click += BtnLogin_Click;
            loginPanel.Controls.Add(btnLogin);

            // ===== Developer Info =====
            // ===== Developer Info =====
            Label lblDevelopers = new Label()
            {
                Text = "DEVELOPERS: Aatazaz Hussain, Inshrah Sajal, Hadiqa Sattar",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.DarkRed,
                Location = new Point((this.ClientSize.Width - 450) / 2, 340)
            };
            this.Controls.Add(lblDevelopers);

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = this.Controls.Find("txtUsername", true).FirstOrDefault()?.Text.Trim();
            string password = this.Controls.Find("txtPassword", true).FirstOrDefault()?.Text.Trim();

            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("User database not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                XDocument doc = XDocument.Load(xmlPath);

                var user = doc.Descendants("User")
                              .Where(u => u.Element("Username")?.Value == username &&
                                          u.Element("Password")?.Value == password)
                              .FirstOrDefault();

                if (user != null)
                {
                    MessageBox.Show("Login successful!", "Success");

                    MainForm mainForm = new MainForm(); // Your master page
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "Login Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading XML: " + ex.Message);
            }
        }
    }
}
