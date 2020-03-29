using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace All4Me
{
    /// <summary>
    /// Pracovní validátor - Validuje vstupní data, upozorňuje uživatele na zadané chyby, komunikuje s okny
    /// </summary>
    public class WorkValidator
    {
        /// <summary>
        /// Správce prácovní evidence
        /// </summary>
        private WorkAdmin admin;
        /// <summary>
        /// Plátno na které se vykreslují veškeré informace
        /// </summary>
        private Canvas canvas;
        /// <summary>
        /// Aktuálně vybraný záznam, slouží při úpravách apod, aby program věděl, který je zakliknut
        /// </summary>
        private WorkRecord selectRecord;
        /// <summary>
        /// Aktuálně vybraný projekt - využíván při úpravě projektu
        /// </summary>
        private WorkProject selectProject;
        /// <summary>
        /// Aktuální strana přehledu
        /// </summary>
        private int overviewActualPage = 0;
        /// <summary>
        /// Aktuální strana výpisu denních záznamů
        /// </summary>
        private int recordsActualPage = 0;
        /// <summary>
        /// Vybrané ID comboBoxu pro měsíc v sekci PŘEHLEDU
        /// </summary>
        private int selectMonthId_overview;
        /// <summary>
        /// Vybrané ID comboBoxu pro rok v sekci PŘEHLEDU
        /// </summary>
        private int selectYearId_overview;
        /// <summary>
        /// Vybraný mód sekci PŘEHLEDU ** TRUE - roční přehled (měsíční bloky) ** FALSE - měsíční přehled (týdenní bloky)
        /// </summary>
        private bool selectMode_overview;
        /// <summary>
        /// Vybrané ID comboBoxu pro měsíc v sekci denních záznamů
        /// </summary>
        private int selectMonthId_records;
        /// <summary>
        /// Vybrané ID comboBoxu pro rok v sekci denních záznamů
        /// </summary>
        private int selectYearId_records;
        /// <summary>
        /// Konstruktor, pouze vytvoří instanci správce
        /// </summary>
        public WorkValidator()
        {
            admin = new WorkAdmin();
        }

        /// <summary>
        /// Nastavení plátna, na které bude Validator vykreslovat
        /// </summary>
        /// <param name="canvas"></param>
        public void DefineCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// Získání názvu všech projektů uložených ve správci
        /// </summary>
        /// <returns>Názvy projektů</returns>
        public List<string> DefineProjectNames()
        {
            List<string> names = new List<string>();
            foreach(WorkProject project in admin.Projects)
            {
                names.Add(project.Name);
            }
            return names;
        }

        /// <summary>
        /// Metoda, která vytiskne všechna data v RECORDS do txt souboru, na místo kam si uživatel vybere
        /// </summary>
        /// <param name="type">FALSE - forma pro EXCEL, TRUE - forma pro WORD</param>
        /// <param name="monthId">Id comboBoxu pro měsíc</param>
        /// <param name="yearId">Id comboBoxu pro rok</param>
        public void PrintWorkRecords(bool type,int yearId, int monthId)
        {
            byte mode = 1;
            // Není vybrán ani měsíc ani rok -- celkévý přehled -- všechny záznamy
            if (yearId == -1 && monthId == -1)
                mode = 2;
            // Není vybrán měsíc, ale rok ANO - roční přehled 
            else if (monthId == -1 && yearId != -1)
                mode = 0;
            // Je vybrán měsíc - validace na čísla měsíců - protože měsíce jsou číslovány od 1 nikoliv od 0
            if (monthId != -1)
                monthId += 1;
            // Je vybrán rok - validace na číslo roku - comboBox začíná od 2020, proto + to
            if (yearId != -1)
                yearId += 2020;
            // Není vybrán rok, nastavení roku na aktuální
            else
                yearId += DateTime.Today.Year + 1;

            admin.PrintWorkRecords(type,yearId,monthId,mode);
        }

        #region Find
        /// <summary>
        /// Nalezne požadovaný projekt a vrátí VM, na který se binduje okno
        /// </summary>
        /// <param name="selectProjectID">ID požadovaného projektu</param>
        /// <returns>VM model projektu</returns>
        public VM_Project FindProjects(int selectProjectID)
        {
            WorkProject project;
            if (selectProjectID < -1 && admin.Projects.Count > selectProjectID)
                throw new ArgumentException("Nevybral jsi žádný projekt");
            else
                project = admin.FindProject(selectProjectID);

            selectProject = project;
            return new VM_Project(project);
        }
        #endregion


        #region GraphicMethods

        /// <summary>
        /// Zadaný objekt přidá na CANVAS a zarovná jej podle zadaných hodnot
        /// </summary>
        /// <param name="element">Prvek, který se umístí na CANVAS</param>
        /// <param name="canvas">Plátno na které bude objekt umísten</param>
        /// <param name="setLeft">Vzdálenost zleva</param>
        /// <param name="setTop">Vzdálenost od vrchu</param>
        /// <param name="setRight">Vzdálenost zprava</param>
        /// <param name="setBottom">Vzdálenost ze spodu</param>
        private void CanvasPositionAddObject(object element, Canvas canvas, int setLeft, int setTop, int setRight, int setBottom)
        {
            // Jedná se o obdelník
            if (element is Rectangle)
            {
                canvas.Children.Add(element as Rectangle);
                Canvas.SetLeft(element as Rectangle,setLeft);
                Canvas.SetTop(element as Rectangle, setTop);
                Canvas.SetRight(element as Rectangle, setRight);
                Canvas.SetBottom(element as Rectangle, setBottom);
            }
            // Jedná se o TextBlock  
            else if (element is TextBlock)
            {
                canvas.Children.Add(element as TextBlock);
                Canvas.SetLeft(element as TextBlock, setLeft);
                Canvas.SetTop(element as TextBlock, setTop);
                Canvas.SetRight(element as TextBlock, setRight);
                Canvas.SetBottom(element as TextBlock, setBottom);
            }
            // Jedná se o tlačítko
            else if (element is Button)
            {
                canvas.Children.Add(element as Button);
                Canvas.SetLeft(element as Button, setLeft);
                Canvas.SetTop(element as Button, setTop);
                Canvas.SetRight(element as Button, setRight);
                Canvas.SetBottom(element as Button, setBottom);
            }
        }

        /// <summary>
        /// Vytvoří měsíční či týdenní bloky pro přehled za ROK nebo MĚSÍC při výběru
        /// </summary>
        /// <param name="rotation">Uživatel rotoval kolečkem - další strana</param>
        /// <param name="yearID">Identifikátor roku v comboBoxu ** 0 - 2020 </param>
        /// <param name="monthID">Indentifikátor měsíce v comboBoxu ** 0 - Leden</param>
        /// <param name="firstStart">Jedná se o první stranu</param>
        public void ConstructWeekMonthOverview(bool rotation,int yearID, int monthID, bool firstStart)
        {
            // Jedná se o první stranu -- nastavení všech základních parametrů
            if(firstStart)
            {
                selectMonthId_overview = monthID;
                selectYearId_overview = yearID;
                if (monthID == -1)
                    selectMode_overview = true;
                else
                    selectMode_overview = false;
                overviewActualPage = 0;
            }
            // TRUE - podle roku, FALSE - podle mesice
            bool choice = true;
            // Výpis se nachází na druhé straně a uživatel rotoval -- skok na první stranu
            if (overviewActualPage >= 1 && rotation)
                overviewActualPage = 0;
            // Rotace, avšak na první straně ** - zvýšení strany na druhou
            else if (rotation)
            {
                overviewActualPage++;
            }
            // Uživatel chce další stranu - získání z uložených parametrů měsíc, rok a to zda zadal přehled pro rok nebo měsíc
            if (rotation)
            {
                monthID = selectMonthId_overview;
                yearID = selectYearId_overview;
                choice = selectMode_overview;
            }
            
            if (yearID == -1 && monthID == -1)
                throw new ArgumentException("Nevybral jsi žádný rok nebo měsíc");
            if (monthID != -1)
                choice = false;

            if (yearID == -1)
                yearID = DateTime.Today.Year;
            else
                yearID += 2020;
            // Smažu plátno od přechozích objektů na něm umístěných
            canvas.Children.Clear();
            // Získám kolekci grafických záznamů
            List<WorkMonthWeekOverview> records;
            // Roční přehled
            if (choice)
            {
                records = admin.CalculateOverview(0, yearID, 2, choice);
            }
            // Měsíční přehled
            else
            {
                records = admin.CalculateOverview(0, yearID, monthID + 1, choice);
            }
               
            int k = 0;
            // Zarovnání všech objektů na CANVAS
            for (int i = 0; i < records.Count; i++)
            {
                if(records[i].Page == overviewActualPage)
                {
                    // Přidání obsluhy na události tlačítek
                    List<object> elements = records[i].ReturnAllAtributs();
                    int[] leftDistance = new int[]
                    {   // Pozadí, separátor pod názvem měsíce a roku - šedý
                    10, 10,
                    // Popis měsíce vlevo a roku vpravo, nebo týdne vlevo a měsíce vpravo
                    20,180,
                    // Prac. dní, hodin, průměrná prac. doba, plánováno hodin
                    20,20,180,
                    //HODNOTY pro výše
                    20,180,20,180
                    };

                    int[] topDistance = new int[]
                    {
                        10, 40,
                        12,12,
                        55,100, 100,
                        72,72,118,118
                    };
                    // Postupně jsou objekty přidány na plátno a zarovnány
                    for (int j = 0; j < elements.Count; j++)
                    {
                        if (k < 3)
                            CanvasPositionAddObject(elements[j], canvas, leftDistance[j], k * 180 + topDistance[j], 0, 0);
                        else if (k >= 3)
                            CanvasPositionAddObject(elements[j], canvas, leftDistance[j] + 340, (k - 3) * 180 + topDistance[j], 0, 0);
   
                    }
                    k++;
                }
            }
        }



        /// <summary>
        /// Vytvoří grafickou reprezentaci denního záznamu o práce, včetně bloků ranní-odpolední
        /// </summary>
        public void ConstructGraphicResult(bool first, bool rotate,bool rotateUp, int monthID, int yearID)
        {
            // Jedná se o první stranu denních záznamů
            //      - uložení hodnot z ComboBoxů -- rok a měsíc
            if (first)
            {
                recordsActualPage = 0;
                selectMonthId_records = monthID;
                selectYearId_records = yearID;
            }
            // Přetečení - první strana a rotace nahoru -- poslední strana, tedy pátá
            if (recordsActualPage == 0 && rotate && rotateUp)
            {
                recordsActualPage = 4;
            }
            // Rotace kolečkem nahoru -- snížení aktuální strany
            else if (rotate && rotateUp)
            {
                recordsActualPage--;
            }

            // Aktuální strana 5 - a rotace dolů -- přetečení na první stranu, tedy Id - 0
            if (recordsActualPage == 4 && rotate && !rotateUp)
                recordsActualPage = 0;
            else if (rotate && !rotateUp)
            {
                recordsActualPage++;
            }
            // Ošetření vstupů
            if (monthID == -1 && yearID == -1)
                throw new ArgumentException("Nevybral jsi ROK a MĚSÍC");
            else if (monthID == -1)
                throw new ArgumentException("Nevybral jsi žádný měsíc");
            else if (yearID == -1)
                throw new ArgumentException("Nevybral jsi žádný rok");
            else
            {
                // Validace údajů, roky v ComboBoxu začínají od 2020 a měsíce -- leden = 0 
                yearID += 2020;
                monthID += 1;
            }

            // Smažu plátno od přechozích objektů na něm umístěných
            canvas.Children.Clear();
            // Získám kolekci grafických záznamů
            List<WorkGraphicRecord> records = admin.ConstructGraphicResult(monthID,yearID);
            int k = 0;
            for (int i = 0; i < records.Count; i++)
            {
                if(records[i].Page == recordsActualPage)
                {
                    // Přidání obsluhy na události tlačítek
                    records[i].AddButtonClick += AddButton_Click;
                    records[i].EditButtonClick += EditButton_Click;
                    records[i].RemoveButtonClick += RemoveButton_Click;
                    List<object> elements = records[i].ReturnAllAtributs();
                    // Obdelníky -- separátory 3x -- číslo dne, den, prac. doba Název a hodnota --
                    // -- Projekt nazev a hodnota -- Tlačítka (Add, Edit, Remove)
                    int[] leftDistance = new int[]
                    {   10, 20, 20,
                    80, 260, 440,
                    28, 28, 460, 460,
                    460,460,
                    605, 605, 605
                    };
                    //Obdelníky -- separátory 3x -- číslo dne, den, prac. doba Název a hodnota --
                    // -- Projekt nazev a hodnota -- Tlačítka (Add, Edit, Remove) -- denní blok 1 -- denní blok 2
                    int[] topDistance = new int[]
                    {
                        10, 20, 80,
                    10, 10, 10,
                    25, 87, 15,40,
                    75,95,
                    15, 55, 95,
                    35,15,75,95,
                    35,15,75,95
                    };
                    // Postupně jsou objekty přidány na plátno a zarovnány
                    for (int j = 0; j < elements.Count; j++)
                    {
                        // Obdelníky, separátory, popisy dne a číslo dne, tlačítka
                        if (j <= 14)
                            CanvasPositionAddObject(elements[j], canvas, leftDistance[j], k * 140 + topDistance[j], 0, 0);
                        // První denní blok info
                        else if (j > 14 && j <= 18)
                            CanvasPositionAddObject(elements[j], canvas, 95, k * 140 + topDistance[j], 0, 0);
                        // Druhý denní blok info
                        else if (j > 18)
                            CanvasPositionAddObject(elements[j], canvas, 180 + 95, k * 140 + topDistance[j], 0, 0);
                    }
                    k++;
                }
            }
        }

        #endregion

        #region ColorButtonsClick

        /// <summary>
        /// Obsluha události kliknutí na REMOVE tlačítko u denního záznamu
        /// </summary>
        /// <param name="sender">Instance denního záznamu</param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            selectRecord = sender as WorkRecord;
            RemoveWorkRecord(selectRecord);
        }

        /// <summary>
        /// Obsluha události kliknutí na EDIT tlačítko u denního záznamu
        /// </summary>
        /// <param name="sender">Instance denního záznamu</param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, EventArgs e)
        {
            // Pokus je sender denní záznam, rovnou se vytvoří proměnná tohoto typy s názvem RECORD
            if (sender is WorkRecord record)
            {
                selectRecord = record;
                string projectName = "";
                foreach (WorkProject project in admin.Projects)
                {
                    if (project.Records.Contains(selectRecord))
                    {
                        projectName = project.Name;
                        break;
                    }
                }
                WorkAddWindow window = new WorkAddWindow(this, new VM_RecordPart(record, projectName));
                window.Show();
            }
            else
                throw new ArgumentException("Nepodařilo se načíst informace o záznamu");

        }

        /// <summary>
        /// Obsluha události kliknutí na ADD tlačítko u denního záznamu
        /// </summary>
        /// <param name="sender">Instance denního záznamu</param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            selectRecord = sender as WorkRecord;
            WorkAddPartWindow window = new WorkAddPartWindow(this);
            window.Show();
        }
        #endregion

        #region AddEditRemove

        /// <summary>
        /// Přidává nebo upravuje vlastní kolekci Projektů
        /// </summary>
        /// <param name="choice">0 - Nový projekt, 1 - Úprava stávajícího</param>
        /// <param name="startDay">Startovní den</param>
        /// <param name="events">Části projektu</param>
        /// <param name="name">Název projektu</param>
        /// <param name="description">Popis</param>
        /// <param name="timePlan">Plánovaná doba na projekt</param>
        /// <param name="project">Projekt k upravení</param>
        /// <param name="endDay">Datum ukončení projektu</param>
        /// <param name="comments">Komentáře k projektu</param>
        /// <param name="doList">Do list věcí, které ještě projekt potřebuje</param>
        public void AddEditProject(byte choice, string startDay, string events, string name,
            string description, string timePlan, string comments, string doList)
        {
            // Pomocné proměnná
            List<string> events_help = new List<string>();
            // Datum počátku projektu
            if (DateTime.TryParse(startDay,out DateTime startDay_help))
            {
                if(startDay_help > DateTime.Now)
                    throw new ArgumentException("Zadal jsi datum z budoucnosti!");
                else if(startDay_help < new DateTime(DateTime.Now.Year - 5, 1, 1))
                    throw new ArgumentException("Zadal jsi datum z minulosti více jak 5 let!");
            }
            else
                throw new ArgumentException("Zadal jsi datum ve špatném formátu! Má vypadat jako 12.12.2020");

            // Plánovaná dotace hodin na daný projekt
            if(decimal.TryParse(timePlan,out decimal timePlan_number))
            {
                if(timePlan_number < 0)
                    throw new ArgumentException("Zadal jsi záporný počet hodin"); 
            }
            else
                throw new ArgumentException("Zadal jsi hodiny ve špatném formátu. Mají vypadat jako 5,25 NEBO 6,5 apod");
            
            // Název projektu
            if (name == "")
                throw new ArgumentException("Nezadal jsi název projektu");
            // Popis projektu
            if (description == "")
                throw new ArgumentException("Nezadal jsi popis projektu");
            // Části projektu - např. Návrh oken, Návrh tříd apod
            if (events.Count() == 0)
                throw new ArgumentException("Nezadal jsi žádné části body projektu. ");
            else
            {
                string word = "";
                if(choice == 0)
                    events += "\n";
                // Validace textu - odstraní \n - nový řádek a \r - stisknutí enteru z řetězce aby bylo možné jej uložit do pozice pole stringu    
                foreach(char c in events)
                {
                    if(c == '\n' || c == '\r')
                    {
                        // Odeberu \n a \r ze slova
                        if (word.Count() > 2)
                        {
                            word = word.Substring(0, word.Count());
                            // Uložení získaného bloku - části projektu pro pole
                            events_help.Add(word);
                        }
                        
                        word = "";
                    }
                    else
                        word += c;
                }
            }
            // Volba 0 - přidání nového záznamu
            if (choice == 0)
            {
                admin.AddEditProject(choice,startDay_help,events_help,name,description, timePlan_number
                    , null, null,null);
            }
            // Úprava stávajícího záznamu
            else if(choice == 1 && selectProject != null)
            {
                // Pomocné proměnné
                List<string> doList_help = new List<string>();
                List<string> comments_help = new List<string>();


                // Prochází znaky a získává bloky textu mezi znaky /n, které uloží do kolekce
                if (comments != "")
                {
                    string word = "";
                    comments += "\n";
                    foreach (char c in comments)
                    {

                        if (c == '\n' || c == '\r')
                        {
                            // Odeberu \n a \r ze slova
                            if (word.Count() > 2)
                            {
                                word = word.Substring(0, word.Count());
                                // Uložení získaného bloku - části projektu pro pole
                                comments_help.Add(word);
                            }

                            word = "";
                        }
                        else
                            word += c;
                    }
                }
                // Prochází znaky a získává bloky textu mezi znaky /n
                if (doList != "")
                {
                    string word = "";
                    doList += "\n";
                    foreach (char c in doList)
                    {
                        if (c == '\n' || c == '\r')
                        {
                            // Odeberu \n a \r ze slova
                            if (word.Count() > 2)
                            {
                                word = word.Substring(0, word.Count());
                                // Uložení získaného bloku - části projektu pro pole
                                doList_help.Add(word);
                            }

                            word = "";
                        }
                        else
                            word += c;
                    }
                }
                // Samotná metoda pro upravení stávajícího záznamu
                admin.AddEditProject(choice, startDay_help, events_help, name, description, timePlan_number
                    , selectProject, comments_help,doList_help);
            }
            else
            {
                throw new ArgumentException("Zvolil jsi špatnou volbu pro Přidání / Úpravu projektu");
            }
            
        }

        /// <summary>
        /// Odebe z kolekce projektů zadaný projekt
        /// </summary>
        /// <param name="project">Projekt k odebrání</param>
        public void RemoveProject(WorkProject project)
        {
            if (project == null)
                throw new ArgumentException("Nevybral jsi žádný projekt pro smazání.");
            else
                admin.RemoveProject(project);
        }


        /// <summary>
        /// Přidá / upraví denní záznam do kolekce v WORK PROJECT
        /// </summary>
        /// <param name="choice">0 - Nový * 1 - Úprava</param>
        /// <param name="project">Projekt, který obsahuje denní záznam</param>
        /// <param name="record">Záznam pro úpravu</param>
        /// <param name="date">Datum záznamu</param>
        /// <param name="planTime">Plánovaná pracovní doba</param>
        /// <param name="part1Start">Začátek ranního bloku</param>
        /// <param name="part1Stop">Konec ranního bloku</param>
        /// <param name="part1Content">Náplň práce ráno</param>
        /// <param name="part2Start">Začátek odpoledního bloku</param>
        /// <param name="part2Stop">Konec odpoledního bloku</param>
        /// <param name="part2Content">Náplň práce odpoledního bloku</param>
        public void AddEditWorkRecord(byte choice, int project, string date, string timePlan,
            string part1Start, string part1Stop, string part1Content,
            string part2Start, string part2Stop, string part2Content)
        {
            TimeSpan timePlan_help;
            WorkProject project_help;
            // Zadané ID projektu je v mezích 
            if (project >= 0 && project < admin.Projects.Count)
            {
                project_help = admin.Projects[project];
            }
            else
                throw new ArgumentException("");

            // Datum dne
            if (DateTime.TryParse(date, out DateTime date_help))
            {
                if (date_help > DateTime.Now)
                    throw new ArgumentException("Zadal jsi datum z budoucnosti!");
                else if (date_help < new DateTime(DateTime.Now.Year - 5, 1, 1))
                    throw new ArgumentException("Zadal jsi datum z minulosti více jak 5 let!");
            }
            else
                throw new ArgumentException("Zadal jsi datum ve špatném formátu! Má vypadat jako 12.12.2020");

            // Plánovaná dotace hodin na daný projekt
            if (decimal.TryParse(timePlan, out decimal timePlan_number))
            {
                if (timePlan_number > 0)
                {
                    int timeMinutes = (int)(timePlan_number * 60);
                    timePlan_help = new TimeSpan(0, timeMinutes, 0);
                }
                else
                    throw new ArgumentException("Zadal jsi záporný počet hodin");
            }
            else
                throw new ArgumentException("Zadal jsi hodiny ve špatném formátu. Mají vypadat jako 5,25 NEBO 6,5 apod");
            
            // Přidání nového denního záznamu - pouze záznam, bez bloků
            if(choice == 0)
                admin.AddEditWorkRecord(0,project_help, selectRecord, date_help, (decimal)timePlan_help.TotalHours);
            // Úprava stávajícího denního záznamu + možnost upravit bloky
            else if (choice == 1 && project_help != null && selectRecord != null)
            {
                // Přidání nebo úprava denního bloku
                if(part1Start != "" && part1Stop != "" && part1Content != "")
                {
                    // Denní záznam neobsahuje bloky -- vytvoří se nový
                    if(selectRecord.WorkParts.Count == 0)
                        AddEditWorkParts(0, part1Start, part1Stop, part1Content, null);
                    //   - Obsahuje, tedy jej chce uživatel upravit
                    else
                        AddEditWorkParts(1, part1Start, part1Stop, part1Content, selectRecord.WorkParts[0]);
                }
                // Přidání nebo úprava denního bloku  
                if (part2Start != "" && part2Stop != "" && part2Content != "")
                {
                    if (selectRecord.WorkParts.Count == 1)
                        AddEditWorkParts(0, part2Start, part2Stop, part2Content, null);
                    else
                        AddEditWorkParts(1, part2Start, part2Stop, part2Content, selectRecord.WorkParts[1]);
                }
                // Úprava stávajícího záznamu, avšak bez zadání ranního či odpoledního bloku

                admin.AddEditWorkRecord(1, project_help, selectRecord, date_help, (decimal)timePlan_help.TotalHours); 
            }   
            else
                throw new ArgumentException("Špatná volba nebo nenalezena instance Projektu");

            // Obnovení canvasu, aby na něm byli vykresleny aktuální informace včetně změny
            ConstructGraphicResult(true,false,false, selectMonthId_records, selectYearId_records);
        }

        /// <summary>
        /// Odebere denní záznam z kolekce v WORK PROJECT
        /// </summary>
        /// <param name="project">Projekt obsahující kolekci denních záznamů</param>
        /// <param name="record">Záznam k vymazání</param>
        public void RemoveWorkRecord(WorkRecord record)
        {
            if (record != null)
                admin.RemoveWorkRecord(record);
            else
                throw new ArgumentException("Špatná volba nebo nenalezena instance Projektu");
            // Obnovení canvasu, aby na něm byli vykresleny aktuální informace včetně změny
            ConstructGraphicResult(true,false,false, selectMonthId_records, selectYearId_records);
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
        public void AddEditWorkParts(byte choice, string start, string stop, string content,
            WorkPart partRecord)
        {
            DateTime stop_help;
            // Hodina start
            if (!DateTime.TryParse(start, out DateTime start_help))
                throw new ArgumentException("Zadal jsi čas ve špatném formátu! Má vypadat jako 7:15");            else

            // Hodina stop
            if (!DateTime.TryParse(stop, out stop_help))
                throw new ArgumentException("Zadal jsi čas ve špatném formátu! Má vypadat jako 7:15");
            // Náplň práce
            if (content == "")
                throw new ArgumentException("Nezadal jsi žádnou náplň práce");

            if (choice == 0 && selectRecord.WorkParts.Count >= 2)
                throw new ArgumentException("Nelze zadat další denní blok, protože tento den již obsahuje dva záznamy.");

            if (selectRecord != null && choice == 0 || selectRecord != null && partRecord != null && choice == 1)
                admin.AddEditWorkParts(choice, start_help, stop_help, content, partRecord, selectRecord);
            
            ConstructGraphicResult(true,false, false,selectMonthId_records, selectYearId_records);
        }

        /// <summary>
        /// Odebere ranní či odpolední blok z deního záznamu
        /// </summary>
        /// <param name="partRecord">Záznam,který bude odebrán</param>
        /// <param name="record"></param>
        public void RemoveWorkParts(WorkPart partRecord, WorkRecord record)
        {
            if (partRecord != null && record != null)
                admin.RemoveWorkParts(partRecord,record);
        }

        #endregion

        #region Load

        /// <summary>
        /// Načtení ID všech tříd
        /// </summary>
        public void LoadIDs()
        {
            admin.LoadIDs();
        }

        /// <summary>
        /// Načtení kolekce projektů
        /// </summary>
        public void LoadProjects()
        {
            admin.LoadProjects();
        }

        #endregion
    }
}
