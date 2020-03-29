using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace All4Me
{
    /// <summary>
    /// Správce pracovní části aplikace, obsahuje veškerou logiku pro práci s touto části projektu
    /// </summary>
    public class WorkAdmin
    {
        /// <summary>
        /// Kolekce projektů
        /// </summary>
        public List<WorkProject> Projects { get; private set; }

        /// <summary>
        /// Cesta pro uložení souborů - základní část pro AppData v C
        /// </summary>
        private string pathBase;
        /// <summary>
        /// Cesta pro uložení pracovních projektů
        /// </summary>
        private string pathWorkProjects;
        /// <summary>
        /// Cesta pro uložení IDs všech částí pracovního diáře
        /// </summary>
        private string pathWorkIDs;
        /// <summary>
        /// Cesta pro uložení záznamů na disk D - do složky práce, která se zálohuje na server
        /// </summary>
        private string pathWorkProjects_PraceD;
        /// <summary>
        /// Cesta pro uložení IDs na disk D - do složky práce, která se zálohuje na server
        /// </summary>
        private string pathWorkIDs_PraceD;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public WorkAdmin()
        {
            Projects = new List<WorkProject>();
            try
            {
                // Vytvoření cesty do C app data
                pathBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "All4Me");
                if (!Directory.Exists(pathBase))
                    Directory.CreateDirectory(pathBase);
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku, zkontrolujte prosím svá oprávnění.");
            }

            try
            {
                if (!Directory.Exists(@"d:\Prace\ZalohyProgramu\All4Me"))
                    Directory.CreateDirectory(@"d:\Prace\ZalohyProgramu\All4Me");
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku, zkontrolujte prosím svá oprávnění.");
            }
            // Cesty pro uložení na C do složky appdata
            pathWorkProjects = Path.Combine(pathBase, "WorkProjects.xml");
            pathWorkIDs = Path.Combine(pathBase, "WorkIDs.xml");
            // Cesty pro uložení na D, tak aby byly záznamy automaticky nahrány disk
            pathWorkProjects_PraceD = Path.Combine(@"d:\Prace\ZalohyProgramu\All4Me", "WorkProjects.xml");
            pathWorkIDs_PraceD = Path.Combine(@"d:\Prace\ZalohyProgramu\All4Me", "WorkIDs.xml");
        }

        /// <summary>
        /// Metoda, která vytiskne všechna data v RECORDS do txt souboru, na místo kam si uživatel vybere
        /// </summary>
        /// <param name="type">FALSE - forma pro EXCEL, TRUE - forma pro WORD</param>
        /// <param name="mode">0 - roční výpis, 1 - měsíční výpis, 2 - všechny záznamy</param>
        /// <param name="month">Měsíc 1 - LEDEN</param>
        /// <param name="year">Rok 2020 - první</param>
        public void PrintWorkRecords(bool type,int year, int month, byte mode)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                FileName = "WorkData",
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };
            save.ShowDialog();
            string path = save.FileName;
            List<WorkRecord> workRecords = new List<WorkRecord>();
            // Varianta pomocí LINQ 
            /*
            
            // Vybere projekt z kolekce projektů, dále denní záznamy v každém projektu
            // Porovná jej s podmínkou za WHERE a pokud splní, tak denní záznam vybere pomocí SELECT
            var query = from project in Projects
                        from record in project.Records
                        where (mode == 0 && record.Date.Year == year || mode == 1 && record.Date.Year == year && record.Date.Month == month || mode == 2)
                        select record;

            foreach(WorkRecord rec in query)
            {
                workRecords.Add(rec);
            }
            */

            foreach (WorkProject project in Projects)
            {
                foreach(WorkRecord record in project.Records)
                {
                    if (mode == 2)
                        workRecords.Add(record);
                    else if (mode == 1 && record.Date.Year == year && record.Date.Month == month)
                        workRecords.Add(record);
                    else if (mode == 0 && record.Date.Year == year)
                        workRecords.Add(record);
                }
            }

            if(workRecords.Count != 0)
            {
                IOrderedEnumerable<WorkRecord> sortRecords = workRecords.OrderByDescending(a => a.Date);
                using (StreamWriter sw = new StreamWriter(path))
                {
                    if (!type)
                        sw.WriteLine("Datum\tPlánovaná doba\tOdpracovaná doba\tRanní blok od - do\tNáplň práce\tOdpolední blok od - do\tNáplň práce");
                    foreach (WorkRecord record in sortRecords)
                    {
                        string[] partTimes = new string[2];
                        string[] partContents = new string[2];
                        for (int i = 0; i < record.WorkParts.Count; i++)
                        {
                            partTimes[i] = record.WorkParts[i].StartHour.ToShortTimeString() + " - " + record.WorkParts[i].EndHour.ToShortTimeString();
                            partContents[i] = record.WorkParts[i].WorkContent;
                        }
                        if (!type)
                        {
                            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", record.Date.ToShortDateString(), record.PlanTime, record.RealTime,
                                partTimes[0], partContents[0], partTimes[1], partContents[1]);

                            // Vyprazdni zasobnik - NUTNE
                            sw.Flush();
                        }
                        else
                        {
                            sw.WriteLine("\n-----------------------------------------\nZáznam ze dne:\t{0}     *********|\n" +
                                "-----------------------------------------", record.Date.ToShortDateString());
                            sw.WriteLine(" -Plánovaná doba\t\t{0} hod\n -Odpracovaná doba\t\t{1} hod\n" +
                                         " -Ráno   ------------------ \n\t-Od - Do\t\t{2}\n \t-Náplň práce\t\t{3}\n" +
                                         " -Odpoledne   ------------- \n\t-Od - Do\t\t{4}\n \t-Náplň práce\t\t{5}",
                                          record.PlanTime, record.RealTime, partTimes[0], partContents[0], partTimes[1], partContents[1]);

                            sw.Flush();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metoda najde ve třídní kolekci záznamy, které odpovídají volbě
        /// </summary>
        /// <param name="selectProjectID">ID projektu</param>
        /// <param name="choice">TRUE - Měsíční bloky (rok), FALSE - Týdenní bloky (mesic)</param>
        /// <param name="month">Měsíc 1 - LEDEN</param>
        /// <param name="year">Rok -- první je 2020</param>
        /// <returns>Projekt k zobrazení</returns>
        public List<WorkMonthWeekOverview> CalculateOverview(int selectProjectID, int year, int month, bool choice)
        {
            // Kolekce pro uložení grafických záznamů přehledu
            List<WorkMonthWeekOverview> records = new List<WorkMonthWeekOverview>();
            string[] monthName = new string[] {"Leden","Únor","Březen","Duben", "Květen", "Červen",
                "Červenec", "Srpen" , "Září", "Říjen", "Listopad", "Prosinec" };
            // Pracovních dní
            int[] workDays;
            // Odpracováno hodin
            decimal[] realHours;
            // Plánováno hodin
            decimal[] planHours;
            // Průměrná pracovní doba
            int[] average;
            // Kolekce obsahující první konečný den pro každý týden v daném měsíci
            List<DateTime> weeks = Week.GetWeek(month, year);
            // FALSE - měsíční přehled * týdenní bloky
            if (!choice)
            {
                // Pole pro uložení pracovních dní -- počet prvků pole jako počet týdnů -- [0] - 1. týden atd
                workDays = new int[weeks.Count/2];
                // Pole pro uložení pracovních hodin
                realHours = new decimal[weeks.Count / 2];
                // Pole pro uložení plánovaných hodin
                planHours = new decimal[weeks.Count / 2];
                // Průměrná doba práce
                average = new int[weeks.Count / 2];

                foreach (WorkProject project in Projects)
                {
                    foreach (WorkRecord record in project.Records)
                    {
                        DateTime day = record.Date;
                        // Prochází se všechny záznamy, všech projektů 
                        //      - pro každý týden se uloží daná hodnota 
                        //      - [0] - 1. týden, [1] - 2. týden až [4] - 5. týden měsíce
                        if (day >= weeks[0] && day <= weeks[1])
                        {
                            workDays[0]++;
                            realHours[0] += record.RealTime;
                            planHours[0] += record.PlanTime;
                        }
                        else if (day >= weeks[2] && day <= weeks[3])
                        {
                            workDays[1]++;
                            realHours[1] += record.RealTime;
                            planHours[1] += record.PlanTime;

                        }
                        else if (day >= weeks[4] && day <= weeks[5])
                        {
                            workDays[2]++;
                            realHours[2] += record.RealTime;
                            planHours[2] += record.PlanTime;
                        }
                        else if (day >= weeks[6] && day <= weeks[7])
                        {
                            workDays[3]++;
                            realHours[3] += record.RealTime;
                            planHours[3] += record.PlanTime;
                        }
                        // Měsíc obsahuje 5 týdnů
                        if (weeks.Count == 10)
                        {
                            if (day >= weeks[8] && day <= weeks[9])
                            {
                                workDays[4]++;
                                realHours[4] += record.RealTime;
                                planHours[4] += record.PlanTime;
                            }
                        }
                    }
                }
            }
            // TRUE - roční přehled * měsíční bloky    
            else
            {
                // Pole o velikosti 12 - každé id pole je jeden měsíc [0] - LEDEN až [11] - PROSINEC
                workDays = new int[12];
                realHours = new decimal[12];
                planHours = new decimal[12];
                average = new int[12];
                int[] monthsId = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                foreach (WorkProject project in Projects)
                {
                    foreach (WorkRecord record in project.Records)
                    {
                        if (record.Date.Year == year)
                        {
                            int id = Array.IndexOf(monthsId, record.Date.Month);

                            workDays[id]++;
                            realHours[id] += record.RealTime;
                            planHours[id] += record.PlanTime;
                        }
                    }
                }
            }
           // Měsíční přehled
            if (!choice)
            {
                int k = 0;
                for (int i = 0; i < 5; i++)
                {
                    // Datum týdne Od - Do
                    string weekText = weeks[k].ToString("dd.MM") + " - " + weeks[k + 1].ToString("dd.MM");
                    // Průměrná délka pracovní doby
                    if (workDays[i] != 0)
                        average[i] = (int)Math.Round((realHours[i] * 60) / workDays[i]);
                    WorkMonthWeekOverview graphic = new WorkMonthWeekOverview(workDays[i], (int)Math.Round(realHours[i])
                    , (int)Math.Round(planHours[i]), new TimeSpan(0, average[i], 0)
                    , weekText, year.ToString());

                    records.Add(graphic);
                    k += 2;
                }
            }
            // Roční přehled
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    if (workDays[i] != 0)
                        average[i] = (int)Math.Round((realHours[i] * 60) / workDays[i]);
                    WorkMonthWeekOverview graphic = new WorkMonthWeekOverview(workDays[i], (int)Math.Round(realHours[i])
                    , (int)Math.Round(planHours[i]), new TimeSpan(0, average[i], 0)
                    , monthName[i], year.ToString());

                    records.Add(graphic);
                }
            }
            // Stanovení stran pro každý jednotlivý záznam přehledu
            UpdateSetPage(records,null);
            return records;
        }

        /// <summary>
        /// Projde všechny záznamy a pro každý stanoví na jakou stranu patří, tuto hodnotu pak uloží do dané třídy
        /// </summary>
        /// <param name="graphicOverview">Kolekce přehledu</param>
        /// <param name="graphicRecords">Kolekce denních záznamů</param>
        private void UpdateSetPage(List<WorkMonthWeekOverview> graphicOverview, List<WorkGraphicRecord> graphicRecords)
        {
            if(graphicOverview != null)
            {
                int[] pageGraphicOverview = new int[2];

                foreach (WorkMonthWeekOverview overview in graphicOverview)
                {
                    // 6 Záznamů na stranu - 6 měsíců nebo týdnů
                    if (pageGraphicOverview[0] < 6)
                    {
                        pageGraphicOverview[0]++;
                        overview.Page = 0;
                    }
                    else
                    {
                        pageGraphicOverview[1]++;
                        overview.Page = 1;
                    }
                }
            }
            else if (graphicRecords != null)
            {
                int[] pageGraphicRecord = new int[5];
                int i = 0;
                foreach (WorkGraphicRecord overview in graphicRecords)
                {
                    // Ošetření, aby nepřetekl počet stran
                    if (i == 5)
                        break;

                    // Čtyři záznamy na stranu
                    if (pageGraphicRecord[i] < 4)
                    {
                        pageGraphicRecord[i]++;
                        overview.Page = i;

                        if (pageGraphicRecord[i] % 4 == 0)
                            i++;
                    }
                }
            }
        }

        /// <summary>
        /// Metoda najde ve třídní kolekci projekt, který odpovídá svým pořadím zadanému ID
        /// </summary>
        /// <param name="selectProjectID">ID projektu</param>
        /// <returns>Projekt k zobrazení</returns>
        public WorkProject FindProject(int projectID)
        {
            return Projects[projectID];
        }

        /// <summary>
        /// Vytvoří grafické denní události pro zobrazení na úvodní obrazovce
        /// </summary>
        /// <returns>Kolekci grafických záznamů</returns>
        public List<WorkGraphicRecord> ConstructGraphicResult(int month, int year)
        {
            List<WorkGraphicRecord> records = new List<WorkGraphicRecord>();
            List<WorkRecord> recordsToShort = new List<WorkRecord>();

            // Varianta pomocí LINQ
            /*
            var query = from project in Projects
                        from record in project.Records
                        where (record.Date.Month == month && record.Date.Year == year)
                        orderby record.Date descending
                        select record;

            foreach (var record in query)
            {
                recordsToShort.Add(record);
            }
            */

            foreach(WorkProject project in Projects)
            {
                foreach(WorkRecord record in project.Records)
                {
                    if (record.Date.Month == month && record.Date.Year == year)
                    {
                        recordsToShort.Add(record);
                    }
                }
            }
            // Kolekce je seřaděna pomocí datumu sestupně
            IOrderedEnumerable<WorkRecord> sort = recordsToShort.OrderByDescending(date => date.Date);
            // Z vytříděné kolekce jsou záznamy vytvořeny v grafické formě pomocí třídy WorkGraphicRecord
            foreach(WorkRecord record in sort)
            {
                string nameProject = "";
                foreach(WorkProject project in Projects)
                {
                    if (project.Records.Contains(record))
                    {
                        nameProject = project.Name;
                        break;
                    }
                }
                if (record.Date.Month == month && record.Date.Year == year)
                {
                    records.Add(new WorkGraphicRecord(record, nameProject));
                }
            }
            UpdateSetPage(null,records);
            return records;
        }


        #region AddEditRemove

        /// <summary>
        /// Přidává nebo upravuje vlastní kolekci Projektů
        /// </summary>
        /// <param name="choice">0 - Nový projekt, 1 - Úprava stávajícího</param>
        /// <param name="startDay">Startovní den</param>
        /// <param name="projectEvents">Části projektu</param>
        /// <param name="name">Název projektu</param>
        /// <param name="description">Popis</param>
        /// <param name="planTime">Plánovaná doba na projekt</param>
        /// <param name="project">Projekt k upravení</param>
        /// <param name="endDay">Datum ukončení projektu</param>
        /// <param name="comments">Komentáře k projektu</param>
        /// <param name="doList">Do list věcí, které ještě projekt potřebuje</param>
        public void AddEditProject(byte choice, DateTime startDay, List<string> projectEvents, string name,
            string description, decimal planTime, WorkProject project
            , List<string> comments, List<string> doList)
        {
            // Nový projekt
            if(choice == 0)
            {
                Projects.Add(new WorkProject(startDay, projectEvents, name, description, planTime));

                SaveIDs(pathWorkIDs);
                SaveProjects(pathWorkProjects);

                SaveIDs(pathWorkIDs_PraceD);
                SaveProjects(pathWorkProjects_PraceD);
            }
            // Úprava stávajícího projektu
            else if(choice == 1 && project != null)
            {
                project.Name = name;
                project.Description = description;
                project.PlanTime = planTime;
                project.ProjectEvents = projectEvents;
                project.StartDay = startDay;
                project.DoList = doList;
                project.Comments = comments;

                SaveIDs(pathWorkIDs);
                SaveProjects(pathWorkProjects);

                SaveIDs(pathWorkIDs_PraceD);
                SaveProjects(pathWorkProjects_PraceD);
            }
        }

        /// <summary>
        /// Odebere z kolekce projektů zadaný projekt
        /// </summary>
        /// <param name="project">Projekt k odebrání</param>
        public void RemoveProject(WorkProject project)
        {
            if(project != null)
                Projects.Remove(project);

            SaveIDs(pathWorkIDs);
            SaveProjects(pathWorkProjects);

            SaveIDs(pathWorkIDs_PraceD);
            SaveProjects(pathWorkProjects_PraceD);
        }

        /// <summary>
        /// Přidá / upraví denní záznam do kolekce v WORK PROJECT
        /// </summary>
        /// <param name="choice">0 - Nový * 1 - Úprava</param>
        /// <param name="project">Projekt, který obsahuje denní záznam</param>
        /// <param name="record">Záznam pro úpravu</param>
        /// <param name="date">Datum záznamu</param>
        /// <param name="planTime">Plánovaná pracovní doba</param>
        public void AddEditWorkRecord(byte choice,WorkProject project, WorkRecord record,
            DateTime date,decimal planTime)
        {
            project.AddEditWorkRecords(choice, date, planTime, record);

            SaveIDs(pathWorkIDs);
            SaveProjects(pathWorkProjects);

            SaveIDs(pathWorkIDs_PraceD);
            SaveProjects(pathWorkProjects_PraceD);
        }

        /// <summary>
        /// Odebere denní záznam z kolekce v WORK PROJECT
        /// </summary>
        /// <param name="project">Projekt obsahující kolekci denních záznamů</param>
        /// <param name="record">Záznam k vymazání</param>
        public void RemoveWorkRecord(WorkRecord record)
        {
            foreach(WorkProject proj in Projects)
            {
                if(proj.Records.Contains(record))
                    proj.RemoveWorkRecord(record);
            }

            SaveIDs(pathWorkIDs);
            SaveProjects(pathWorkProjects);

            SaveIDs(pathWorkIDs_PraceD);
            SaveProjects(pathWorkProjects_PraceD);
        }

        /// <summary>
        /// Přidání k danému dny dopolední či odpoledního pracovního blok WORK PART
        /// </summary>
        /// <param name="choice">0 - Nový záznam ** 1 - Úprava stávajícího</param>
        /// <param name="start">Začátek pracovní doby</param>
        /// <param name="stop">Konec pracovní doby</param>
        /// <param name="content">Náplň práce</param>
        /// <param name="partRecord">Záznam k upravení či výmazu</param>
        /// <param name="record"></param>
        public void AddEditWorkParts(byte choice, DateTime start, DateTime stop, string content,
            WorkPart partRecord, WorkRecord record)
        {
            record.AddEditWorkParts(choice, start, stop, content, partRecord);

            SaveIDs(pathWorkIDs);
            SaveProjects(pathWorkProjects);

            SaveIDs(pathWorkIDs_PraceD);
            SaveProjects(pathWorkProjects_PraceD);
        }

        /// <summary>
        /// Odebere ranní či odpolední blok z deního záznamu
        /// </summary>
        /// <param name="partRecord">Záznam,který bude odebrán</param>
        /// <param name="record"></param>
        public void RemoveWorkParts(WorkPart partRecord, WorkRecord record)
        {
            record.RemoveWorkParts(partRecord);

            SaveIDs(pathWorkIDs);
            SaveProjects(pathWorkProjects);

            SaveIDs(pathWorkIDs_PraceD);
            SaveProjects(pathWorkProjects_PraceD);
        }
        #endregion


        #region SaveLoad

        /// <summary>
        /// Uložení ID všech tříd
        /// </summary>
        public void SaveIDs(string path)
        {
            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(WorkProject.workProjectID);
                sw.WriteLine(WorkRecord.workRecordID);
                sw.WriteLine(WorkPart.workPartID);
            }
        }

        /// <summary>
        /// Uložení kolekce projektů
        /// </summary>
        public void SaveProjects(string path)
        {
            XmlSerializer serializer = new XmlSerializer(Projects.GetType());
            using(StreamWriter writter = new StreamWriter(path))
            {
                serializer.Serialize(writter, Projects);
            }
        }

        /// <summary>
        /// Načtení ID všech tříd
        /// </summary>
        public void LoadIDs()
        {
            if (File.Exists(pathWorkIDs))
            {
                using(StreamReader reader = new StreamReader(pathWorkIDs))
                {
                    WorkProject.workProjectID = int.Parse(reader.ReadLine());
                    WorkRecord.workRecordID = int.Parse(reader.ReadLine());
                    WorkPart.workPartID = int.Parse(reader.ReadLine());
                }
            }
            else
            {
                WorkProject.workProjectID = 0;
                WorkRecord.workRecordID = 0;
                WorkPart.workPartID = 0;
            }
        }

        /// <summary>
        /// Načtení kolekce projektů
        /// </summary>
        public void LoadProjects()
        {
            XmlSerializer serializer = new XmlSerializer(Projects.GetType());
            if (File.Exists(pathWorkProjects))
            {
                using(StreamReader reader = new StreamReader(pathWorkProjects))
                {
                    Projects = (List<WorkProject>)serializer.Deserialize(reader);
                }
            }
            else
            {
                Projects = new List<WorkProject>();
            }
        }
        #endregion


    }
}
