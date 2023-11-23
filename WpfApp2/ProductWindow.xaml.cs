using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private HttpClient client;
        private Admission admission;
        public ProductWindow(String token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(()=>LoadAdmissions());
        }
        private async void LoadAdmissions()
        {
            List<Admission>? list = await client.GetFromJsonAsync<List<Admission>>("http://localhost:5054/api/Admission");
            Dispatcher.Invoke(() =>
            {
                cbGroup.ItemsSource = list?.Select(p => p.Name);
            });
        }
        public string? NameProperty
        {
            get { return Name.Text; }
        }
        public int CountOfSoldProperty
        {
            get { return int.Parse(CountOfSold.Text); }
        }
        public double PriceOfSoldProperty
        {
            get { return double.Parse(PriceOfSold.Text); }
        }
        public int VenorCodeProperty
        {
            get { return int.Parse(VenorCode.Text); }
        }
        public DateTime SellingDateProperty
        {
            get { return  DateTime.Parse(SellingDate.Text); }
        } 
        public async Task<int> getIdAdmission()
        {
            Admission? admission = await client.GetFromJsonAsync<Admission>("http://localhost:5054/api/Admission/" + cbGroup.Text);
            return admission!.VenorCode;
            //
        }

        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelProduct_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
