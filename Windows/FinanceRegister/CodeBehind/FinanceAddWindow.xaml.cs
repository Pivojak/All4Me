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
    /// Interakční logika pro FinancePridejWindow.xaml
    /// </summary>
    public partial class FinanceAddWindow : Window
    {
        /// <summary>
        /// Finanční validátor
        /// </summary>
        private Validator validator;
        /// <summary>
        /// Plátno se kterým se pracuje 
        /// </summary>
        private Canvas canvas;
        /// <summary>
        /// Grafický záznam
        /// </summary>
        private FinanceGraphicRecord graphicRecord;
        //
        private bool select = false;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="validat">Finanční validátor</param>
        /// <param name="canvas">Plátno, kde jsou vykresleny transakce</param>
        public FinanceAddWindow(Validator validat,Canvas canvas)
        {
            InitializeComponent();
            validator = validat;
            DataContext = validator.TodayDate;
            this.canvas = canvas;
            select = true;
        }

        /// <summary>
        /// Rozšířený konstruktor - úprava stávajícího záznamu
        /// </summary>
        /// <param name="validat">Finanční validátor</param>
        /// <param name="graphicRecord">Grafický záznam</param>
        /// <param name="canvas">Plátno s transakcemi</param>
        public FinanceAddWindow(Validator validat, FinanceGraphicRecord graphicRecord,Canvas canvas)
        {
            InitializeComponent();
            validator = validat;
            this.canvas = canvas;
            this.graphicRecord = graphicRecord;
            DataContext = graphicRecord.FinanceRecord;
            dateTextBox.DataContext = this.graphicRecord.FinanceRecord.Date;
            int[] ids = new int[3];
            try
            {
                // Získání IDs - intů z enumů
                ids = validator.EnumValidatorReverse(graphicRecord.FinanceRecord.TypeRecord, graphicRecord.FinanceRecord.Category, graphicRecord.FinanceRecord.TypeBalance);
            }
            catch
            {

            }
            // Nastavní počáteční hodnoty ComboBoxu, tedy těch hodnot, které měněný záznam obsahuje
            typeComboBox.SelectedIndex = ids[1];
            if(ids[1] != 1)
                categoryComboBox.SelectedIndex = ids[0];
            balanceComboBox.SelectedIndex = ids[2];
        }

        /// <summary>
        /// Tlačítko uložení - uložení dat ** Nový finanční záznam OR Úprava stávajícího
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Vytvoření nového záznamu
            if (graphicRecord == null)
            {
                try
                {
                    // Validace vybraných Id na výčtové typy
                    (TypeRecord type, Category selectCategory, TypeBalance balance, Month month) = validator.EnumValidator(categoryComboBox.SelectedIndex, typeComboBox.SelectedIndex, balanceComboBox.SelectedIndex,12);
                    // Přidání nového záznamu
                    validator.EnterFinanceRecord(priceTextBox.Text, nameTextBox.Text, dateTextBox.Text, descriptionTextBox.Text, placeTextBox.Text, type, selectCategory
                                           , null, balance);
                    // Aktualizace transakcí na plátně, aby zobrazovali i přidanou
                    validator.ViewGraphicFinance(0,true,false);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            // Úprava stávajícího záznamu
            else if(graphicRecord != null)
            {
                try
                {
                    // Úprava stávajícího záznamu
                    (TypeRecord type, Category selectCategory, TypeBalance balance, Month month) = validator.EnumValidator(categoryComboBox.SelectedIndex, typeComboBox.SelectedIndex, balanceComboBox.SelectedIndex,12);
                    validator.EnterFinanceRecord(priceTextBox.Text, nameTextBox.Text, dateTextBox.Text, descriptionTextBox.Text, placeTextBox.Text, type, selectCategory
                                           , graphicRecord.FinanceRecord, balance);
                    // Aktualizace plátna s transakcemi, aby reflektovalo provedené změny
                    validator.ViewGraphicFinance(0,true,false);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }



        }

        /// <summary>
        /// Změna v ComboBoxu pro výběr typu transakce - PŘÍJEM, VÝDEJ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (select)
            {
                if (typeComboBox.SelectedIndex == 1)
                    categoryComboBox.IsEnabled = false;
                else
                    categoryComboBox.IsEnabled = true;
            }
        }

        /// <summary>
        /// Tlačítko pro odebrání záznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (graphicRecord != null)
            {
                try
                {
                    validator.RemoveFinanceRecord(graphicRecord.FinanceRecord);
                    validator.ViewGraphicFinance(0,true,false);
                    Close();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
