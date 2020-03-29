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
    /// Třída pro vytváření struktur na plátno, například modro - bíle bloky apod. pro účely této aplikace. Vytvoří požadovaný vzhled a vrátí požadované prvky pro
    /// umístění na CANVAS
    /// </summary>
    public static class Graphic
    {
        #region FinanceMetods
        /// <summary>
        /// Generuje grafický řádek pro vypsání údajů - Modrý blok vlevo + bílý blok vpravo + Název vlevo + Částka vpravo
        /// </summary>
        /// <param name="name">Nízev operace či transakce</param>
        /// <param name="price">Částka - bílý obdelník</param>
        /// <param name="width">Šířky obdelníků</param>
        /// <param name="height">Výšky obdelníků</param>
        /// <param name="radiusXY">Zaoblení obdelníků</param>
        /// <returns>TRUPLE value</returns>
        public static (Rectangle blue, Rectangle white, TextBlock name, TextBlock price) BlueWhitePanel(string name, string price, int[] width, int[] height, int[] radiusXY)
        {
            // Modrý pruh vlevo
            Rectangle rectangleLeft = new Rectangle
            {
                Width = width[0],
                Height = height[0],
                RadiusX = radiusXY[0],
                RadiusY = radiusXY[0],
                Fill = new SolidColorBrush(Color.FromArgb(255, 62, 188, 250))

            };
            // Bílý pruh vpravo
            Rectangle rectangleRight = new Rectangle
            {
                Width = width[0],
                Height = height[1],
                RadiusX = radiusXY[1],
                RadiusY = radiusXY[1],
                Fill = Brushes.White
            };
            // Název balancu - Bankovní účet nebo hotovost v tomto případě. Umísteno vlevo na modrém pruhu
            TextBlock typeBalance = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = name
            };
            // Číselná hodnota danému balancu, umístěna vpravo na bílém bloku
            TextBlock balance = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = price + "\tKč"
            };
            return (rectangleLeft, rectangleRight, typeBalance, balance);
        }

        /// <summary>
        /// Metoda pro vykreslení vyskakovacího okna při najetí na kategorii Výdajů 
        /// </summary>
        /// <param name="categoryName">Název kategorie</param>
        /// <param name="radiusXY">Zaoblení bílého a modrého obdleníku</param>
        /// <param name="windowBlueRectangleHeight">Výška modrého obdelníku</param>
        /// <param name="windowHeights">Výška okna - celého</param>
        /// <param name="windowWide">Šířka okna</param>
        /// <returns>Dva obdelníky - okno a horní modrá lišta, TextBlock s popisem</returns>
        public static (Rectangle menu, Rectangle topRectangle, TextBlock desc) CategoryChartInfoWindow
            (int windowHeights, int windowBlueRectangleHeight, int windowWide, int radiusXY, string categoryName)
        {
            // Plocha bubliny - bílá
            Rectangle menu = new Rectangle
            {
                Height = windowHeights,
                Width = windowWide,
                Fill = Brushes.White,
                RadiusX = radiusXY,
                RadiusY = radiusXY
            };
            // Horní obdelník
            Rectangle topRectangle = new Rectangle
            {
                Width = windowWide,
                Height = windowBlueRectangleHeight,
                RadiusX = radiusXY,
                RadiusY = radiusXY,
                Fill = new SolidColorBrush(Color.FromArgb(255, 62, 188, 250))
            };
            // Nadpis
            TextBlock desc = new TextBlock
            {
                Text = "Přehled výdajů v kategorii - " + categoryName,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            return (menu, topRectangle, desc);
        }

        /// <summary>
        /// Vypsání hodnoty finančího záznamu do řádku ve formátu - Název / Částka / Datum
        /// </summary>
        /// <param name="fontSize">Velikost písma</param>
        /// <param name="name">Název záznamu</param>
        /// <param name="price">Částka záznamu</param>
        /// <param name="date">Datum záznamu</param>
        /// <returns></returns>
        public static (TextBlock recordName, TextBlock recordPrice, TextBlock recordDate) FinanceRecordRowWrite
            (int fontSize,string name, string price, string date)
        {
            TextBlock recordName = new TextBlock
            {
                Text = name,
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            TextBlock recordPrice = new TextBlock
            {
                Text = price + " Kč",
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            TextBlock recordDate = new TextBlock
            {
                Text = date,
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            return (recordName, recordPrice, recordDate);
        }

        /// <summary>
        /// Rozdělovací blok u statistiky financí - Měsíční a roční statistika
        /// </summary>
        /// <param name="height">Výška toho obdelníku</param>
        /// <param name="width">Šířka</param>
        /// <param name="radiusXY">Zaoblení obdelníků</param>
        /// <param name="color">Parma obdelníku</param>
        /// <param name="text">Popis</param>
        /// <returns></returns>
        public static (Rectangle rectangle, TextBlock description) SplitPanel(int height, int width, int radiusXY, Brush color,string text)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                RadiusX = radiusXY,
                RadiusY = radiusXY,
                Fill = color
            };

            TextBlock description = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Text = text

            };
            return (rectangle, description);
        }
    }
    #endregion
}
