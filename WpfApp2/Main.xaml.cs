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
        public async Task Load()
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

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow(token!);
            if (productWindow.ShowDialog() == true)
            {
                Sell sell = new Sell
                {
                    Id = await productWindow.getIdAdmission(),
                    SellingDate = productWindow.SellingDateProperty,
                    Name = productWindow.NameProperty,
                    CountOfSold = productWindow.CountOfSoldProperty,
                    PriceOfSold = productWindow.PriceOfSoldProperty,
                    VenorCode = productWindow.VenorCodeProperty
                };
                JsonContent content = JsonContent.Create(sell);
                using var response = await httpClient.PostAsync("http://localhost:5054/api/Sell", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
    }
}
