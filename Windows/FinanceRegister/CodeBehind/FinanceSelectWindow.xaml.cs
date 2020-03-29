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
    /// Interakční logika pro FinanceSelectWindow.xaml
    /// </summary>
    public partial class FinanceSelectWindow : Window
    {
        /// <summary>
        /// Finanční validátor
        /// </summary>
        private Validator validator;
        /// <summary>
        /// Plátno pro zobrazení vybraných transakcí
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="canvas">Plátno pro zobrazení vybraných transakcí</param>
        /// <param name="validator">Finanční validátor</param>
        public FinanceSelectWindow(Canvas canvas, Validator validator)
        {
            InitializeComponent();
            this.canvas = canvas;
            this.validator = validator;
        }
        /// <summary>
        /// Tlačítko TISK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FinancePrintSelectWindow window = new FinancePrintSelectWindow(validator);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
            
        }
        /// <summary>
        /// Tlačítko HLEDEJ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (TypeRecord type, Category category, TypeBalance balance, Month month) = validator.EnumValidator(categoryComboBox.SelectedIndex, typeRecordComboBox.SelectedIndex,
                    typeBalanceComboBox.SelectedIndex,monthComboBox.SelectedIndex);
                validator.FindFinanceRecords(type,category, balance, month);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
