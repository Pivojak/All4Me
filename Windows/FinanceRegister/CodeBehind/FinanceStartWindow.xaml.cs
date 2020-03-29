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
    /// Interakční logika pro FinanceStartWindow.xaml
    /// </summary>
    public partial class FinanceStartWindow : Window
    {
        /// <summary>
        /// Finanční validátor
        /// </summary>
        private Validator validator;
        /// <summary>
        /// Flag - vykreslení dat statistiky
        /// </summary>
        private bool otherData = false;
        /// <summary>
        /// Flag - vykreslení grafu výdajů v kategoriích
        /// </summary>
        private bool categoryFlag = false;
        /// <summary>
        /// Flag - vykreslovaná data jsou ze sekce výběru, tedy filtrovaná
        /// </summary>
        private bool filterData = false;
        /// <summary>
        /// Flag - druhá strana
        /// </summary>
        private bool nextPage = false;
        /// <summary>
        /// Flag - vykreslení roční statistiky
        /// </summary>
        private bool yearFlag = false;
        /// <summary>
        /// Flag - statistika měsíce nebo roku
        /// </summary>
        private bool yearMonthStatisticFlag = false;

        /// <summary>
        /// Základní konstrukto
        /// </summary>
        /// <param name="validat">Finanční validátor</param>
        public FinanceStartWindow(Validator validat)
        {
            InitializeComponent();
            validator = validat;
            validator.DefineCanvas(informationCanvas);
            // Přidání obsluhy události ve formě metody RectangleButton_Click
            validator.RectangleButtonClick += RectangleButton_Click;
            // Přidání obsluhy události z validátoru
            validator.ViewSelectData += Validator_ViewSelectData;
            // Nastavení počáteční hodnoty horních comboBoxů pro výběr roku a měsíce
            yearComboBox.SelectedItem = DateTime.Now.Year;
            monthComboBox.SelectedIndex = DateTime.Now.Month - 1;
        }

        /// <summary>
        /// Obsluha události, která reaguje na vyvolanou událost ve VALIDATORU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectangleButton_Click(object sender, EventArgs e)
        {
            FinanceAddWindow okno = new FinanceAddWindow(validator, sender as FinanceGraphicRecord ,informationCanvas);
            okno.Show();
        }

        /// <summary>
        /// Vstup do sekce statistiky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticButton_Click(object sender, RoutedEventArgs e)
        {
            yearMonthStatisticFlag = false;
            categoryFlag = false;
            otherData = true;
            VisibilityMonthYearComboBox(false, 0);
            Width = 550;
            // Vytvoření ,,menu,, na canvasu, kde uživatel vybere jakou část statistiky by si přál. 
            Rectangle topRect = new Rectangle
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 85, 169, 176)),
                Height = 750,
                Width = 500,

            };
            // Tlačítko pro zobrazení sloupcového grafu
            Button categoryButton = new Button
            {
                Name = "categoryButton",
                Content = "Kategorie",
                Background = new SolidColorBrush(Color.FromArgb(255, 20, 82, 87)),
                Foreground = Brushes.White,
                Width = 140,
                Height = 70,
                FontSize = 20,
                FontWeight = FontWeights.Bold 
            };
            // Tlačítko pro zobrazení statistiky za rok
            Button yearButton = new Button
            {
                Name = "yearButton",
                Content = "Rok",
                Background = new SolidColorBrush(Color.FromArgb(255, 20, 82, 87)),
                Foreground = Brushes.White,
                Width = 140,
                Height = 70,
                FontSize = 20,
                FontWeight = FontWeights.Bold
            };
            // Tlačítko pro zobrazení statistiky za měsíc
            Button monthButton = new Button
            {
                Name = "monthButton",
                Content = "Měsíc",
                Background = new SolidColorBrush(Color.FromArgb(255, 20, 82, 87)),
                Foreground = Brushes.White,
                Width = 140,
                Height = 70,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
            };
            // Přidání obsluhy pro události kliknutí na daná tlačítka
            categoryButton.Click += CategoryButton_Click1;
            monthButton.Click += MonthButton_Click;
            yearButton.Click += YearButton_Click;
            // Smazání plátna
            informationCanvas.Children.Clear();

            informationCanvas.Children.Add(topRect);
            Canvas.SetTop(topRect, 0); Canvas.SetLeft(topRect, 0);
            // Přidání objektů na CANVAS
            informationCanvas.Children.Add(categoryButton);
            informationCanvas.Children.Add(yearButton);
            informationCanvas.Children.Add(monthButton);
            // Zarovnání na canvas
            Canvas.SetBottom(categoryButton, 165);  Canvas.SetLeft(categoryButton, 10);
            Canvas.SetBottom(yearButton, 240); Canvas.SetLeft(yearButton, 10);
            Canvas.SetBottom(monthButton, 315); Canvas.SetLeft(monthButton, 10);
        }
        /// <summary>
        /// Vstup do sekce roční statistiky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearButton_Click(object sender, RoutedEventArgs e)
        {
            yearMonthStatisticFlag = true;
            categoryFlag = false;
            otherData = true;
            yearFlag = true;
            yearComboBox.ItemsSource = validator.Years;
            // Zobrazení hodního comboBoxu pro výběr roku
            VisibilityMonthYearComboBox(true, 2);
            try
            {
                Width = 650;
                validator.CalculateStatisticParametres(0, yearComboBox.SelectedItem.ToString(), 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Vstup do sekce měsíčních výdajů
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            yearMonthStatisticFlag = true;
            categoryFlag = false;
            otherData = true;
            yearFlag = false;
            yearComboBox.ItemsSource = validator.Years;
            // Výběr měsíce a roku na horní straně okna
            VisibilityMonthYearComboBox(true, 1);
            try
            {
                Width = 650;
                validator.CalculateStatisticParametres(monthComboBox.SelectedIndex, yearComboBox.SelectedItem.ToString(), 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Vstup do sekce výdajů dle kategorií - zobrazeno pomocí grafu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryButton_Click1(object sender, RoutedEventArgs e)
        {
            yearComboBox.ItemsSource = validator.Years;
            yearMonthStatisticFlag = false;
            categoryFlag = true;
            otherData = true;
            // Výběr roku a měsíce na horní liště okna
            VisibilityMonthYearComboBox(true,1);
            try
            {
                Width = 820;
                validator.ConstructChartColumn(monthComboBox.SelectedIndex,yearComboBox.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Přidání nové transakce - tlačítko PŘIDEJ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            yearMonthStatisticFlag = false;
            categoryFlag = false;
            otherData = false;
            // Schování horních comboBoxů pro výběr měsíce a roku
            VisibilityMonthYearComboBox(false, 0);
            Width = 550;
            FinanceAddWindow okno = new FinanceAddWindow(validator,informationCanvas);
            okno.Show();
        }

        /// <summary>
        /// Uložení všech dat a uzavření okna - tlačítko ULOŽIT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            yearMonthStatisticFlag = false;
            categoryFlag = false;
            otherData = false;
            Width = 550;
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        /// <summary>
        /// Výběr formy výstupu, tisk ve formě pro Word či Excel. Zobrazí se nové okno kde si uživatel vybere - tlačítko TISK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            VisibilityMonthYearComboBox(false, 0);
            yearMonthStatisticFlag = false;
            categoryFlag = false;
            otherData = false;
            Width = 550;
            FinancePrintSelectWindow window = new FinancePrintSelectWindow(validator);
            window.Show();
        }
        /// <summary>
        /// Rotace kolečkem myší v okně financí
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                // Jedná se o data transakcí - základní vykreslení
                if (!otherData)
                {
                    // Uživatel je na první straně transakcí
                    if (!nextPage)
                    {
                        // Vybraná data - pomocí tlačítka VYBER
                        if (filterData)
                            validator.ViewGraphicFinance(3, false, true);
                        else
                            validator.ViewGraphicFinance(0, false, true);
                        nextPage = true;
                    }
                    // Uživatel je na druhé straně transakcí
                    else
                    {
                        if (filterData)
                            validator.ViewGraphicFinance(3, true, false);
                        else
                            validator.ViewGraphicFinance(0, true, false);
                        nextPage = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }

        /// <summary>
        /// Obsluha události z validátoru, která iformuje o tom, že jsou zobrazována filtrovaná data - nastaví se FLAG, který to oznamuje dalším částem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Validator_ViewSelectData(object sender, EventArgs e)
        {
            filterData = true;
        }


        /// <summary>
        /// Uživateli se zobrazí okno pro výběr dat, může si vybrat dle času, kategorie, příjmy či výdaje apod - Tlačítko VYBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            yearMonthStatisticFlag = false;
            categoryFlag = false;
            otherData = false;
            VisibilityMonthYearComboBox(false, 0);

            Width = 550;
            FinanceSelectWindow window = new FinanceSelectWindow(informationCanvas,validator);

            window.Show();
        }

        /// <summary>
        /// Zobrazí se původní data, tedy tranaskce od nejnovější po starší - tlačítko zobraz vše
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewAllButton_Click(object sender, RoutedEventArgs e)
        {
            yearMonthStatisticFlag = false;
            categoryFlag = false;
            VisibilityMonthYearComboBox(false, 0);
            otherData = false;
            Width = 550;
            filterData = false;

            try
            {
                validator.ViewGraphicFinance(0, true, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Změna vybrané hodnoty v měsíčním ComboBoxu na horní straně okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Jedná se o data pro měsíční či roční statistiku
                if (yearMonthStatisticFlag)
                {
                    validator.CalculateStatisticParametres(monthComboBox.SelectedIndex, yearComboBox.SelectedItem.ToString(), 0);
                }
                // Jedná se o data pro sloupcový graf
                else if (categoryFlag)
                {
                    validator.ConstructChartColumn(monthComboBox.SelectedIndex, yearComboBox.SelectedItem.ToString());
                }
                    
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Změna vybrané hodnoty v ročním ComboBoxu na horní straně okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Jedná se o data pro měsíční či roční statistiku
                if (yearMonthStatisticFlag)
                {
                    if (yearFlag)
                    {
                        validator.CalculateStatisticParametres(0, yearComboBox.SelectedItem.ToString(), 1);
                    }
                    else if (!yearFlag)
                    {
                        validator.CalculateStatisticParametres(monthComboBox.SelectedIndex, yearComboBox.SelectedItem.ToString(), 0);
                    }
                }
                // Jedná se o data pro kategorii
                else if (categoryFlag)
                { 
                    validator.ConstructChartColumn(monthComboBox.SelectedIndex, yearComboBox.SelectedItem.ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Schová horní ovládací panel obsahující výběr roku a měsíce, při volbě statistiky
        /// </summary>
        /// <param name="show">Zobrazit - TRUE ** Nezobrazit - FALSE</param>
        /// <param name="type">0 - Pouze změna visibility ** 1 - Volba pro statistiku, nastaví také vybraný měsíc a rok, podle akt. datumu ** 2 - Zobrazi a nastavi pouze ROK</param>
        private void VisibilityMonthYearComboBox(bool show, byte type)
        {
            // Zobrazení comboBoxů
            if (show)
            {
                otherData = true;
                yearComboBox.Visibility = Visibility.Visible;
                if (type != 2)
                {
                    monthComboBox.Visibility = Visibility.Visible;
                    monthDescription.Visibility = Visibility.Visible;
                }
                yearDescription.Visibility = Visibility.Visible;
            }
            // Schování comboBoxů
            else
            {
                yearComboBox.Visibility = Visibility.Hidden;
                monthComboBox.Visibility = Visibility.Hidden;
                monthDescription.Visibility = Visibility.Hidden;
                yearDescription.Visibility = Visibility.Hidden;
            }
            // Rok i měsíc viditelný
            if (type == 1)
            {
                yearComboBox.SelectedItem = DateTime.Now.Year;
                monthComboBox.SelectedIndex = DateTime.Now.Month - 1;
            }
            // Pouze výběr roku
            else if (type == 2)
                yearComboBox.SelectedItem = DateTime.Now.Year;
        }

    }
}
