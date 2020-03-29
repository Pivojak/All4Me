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
    /// Úvodní okno pracovního bloku - vlevo ovládací menu a vpravo canvas pro vykreslení pracovních dnů
    /// </summary>
    public partial class WorkStartWindow : Window
    {
        /// <summary>
        /// Validátor dat pro blok práce
        /// </summary>
        private WorkValidator validator;
        /// <summary>
        /// Zda se jedná o data vybraná pomocí tlačítka Vyber či nikoliv
        /// </summary>
        private bool selectData = false;
        /// <summary>
        /// Úvodní vykreslení, tedy první strana a obsah na ní
        /// </summary>
        private bool firstStart = true;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        public WorkStartWindow()
        {
            InitializeComponent();
            validator = new WorkValidator();
            validator.LoadIDs();
            validator.LoadProjects();
            validator.DefineCanvas(informationCanvas);
            // nastavení počáteční hodnoty měsíce a roku podle aktuálního datumu
            monthComboBox.SelectedIndex = DateTime.Today.Month - 1;
            yearComboBox.SelectedIndex = DateTime.Today.Year - 2020;
            // Zviditelnění vrchních kombo boxů pro výběr měsíce a roku
            yearComboBox.Visibility = Visibility.Visible;
            monthComboBox.Visibility = Visibility.Visible;
            // Vykreslení první strany denních záznamů
            validator.ConstructGraphicResult(true,false,false, monthComboBox.SelectedIndex, yearComboBox.SelectedIndex);
            firstStart = false;
        }
        /// <summary>
        /// Přidání nového projektu nebo denního záznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            selectData = false;
            WorkAddSelectWindow window = new WorkAddSelectWindow(validator);
            window.Show();
        }

        /// <summary>
        /// Zobrazení první strany denní záznamů - úvodní stav
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewAllButton_Click(object sender, RoutedEventArgs e)
        {
            yearComboBox.Visibility = Visibility.Visible;
            monthComboBox.Visibility = Visibility.Visible;

            selectData = false;

            try
            {
                validator.ConstructGraphicResult(true, false,false,monthComboBox.SelectedIndex,yearComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Okno pro výběr, možnost měsíčních nebo týdenních bloků
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            yearComboBox.Visibility = Visibility.Hidden;
            monthComboBox.Visibility = Visibility.Hidden;
            selectData = true;
            WorkSelectWindow window = new WorkSelectWindow(validator);
            window.Show();
        }

        /// <summary>
        /// Uzavře okno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            selectData = false;
            try
            {
                MainWindow window = new MainWindow();
                window.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Vyvolá výběr projektu, pro jeho úprava a zobrazení informací v okně projektu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            selectData = false;
            WorkProjectSelect window = new WorkProjectSelect(validator);
            window.Show();
        }

        /// <summary>
        /// Otočení kolečka myši - řeší také směr, zda rotace dolů či nahoru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                bool rotateUpflag = false;
                // Delta vrací hodnotu rotace
                //      - Kolečkem dolů je hodnota -
                //      - Kolečkem nahoru je hodnota +
                if (e.Delta > 0)
                    rotateUpflag = true;

                if (selectData)
                    validator.ConstructWeekMonthOverview(true, 2, 2, false);
                else
                    validator.ConstructGraphicResult(false, true,rotateUpflag, monthComboBox.SelectedIndex,yearComboBox.SelectedIndex);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        /// <summary>
        /// Změna v comboBoxu pro rok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(!firstStart)
                    validator.ConstructGraphicResult(true, false,false, monthComboBox.SelectedIndex, yearComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Změna v comboBoxu pro měsíc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(!firstStart)
                    validator.ConstructGraphicResult(true, false,false, monthComboBox.SelectedIndex, yearComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
