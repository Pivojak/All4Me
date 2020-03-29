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
    /// Interakční logika pro NoteSelectColorWindow.xaml
    /// </summary>
    public partial class NoteSelectColorWindow : Window
    {
        /// <summary>
        /// Správce sekce poznámek
        /// </summary>
        private NoteAdmin admin;
        /// <summary>
        /// Vybraná barva pro danou stranu potažmo poznámku
        /// </summary>
        private byte selectColor;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="admin"></param>
        public NoteSelectColorWindow(NoteAdmin admin)
        {
            InitializeComponent();
            this.admin = admin;
            selectColor = 0;
        }

        /// <summary>
        ///  Kliknutí na tlačítko uložení
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(pageComboBox.SelectedIndex != -1)
            {
                // Nastavení barvy
                admin.SetPageColor(pageComboBox.SelectedIndex + 1, selectColor);
            }
                
            Close();
        }
        // Tlačítka reprezentující barvu, jež si vybral uživatel - šedá
        private void ColorGrayButton_Click(object sender, RoutedEventArgs e)
        {
            selectColor = 1;
        }
        // Červená
        private void ColorRedButton_Click(object sender, RoutedEventArgs e)
        {
            selectColor = 2;
        }
        // Modrá
        private void ColorBlueButton_Click(object sender, RoutedEventArgs e)
        {
            selectColor = 3;
        }
        // Zelená
        private void ColorGreenButton_Click(object sender, RoutedEventArgs e)
        {
            selectColor = 4;
        }
        // Žlutá
        private void ColorYellowButton_Click(object sender, RoutedEventArgs e)
        {
            selectColor = 5;
        }
    }
}
