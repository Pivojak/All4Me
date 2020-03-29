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
using System.Windows.Shapes;

namespace All4Me
{
    /// <summary>
    /// Interakční logika pro FinancePrintSelectWindow.xaml
    /// </summary>
    public partial class FinancePrintSelectWindow : Window
    {
        /// <summary>
        /// Finanční validátor
        /// </summary>
        private Validator validator;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="validator">Finanční validátor</param>
        public FinancePrintSelectWindow(Validator validator)
        {
            InitializeComponent();
            this.validator = validator;
        }
        /// <summary>
        /// Export ve formě pro EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForTableButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validator.PrintFinanceRecords(false);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
            
        }
        /// <summary>
        /// Export ve formě pro WORD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForWordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validator.PrintFinanceRecords(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }
    }
}
