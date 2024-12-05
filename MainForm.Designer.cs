using System;
using System.Windows.Forms;

namespace SMPPClient
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblHost, lblPort, lblUser, lblPassword, lblSender, lblDestination, lblMessage;
        private TextBox txtHost, txtPort, txtUser, txtPassword, txtSender, txtDestination, txtMessage;
        private Button btnSend;

        private void InitializeComponent()
        {
            // Labels
            this.lblHost = new Label() { Text = "Host:", Left = 20, Top = 0, Width = 200 };
            this.lblPort = new Label() { Text = "Port:", Left = 240, Top = 0, Width = 80 };
            this.lblUser = new Label() { Text = "User:", Left = 20, Top = 40, Width = 200 };
            this.lblPassword = new Label() { Text = "Password:", Left = 240, Top = 40, Width = 200 };
            this.lblSender = new Label() { Text = "Sender:", Left = 20, Top = 80, Width = 200 };
            this.lblDestination = new Label() { Text = "Destination:", Left = 240, Top = 80, Width = 200 };
            this.lblMessage = new Label() { Text = "Message:", Left = 20, Top = 120, Width = 420 };

            // TextBoxes
            this.txtHost = new TextBox() { Left = 20, Top = 20, Width = 200 };
            this.txtPort = new TextBox() { Left = 240, Top = 20, Width = 80 };
            this.txtUser = new TextBox() { Left = 20, Top = 60, Width = 200 };
            this.txtPassword = new TextBox() { Left = 240, Top = 60, Width = 200, UseSystemPasswordChar = true };
            this.txtSender = new TextBox() { Left = 20, Top = 100, Width = 200 };
            this.txtDestination = new TextBox() { Left = 240, Top = 100, Width = 200 };
            this.txtMessage = new TextBox() { Left = 20, Top = 140, Width = 420, Height = 100, Multiline = true };

            // Button
            this.btnSend = new Button() { Left = 20, Top = 260, Width = 420, Text = "Send SMS" };

            // Add Controls
            this.Controls.Add(lblHost);
            this.Controls.Add(txtHost);
            this.Controls.Add(lblPort);
            this.Controls.Add(txtPort);
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUser);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblSender);
            this.Controls.Add(txtSender);
            this.Controls.Add(lblDestination);
            this.Controls.Add(txtDestination);
            this.Controls.Add(lblMessage);
            this.Controls.Add(txtMessage);
            this.Controls.Add(btnSend);

            this.txtHost.TextChanged += OnSettingsChanged;
            this.txtPort.TextChanged += OnSettingsChanged;
            this.txtUser.TextChanged += OnSettingsChanged;
            this.txtPassword.TextChanged += OnSettingsChanged;

            this.txtSender.TextChanged += OnSenderChanged;
            this.txtDestination.TextChanged += OnDestinationChanged;

            // Button Event
            this.btnSend.Click += new EventHandler(this.btnSend_Click);

            // Form Properties
            this.Text = "SMPP Sender";
            this.ClientSize = new System.Drawing.Size(480, 320);
        }

        private void OnSettingsChanged(object sender, EventArgs e)
        {
            settingsChanged = true;
        }

        private void OnSenderChanged(object sender, EventArgs e)
        {
            senderChanged = true;
        }

        private void OnDestinationChanged(object sender, EventArgs e)
        {
            destinationChanged = true;
        }

    }
}
