using Grant2FW.DataBaseModels;
using Grant2FW.ViewModel;
using MaterialDesignColors;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using System.Xml.Linq;

namespace Grant2FW.Views
{

    /// <summary>
    /// Логика взаимодействия для ListOfHouses.xaml
    /// </summary>
    public partial class ListOfHouses : Page
    {
        public ListOfHouses()
        {
            InitializeComponent();
            ListHouses.ItemsSource = ViewModel.ListOfHousesVM.getList();
            NameComplex.TextChanged += FilterBoxes_TextChanged;
            NameNumber.TextChanged += FilterBoxes_TextChanged;
            NameStreet.TextChanged += FilterBoxes_TextChanged;
        }
        private void FilterBoxes_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshFilteredList();
        }

        private void RefreshFilteredList()
        {
            ObservableCollection<Element> tempElements = new ObservableCollection<Element>();

            ComboBoxItem typeItem = NameStatus.SelectedItem as ComboBoxItem;
            string value = (typeItem != null) ? typeItem.Content.ToString() : string.Empty;

            string complexFilter = value;
            string complexNameFilter = NameComplex.Text;
            string numberFilter = NameNumber.Text;
            string streetFilter = NameStreet.Text;

            var query = ListOfHousesVM.getList()
                .Where(element =>
                    (complexFilter == null || string.IsNullOrEmpty(complexFilter) || element.Status.Contains(complexFilter)) &&
                    (string.IsNullOrEmpty(complexNameFilter) || element.ComplexName.Contains(complexNameFilter)) &&
                    (string.IsNullOrEmpty(numberFilter) || element.Number_House.Contains(numberFilter)) &&
                    (string.IsNullOrEmpty(streetFilter) || element.Street.Contains(streetFilter)))
                .OrderBy(element => element.ComplexName)
                .ThenBy(element => element.Street)
                .ThenBy(element => element.Number_House)
                .ThenBy(element => element.Status);

            foreach (var result in query)
            {
                tempElements.Add(result);
            }
            ListOfHousesVM.housesCollection = tempElements;
            ListHouses.ItemsSource = ListOfHousesVM.housesCollection;
            ListHouses.Items.Refresh();
        }

        private void AddHouse_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClassNavigation.frameobj.Navigate(new HouseAddPage());
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            int idCopy = (button.DataContext as Element).IdCopy;
            var housingCopy = DataBase.ClassDataBase.dataobj.HousingCopies.FirstOrDefault(copy => copy.Id == idCopy);

            housingCopy.IsActual = false;

            if (MessageBox.Show("Вы уверены, что хотите удалить данные?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ListOfHousesVM.housesCollection.Remove((button.DataContext as Element));
                DataBase.ClassDataBase.dataobj.SaveChanges();
                MessageBox.Show($"Запись дома удалена: {housingCopy.Number_House}, {housingCopy.Housing.Street}");
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var CopyHouse = (button.DataContext as Element);
            ViewModel.ClassNavigation.frameobj.Navigate(new HouseAddPage(CopyHouse, 1));
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var CopyHouse = (button.DataContext as Element);
            ViewModel.ClassNavigation.frameobj.Navigate(new HouseAddPage(CopyHouse, 2));
        }

        private void NameStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshFilteredList();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            NameStatus.SelectedValue = "";
            NameComplex.Text = "";
            NameNumber.Text = "";
            NameStreet.Text = "";
        }

    }

}
