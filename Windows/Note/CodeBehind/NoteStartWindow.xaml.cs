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
    /// Interakční logika pro NoteStartWindow.xaml
    /// </summary>
    public partial class NoteStartWindow : Window
    {
        /// <summary>
        /// Správce poznámkového bloku
        /// </summary>
        private NoteAdmin admin;
        /// <summary>
        /// Vybraná strana - úvodní
        /// </summary>
        private byte selectPage = 1;
        /// <summary>
        /// Změna strany událost - kliknutí na jedno z 5 tlačítek na horní straně pro změnu strany
        /// </summary>
        private event EventHandler PageChange;

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        public NoteStartWindow()
        {
            InitializeComponent();
            // Vytvoření jediné instance správce pro tuto sekci
            admin = new NoteAdmin(informationCanvas);
            page1Button.Background = Brushes.Black;
            // Načtení Identifikátorů a samotných poznámek
            admin.LoadId_Color();
            admin.LoadRecord();
            admin.ConstructGraphicRecord(selectPage);
            // Kliknutí na poznámku - přidání obsluhy do této události
            admin.GraphicRecord_RectangleButtonClick += Admin_GraphicRecord_RectangleButtonClick;
            // Přidání obsluhy události změny strany
            PageChange += PageChangeSelect;

        }
        /// <summary>
        /// Obsluha kliknutí na oblast poznámky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_GraphicRecord_RectangleButtonClick(object sender, EventArgs e)
        {
            NoteAddWindow window = new NoteAddWindow(admin, sender as NoteRecord,selectPage);
            window.Show();
        }

        /// <summary>
        /// Kliknutí na ukládací tlačítko
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            admin.SaveId_Color();
            admin.SaveRecord();
            
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        /// <summary>
        /// Tlačítko přidání
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NoteAddWindow window = new NoteAddWindow(admin,null, selectPage);
            window.Show();
        }

        /// <summary>
        /// Tlačítko 1 strany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page1Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeBackgroundButtons();
            (sender as Button).Background = Brushes.Black;
            selectPage = 1;
            PageChange(null, EventArgs.Empty);
        }
        /// <summary>
        /// Tlačítko 2 strany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page2Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeBackgroundButtons();
            (sender as Button).Background = Brushes.Black;
            selectPage = 2;
            PageChange(null, EventArgs.Empty);
        }

        /// <summary>
        /// Tlačítko 3 strany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page3Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeBackgroundButtons();
            (sender as Button).Background = Brushes.Black;
            selectPage = 3;
            PageChange(null, EventArgs.Empty);
        }

        /// <summary>
        /// Tlačítko 4 strany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page4Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeBackgroundButtons();
            (sender as Button).Background = Brushes.Black;
            selectPage = 4;
            PageChange(null, EventArgs.Empty);
        }

        /// <summary>
        /// Tlačítko páté strany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page5Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeBackgroundButtons();
            (sender as Button).Background = Brushes.Black;
            selectPage = 5;
            PageChange(null, EventArgs.Empty);
        }

        /// <summary>
        /// Změna strany - uživatel kliknul na nějaké tlačítko strany
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageChangeSelect(object sender, EventArgs e)
        {
            admin.ConstructGraphicRecord(selectPage);
        }
        /// <summary>
        /// Změna barvy pozadí poznámky
        /// </summary>
        private void ChangeBackgroundButtons()
        {
            page1Button.Background = new SolidColorBrush(Color.FromArgb(255, 20, 82, 87));
            page2Button.Background = new SolidColorBrush(Color.FromArgb(255, 20, 82, 87));
            page3Button.Background = new SolidColorBrush(Color.FromArgb(255, 20, 82, 87));
            page4Button.Background = new SolidColorBrush(Color.FromArgb(255, 20, 82, 87));
            page5Button.Background = new SolidColorBrush(Color.FromArgb(255, 20, 82, 87));
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            NoteSelectColorWindow window = new NoteSelectColorWindow(admin);
            window.Show();
        }
    }
}
