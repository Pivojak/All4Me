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
    /// Okno pro PŘIDÁNÍ / ÚPRAVU denního záznamu
    /// </summary>
    public partial class WorkAddWindow : Window
    {
        /// <summary>
        /// Validátor dat pro blok práce
        /// </summary>
        private WorkValidator validator;
        /// <summary>
        /// View model pro bindování dat
        /// </summary>
        private VM_RecordPart viewModel = null;
        /// <summary>
        /// Kolekce názvů projektů pro ComboBox
        /// </summary>
        private List<string> projectsName = new List<string>();

        /// <summary>
        /// Konstruktor pro přidání nového záznamu
        /// </summary>
        /// <param name="validator">Validátor dat pro blok práce</param>
        public WorkAddWindow(WorkValidator validator)
        {   
            this.validator = validator;
            InitializeComponent();
            projectsName = validator.DefineProjectNames();
            projectComboBox.DataContext = projectsName;
            dateTextBox.DataContext = DateTime.Now;         
        }
        /// <summary>
        /// Konstruktor pro úpravu stávajícího záznamu
        /// </summary>
        /// <param name="validator">Validátor dat pro blok práce</param>
        /// <param name="viewModel">View model pro bindování denních bloků</param>
        public WorkAddWindow(WorkValidator validator, VM_RecordPart viewModel)
        {
            this.validator = validator;
            this.viewModel = viewModel;
            InitializeComponent();
            // Získám názvy projektů
            projectsName = validator.DefineProjectNames();
            // Nastavím zdroj pro ComboBox
            projectComboBox.DataContext = projectsName;
            // Nastavím comboBox na projekt do kterého záznam patří
            projectComboBox.SelectedIndex = projectsName.IndexOf(viewModel.ProjectName);

            DataContext = viewModel;
            dateTextBox.DataContext = viewModel.RecordDate;
        }

        /// <summary>
        /// Uložení zadaných údajů - kliknutí na tlačítko uložit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // přidání nového záznamu
                if (viewModel == null)
                    validator.AddEditWorkRecord(0, projectComboBox.SelectedIndex, dateTextBox.Text, planeTimeTextBox.Text,"","","","","","");
                // Úprava stávajícího záznamu
                else
                    validator.AddEditWorkRecord(1, projectComboBox.SelectedIndex, dateTextBox.Text, planeTimeTextBox.Text,fromPart1TextBox.Text, toPart1TextBox.Text,
                        contentPart1TextBox.Text, fromPart2TextBox.Text, toPart2TextBox.Text, contentPart2TextBox.Text);

                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
