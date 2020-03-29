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
    /// Interakční logika pro WorkPrintSelectWindow.xaml
    /// </summary>
    public partial class WorkPrintSelectWindow : Window
    {
        /// <summary>
        /// Validátor dat pro blok práce
        /// </summary>
        private WorkValidator validator;
        /// <summary>
        /// Identifikátor roku
        /// </summary>
        private int yearId;
        /// <summary>
        /// Identifikátor měsíce
        /// </summary>
        private int monthId;


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="validator">Instance pracovního validátoru</param>
        /// <param name="yearId">Hodnota comboBoxu ROK v okně select</param>
        /// <param name="monthId">Hodnota comboBoxu MĚSÍC v okně select</param>
        public WorkPrintSelectWindow(WorkValidator validator,int yearId, int monthId)
        {
            this.validator = validator;
            this.yearId = yearId;
            this.monthId = monthId;
            InitializeComponent();
        }

        /// <summary>
        /// Extrahování do txt, které je formátováno pro využití ve WORD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForWordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validator.PrintWorkRecords(true,yearId,monthId);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Extrahování do txt, které je formátováno pro vložení do EXCELU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForTableButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validator.PrintWorkRecords(false, yearId, monthId);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
