using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All4Me
{
    /// <summary>
    /// View Model pro úpravu denního záznamu nebo bloku - Bindováno na tuto třídu
    /// </summary>
    public class VM_RecordPart
    {
        /// <summary>
        /// Denní záznam, který se upravuje
        /// </summary>
        private WorkRecord selectRecord;
        /// <summary>
        /// Datum záznamu
        /// </summary>
        public DateTime RecordDate
        {
            get
            {
                return selectRecord.Date;
            }
        }
        /// <summary>
        /// Plánova denní pracovní doba
        /// </summary>
        public string RecordPlanTime
        {
            get
            {
                return selectRecord.PlanTime.ToString();
            }
        }
        /// <summary>
        /// Začátek prvního bloku
        /// </summary>
        public DateTime? Part1StartHour
        {
            get
            {
                if (selectRecord.WorkParts.Count > 0)
                    return selectRecord.WorkParts[0].StartHour;
                else
                    return null;
            }
        }
        /// <summary>
        /// Konec prvního bloku
        /// </summary>
        public DateTime? Part1EndHour
        {
            get
            {
                if (selectRecord.WorkParts.Count > 0)
                    return selectRecord.WorkParts[0].EndHour;
                else
                    return null;
            }
        }
        /// <summary>
        /// Začátek druhého bloku
        /// </summary>
        public DateTime? Part2StartHour
        {
            get
            {
                if (selectRecord.WorkParts.Count > 1)
                    return selectRecord.WorkParts[1].StartHour;
                else
                    return null;
            }
        }
        /// <summary>
        /// Konec druhého bloku
        /// </summary>
        public DateTime? Part2EndHour
        {
            get
            {
                if (selectRecord.WorkParts.Count > 1)
                    return selectRecord.WorkParts[1].EndHour;
                else
                    return null;
            }
        }
        /// <summary>
        /// Náplň práce v prvním bloku
        /// </summary>
        public string Part1WorkContent
        {
            get
            {
                if (selectRecord.WorkParts.Count > 0)
                    return selectRecord.WorkParts[0].WorkContent;
                else
                    return null;
            }
        }
        /// <summary>
        /// Náplň práce v druhém bloku
        /// </summary>
        public string Part2WorkContent
        {
            get
            {
                if (selectRecord.WorkParts.Count > 1)
                    return selectRecord.WorkParts[1].WorkContent;
                else
                    return null;
            }
        }
        /// <summary>
        /// Název projektu, aby bylo možné jej porovnat a nastavit správný v ComboBoxu při úpravě
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        /// Konstruktor pro VIEW MODEL - Denní záznam a denní blok
        /// </summary>
        /// <param name="record">Denní zíznam pro bindování</param>
        public VM_RecordPart(WorkRecord record, string projectName)
        {
            selectRecord = record;
            ProjectName = projectName;
        }
    }
}
