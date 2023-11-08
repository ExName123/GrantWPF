using Grant2FW.ViewModel;
using MaterialDesignColors;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ComplexAddPage.xaml
    /// </summary>
    public partial class ComplexAddPage : Page
    {
        public object Complex { get; set; }
        public ComplexAddPage()
        {
            InitializeComponent();

            LoadCmbStatus();

            ListHousesOfComplex.IsEnabled = false;
            EditComplex.Visibility = Visibility.Hidden;
            Back.Visibility = Visibility.Hidden;
        }
        public ComplexAddPage(ElementComplex element, int typeOperation)
        {
            InitializeComponent();

            NameComplex.Text = element.ComplexName;
            City.Text = element.City;
            StatusComplexValue.SelectedItem = element.Status;
            AdditionalCost.Text = Convert.ToString(element.Additional_Cost_Apartament_House);
            CostBuilding.Text = Convert.ToString(element.Cost_House_Construction);

            LoadCmbStatus();

            switch (typeOperation)
            {
                case 1:
                    AddComplex.Visibility = Visibility.Hidden;
                    Back.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    EditComplex.Visibility = Visibility.Hidden;
                    AddComplex.Visibility = Visibility.Hidden;
                    break;
            }
            Complex = element;
            LoadListView();
        }
        /// <summary>
        /// загрузка статусов для ЖК
        /// </summary>
        public void LoadCmbStatus()
        {
            var result = DataBase.ClassDataBase.dataobj.Status;
            {
                var statuses = result.Select(hc => hc.StatusName).ToList();

                StatusComplexValue.ItemsSource = statuses;
            }
        }

        public void LoadListView()
        {
            ListHousesOfComplex.ItemsSource = ListOfHousesOfComplex.getList((int)(Complex as ElementComplex).OriginalComplexId);

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ClassNavigation.frameobj.Navigate(new Views.ListOfComplexes());
        }

        private void EditComplex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? value = (Complex as ElementComplex).OriginalComplexId;
                var ComplexCopiesToUpdate = DataBase.ClassDataBase.dataobj.HousingComplexCopies.Where(copy => copy.IdHousingComplex == value);

                foreach (var copy in ComplexCopiesToUpdate)
                {
                    copy.IsActual = false;
                }

                DataBase.ClassDataBase.dataobj.SaveChanges();

                var StatusInt = DataBase.ClassDataBase.dataobj.Status.FirstOrDefault(x => x.StatusName == StatusComplexValue.SelectedItem.ToString());


                if(CostBuilding.Text == string.Empty)
                {
                    CostBuilding.Text = 0.ToString();
                }
                if (AdditionalCost.Text == string.Empty)
                {
                    AdditionalCost.Text = 0.ToString();
                }
                DataBaseModels.HousingComplexCopies ComplexCopy = new DataBaseModels.HousingComplexCopies()
                {
                    IdHousingComplex = (Complex as ElementComplex).OriginalComplexId,
                    City = City.Text,
                    Cost_Complex_Construction = Convert.ToInt32(CostBuilding.Text),
                    Additional_Cost_Complex = Convert.ToInt32(AdditionalCost.Text),
                    Name = NameComplex.Text,
                    Status = StatusInt.Id,
                    IsActual = true
                };

                if (MessageBox.Show("Вы уверены, что хотите изменить данные?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DataBase.ClassDataBase.dataobj.HousingComplexCopies.Add(ComplexCopy);
                    DataBase.ClassDataBase.dataobj.SaveChanges();

                    MessageBox.Show($"Запись успешно изменена:  ЖК - {ComplexCopy.Name}, ", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClassNavigation.frameobj.Navigate(new Views.ListOfComplexes());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
        }

        private void AddComplex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseModels.HousingComplex Complex = new DataBaseModels.HousingComplex()
                {
                    City = City.Text,
                    Name = NameComplex.Text,
                };

                DataBase.ClassDataBase.dataobj.HousingComplex.Add(Complex);
                DataBase.ClassDataBase.dataobj.SaveChanges();

                var StatusInt = DataBase.ClassDataBase.dataobj.Status.FirstOrDefault(x => x.StatusName == StatusComplexValue.SelectedItem.ToString());

                DataBaseModels.HousingComplexCopies ComplexCopy = new DataBaseModels.HousingComplexCopies()
                {
                    City = City.Text,
                    Name = NameComplex.Text,
                    Additional_Cost_Complex = Convert.ToInt32(AdditionalCost.Text),
                    Cost_Complex_Construction = Convert.ToInt32(CostBuilding.Text),
                    IsActual = true,
                    IdHousingComplex = Complex.Id,
                    Status = StatusInt.Id
                };

                DataBase.ClassDataBase.dataobj.HousingComplexCopies.Add(ComplexCopy);
                DataBase.ClassDataBase.dataobj.SaveChanges();

                MessageBox.Show($"Запись успешно добавлена: ЖК - {ComplexCopy.Name} ", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                ClassNavigation.frameobj.Navigate(new Views.ListOfComplexes());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
        }
    }
}
