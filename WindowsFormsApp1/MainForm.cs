using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        private Panel sidebarPanel;
        private Panel mainContentPanel;
        private Button btnToggleMenu;
        private Button btnDashboard;
        private Button btnLogout;
        private Label lblTitle;
        private bool isSidebarVisible = true;

        public MainForm()
        {
            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            this.Text = "Event Management System";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.White;

            // Sidebar panel
            sidebarPanel = new Panel()
            {
                BackColor = Color.FromArgb(40, 40, 60),
                Size = new Size(200, this.Height),
                Location = new Point(0, 0),
                Dock = DockStyle.Left
            };
            this.Controls.Add(sidebarPanel);

            // App Title
            lblTitle = new Label()
            {
                Text = "☰ Event Manager",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };
            sidebarPanel.Controls.Add(lblTitle);

            // Dashboard Button
            btnDashboard = new Button()
            {
                Text = "📊 Dashboard",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 50, 70),
                FlatStyle = FlatStyle.Flat,
                Width = 180,
                Height = 40,
                Location = new Point(10, 80),
                TextAlign = ContentAlignment.MiddleLeft
            };
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.Click += BtnDashboard_Click;
            sidebarPanel.Controls.Add(btnDashboard);

            // Logout Button
            btnLogout = new Button()
            {
                Text = "🔒 Logout",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 50, 70),
                FlatStyle = FlatStyle.Flat,
                Width = 180,
                Height = 40,
                Location = new Point(10, 130),
                TextAlign = ContentAlignment.MiddleLeft
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;
            sidebarPanel.Controls.Add(btnLogout);

            // Toggle Button ☰
            btnToggleMenu = new Button()
            {
                Text = "☰",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(40, 40),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            btnToggleMenu.Click += ToggleMenu_Click;
            this.Controls.Add(btnToggleMenu);
            btnToggleMenu.BringToFront();

            // Main Content Panel
            mainContentPanel = new Panel()
            {
                Location = new Point(200, 0),
                Size = new Size(this.Width - 200, this.Height),
                AutoScroll = true,
                BackColor = Color.White
            };
            this.Controls.Add(mainContentPanel);
        }

        private void ToggleMenu_Click(object sender, EventArgs e)
        {
            isSidebarVisible = !isSidebarVisible;
            sidebarPanel.Visible = isSidebarVisible;

            if (isSidebarVisible)
            {
                mainContentPanel.Location = new Point(200, 0);
                mainContentPanel.Size = new Size(this.Width - 200, this.Height);
            }
            else
            {
                mainContentPanel.Location = new Point(0, 0);
                mainContentPanel.Size = new Size(this.Width, this.Height);
            }

            btnToggleMenu.BringToFront();
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            mainContentPanel.Controls.Clear();
            admindashboard dashboard = new admindashboard
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            mainContentPanel.Controls.Add(dashboard);
            dashboard.Show();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }
    }
}
