using Pract6.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Recipe> recipe { get; set; } = new ObservableCollection<Recipe>();

        private Recipe? _recept;
        public Recipe? recept
        {
            get => _recept;
            set { _recept = value; OnPropertyChanged(); }
        }

        private HttpClient client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:7026/");
            DataContext = this;
            LoadList();
        }

        public async Task LoadList()
        {
            try
            {
                var response = await client.GetFromJsonAsync<List<Recipe>>("api/v1/Recipes");
                if (response != null)
                {
                    recipe.Clear();
                    foreach (var item in response)
                    {
                        recipe.Add(item);
                    }

                    ICollectionView view = CollectionViewSource.GetDefaultView(recipe);
                    view.Filter = FilterRecipe;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к серверу: {ex.Message}");
            }
        }
        private bool FilterRecipe(object obj)
        {
            if (obj is not Recipe item) 
                return false;

            string searchText = SearchTextBox.Text.Trim().ToLower();
            bool Search = string.IsNullOrEmpty(searchText) ||
                                 (item.Name != null && item.Name.ToLower().Contains(searchText)) ||
                                 (item.Cuisine != null && item.Cuisine.ToLower().Contains(searchText));

            bool Vegetarian = true;

            if (VegetarianFilterComboBox != null)
            {
                switch (VegetarianFilterComboBox.SelectedIndex)
                {
                    case 1:
                        Vegetarian = item.IsVegetarian;
                        break;
                    case 2: 
                        Vegetarian = !item.IsVegetarian;
                        break;
                    default: 
                        Vegetarian = true;
                        break;
                }
            }

            return Search && Vegetarian;
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(recipe)?.Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPage(client));
        }

        private void Correct_Click(object sender, RoutedEventArgs e)
        {
            if (recept == null)
            {
                MessageBox.Show("Выберите рецепт из списка для изменения!");
                return;
            }
            NavigationService.Navigate(new CorrectPage(recept, client));
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (recept == null)
            {
                MessageBox.Show("Выберите рецепт для удаления!");
                return;
            }

            var result = MessageBox.Show($"Удалить рецепт \"{recept.Name}\"?", "Подтверждение", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"api/v1/Recipes/{recept.Id}");
                response.EnsureSuccessStatusCode();
                LoadList();
                MessageBox.Show("Рецепт успешно удален");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}