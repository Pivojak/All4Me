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
    /// Interakční logika pro WorkAddProjectWindow.xaml
    /// </summary>
    public partial class WorkAddProjectWindow : Window
    {
        /// <summary>
        /// Validátor pro pracovní blok
        /// </summary>
        private WorkValidator validator;
        /// <summary>
        /// View model, na který se binduje, pokud jde o úpravu stávajícího projektu
        /// </summary>
        private VM_Project viewModel;
        /// <summary>
        /// Flag pro stanovení, zda okno bylo zavřeno pomocí SAVE tlačítka a ne pomocí křížku
        ///      - TRUE uložení a chtěné zavření, - FALSE zavření přeš křížek, Dotaz na zavření
        /// </summary>
        private bool correctClose;

        /// <summary>
        /// Kontruktor - Přidání nového projektu
        /// </summary>
        /// <param name="validator">Validátor pro pracovní blok</param>
        public WorkAddProjectWindow(WorkValidator validator)
        {
            this.validator = validator;
            InitializeComponent();
        }

        /// <summary>
        /// Kunstruktor - úprava stávajícího projektu
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="viewModel"></param>
        public WorkAddProjectWindow(WorkValidator validator, VM_Project viewModel)
        {
            this.validator = validator;
            this.viewModel = viewModel;
            InitializeComponent();

            DataContext = viewModel;
        }

        /// <summary>
        /// Uložení zadaných dat * pro vytvoření nového nebo úpravu projektu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Nový projekt
                if(viewModel == null)
                    validator.AddEditProject(0, dateTextBox.Text, projectEventsTextBox.Text, nameTextBox.Text, descriptionTextBox.Text, planTimeTextBox.Text,
                                            commentsTextBox.Text, doListTextBox.Text);
                // Úprava stávajícího projektu
                else
                    validator.AddEditProject(1, dateTextBox.Text, projectEventsTextBox.Text, nameTextBox.Text, descriptionTextBox.Text, planTimeTextBox.Text,
                                            commentsTextBox.Text, doListTextBox.Text);
                correctClose = true;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        /// <summary>
        /// Obsluha, která informuje o zavírání okna - info od DEJVA txt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Ochrana před nechtěným uzavřením okna a ztrátou dat

            // Okno uzavřeno pomocí křížku
            if (!correctClose)
            {
                // Dotaz uživatele, zda si opravdu přeje uzavřít okno bez uložení
                MessageBoxResult result = MessageBox.Show("Opravdu si přejete uzavřít okno bez uložení ? ", "Pozor", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    // Tlačítko ANO u vyvolaného message boxu
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                        e.Cancel = true;
                        break;
                }
            }
            else
                e.Cancel = false;
        }
    }
}
