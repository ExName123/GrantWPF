using Grant2FW.AdditionalClasses;
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
            InitializeComponent();
            ViewModel.ClassNavigation.frameobj = MainFrame;
            MainFrame.Navigate(new Views.ListOfHouses());
        }
        public static readonly DependencyProperty IsMinimizedProperty =

             DependencyProperty.Register("IsMinimized", typeof(bool),

             typeof(MainWindow), new UIPropertyMetadata(false, new PropertyChangedCallback(OnMinimizedPropertyChanged)));

        public bool IsMinimized

        {

            get { return (bool)GetValue(IsMinimizedProperty); }

            set { SetValue(IsMinimizedProperty, value); }

        }

        static void OnMinimizedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)

        {

            MainWindow window = d as MainWindow;

            if (window != null)

            {

                window.WindowState = WindowState.Minimized;

            }

        }

        //Maximize

        public static readonly DependencyProperty IsMaximizeProperty =

       DependencyProperty.Register("IsMaximize", typeof(bool),

       typeof(MainWindow), new UIPropertyMetadata(false, new PropertyChangedCallback(OnMaximizedPropertyChanged)));

        public bool IsMaximize

        {

            get { return (bool)GetValue(IsMaximizeProperty); }

            set { SetValue(IsMaximizeProperty, value); }

        }

        static void OnMaximizedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)

        {

            MainWindow window = d as MainWindow;

            if (window != null)

            {

                if (window.WindowState == WindowState.Maximized)

                {

                    window.WindowState = WindowState.Normal;

                }

                else window.WindowState = WindowState.Maximized;

            }

        }

        //Close

        public static readonly DependencyProperty ClosedProperty =

   DependencyProperty.Register("Closed", typeof(bool),

   typeof(MainWindow), new UIPropertyMetadata(false, new PropertyChangedCallback(OnClosedPropertyChanged)));

        public bool Closed

        {

            get { return (bool)GetValue(ClosedProperty); }

            set { SetValue(ClosedProperty, value); }

        }

        static void OnClosedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)

        {

            MainWindow window = d as MainWindow;

            if (window != null)

            {

                window.Close();

            }

        }

    }

}

