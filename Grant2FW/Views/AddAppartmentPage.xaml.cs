using Grant2FW.DataBaseModels;
using Grant2FW.ViewModel;
using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Numerics;
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
    /// Логика взаимодействия для AddAppartmentPage.xaml
    /// </summary>
    public partial class AddAppartmentPage : Page
    {
        public object Appartment { get; set; }
        public AddAppartmentPage()
        {
            InitializeComponent();
            SetValuesStatusesAndComplex();
            EditAppartment.Visibility = Visibility.Hidden;
            Back.Visibility = Visibility.Hidden;
        }
        public AddAppartmentPage(ElementAppartment element, int typeOperation)
        {
            InitializeComponent();
            numberAppartment.Text = element.NumberRoom;
            area.Text = element.Area.ToString();
            countRooms.Text = element.NumberRooms.ToString();
            section.Text = element.Section.ToString();
            floor.Text = element.Floor.ToString();
            additionalCost.Text = element.AddedValue.ToString();
            costBuilding.Text = element.ExpensesBuildingAnApartment.ToString();

            status.SelectedItem = element.Status;
            complex.SelectedItem = element.NameComplex;
            complex.Tag = element.IdComplex;


            SetValuesComboBoxes(element.NumberHouse);

            switch (typeOperation)
            {
                case 1:
                    Appartment = element;
                    AddAppartment.Visibility = Visibility.Hidden;
                    Back.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    EditAppartment.Visibility = Visibility.Hidden;
                    AddAppartment.Visibility = Visibility.Hidden;

                    break;
            }
        }
        /// <summary>
        /// метод для установки значений в comboboxes (список комплексов и статусов квартир)
        /// </summary>
        public void SetValuesStatusesAndComplex()
        {
            var result = DataBase.ClassDataBase.dataobj.HousingComplex;
            {
                var housingComplexNames = result.Select(hc => hc.Name).ToList();

                complex.ItemsSource = housingComplexNames;

            }
            var resultStatus = DataBase.ClassDataBase.dataobj.StatusesAppartment;
            {
                var statusNames = resultStatus.Select(hc => hc.Name).ToList();

                status.ItemsSource = statusNames;
            }
        }
        /// <summary>
        /// метод для установки значений в combobox для домов определенного ЖК
        /// </summary>
        /// <param name="numberHouse"></param>
        public void SetValuesComboBoxes(string numberHouse)
        {
            SetValuesStatusesAndComplex();
            string selectedItem = complex.SelectedItem.ToString();
            var complexId = DataBase.ClassDataBase.dataobj.HousingComplexCopies
                .Where(item => item.Name == selectedItem && item.IsActual == true)
                .FirstOrDefault();

            var items = DataBase.ClassDataBase.dataobj.HousingCopies
          .Where(item => (int)item.IdComplex == complexId.IdHousingComplex && item.IsActual == true)
          .Select(item => item.Number_House)
          .ToList();

            house.SelectedItem = numberHouse;
            house.ItemsSource = items;
            house.SelectedItem = numberHouse;
              
        }
        /// <summary>
        /// метод для добавления квартиры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAppartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedItem = complex.SelectedItem.ToString();

                var complexId = DataBase.ClassDataBase.dataobj.HousingComplexCopies
                  .Where(item => item.Name == selectedItem)
                  .FirstOrDefault();

                selectedItem = house.SelectedItem.ToString();
                var houseId = DataBase.ClassDataBase.dataobj.HousingCopies
                    .Where(item => item.Number_House == selectedItem && (int)item.IdComplex == complexId.IdHousingComplex)
                    .FirstOrDefault();

                DataBaseModels.Appartments appartment = new DataBaseModels.Appartments()
                {
                    IdHouse = houseId.OriginalHousingId
                };

                string statusItem = status.SelectedItem.ToString();
                var statusId = DataBase.ClassDataBase.dataobj.StatusesAppartment
                    .Where(item => item.Name == statusItem)
                    .FirstOrDefault();

                DataBase.ClassDataBase.dataobj.Appartments.Add(appartment);

                DataBase.ClassDataBase.dataobj.SaveChanges();
                DataBaseModels.ApartmentsCopy appartmentCopy = new DataBaseModels.ApartmentsCopy()
                {
                    NumberOfRooms = Convert.ToInt32(countRooms.Text),
                    Added_value = Convert.ToInt32(additionalCost.Text),
                    Area = Convert.ToDecimal(area.Text),
                    Section = Convert.ToInt32(section.Text),
                    Floor = Convert.ToInt32(floor.Text),
                    Expenses_Building_An_Apartment = Convert.ToInt32(costBuilding.Text),
                    IdStatus = statusId.Id,
                    IdApartment = appartment.Id,
                    Address = numberAppartment.Text,
                    IsActual = true
                };

                DataBase.ClassDataBase.dataobj.ApartmentsCopy.Add(appartmentCopy);
                DataBase.ClassDataBase.dataobj.SaveChanges();

                MessageBox.Show($"Запись успешно добавлена: Номер квартиры: {appartmentCopy.Address}, площадь:{appartmentCopy.Area}, подъезд:{appartmentCopy.Section}  ", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                ClassNavigation.frameobj.Navigate(new Views.PageAppartments());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
        }
        /// <summary>
        /// метод для изменения квартиры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditAppartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? value = (Appartment as ElementAppartment).IdAppartment;
                var appartmentCopiesToUpdate = DataBase.ClassDataBase.dataobj.ApartmentsCopy.Where(copy => copy.IdApartment == value);

                foreach (var copy in appartmentCopiesToUpdate)
                {
                    copy.IsActual = false;
                }

                DataBase.ClassDataBase.dataobj.SaveChanges();


                string statusItem = status.SelectedItem.ToString();
                var statusId = DataBase.ClassDataBase.dataobj.StatusesAppartment
                    .Where(item => item.Name == statusItem)
                    .FirstOrDefault();

                DataBaseModels.ApartmentsCopy appartmentCopy = new DataBaseModels.ApartmentsCopy()
                {
                    NumberOfRooms = Convert.ToInt32(countRooms.Text),
                    Added_value = Convert.ToInt32(additionalCost.Text),
                    Area = Convert.ToDecimal(area.Text),
                    Section = Convert.ToInt32(section.Text),
                    Floor = Convert.ToInt32(floor.Text),
                    Expenses_Building_An_Apartment = Convert.ToInt32(costBuilding.Text),
                    IdStatus = statusId.Id,
                    IdApartment = (Appartment as ElementAppartment).IdAppartment,
                    Address = numberAppartment.Text,
                    IsActual = true
                };


                if (MessageBox.Show("Вы уверены, что хотите изменить данные?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DataBase.ClassDataBase.dataobj.ApartmentsCopy.Add(appartmentCopy);
                    DataBase.ClassDataBase.dataobj.SaveChanges();

                    MessageBox.Show($"Запись успешно изменена: Номер квартиры: {appartmentCopy.Address}, площадь:{appartmentCopy.Area}, подъезд:{appartmentCopy.Section}  ", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClassNavigation.frameobj.Navigate(new Views.PageAppartments());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
        }
        /// <summary>
        /// метод для возврата назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ClassNavigation.frameobj.Navigate(new Views.PageAppartments());
        }
        /// <summary>
        ///  метод для установки значений в combobox для домов определенного ЖК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void complex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = (sender as ComboBox).SelectedItem.ToString();
            var complexId = DataBase.ClassDataBase.dataobj.HousingComplexCopies
                .Where(item => item.Name == selectedItem && item.IsActual == true)
                .FirstOrDefault();

            var items = DataBase.ClassDataBase.dataobj.HousingCopies
                .Where(item => (int)item.IdComplex == complexId.IdHousingComplex)
                .Select(item => item.Number_House)
                .ToList();

            house.ItemsSource = items;
        }
    }
}
