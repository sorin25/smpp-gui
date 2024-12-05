using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JamaaTech.Smpp.Net.Client;
using JamaaTech.Smpp.Net.Lib;
using System.Threading;

namespace SMPPClient
{
    public partial class MainForm : Form
    {
        private static readonly Random RND = new Random();
        private SQLiteConnection _connection;
        private readonly string dbPath = "settings.db";
        private readonly string logFilePath = "sms_log.txt";
        private bool settingsChanged = false;
        private bool senderChanged = false;
        private bool destinationChanged = false;
        public MainForm()
        {
            InitializeComponent();
            InitializeDatabase();
            InitializeConnection();
            LoadDefaults();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                using (var conn = new SQLiteConnection($"Data Source={dbPath};"))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Settings (
                            Host TEXT,
                            Port INTEGER,
                            User TEXT,
                            Password TEXT
                        );

                        CREATE TABLE IF NOT EXISTS Autocomplete (
                            Value TEXT,
                            Type TEXT
                        );
                    ";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InitializeConnection()
        {
            _connection = new SQLiteConnection($"Data Source={dbPath};");
            _connection.Open();
        }

        private void LoadDefaults()
        {
            var cmd = new SQLiteCommand("SELECT Host, Port, User, Password FROM Settings LIMIT 1", _connection);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtHost.Text = reader["Host"].ToString();
                txtPort.Text = reader["Port"].ToString();
                txtUser.Text = reader["User"].ToString();
                txtPassword.Text = reader["Password"].ToString();
            }
            reader.Close();

            LoadAutocomplete("Sender", txtSender);
            LoadAutocomplete("Destination", txtDestination);
        }

        private void LoadAutocomplete(string type, TextBox textBox)
        {
            var cmd = new SQLiteCommand("SELECT Value FROM Autocomplete WHERE Type = @Type", _connection);
            cmd.Parameters.AddWithValue("@Type", type);
            var reader = cmd.ExecuteReader();

            var autoComplete = new AutoCompleteStringCollection();
            while (reader.Read())
            {
                autoComplete.Add(reader["Value"].ToString());
            }
            reader.Close();

            textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox.AutoCompleteCustomSource = autoComplete;
        }

        private void SaveDefaults()
        {
            if (!settingsChanged)
            {
                return;
            }

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Settings";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"
                    INSERT INTO Settings (Host, Port, User, Password) VALUES 
                    (@Host, @Port, @User, @Password)";
            cmd.Parameters.AddWithValue("@Host", txtHost.Text);
            cmd.Parameters.AddWithValue("@Port", int.Parse(txtPort.Text));
            cmd.Parameters.AddWithValue("@User", txtUser.Text);
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
            cmd.ExecuteNonQuery();
        }

        private void SaveAutocomplete(string value, string type)
        {
            var cmd = new SQLiteCommand("INSERT INTO Autocomplete (Value, Type) VALUES (@Value, @Type)", _connection);
            cmd.Parameters.AddWithValue("@Value", value);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.ExecuteNonQuery();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                SmppClient client = new SmppClient();

                // Configure the connection settings
                client.Properties.SystemID = txtUser.Text;         // SMPP username
                client.Properties.Password = txtPassword.Text;    // SMPP password
                client.Properties.Port = int.Parse(txtPort.Text); // SMPP server port
                client.Properties.Host = txtHost.Text;            // SMPP server host
                client.Properties.SystemType = "";                // Optional
                client.Properties.DefaultServiceType = "";        // Optional
                client.Properties.UseSeparateConnections = false; // Optional
                //Resume a lost connection after 30 seconds
                client.AutoReconnectDelay = 30000;

                //Send Enquire Link PDU every 15 seconds
                client.KeepAliveInterval = 15000;
                client.Start();

                const int connectionTimeout = 10000; // Timeout in milliseconds (10 seconds)
                int elapsed = 0;
                const int checkInterval = 100; // Check interval in milliseconds (0.1 seconds)

                while (client.ConnectionState != SmppConnectionState.Connected)
                {
                    Thread.Sleep(checkInterval);
                    elapsed += checkInterval;

                    if (elapsed >= connectionTimeout)
                    {
                        throw new TimeoutException("Connection timed out. Unable to establish a connection to the SMPP server.");
                    }
                }
                TextMessage message = new TextMessage
                {
                    DestinationAddress = txtDestination.Text,
                    SourceAddress = txtSender.Text,
                    Text = txtMessage.Text,
                    RegisterDeliveryNotification = true,
                    UserMessageReference = RND.Next(0, int.MaxValue).ToString(),
                };
                // Send the message
                client.SendMessage(message);
                // Log the message
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {txtSender.Text} -> {txtDestination.Text}: {txtMessage.Text}{Environment.NewLine}");

                // Save settings and autocomplete
                SaveDefaults();
                if (senderChanged && !txtSender.AutoCompleteCustomSource.Contains(txtSender.Text))
                {
                    SaveAutocomplete(txtSender.Text, "Sender");
                    txtSender.AutoCompleteCustomSource.Add(txtSender.Text);
                }
                if (destinationChanged && !txtDestination.AutoCompleteCustomSource.Contains(txtDestination.Text))
                {
                    SaveAutocomplete(txtDestination.Text, "Destination");
                    txtDestination.AutoCompleteCustomSource.Add(txtDestination.Text);
                }


                MessageBox.Show("Message sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Ensure the connection is closed when the form is closed
            _connection?.Close();
            base.OnFormClosed(e);
        }
    }
}
