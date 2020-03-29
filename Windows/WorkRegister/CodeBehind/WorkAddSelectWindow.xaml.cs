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
    /// Interakční logika pro WorkAddSelectWindow.xaml
    /// </summary>
    public partial class WorkAddSelectWindow : Window
    {
        /// <summary>
        /// Validátor dat pro blok práce
        /// </summary>
        private WorkValidator validator;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="validator">Validátor dat pro blok práce</param>
        public WorkAddSelectWindow(WorkValidator validator)
        {
            this.validator = validator;
            InitializeComponent();
        }

        /// <summary>
        /// Přidání nového projektu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectRecord_Click(object sender, RoutedEventArgs e)
        {
            WorkAddProjectWindow window = new WorkAddProjectWindow(validator);
            window.Show();
            Close();
        }

        /// <summary>
        /// Přidání nového denního záznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayRecord_Click(object sender, RoutedEventArgs e)
        {
            WorkAddWindow window = new WorkAddWindow(validator);
            window.Show();
            Close(); 
        }
    }
}
