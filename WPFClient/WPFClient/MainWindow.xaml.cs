using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;


namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7187/chatHub").WithAutomaticReconnect().Build();

            connection.Reconnecting += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Reconnecting...\n";
                    messages.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };

            connection.Reconnected += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Reconnected\n";
                    messages.Items.Clear();
                    messages.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };

            connection.Closed += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Connection CLosed\n";
                    messages.Items.Add(newMessage);
                    OpenConnection.IsEnabled = true;
                    sendMesage.IsEnabled = false;
                });
                return Task.CompletedTask;
            };
        }

        private async void OpenConnection_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}\n";
                    messages.Items.Add(newMessage);
                });
            });

            try
            {
                await connection.StartAsync();
                messages.Items.Add("Connection Started\n");
                OpenConnection.IsEnabled = false;
                sendMesage.IsEnabled = true;
            }
            catch(Exception ex)
            {
                var newMessage = $"Error: {ex.Message}\n";
                messages.Items.Add(newMessage);
            }
        }

        private async void sendMesage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("SendMessage", 
                    "WPF Client", messageInput.Text);
            }
            catch (Exception ex)
            {
                var newMessage = $"Error: {ex.Message}\n";
                messages.Items.Add(newMessage);
            }
        
        }
    }
}