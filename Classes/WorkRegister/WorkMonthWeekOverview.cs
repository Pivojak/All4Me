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
    /// Grafická reprezentace měsíčního či týdenního bloku pro pracovní statisku práce za ROK či MĚSÍC
    /// </summary>
    public class WorkMonthWeekOverview
    {
        /// <summary>
        /// Strana na kterou bude vykreslen
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Obdelník pozadí
        /// </summary>
        public Rectangle BackgroundRect { get; private set; }
        /// <summary>
        /// Čtverec název měsíce nebo datum týdne
        /// </summary>
        public Rectangle MonthRect { get; private set; }
        /// <summary>
        /// Název měsíce nebo datum týdne
        /// </summary>
        public TextBlock MonthText { get; private set; }
        /// <summary>
        /// Číslo roku nebo název měsíce
        /// </summary>
        public TextBlock YearText { get; private set; }
        /// <summary>
        /// Odpracování dní
        /// </summary>
        public TextBlock WorkDays { get; private set; }
        /// <summary>
        /// Průměrná denní pracovní doba
        /// </summary>
        public TextBlock AverageWorkTime { get; private set; }
        /// <summary>
        /// Plánovano hodin
        /// </summary>
        public TextBlock PlanWorkTime { get; private set; }
        /// <summary>
        /// Odpracování dní
        /// </summary>
        public TextBlock WorkDays_Value { get; private set; }
        /// <summary>
        /// Odpracováno hodin
        /// </summary>
        public TextBlock WorkHours_Value { get; private set; }
        /// <summary>
        /// Průměrná denní pracovní doba
        /// </summary>
        public TextBlock AverageWorkTime_Value { get; private set; }
        /// <summary>
        /// Plánovano hodin
        /// </summary>
        public TextBlock PlanWorkTime_Value { get; private set; }
        
        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="workDays">Počet pracovních dní</param>
        /// <param name="workHours">Počet pracovních hodin</param>
        /// <param name="planWorkHours">Plánovaný počet pracovních hodin</param>
        /// <param name="averageWorkTime">Průměrná denní pracovní doba</param>
        /// <param name="monthWeek">Název měsíce nebo týden od - do</param>
        /// <param name="yearMonth">Název roku nebo měsíce</param>
        public WorkMonthWeekOverview(int workDays, int workHours, int planWorkHours, TimeSpan averageWorkTime, string monthWeek, string yearMonth)
        {
            // Pozadí
            BackgroundRect = new Rectangle
            {
                Width = 300,
                Height = 130,
                RadiusX = 1,
                RadiusY = 1,
                Fill = Brushes.White,
                StrokeThickness = 1,
                Stroke = Brushes.Gray
            };
            // Název měsíce pozadí
            MonthRect = new Rectangle
            {
                Width = 300,
                Height = 3,
                Fill = Brushes.Gray,
            };
            
            // Název měsíce text
            MonthText = new TextBlock
            {
                FontSize = 21,
                FontWeight = FontWeights.ExtraBold,
                Foreground = Brushes.Black,
                Text = monthWeek

            };
            // Název roku - text
            YearText = new TextBlock
            {
                FontSize = 21,
                FontWeight = FontWeights.ExtraBold,
                Foreground = Brushes.Black,
                Text = yearMonth

            };
            // Pracovních dní
            WorkDays = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = "Odpracováno"

            };

            AverageWorkTime = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = "Průměr" 

            };


            PlanWorkTime = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Text = "Plán"

            };

            WorkDays_Value = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = workDays.ToString() + " dní"

            };

            WorkHours_Value = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = workHours.ToString() + " hod"

            };

            AverageWorkTime_Value = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = averageWorkTime.Hours + " hod " + averageWorkTime.Minutes + " min"

            };


            PlanWorkTime_Value = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.DemiBold,
                Foreground = Brushes.Black,
                Text = planWorkHours + " hod"

            };
        }

        /// <summary>
        /// Přidá všechny vlastnosti třídy do jedné kolekce, tak aby bylo možné objekty přidat na CANVS
        /// </summary>
        /// <returns>Kolekci všech vlastností této třídy 21 prvků -- 0 - 11 * Obdelníky, separátory, tlačítka -- 12 - 20 * Denní bloky</returns>
        public List<object> ReturnAllAtributs()
        {
            List<object> result = new List<object>();
            result.Add(BackgroundRect);
            result.Add(MonthRect);

            result.Add(MonthText);
            result.Add(YearText);
            
            result.Add(WorkDays);
            result.Add(AverageWorkTime);
            result.Add(PlanWorkTime);

            result.Add(WorkDays_Value);
            result.Add(WorkHours_Value);
            result.Add(AverageWorkTime_Value);
            result.Add(PlanWorkTime_Value);

            return result;
        }

    }
}

