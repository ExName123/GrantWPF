using Grant2FW.DataBaseModels;
using Grant2FW.ViewModel;
using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FuzzyString;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Grant2FW.Views
{
    /// <summary>
    /// Логика взаимодействия для PageAppartments.xaml
    /// </summary>
    public partial class PageAppartments : Page
    {
        public PageAppartments()
        {
            InitializeComponent();

            ListAppartments.ItemsSource = ViewModel.ListOfAppartments.getList();
            NameSection.TextChanged += FilterBoxes_TextChanged;
            NameFloor.TextChanged += FilterBoxes_TextChanged;


            var result = DataBase.ClassDataBase.dataobj.HousingComplex;
            {
                var housingComplexNames = result.Select(hc => hc.Name).ToList();

                NameComplex.ItemsSource = housingComplexNames;

            }
        }
        int CalculateLevenshteinDistance(string str1, string str2)
        {
            return str1.LevenshteinDistance(str2);
        }
        private void FilterBoxes_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshFilteredListLevenstein();
        }
        private void RefreshFilteredListLevenstein()
        {
            ObservableCollection<ElementAppartment> tempElements = new ObservableCollection<ElementAppartment>();
            ObservableCollection<ElementAppartment> tempElementsLevenstein = new ObservableCollection<ElementAppartment>();

            string valueHouse = (NameHouse.SelectedItem != null) ? NameHouse.SelectedItem.ToString() : string.Empty;
            string valueComplex = (NameComplex.SelectedItem != null) ? NameComplex.SelectedItem.ToString() : string.Empty;

            ComboBoxItem statusItem = NameSell.SelectedItem as ComboBoxItem;
            string valueSell = (statusItem != null) ? statusItem.Content.ToString() : string.Empty;

            int? sectionFilter = ClassesCheck.Check.IsIntValid(NameSection.Text) ? (int?)Convert.ToInt32(NameSection.Text) : null;
            int? floorFilter = ClassesCheck.Check.IsIntValid(NameFloor.Text) ? (int?)Convert.ToInt32(NameFloor.Text) : null;



            string searchString = $"{valueComplex}{valueHouse}{floorFilter}{sectionFilter}";
            if (valueComplex != "" && valueHouse != "" && sectionFilter.ToString() != "" && floorFilter.ToString() != "")
            {
                ListOfAppartments.appartmentsCollection.Clear();

                foreach (var result in ListOfAppartments.getList())
                {
                    if (AdditionalClasses.ClassLevenstein.LevenshteinDistance((result.NameComplex + result.NumberHouse + result.Floor + result.Section), (searchString)) <= 4)
                    {
                        tempElementsLevenstein.Add(result);
                    }
                }
                ListOfAppartments.appartmentsCollection = tempElementsLevenstein;
                ListAppartments.ItemsSource = ListOfAppartments.appartmentsCollection;
                ListAppartments.Items.Refresh();
            }
            else
            {
                RefreshFilteredList();
            }

        }
        /// <summary>
        /// обновляет список согласно фильтрам возможны 2 варианта:
        /// если пользователь ввел все данные, то список фильтруется по правилу расстояние Левенштейна <= 4
        /// иначе ищется совпадение по полю
        /// </summary>
        private void RefreshFilteredList()
        {
            ObservableCollection<ElementAppartment> tempElements = new ObservableCollection<ElementAppartment>();


            string valueHouse = (NameHouse.SelectedItem != null) ? NameHouse.SelectedItem as string : string.Empty;
            string valueComplex = (NameComplex.SelectedItem != null) ? NameComplex.SelectedItem as string : string.Empty;

            ComboBoxItem statusItem = NameSell.SelectedItem as ComboBoxItem;
            string valueSell = (statusItem != null) ? statusItem.Content.ToString() : string.Empty;

            int? sectionFilter = ClassesCheck.Check.IsIntValid(NameSection.Text) ? (int?)Convert.ToInt32(NameSection.Text) : null;
            int? floorFilter = ClassesCheck.Check.IsIntValid(NameFloor.Text) ? (int?)Convert.ToInt32(NameFloor.Text) : null;

            var filteredData = ListOfAppartments.getList()
                .Where(item =>
                    (
                        (string.IsNullOrEmpty(valueHouse) || item.NumberHouse == valueHouse || item.NumberHouse.Contains(valueHouse)) &&
                        (string.IsNullOrEmpty(valueComplex) || item.NameComplex == valueComplex || item.NameComplex.Contains(valueComplex)) &&
                        (string.IsNullOrEmpty(valueSell) || item.Status == valueSell || item.Status.Contains(valueSell))
                    ) &&
                    (sectionFilter == null || item.Section == sectionFilter) &&
                    (floorFilter == null || item.Floor == floorFilter)
                )
                .OrderBy(element => element.NumberHouse)
                .ThenBy(element => element.Status)
                .ThenBy(element => element.NameComplex);

            foreach (var result in filteredData)
            {
                tempElements.Add(result);
            }
            ListOfAppartments.appartmentsCollection = tempElements;
            ListAppartments.ItemsSource = ListOfAppartments.appartmentsCollection;
            ListAppartments.Items.Refresh();
        }
        private void NameHouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((int)NameComplex.Tag == 1))
                RefreshFilteredListLevenstein();
        }

        private void NameComplex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NameComplex.Tag = 1;


            string selectedItem = ((sender as ComboBox).SelectedItem != null) ? (sender as ComboBox).SelectedItem.ToString() : string.Empty;
            var complexId = DataBase.ClassDataBase.dataobj.HousingComplexCopies
                .Where(item => item.Name == selectedItem && item.IsActual == true)
                .FirstOrDefault();

            if (selectedItem != string.Empty)
            {
                var items = DataBase.ClassDataBase.dataobj.HousingCopies
                    .Where(item => (int)item.IdComplex == complexId.IdHousingComplex)
                    .Select(item => item.Number_House)
                    .ToList();

                NameHouse.ItemsSource = items;
            }

            RefreshFilteredListLevenstein();

            NameComplex.Tag = 0;
        }

        private void NameSell_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshFilteredListLevenstein();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            NameSell.SelectedValue = "";
            NameComplex.SelectedValue = "";
            NameHouse.SelectedValue = "";
            NameSection.Text = "";
            NameFloor.Text = "";
        }

        private void AddAppartment_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClassNavigation.frameobj.Navigate(new AddAppartmentPage());
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var copyAppartment = (button.DataContext as ElementAppartment);
            ViewModel.ClassNavigation.frameobj.Navigate(new AddAppartmentPage(copyAppartment, 1));
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var copyAppartment = (button.DataContext as ElementAppartment);
            ViewModel.ClassNavigation.frameobj.Navigate(new AddAppartmentPage(copyAppartment, 2));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            int idCopy = (button.DataContext as ElementAppartment).IdCopy;
            var appartmentCopy = DataBase.ClassDataBase.dataobj.ApartmentsCopy.FirstOrDefault(copy => copy.Id == idCopy);

            appartmentCopy.IsActual = false;

            if (MessageBox.Show("Вы уверены, что хотите удалить данные?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ListOfAppartments.appartmentsCollection.Remove((button.DataContext as ElementAppartment));
                DataBase.ClassDataBase.dataobj.SaveChanges();
                MessageBox.Show($"Запись квартиры удалена: кв.{appartmentCopy.Address}, {appartmentCopy.NumberOfRooms} комнат, {appartmentCopy.Area} кв.м");
            }
        }
    }
}
