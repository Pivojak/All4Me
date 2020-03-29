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
    /// Interakční logika pro WorkAddPartWindow.xaml
    /// </summary>
    public partial class WorkAddPartWindow : Window
    {
        /// <summary>
        /// Validátor pro pracovní blok
        /// </summary>
        private WorkValidator validator;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="validator">Validátor pro pracovní blok</param>
        public WorkAddPartWindow(WorkValidator validator)
        {
            this.validator = validator;
            InitializeComponent();
        }

        /// <summary>
        /// Přidání nového denního bloku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validator.AddEditWorkParts(0, fromPartTextBox.Text, toPartTextBox.Text, contentPartTextBox.Text, null);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
