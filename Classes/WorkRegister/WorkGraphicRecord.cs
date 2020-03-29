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
    public class WorkGraphicRecord
    {
        /// <summary>
        /// Strana na které je daný záznam vykreslen
        /// </summary>
        public int Page { get;  set; }
        /// <summary>
        /// Událost kliknutí na přidávací tlačíko
        /// </summary>
        public event EventHandler AddButtonClick;
        /// <summary>
        /// Událost kliknutí na upravovací tlačítko
        /// </summary>
        public event EventHandler EditButtonClick;
        /// <summary>
        /// Událost kliknutí na mazací tlačítko
        /// </summary>
        public event EventHandler RemoveButtonClick;
        /// <summary>
        /// Obdelník pozadí
        /// </summary>
        public Rectangle BackgroundRect { get; private set; }
        /// <summary>
        /// Čtverec název dne
        /// </summary>
        public Rectangle DayRect { get; private set; }
        /// <summary>
        /// Čtverec číslo dne
        /// </summary>
        public Rectangle DayNumberRect { get; private set; }
        /// <summary>
        /// Oddělovač 1
        /// </summary>
        public Rectangle Separator1 { get; private set; }
        /// <summary>
        /// Oddělovač 2
        /// </summary>
        public Rectangle Separator2 { get; private set; }
        /// <summary>
        /// Oddělovač 3
        /// </summary>
        public Rectangle Separator3 { get; private set; }
        /// <summary>
        /// Název dne
        /// </summary>
        public TextBlock DayText { get; private set; }
        /// <summary>
        /// Číslo dne
        /// </summary>
        public TextBlock DayNumberText { get; private set; }
        /// <summary>
        /// Odpracovaná doba NADPIS
        /// </summary>
        public TextBlock RealTime { get; private set; }
        /// <summary>
        /// Odpracovaná doba HODNOTA
        /// </summary>
        public TextBlock RealTime_Value { get; private set; }
        /// <summary>
        /// Projekt NADPIS
        /// </summary>
        public TextBlock Project { get; private set; }
        /// <summary>
        /// Projekt HODNOTA
        /// </summary>
        public TextBlock Project_Value { get; private set; }
        /// <summary>
        /// Pracovní doba od - do HODNOTA
        /// </summary>
        public TextBlock[] PartFromToText_Value { get; private set; }
        /// <summary>
        /// Pracovní doba od - do NADPIS
        /// </summary>
        public TextBlock[] PartFromToText { get; private set; }
        /// <summary>
        /// Náplň práce NADPIS
        /// </summary>
        public TextBlock[] PartWorkContent { get; private set; }
        /// <summary>
        /// Náplň práce HODNOTA
        /// </summary>
        public TextBlock[] PartWorkContent_Value { get; private set; }
        /// <summary>
        /// Tlačítko přidej
        /// </summary>
        public Button AddButton { get; private set; }
        /// <summary>
        /// Tlačítko uprav
        /// </summary>
        public Button EditButton { get; private set; }
        /// <summary>
        /// Tlačítko odeber
        /// </summary>
        public Button RemoveButton { get; private set; }
        /// <summary>
        /// Vztažný denní záznam
        /// </summary>
        public WorkRecord Record { get; private set; }

        public WorkGraphicRecord(WorkRecord record,string projectName)
        {
            this.Record = record;
            PartFromToText = new TextBlock[2];
            PartFromToText_Value = new TextBlock[2];
            PartWorkContent = new TextBlock[2];
            PartWorkContent_Value = new TextBlock[2];
            // České zkratky pro dny v týdnu pro výpis do šedého čtverce u denní události
            string[] daysOfWeekCZ = new string[] { "Po", "Út", "St", "Čt", "Pá", "So", "Ne" };
            string projName = "";
            // Formátování názvu projektu, pro případ, že by byl přiliš dlouhý
            //      -- zalomí se na nový řádek 
            //      -- maximálně dva řádky
            //      -- každý řádek maximálně 19 znaků
            if (projectName.Count() > 17)
            {
                projName = projectName.Substring(0, 17);
                projName += "-\n";
                if(projectName.Count() > 37)
                    projName += projectName.Substring(17, 19);
                else
                    projName += projectName.Substring(17, projectName.Count() - 17);
            }
            else
                projName = projectName;

            BackgroundRect = new Rectangle
            {
                Width = 665,
                Height = 125,
                RadiusX = 1,
                RadiusY = 1,
                Fill = Brushes.White,
                StrokeThickness = 1,
                Stroke = Brushes.Gray
            };

            DayRect = new Rectangle
            {
                Width = 45,
                Height = 45,
                Fill = Brushes.Gray,
            };

            DayNumberRect = new Rectangle
            {
                Width = 45,
                Height = 45,
                Fill = Brushes.Gray
            };

            Separator1 = new Rectangle
            {
                Width = 3,
                Height = 125,
                Fill = Brushes.Gray
            };
            Separator2 = new Rectangle
            {
                Width = 3,
                Height = 125,
                Fill = Brushes.Gray
            };
            Separator3 = new Rectangle
            {
                Width = 3,
                Height = 125,
                Fill = Brushes.Gray
            };

            if(record.Date.DayOfWeek != DayOfWeek.Sunday)
            {
                DayText = new TextBlock
                {
                    FontSize = 25,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.Black,
                    Text = daysOfWeekCZ[(int)record.Date.DayOfWeek - 1]
                };

            }
            else
            {
                // Jedná se o neděli -- která má hodnota v ENUM 0 nikoliv 6 jak by se čekalo !!
                DayText = new TextBlock
                {
                    FontSize = 25,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.Black,
                    Text = daysOfWeekCZ[6]
                };
            }

            DayNumberText = new TextBlock
            {
                FontSize = 25,
                FontWeight = FontWeights.ExtraBold,
                Foreground = Brushes.Black,
                Text = record.Date.Day.ToString()

            };

            AddButton = new Button
            {
                Content = "+",
                Width = 60,
                Height = 35,
                Background = Brushes.Green,
                FontSize = 25,
                FontWeight = FontWeights.ExtraBold,
                BorderBrush = Brushes.White
            };

            RemoveButton = new Button
            {
                Content = "X",
                Width = 60,
                Height = 35,
                Background = Brushes.Red,
                FontSize = 25,
                FontWeight = FontWeights.ExtraBold,
                BorderBrush = Brushes.White
            };

            EditButton = new Button
            {
                Content = "/",
                Background = Brushes.Yellow,
                Width = 60,
                Height = 35,
                FontSize = 25,
                FontWeight = FontWeights.ExtraBold,
                BorderBrush = Brushes.White
            };


            // Celkova doba
            TimeSpan realTime = new TimeSpan(0, (int)(record.RealTime * 60), 0);
            RealTime = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = "Odpracováno"

            };

            RealTime_Value = new TextBlock
            {
                FontSize = 23,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = realTime.Hours + " h " + realTime.Minutes + " min"

            };

            // Projekt
            Project = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = "Projekt"

            };

            Project_Value = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = projName

            };

            AddButton.Click += AddButton_Click;
            EditButton.Click += EditButton_Click;
            RemoveButton.Click += RemoveButton_Click;

            for (int i = 0; i < record.WorkParts.Count; i++)
            {
                ConstructWorkParts(record.WorkParts[i], i);
            }
        }

        /// <summary>
        /// Kliknutí na odebírací tlačítko a vyvolání události (RemoveButtonClick) této třídy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonClick(this.Record, EventArgs.Empty);
        }

        /// <summary>
        /// Kliknutí na upravovací tlačítko a vyvolání události (RemoveButtonClick) této třídy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditButtonClick(this.Record, EventArgs.Empty);
        }

        /// <summary>
        /// Kliknutí na přidávací tlačítko a vyvolání události (RemoveButtonClick) této třídy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddButtonClick(this.Record, EventArgs.Empty);
        }

        /// <summary>
        /// Přidá všechny vlastnosti třídy do jedné kolekce, tak aby bylo možné objekty přidat na CANVS
        /// </summary>
        /// <returns>Kolekci všech vlastností této třídy 21 prvků -- 0 - 11 * Obdelníky, separátory, tlačítka -- 12 - 20 * Denní bloky</returns>
        public List<object> ReturnAllAtributs()
        {
            List<object> result = new List<object>();
            result.Add(BackgroundRect);
            result.Add(DayRect);
            result.Add(DayNumberRect);
            result.Add(Separator1);
            result.Add(Separator2);
            result.Add(Separator3);
            result.Add(DayText);
            result.Add(DayNumberText);
            result.Add(RealTime);
            result.Add(RealTime_Value);
            result.Add(Project);
            result.Add(Project_Value);
            result.Add(AddButton);
            result.Add(EditButton);
            result.Add(RemoveButton);

            result.Add(PartFromToText_Value[0]);
            result.Add(PartFromToText[0]);
            result.Add(PartWorkContent[0]);
            result.Add(PartWorkContent_Value[0]);

            result.Add(PartFromToText_Value[1]);
            result.Add(PartFromToText[1]);
            result.Add(PartWorkContent[1]);
            result.Add(PartWorkContent_Value[1]);

            return result;
        }

        /// <summary>
        /// Metoda vytvoří grafickou formu pro každý blok v denním záznamu (Ráno a odpoledne)
        /// </summary>
        /// <param name="record"></param>
        /// <param name="index"></param>
        private void ConstructWorkParts(WorkPart record, int index)
        {

            PartFromToText_Value[index] = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = record.StartHour.ToShortTimeString() + " - " + record.EndHour.ToShortTimeString()

            };

            PartFromToText[index] = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = "Pracovní doba"

            };

            PartWorkContent[index] = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = "Náplň práce"

            };

            // Počet znaků celkem v textu
            int charCount = 0;
            // Skutečný počet znaků - protože řádek končí např po 10 znacích
            int charCount_real = 0;
            // Počet znaků na řádku
            int charRowCount = 0;
            // Počet řádků
            int rowCount = 0;

            string text = record.WorkContent;
            // Prochází znaky daného textu
            foreach (char c in text)
            {
                // Nalezen znak pro zalomení na další řádek
                if (c == '\n')
                {
                    rowCount++;
                    // Skutečný počet znaků celkem - řádek končí např po 10 znacích
                    charCount_real += charRowCount;
                    charRowCount = 0;
                }

                if (charRowCount == 25)
                {
                    rowCount++;
                    charCount_real += charRowCount;
                    // Délka textu na řádku je 34 znaků - MAXIMUM pro řádek, proto se
                    // text zalomí 
                    text = text.Insert(charCount_real, "\n");

                    charRowCount = 0;
                }

                if (rowCount == 2)// || charCount >= 55)
                {
                    // Omezení celkového počtu znaků na blok poznámky - nebo řádků
                    text = text.Substring(0, charCount);
                    break;
                }
                charCount++;
                charRowCount++;
            }

            PartWorkContent_Value[index] = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.ExtraLight,
                Foreground = Brushes.Black,
                Text = text

            };
        }
    }
}
