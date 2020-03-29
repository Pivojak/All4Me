using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace All4Me
{
    /// <summary>
    /// Pracovní projekt, který ukládá Denní záznamy do své kolekce
    /// </summary>
    public class WorkProject
    {
        /// <summary>
        /// Statická proměnná uchovájící identifikátor následujícího projektu
        /// </summary>
        public static int workProjectID;
        /// <summary>
        /// Jedinečný identifikátor projektu
        /// </summary>
        public int WorkProjectId { get; set; }
        /// <summary>
        /// Název projektu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Podrobný popis projektu
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Plánovaný čas na projekt
        /// </summary>
        public decimal PlanTime { get; set; }
        /// <summary>
        /// Skutečná doba trvání projektu
        /// </summary>
        public decimal RealTime { get; set; }
        /// <summary>
        /// Kolekce ukládající denní záznamy práce
        /// </summary>
        public List<WorkRecord> Records { get; set; }
        /// <summary>
        /// Barva daného projektu, pro snadnější orientaci ve výpisu
        /// </summary>
        public Brush ProjectColor { get; set; }
        /// <summary>
        /// Jednotlivé části projektu - Finance, Poznámkový blok apod
        /// </summary>
        public List<string> ProjectEvents { get; set; }
        /// <summary>
        /// Počáteční den projektu
        /// </summary>
        public DateTime StartDay { get; set; }
        /// <summary>
        /// Konečný den projektu - skutečný
        /// </summary>
        public DateTime EndDay { get; set; }
        /// <summary>
        /// Poznámky k projektu - co nefunguje apod
        /// </summary>
        public List<string> Comments { get; set; }
        /// <summary>
        /// Do list věcí, které jsou ještě nutné udělat na projektu
        /// </summary>
        public List<string> DoList { get; set; }

        /// <summary>
        /// Prázdný konstruktor pro uložení pomocí XMLserializéru
        /// </summary>
        public WorkProject()
        {

        }

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="startDay">Počáteční den projektu</param>
        /// <param name="projectEvents">Části projektu</param>
        /// <param name="name">Název projektu</param>
        /// <param name="desc">Popis projektu</param>
        /// <param name="planTime">Plánovaná doba na projekt v HOD</param>
        public WorkProject(DateTime startDay, List<string> projectEvents, string name, string desc
            , decimal planTime)
        {
            // Nastaveni vlastnosti
            StartDay = startDay;
            ProjectEvents = projectEvents;
            Name = name;
            Description = desc;
            PlanTime = planTime;
            // Inicializace
            Records = new List<WorkRecord>();
            Comments = new List<string>();
            DoList = new List<string>();
            // ID
            WorkProjectId = workProjectID;
            workProjectID++;
        }

        /// <summary>
        /// Přidá / upraví denní záznam do kolekce v WORK PROJECT
        /// </summary>
        /// <param name="choice">0 - Nový * 1 - Úprava</param>
        /// <param name="date">Datum záznamu</param>
        /// <param name="planTime">Plánovaná pracovní doba</param>
        /// <param name="record">Záznam pro úpravu</param>
        public void AddEditWorkRecords(byte choice, DateTime date, decimal planTime, WorkRecord record)
        {
            if(choice == 0)
            {
                Records.Add(new WorkRecord(date, planTime));
            }
            else if(choice == 1 && record != null)
            {
                record.Date = date;
                record.PlanTime = planTime;
            }
        }

        /// <summary>
        /// Odebere denní záznam z kolekce ve WORK PROJECT
        /// </summary>
        /// <param name="record">Záznam, který bude smazán</param>
        public void RemoveWorkRecord(WorkRecord record)
        {
            if(record.WorkParts.Count > 0)
            {
                record.RemoveWorkParts(record.WorkParts[record.WorkParts.Count - 1]);
            }
            else if(record != null)
                Records.Remove(record);
        }
    }
}
