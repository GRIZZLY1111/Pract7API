using Pract6.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pract7.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        public Recipe NewRecept { get; set; } = new Recipe();
        private readonly HttpClient client;

        public AddPage(HttpClient client)
        {
            this.client = client;
            InitializeComponent();
            DataContext = this;
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewRecept.Name))
            {
                MessageBox.Show("Введите название!");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewRecept.Cuisine))
            {
                MessageBox.Show("Введите принадлежность кухни!");
                return;
            }

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/Recipes", NewRecept);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Рецепт успешно добавлен!");
                NavigationService.Navigate(new MainPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}