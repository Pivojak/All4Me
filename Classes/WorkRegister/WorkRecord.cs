using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All4Me
{
    /// <summary>
    /// Třída reprezentující jeden pracovní den - obsahují dvě a více bloku WORK PART
    /// </summary>
    public class WorkRecord
    {
        /// <summary>
        /// Vnitřní hodnota ID
        /// </summary>
        public static int workRecordID;
        /// <summary>
        /// Hodnota ID dané instance
        /// </summary>
        public int WorkRecordId { get; set; }
        /// <summary>
        /// Datum pracovního dne
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Plánovaná doba práce pro daný den
        /// </summary>
        public decimal PlanTime { get; set; }
        /// <summary>
        /// Pracovní 
        /// </summary>
        public decimal RealTime { get; set; }
        /// <summary>
        /// Kolekce obsahující části pracovního dne - Dopoledne a Odpoledne např.
        /// </summary>
        public List<WorkPart> WorkParts { get; set; }
        /// <summary>
        /// Efektivita - odlišnost Plánu a Reálné pracovní doby
        /// </summary>
        public bool Effectivity { get; set; }

        /// <summary>
        /// Prázdný konstruktor pro ukládání na disk
        /// </summary>
        public WorkRecord()
        {

        }

        /// <summary>
        /// Kompletní konstruktor pro vytvoření nového denního záznamu
        /// </summary>
        /// <param name="date">Datum záznamu</param>
        /// <param name="planTime">Plánovaná denní pracovní doba</param>
        public WorkRecord(DateTime date, decimal planTime )
        {
            Date = date;
            PlanTime = planTime;
            WorkParts = new List<WorkPart>();
            WorkRecordId = workRecordID;
            workRecordID++;
        }

        /// <summary>
        /// Přidání k danému dny dopolední či odpoledního pracovního blok WORK PART
        /// </summary>
        /// <param name="choice">0 - Nový záznam ** 1 - Úprava stávajícího</param>
        /// <param name="start">Začátek pracovní doby</param>
        /// <param name="stop">Konec pracovní doby</param>
        /// <param name="content">Náplň práce</param>
        /// <param name="partRecord">Záznam k upravení či výmazu</param>
        public void AddEditWorkParts(byte choice,DateTime start, DateTime stop, string content,
            WorkPart partRecord)
        {
            if(choice == 0)
            {
                // Přidání nového denního bloku, pokud již dva neobsahuje
                if(WorkParts.Count < 2)
                    WorkParts.Add(new WorkPart(start, stop, content));
                CalculateRealTime();
            }
            else if(choice == 1 && partRecord != null)
            {
                partRecord.StartHour = start;
                partRecord.EndHour = stop;
                partRecord.WorkContent = content;
                CalculateRealTime();
            }
        }

        /// <summary>
        /// Výpočet odpracované doby
        /// </summary>
        private void CalculateRealTime()
        {
            // Denní záznam obsahuje nějaký blok
            if (WorkParts.Count != 0)
            {
                TimeSpan real = new TimeSpan();
                decimal realTime = 0;
                foreach (WorkPart part in WorkParts)
                {
                    real = part.EndHour - part.StartHour;
                    realTime += (decimal)real.TotalHours;
                }
                RealTime = realTime;

                if (RealTime >= ((decimal)0.9 * PlanTime))
                {
                    Effectivity = true;
                }
            }
        }

        /// <summary>
        /// Odebere ranní či odpolední blok z deního záznamu
        /// </summary>
        /// <param name="partRecord">Záznam,který bude odebrán</param>
        public void RemoveWorkParts(WorkPart partRecord)
        {
            if (partRecord != null)
                WorkParts.Remove(partRecord);
        }

    }
}
