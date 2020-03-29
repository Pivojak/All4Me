using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All4Me
{
    /// <summary>
    /// Část pracovního záznamu - Dopoledne, Odpoledne apod
    /// </summary>
    public class WorkPart
    {
        public static int workPartID;
        /// <summary>
        /// Id části pracovního záznamu za den
        /// </summary>
        public int WorkPartId { get;  set; }
        /// <summary>
        /// Počáteční hodina pracovního bloku
        /// </summary>
        public DateTime StartHour { get; set; }
        /// <summary>
        /// Konečná hodina pracovního bloku
        /// </summary>
        public DateTime EndHour { get; set; }
        /// <summary>
        /// Náplň práce pracovního bloku
        /// </summary>
        public string WorkContent { get; set; }

        /// <summary>
        /// Prázdný konstruktor pro možnost uložení dat na pevný disk
        /// </summary>
        public WorkPart()
        {

        }

        /// <summary>
        /// Kompletní konstruktor pro vytvoření části pracovního záznamu
        /// </summary>
        /// <param name="start">Počáteční hodina pracovního bloku</param>
        /// <param name="stop">Konečná hodina pracovního bloku</param>
        /// <param name="content">Prázdný konstruktor pro možnost uložení dat na pevný disk</param>
        public WorkPart(DateTime start, DateTime stop, string content)
        {
            StartHour = start;
            EndHour = stop;
            WorkContent = content;
            WorkPartId = workPartID;
            workPartID++;
        }

    }
}
