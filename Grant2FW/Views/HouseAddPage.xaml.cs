using Grant2FW.DataBaseModels;
using Grant2FW.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace Grant2FW.Views
{
    /// <summary>
    /// Логика взаимодействия для HouseAddPage.xaml
    /// </summary>
    public partial class HouseAddPage : Page
    {
        public object House { get; set; }
        public HouseAddPage()
        {
            InitializeComponent();
            var result = DataBase.ClassDataBase.dataobj.HousingComplex;
            {
                var housingComplexNames = result.Select(hc => hc.Name).ToList();

                ComplexValue.ItemsSource = housingComplexNames;
            }
            EditHouse.Visibility = Visibility.Hidden;
            Back.Visibility = Visibility.Hidden;
        }

        public HouseAddPage(Element element, int typeOperation)
        {
            InitializeComponent();
            StreetValue.Text = element.Street;
            NumberHouse.Text = element.Number_House;
            ComplexValue.SelectedItem = element.ComplexName;
            AdditionalCost.Text = Convert.ToString(element.Added_Value);
            CostBuilding.Text = Convert.ToString(element.Cost_House_Construction);

            var result = DataBase.ClassDataBase.dataobj.HousingComplex;
            {
                var housingComplexNames = result.Select(hc => hc.Name).ToList();

                ComplexValue.ItemsSource = housingComplexNames;
            }

            switch (typeOperation)
            {
                case 1:
                    House = element;
                    AddHouse.Visibility = Visibility.Hidden;
                    Back.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    EditHouse.Visibility = Visibility.Hidden;
                    AddHouse.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void AddHouse_Click(object sender, RoutedEventArgs e)
        {
            int housingComplexId = DataBase.ClassDataBase.dataobj.HousingComplex
                .Where(complex => complex.Name == ComplexValue.SelectedItem.ToString())
                .Select(complex => complex.Id)
                .FirstOrDefault();

            DataBaseModels.Housing house = new DataBaseModels.Housing()
            {
                IdHousingComplex = housingComplexId,
                Street = StreetValue.Text
            };

            DataBase.ClassDataBase.dataobj.Housing.Add(house);
            DataBase.ClassDataBase.dataobj.SaveChanges();


            DataBaseModels.HousingCopies houseCopy = new DataBaseModels.HousingCopies()
            {
                OriginalHousingId = house.Id,
                Number_House = NumberHouse.Text,
                Cost_House_Construction = Convert.ToInt32(CostBuilding.Text),
                Added_Value = Convert.ToInt32(AdditionalCost.Text),
                IsActual = true
            };

            DataBase.ClassDataBase.dataobj.HousingCopies.Add(houseCopy);
            DataBase.ClassDataBase.dataobj.SaveChanges();



            MessageBox.Show($"Запись успешно добавлена: {houseCopy.Number_House}, {house.Street}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            ClassNavigation.frameobj.Navigate(new Views.ListOfHouses());

        }

        private void EditHouse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? value = (House as Element).OriginalHousingId;
                var housingCopiesToUpdate = DataBase.ClassDataBase.dataobj.HousingCopies.Where(copy => copy.OriginalHousingId == value);

                foreach (var copy in housingCopiesToUpdate)
                {
                    copy.IsActual = false;
                }

                DataBase.ClassDataBase.dataobj.SaveChanges();


                DataBaseModels.HousingCopies houseCopy = new DataBaseModels.HousingCopies()
                {
                    OriginalHousingId = (House as Element).OriginalHousingId,
                    Number_House = NumberHouse.Text,
                    Cost_House_Construction = Convert.ToInt32(CostBuilding.Text),
                    Added_Value = Convert.ToInt32(AdditionalCost.Text),
                    IsActual = true
                };


                if (MessageBox.Show("Вы уверены, что хотите изменить данные?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DataBase.ClassDataBase.dataobj.HousingCopies.Add(houseCopy);
                    DataBase.ClassDataBase.dataobj.SaveChanges();

                    MessageBox.Show($"Запись успешно изменена:  Номер дома - {houseCopy.Number_House}, {(House as Element).Street}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClassNavigation.frameobj.Navigate(new Views.ListOfHouses());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ClassNavigation.frameobj.Navigate(new Views.ListOfHouses());
        }
    }
}
