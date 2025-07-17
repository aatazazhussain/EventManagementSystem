using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class admindashboard : Form
    {
        private DataTable eventTable = new DataTable();
        private string xmlPath = Path.Combine(Application.StartupPath, "events.xml");
        private DataGridView dgvEvents;
        private Label lblNearestEvent, lblSearch;
        private TextBox txtEventName, txtVenue;
        private DateTimePicker dtpDate, dtpSearch;
        private Button btnSearch, btnInvite;

        public admindashboard()
        {
            this.Text = "Admin Dashboard";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.WhiteSmoke;
            this.Load += admindashboard_Load;
        }

        private void admindashboard_Load(object sender, EventArgs e)
        {
            InitializeEventTable();
            InitializeUI();
        }

        private void InitializeEventTable()
        {
            eventTable.TableName = "Event";
            eventTable.Columns.Add("Event Name");
            eventTable.Columns.Add("Venue");
            eventTable.Columns.Add("Date");
        }

        private void InitializeUI()
        {
            Label heading = new Label()
            {
                Text = "EMS Dashboard",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                AutoSize = true,
                Location = new Point(20, 10)
            };
            this.Controls.Add(heading);

            FlowLayoutPanel inputPanel = new FlowLayoutPanel()
            {
                Location = new Point(20, 50),
                Width = this.Width - 60,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            txtEventName = new TextBox() { Width = 200 };
            txtVenue = new TextBox() { Width = 200 };
            dtpDate = new DateTimePicker() { Width = 200 };

            inputPanel.Controls.Add(CreateLabeledPanel("Event Name:", txtEventName));
            inputPanel.Controls.Add(CreateLabeledPanel("Venue:", txtVenue));
            inputPanel.Controls.Add(CreateLabeledPanel("Date:", dtpDate));
            this.Controls.Add(inputPanel);

            TableLayoutPanel buttonTable = new TableLayoutPanel()
            {
                RowCount = 2,
                ColumnCount = 3,
                Location = new Point(20, 200),
                Size = new Size(this.Width - 60, 100),
                AutoSize = true
            };

            for (int i = 0; i < 3; i++)
                buttonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));

            buttonTable.Controls.Add(CreateButton("Add Event", BtnAddEvent_Click), 0, 0);
            buttonTable.Controls.Add(CreateButton("Export to XML", BtnExportXML_Click), 1, 0);
            buttonTable.Controls.Add(CreateButton("Load from XML", BtnLoadXML_Click), 2, 0);
            buttonTable.Controls.Add(CreateButton("Clear Table", BtnClear_Click), 0, 1);
            buttonTable.Controls.Add(CreateButton("Delete Selected", BtnDelete_Click), 1, 1);
            buttonTable.Controls.Add(CreateButton("Update Selected", BtnUpdate_Click), 2, 1);
            this.Controls.Add(buttonTable);

            dgvEvents = new DataGridView()
            {
                Location = new Point(20, 320),
                Size = new Size(this.Width - 60, this.Height - 400),
                DataSource = eventTable,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            this.Controls.Add(dgvEvents);

            lblNearestEvent = new Label()
            {
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, dgvEvents.Bottom + 10),
                Text = ""
            };
            this.Controls.Add(lblNearestEvent);

            int baseY = lblNearestEvent.Bottom + 30;

            lblSearch = new Label()
            {
                Text = "Search by Date:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, baseY),
                AutoSize = true
            };

            dtpSearch = new DateTimePicker()
            {
                Width = 200,
                Location = new Point(140, baseY - 5)
            };

            btnSearch = new Button()
            {
                Text = "Search",
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(360, baseY - 7),
                Size = new Size(100, 30)
            };
            btnSearch.Click += BtnSearch_Click;

            btnInvite = new Button()
            {
                Text = "Invite",
                BackColor = Color.DarkSlateBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(480, baseY - 7),
                Size = new Size(100, 30)
            };
            btnInvite.Click += BtnInvite_Click;

            this.Controls.Add(lblSearch);
            this.Controls.Add(dtpSearch);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnInvite);

            this.Resize += (s, e) =>
            {
                dgvEvents.Size = new Size(this.ClientSize.Width - 60, this.ClientSize.Height - 400);
                lblNearestEvent.Location = new Point(20, dgvEvents.Bottom + 10);
                int newY = lblNearestEvent.Bottom + 30;
                lblSearch.Location = new Point(20, newY);
                dtpSearch.Location = new Point(140, newY - 5);
                btnSearch.Location = new Point(360, newY - 7);
                btnInvite.Location = new Point(480, newY - 7);
            };
        }

        private Panel CreateLabeledPanel(string labelText, Control control)
        {
            Label lbl = new Label()
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10),
                Width = 100
            };

            Panel panel = new Panel()
            {
                Width = 320,
                Height = 40
            };

            lbl.Location = new Point(0, 10);
            control.Location = new Point(110, 7);

            panel.Controls.Add(lbl);
            panel.Controls.Add(control);
            return panel;
        }

        private Button CreateButton(string text, EventHandler handler)
        {
            Button btn = new Button()
            {
                Text = text,
                Size = new Size(160, 40),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(10)
            };
            btn.Click += handler;
            return btn;
        }

        // Button Handlers
        private void BtnAddEvent_Click(object sender, EventArgs e)
        {
            string name = txtEventName.Text.Trim();
            string venue = txtVenue.Text.Trim();
            string date = dtpDate.Value.ToShortDateString();

            foreach (DataRow row in eventTable.Rows)
            {
                if (row["Event Name"].ToString() == name &&
                    row["Venue"].ToString() == venue &&
                    row["Date"].ToString() == date)
                {
                    MessageBox.Show("This event already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            eventTable.Rows.Add(name, venue, date);
            MessageBox.Show("Event added.");
            ShowNearestEvent();
        }

        private void BtnExportXML_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable unique = eventTable.Clone();

                foreach (DataRow row in eventTable.Rows)
                {
                    bool duplicate = unique.AsEnumerable().Any(r =>
                        r["Event Name"].ToString() == row["Event Name"].ToString() &&
                        r["Venue"].ToString() == row["Venue"].ToString() &&
                        r["Date"].ToString() == row["Date"].ToString());

                    if (!duplicate)
                        unique.ImportRow(row);
                }

                unique.WriteXml(xmlPath, XmlWriteMode.WriteSchema);
                MessageBox.Show("Exported to XML.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export error: " + ex.Message);
            }
        }

        private void BtnLoadXML_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(xmlPath))
                {
                    eventTable.Clear();
                    eventTable.ReadXml(xmlPath);
                    MessageBox.Show("Loaded from XML.");
                    ShowNearestEvent();
                }
                else
                {
                    MessageBox.Show("XML file not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load error: " + ex.Message);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all events?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                eventTable.Clear();
                MessageBox.Show("Table cleared.");
                ShowNearestEvent();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Delete selected event?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dgvEvents.Rows.RemoveAt(dgvEvents.SelectedRows[0].Index);
                    eventTable.WriteXml(xmlPath, XmlWriteMode.WriteSchema);
                    MessageBox.Show("Deleted.");
                    ShowNearestEvent();
                }
            }
            else
            {
                MessageBox.Show("No row selected.");
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count > 0)
            {
                int index = dgvEvents.SelectedRows[0].Index;
                eventTable.Rows[index]["Event Name"] = txtEventName.Text;
                eventTable.Rows[index]["Venue"] = txtVenue.Text;
                eventTable.Rows[index]["Date"] = dtpDate.Value.ToShortDateString();
                eventTable.WriteXml(xmlPath, XmlWriteMode.WriteSchema);
                MessageBox.Show("Updated.");
                ShowNearestEvent();
            }
            else
            {
                MessageBox.Show("No row selected.");
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchDate = dtpSearch.Value.ToShortDateString();

            try
            {
                var results = eventTable.AsEnumerable()
                    .Where(r => r["Date"].ToString() == searchDate);

                if (results.Any())
                    dgvEvents.DataSource = results.CopyToDataTable();
                else
                    MessageBox.Show("No events found on selected date.");
            }
            catch
            {
                MessageBox.Show("No matching records.");
            }
        }

        private void BtnInvite_Click(object sender, EventArgs e)
        {
            new InviteForm().Show();
        }

        private void ShowNearestEvent()
        {
            lblNearestEvent.Text = "";

            if (eventTable.Rows.Count == 0)
                return;

            DateTime today = DateTime.Today;
            DateTime nearestDate = DateTime.MaxValue;
            List<DataRow> nearestEvents = new List<DataRow>();

            foreach (DataRow row in eventTable.Rows)
            {
                DateTime date;
                if (DateTime.TryParse(row["Date"].ToString(), out date))
                {
                    if (date >= today && date < nearestDate)
                        nearestDate = date;
                }
            }

            foreach (DataRow row in eventTable.Rows)
            {
                DateTime date;
                if (DateTime.TryParse(row["Date"].ToString(), out date))
                {
                    if (date == nearestDate)
                        nearestEvents.Add(row);
                }
            }

            if (nearestEvents.Count > 0)
            {
                lblNearestEvent.Text = "Nearest Upcoming Event(s):";
                foreach (DataRow row in nearestEvents)
                    lblNearestEvent.Text += $"\n→ {row["Event Name"]} at {row["Venue"]} on {row["Date"]}";
            }
        }
    }
}
