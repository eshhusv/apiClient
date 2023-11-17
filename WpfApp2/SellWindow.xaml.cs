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
    /// Логика взаимодействия для SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {
        private HttpClient httpClient;
        private Sell? sell;
        public SellWindow(string token)
        {
            InitializeComponent();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => Load());
        }

        private async Task Load()
        {
            List<Sell>? list = await httpClient.GetFromJsonAsync<List<Sell>>("http://localhost:5054/api/Sell");
            Dispatcher.Invoke(() =>
            {
                ListSell.ItemsSource = null;
                ListSell.Items.Clear();
                ListSell.ItemsSource = list;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
