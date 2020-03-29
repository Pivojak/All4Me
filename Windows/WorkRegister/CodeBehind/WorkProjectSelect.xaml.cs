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
    /// Okno pro výběr projektu, který chce uživatel zobrazit - obsahuje pouze comboBox s projekty
    /// </summary>
    public partial class WorkProjectSelect : Window
    {
        /// <summary>
        /// Validátor dat pro blok práce
        /// </summary>
        private WorkValidator validator;
        /// <summary>
        /// View model na který je bindováno v případě úpravy projektu - využije se až v okně projektu
        /// </summary>
        private VM_Project viewModel;
        /// <summary>
        /// Kolekce názvů projektů
        /// </summary>
        private List<string> projectNames;

        /// <summary>
        /// Kontruktor okna
        /// </summary>
        /// <param name="validator">Validátor dat pro blok práce</param>
        public WorkProjectSelect(WorkValidator validator)
        {
            projectNames = validator.DefineProjectNames();

            InitializeComponent();
            projectComboBox.DataContext = projectNames;
            this.validator = validator;
        }
        /// <summary>
        /// Tlačítko pro vyhledání projektu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel = validator.FindProjects(projectComboBox.SelectedIndex);
            WorkAddProjectWindow window = new WorkAddProjectWindow(validator, viewModel);
            window.Show();
            Close();
        }

    }
}
