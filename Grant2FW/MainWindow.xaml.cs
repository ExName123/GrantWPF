using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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

namespace Grant2FW
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            try { 
            AdditionalClasses.ClassCheckConnection.CheckDatabaseConnection();
            
            InitializeComponent();
            ViewModel.ClassNavigation.frameobj = MainFrame;
            MainFrame.Navigate(new Views.ListOfHouses());
            }
            catch (Exception ex)
            {
                textBlockHouses.IsEnabled =  false;
                textBlockAppartments.IsEnabled = false;
                textBlockComplex.IsEnabled = false;
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
      
        private void TextBlockHouses_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new Views.ListOfHouses());
            Clear();
            textBlockHouses.Background = Brushes.White;
        }

        private void TextBlockComplex_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new Views.ListOfComplexes());
            Clear();
            textBlockComplex.Background = Brushes.White;
        }

        private void TextBlockAppartments_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new Views.PageAppartments());
            Clear();
            textBlockAppartments.Background = Brushes.White;
        }

        private void TextBlockExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        public void Clear()
        {
            textBlockAppartments.ClearValue(TextBlock.BackgroundProperty);
            textBlockComplex.ClearValue(TextBlock.BackgroundProperty);
            textBlockHouses.ClearValue(TextBlock.BackgroundProperty);
          
        }
    }

}

