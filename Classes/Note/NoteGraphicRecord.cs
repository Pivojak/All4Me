using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace All4Me
{
    public class NoteGraphicRecord
    {
        /// <summary>
        /// Kliknutí na obdelník nebo na text poznámky. Vyvolá tuto událost, která o tom informuje třídy výše
        /// </summary>
        public event EventHandler RectangleButtonClick;
        /// <summary>
        /// Název poznámky
        /// </summary>
        public TextBlock Name { get; private set; }
        /// <summary>
        /// Samotný text poznámky
        /// </summary>
        public TextBlock Text { get; private set; }
        /// <summary>
        /// Datum vytvoření poznámky
        /// </summary>
        public TextBlock Date { get; private set; }
        /// <summary>
        /// Obdelník pokrývající celou plochu poznámky
        /// </summary>
        public Rectangle RectangleDown { get; private set; }
        /// <summary>
        /// Odelník hlavičky události
        /// </summary>
        public Rectangle RectangleTop { get; private set; }
        /// <summary>
        /// Barva konkrétní poznámky
        /// </summary>
        public Brushes EditableColor { get; private set; }
        /// <summary>
        /// Samotná uložená poznámka v paměti - reference na ni
        /// </summary>
        public NoteRecord Record { get; private set; }

        /// <summary>
        /// Vytvoří grafickou podobu poznámky
        /// </summary>
        /// <param name="record">Poznámka, která má být graficky zpracována</param>
        /// <param name="topColor">0 - Světle modrá, 1 - Šedá, 2 - Červená, 3 - Modrá, 4 - Zelená, 5 - Žlutá</param>
        public NoteGraphicRecord(NoteRecord record, byte topColor)
        {
            Brush topBrush = new SolidColorBrush(Color.FromArgb(255, 62, 188, 250)); ;
            Brush downBrush = Brushes.White;
            switch (topColor)
            {
                case 1:
                    // Šedá
                    topBrush = new SolidColorBrush(Color.FromArgb(220, 196, 185, 185));
                    break;
                case 2:
                    // Červená
                    topBrush = new SolidColorBrush(Color.FromArgb(220, 255, 0, 0));
                    break;
                case 3:
                    // Modrá
                    topBrush = new SolidColorBrush(Color.FromArgb(220, 64, 64, 210));
                    break;
                case 4:
                    // Zelená
                    topBrush = new SolidColorBrush(Color.FromArgb(220, 34, 139, 34));
                    break;
                case 5:
                    // Žlutá
                    topBrush = new SolidColorBrush(Color.FromArgb(220, 206, 255, 108));
                    break;
                default:
                    // Světle modrá
                    topBrush = new SolidColorBrush(Color.FromArgb(220, 62, 188, 250));
                    break;
            }

            Name = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = record.Name

            };

            Text = new TextBlock
            {
                FontSize = 13,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = record.Text

            };

            Date = new TextBlock
            {
                FontSize = 12,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = record.Date.ToShortDateString()

            };

            RectangleTop = new Rectangle
            {
                Width = 250,
                Height = 30,
                RadiusX = 5,
                RadiusY = 5,
                Fill = topBrush

            };

            RectangleDown = new Rectangle
            {
                Width = 250,
                Height = 250,
                RadiusX = 8,
                RadiusY = 8,
                Fill = downBrush,
                StrokeThickness = 2,
                Stroke = Brushes.Gray,
            };
            RectangleDown.MouseLeftButtonDown += RectangleDown_MouseLeftButtonDown;
            Text.MouseLeftButtonDown += Text_MouseLeftButtonDown;
            Record = record;
        }
        /// <summary>
        /// Kliknutí na text v poznámce - je to protože, text může zabírat značnou část poznámky a pak nelze snadno kliknout na ondelník pod tím
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Text_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectangleButtonClick(Record, EventArgs.Empty);
        }
        /// <summary>
        /// Kliknutí na obdelník poznámky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectangleDown_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectangleButtonClick(Record, EventArgs.Empty);
        }
    }
}
