using Grant2FW.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace Grant2FW.Views
{
    /// <summary>
    /// Логика взаимодействия для ListOfComplexes.xaml
    /// </summary>
    public partial class ListOfComplexes : Page
    {
        public ListOfComplexes()
        {
            InitializeComponent();
            ListComplexes.ItemsSource = ViewModel.ListOfComplexesVM.getList();
            NameComplex.TextChanged += FilterBoxes_TextChanged;
        }
        private void NameStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshFilteredList();
        }
       /// <summary>
       /// обновляет список согласно фильтрам
       /// </summary>
        private void RefreshFilteredList()
        {
            ObservableCollection<ElementComplex> tempElements = new ObservableCollection<ElementComplex>();

            ComboBoxItem typeItem = NameStatus.SelectedItem as ComboBoxItem;
            string value = (typeItem != null) ? typeItem.Content.ToString() : string.Empty;

            string complexFilter = value;
            string complexNameFilter = NameComplex.Text;

            var query = ListOfComplexesVM.getList()
                .Where(element =>
                    (complexFilter == null || string.IsNullOrEmpty(complexFilter) || element.Status.Contains(complexFilter)) &&
                    (string.IsNullOrEmpty(complexNameFilter) || element.ComplexName.Contains(complexNameFilter)) &&
                     (string.IsNullOrEmpty(NameCity.Text) || element.City.Contains(NameCity.Text))
                    )
                .OrderBy(element => element.ComplexName)
                .ThenBy(element => element.Status)
                 .ThenBy(element => element.City);

            foreach (var result in query)
            {
                tempElements.Add(result);
            }
            ListOfComplexesVM.complexesCollection = tempElements;
            ListComplexes.ItemsSource = ListOfComplexesVM.complexesCollection;
            ListComplexes.Items.Refresh();
        }
        private void FilterBoxes_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshFilteredList();
        }
        private void AddComplex_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClassNavigation.frameobj.Navigate(new ComplexAddPage());
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            NameStatus.SelectedValue = "";
            NameComplex.Text = "";

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var CopyComplex = (button.DataContext as ElementComplex);
            ViewModel.ClassNavigation.frameobj.Navigate(new ComplexAddPage(CopyComplex, 1));
        }
        /// <summary>
        /// просмотр страницы ЖК со сведениями
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var CopyComplex = (button.DataContext as ElementComplex);
            ViewModel.ClassNavigation.frameobj.Navigate(new ComplexAddPage(CopyComplex, 2));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            int idCopy = (button.DataContext as ElementComplex).IdCopy;
            var complexCopy = DataBase.ClassDataBase.dataobj.HousingComplexCopies.FirstOrDefault(copy => copy.Id == idCopy);

            complexCopy.IsActual = false;

            if (MessageBox.Show("Вы уверены, что хотите удалить данные?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ListOfComplexesVM.complexesCollection.Remove((button.DataContext as ElementComplex));
                DataBase.ClassDataBase.dataobj.SaveChanges();
                MessageBox.Show($"Запись ЖК удалена: {complexCopy.Name}, {complexCopy.Status}");
            }
        }
    }
}
