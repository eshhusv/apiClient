using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private HttpClient httpClient;
        private MainWindow mainWindow;
        private string token;
        public Main(Response response)
        {
            InitializeComponent();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.access_token);
            token = response.access_token;
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<Admission>? list = await httpClient.GetFromJsonAsync<List<Admission>>("http://localhost:5054/api/Admission");
            int i = 0;
            Dispatcher.Invoke(() =>
            {
                ListAdmission.ItemsSource = null;
                ListAdmission.Items.Clear();
                ListAdmission.ItemsSource = list;
            });
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.mainWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SellWindow sellWindow = new SellWindow(token);
            sellWindow.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow(token!);
            productWindow.ShowDialog();
        }
    }
}
