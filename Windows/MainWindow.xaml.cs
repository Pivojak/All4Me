using System;
using System.Collections.Generic;
using System.IO;
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

namespace All4Me
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Validátor pro FINANCE
        private Validator validator;
        // Validátor pro pracovní Evidenci
        private WorkValidator workValidator;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        public MainWindow()
        {
            // Načtení parametrů uložených v XML souboru
            InitializeComponent();
            validator = new Validator();
            // Načtení dat pro finance
            validator.LoadFinance();
            validator.LoadBalance();
            // Instance pracovního validátoru používaná všude v práci
            workValidator = new WorkValidator();
        }
        /// <summary>
        /// Vstup do sekce poznámkový blok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            NoteStartWindow window = new NoteStartWindow();
            window.Show();
            Close();
        }

        /// <summary>
        /// Vstup do sekce evidence práce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobButton_Click(object sender, RoutedEventArgs e)
        {
            WorkStartWindow window = new WorkStartWindow();
            window.Show();
            Close();
        }

        /// <summary>
        /// Vstup do sekce pro evidenci financí
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinanceButton_Click(object sender, RoutedEventArgs e)
        {
            FinanceStartWindow window = new FinanceStartWindow(validator);
            validator.ViewGraphicFinance(0,true,false);
            window.Show();
            Close();
        }

        /// <summary>
        /// Zavření okna - kliknutí na logo All4Me
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
