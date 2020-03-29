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
    /// <summary>
    /// Třída reprezentující grafickou podobu jedné transakce
    /// </summary>
    public class FinanceGraphicRecord
    {
        /// <summary>
        /// Událost třídy FinanceGraphicRecord, která se vyvolá při kliknutí na RectangleDown myší. Předá jako sender tuto třídu
        /// </summary>
        public event EventHandler RectangleEvent;
        /// <summary>
        /// Částka transakce
        /// </summary>
        public TextBlock Price { get; private set; }
        /// <summary>
        /// Datum transakce
        /// </summary>
        public TextBlock Date { get; private set; }
        /// <summary>
        /// Název transakce
        /// </summary>
        public TextBlock Title { get; private set; }
        /// <summary>
        /// Horní modrý obdelník
        /// </summary>
        public Rectangle RectangleTop { get; private set; }
        /// <summary>
        /// Spodní bílý obdelník
        /// </summary>
        public Rectangle RectangleDown { get; private set; }
        /// <summary>
        /// Levý zelený nebo červený obdelník, podle druhu transakce - PŘÍJEM x VÝDAJ
        /// </summary>
        public Rectangle RectangleLeft { get; private set; }
        /// <summary>
        /// Vztažný finanční záznam
        /// </summary>
        public FinanceRecord FinanceRecord { get; set; }

        /// <summary>
        /// Základní konstruktor - vytvoří vše potřebné pro vykreslení
        /// </summary>
        /// <param name="record"></param>
        public FinanceGraphicRecord(FinanceRecord record)
        {
            Price = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = record.Price.ToString() + "\tKč"

            };

            Date = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = record.Date.ToShortDateString()

            };

            Title = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = record.Name

            };

            RectangleTop = new Rectangle
            {
                Width = 320,
                Height = 25,
                RadiusX = 3,
                RadiusY = 3,
                Fill = new SolidColorBrush(Color.FromArgb(255, 62, 188, 250))

            };

            RectangleDown = new Rectangle
            {
                Width = 320,
                Height = 25,
                RadiusX = 5,
                RadiusY = 5,
                Fill = Brushes.White
            };

            RectangleLeft = new Rectangle
            {
                Width = 15,
                Height = 25,
                Fill = new SolidColorBrush(Color.FromArgb(200, 69, 253, 0))
            };

            if (record.TypeRecord == TypeRecord.Costs)
                RectangleLeft.Fill = new SolidColorBrush(Color.FromArgb(200, 255, 4, 4));

            FinanceRecord = record;
            RectangleDown.MouseDown += RectangleDown_MouseDown;
        }

        /// <summary>
        /// Obsluha události kliknutí na obdelník, vyvolá se událost celé této třídy, tedy FinanceGraphicRecord
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectangleDown_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectangleEvent(this,EventArgs.Empty);
        }
    }
}
