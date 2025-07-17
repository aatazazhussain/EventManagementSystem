using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WindowsFormsApp1
{
    public class InviteForm : Form
    {
        private TextBox txtName;
        private ComboBox cmbEvents;
        private RichTextBox rtbLetter;
        private Button btnGenerate, btnDownload;
        private DataTable eventTable;
        private string xmlPath = Path.Combine(Application.StartupPath, "events.xml");

        public InviteForm()
        {
            this.Text = "Create Invite Letter";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            InitializeUI();
            LoadEvents();
        }

        private void InitializeUI()
        {
            Label lblName = new Label()
            {
                Text = "Invitee Name:",
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            txtName = new TextBox()
            {
                Location = new System.Drawing.Point(150, 18),
                Width = 250,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            Label lblEvent = new Label()
            {
                Text = "Select Event:",
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            cmbEvents = new ComboBox()
            {
                Location = new System.Drawing.Point(150, 58),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            btnGenerate = new Button()
            {
                Text = "Generate Invite",
                Location = new System.Drawing.Point(420, 56),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                Width = 130,
                Height = 30,
                Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnGenerate.Click += BtnGenerate_Click;

            rtbLetter = new RichTextBox()
            {
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(530, 350),
                Font = new System.Drawing.Font("Segoe UI", 11)
            };

            btnDownload = new Button()
            {
                Text = "Download as PDF",
                Location = new System.Drawing.Point(200, 470),
                Width = 160,
                Height = 40,
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnDownload.Click += BtnDownload_Click;

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblEvent);
            this.Controls.Add(cmbEvents);
            this.Controls.Add(btnGenerate);
            this.Controls.Add(rtbLetter);
            this.Controls.Add(btnDownload);
        }

        private void LoadEvents()
        {
            eventTable = new DataTable();

            if (File.Exists(xmlPath))
            {
                eventTable.ReadXml(xmlPath);
                foreach (DataRow row in eventTable.Rows)
                {
                    cmbEvents.Items.Add($"{row["Event Name"]} | {row["Venue"]} | {row["Date"]}");
                }
            }
            else
            {
                MessageBox.Show("No events found. Please add events first.");
                this.Close();
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "" || cmbEvents.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter a name and select an event.");
                return;
            }

            string name = txtName.Text.Trim();
            string[] eventDetails = cmbEvents.SelectedItem.ToString().Split('|');

            string eventName = eventDetails[0].Trim();
            string venue = eventDetails[1].Trim();
            string date = eventDetails[2].Trim();

            string letter = $"Dear {name},\n\n" +
                            $"You are cordially invited to attend the event \"{eventName}\" " +
                            $"which will be held at {venue} on {date}.\n\n" +
                            $"We look forward to your presence.\n\n" +
                            $"Best Regards,\nEvent Management System";

            rtbLetter.Text = letter;
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbLetter.Text))
            {
                MessageBox.Show("Please generate the invite letter first.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"Invite_{txtName.Text.Trim().Replace(" ", "_")}.pdf"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                    {
                        iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 40, 40, 40, 40);
                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                        doc.Open();

                        // Use iTextSharp Font explicitly
                        iTextSharp.text.Font pdfFont = FontFactory.GetFont("Segoe UI", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                        Paragraph para = new Paragraph(rtbLetter.Text, pdfFont);
                        doc.Add(para);
                        doc.Close();
                        writer.Close();
                    }

                    MessageBox.Show("PDF downloaded successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("PDF generation failed: " + ex.Message);
                }
            }
        }
    }
}
