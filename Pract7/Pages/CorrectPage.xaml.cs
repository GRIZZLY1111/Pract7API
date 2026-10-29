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
    /// Логика взаимодействия для CorrectPage.xaml
    /// </summary>
    public partial class CorrectPage : Page
    {
        public Recipe CorrectRecept { get; set; }
        private readonly HttpClient client;

        public CorrectPage(Recipe recept, HttpClient client)
        {
            CorrectRecept = new Recipe
            {
                Id = recept.Id,
                Name = recept.Name,
                Cuisine = recept.Cuisine,
                CookingTime = recept.CookingTime,
                Calories = recept.Calories,
                IsVegetarian = recept.IsVegetarian
            };
            this.client = client;
            InitializeComponent();
            DataContext = this;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CorrectRecept.Name))
            {
                MessageBox.Show("Введите название!");
                return;
            }

            if (string.IsNullOrWhiteSpace(CorrectRecept.Cuisine))
            {
                MessageBox.Show("Введите принадлежность кухни!");
                return;
            }

            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/v1/Recipes/{CorrectRecept.Id}", CorrectRecept);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Рецепт успешно изменен!");
                NavigationService.Navigate(new MainPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
