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
    /// Okno pro výběr statistiky (roční nebo měsíční) uživatel ve dvou comboBoxem vybere požadované hodnoty
    /// </summary>
    public partial class WorkSelectWindow : Window
    {
        /// <summary>
        /// Validátor dat pro blok práce
        /// </summary>
        private WorkValidator validator;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="validator">Validátor dat pro blok práce</param>
        public WorkSelectWindow(WorkValidator validator)
        {         
            InitializeComponent();
            this.validator = validator;
        }

        /// <summary>
        /// Vyhledá požadované záznamy a zobrazí na je na plátně na hlavní straně v týdenních nebo měsíčních blocích
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vytvoření přehledu
                validator.ConstructWeekMonthOverview(false, yearComboBox.SelectedIndex, monthComboBox.SelectedIndex,true);
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        
        /// <summary>
        /// Vyvolá okno, kde si uživatel zvolí zda chce txt ve formátu WORDU nebo EXCELU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            // Okno pro výběr zda uživatel požaduje TXT ve formě pro WORD nebo pro EXCEL
            WorkPrintSelectWindow window = new WorkPrintSelectWindow(validator, yearComboBox.SelectedIndex, monthComboBox.SelectedIndex);
            window.Show();
            Close();
        }
    }
}
