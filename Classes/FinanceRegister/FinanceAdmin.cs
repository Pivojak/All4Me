using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Path = System.IO.Path;
using Microsoft.Win32;
using Draw = System.Drawing;

namespace All4Me
{
    /// <summary>
    /// Správce pro práci s finančními záznamy
    /// </summary>
    public class FinanceAdmin
    {
        /// <summary>
        /// Kolekce finančních záznamů
        /// </summary>
        public ObservableCollection<FinanceRecord> Records { get; private set; } = new ObservableCollection<FinanceRecord>();
        /// <summary>
        /// Kolekce nalezených záznamů po filtraci
        /// </summary>
        private ObservableCollection<FinanceRecord> findRecord = new ObservableCollection<FinanceRecord>();
        /// <summary>
        /// Cesta pro uložení Finance.xml na disku C
        /// </summary>
        private string pathFinance;
        /// <summary>
        /// Cesta pro uložení na disk C - základní část do AppData
        /// </summary>
        private string pathBase;
        /// <summary>
        /// Cesta pro uložení Balance.xml na disku C
        /// </summary>
        private string pathBalance;
        // Cesty pro uložení na disk D
        private string pathFinancePraceD;
        private string pathBalancePraceD;
        /// <summary>
        /// 0 - Bankovní účet || 1 - Hotovost
        /// </summary>
        public ObservableCollection<int> Balance { get; private set; } = new ObservableCollection<int>();

        /// <summary>
        /// Základní konstruktor správce Financí
        /// </summary>
        public FinanceAdmin()
        {
            try
            {
                // Vytvoření cesty do C app data
                pathBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "All4Me");
                if (!Directory.Exists(pathBase))
                    Directory.CreateDirectory(pathBase);
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.", pathFinance);
            }

            try
            {
                if (!Directory.Exists(@"d:\Prace\ZalohyProgramu\All4Me"))
                    Directory.CreateDirectory(@"d:\Prace\ZalohyProgramu\All4Me");
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.", pathFinancePraceD);
            }
            // Zkombinování cesty do AppData + název výsledného souboru
            pathFinance = Path.Combine(pathBase, "Finance.xml");
            pathBalance = Path.Combine(pathBase, "Balance.xml");
            // Spojení cesty na disk D + název souboru
            pathFinancePraceD = Path.Combine(@"d:\Prace\ZalohyProgramu\All4Me", "Finance.xml");
            pathBalancePraceD = Path.Combine(@"d:\Prace\ZalohyProgramu\All4Me", "Balance.xml");
        }

        /// <summary>
        /// Metoda, která vytiskne všechna data v RECORDS do txt souboru, na místo kam si uživatel vybere
        /// </summary>
        /// <param name="type">FALSE - forma pro EXCEL, TRUE - forma pro WORD</param>
        public void PrintFinanceRecords(bool type)
        {
            // Dialog pro uložení souboru TXT
            SaveFileDialog save = new SaveFileDialog
            {
                FileName = "FinanceData",
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };
            save.ShowDialog();
            // Získání cesty pro uložení, které zadal uživatel
            string path = save.FileName;
            // Seřazení záznamů podle data - sestupně
            IEnumerable sortRecords = Records.OrderByDescending(a => a.Date);
            // Uložení do souboru zadaného výše
            using (StreamWriter sw = new StreamWriter(path))
            {
                // Varianta pro EXCEL
                if(!type)
                    sw.WriteLine("Název\tČástka\tPopis\tMísto\tPlaceno z\tVýdaje / příjmy\tKategorie");
                foreach (FinanceRecord record in sortRecords)
                {
                    // Varianta pro EXCEL
                    if (!type)
                    {
                        sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", record.Name, record.Price, record.Description, record.Place, record.TypeBalance.ToString(), record.TypeRecord.ToString(), record.Category.ToString());
                        // Vyprazdni zasobnik - NUTNE
                        sw.Flush();
                    }
                    // Varianta pro WORD
                    else
                    {
                        // Vepsání do TXT souboru
                        sw.WriteLine("------------------------------------------\nZáznam ze dne:\t{0}", record.Date.ToShortDateString());
                        sw.WriteLine(" -Název\t\t\t{0}\n -Částka\t\t{1}\n" +
                                     " -Typ transakce\t\t{2}\n -Kategorie\t\t{3}",
                                      record.Name, record.Price, record.TypeRecord.ToString(), record.Category.ToString());
                        if (record.Description != "")
                            sw.WriteLine(" -Popis\t\t\t{0}", record.Description);
                        if (record.Place != "")
                            sw.WriteLine(" -Místo\t\t\t{0}", record.Place);

                        sw.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// Metoda, která vyhledá záznamy podle zadaných parametrů, buď podle jedno nebo až tří
        /// </summary>
        /// <param name="category">Kategorie výdajů</param>
        /// <param name="balance">Typ zůstatku bankovní účet nebo hotovost</param>
        /// <param name="month">Měsíc kdy byla transakce uskutečněna</param>
        public void FindRecords(TypeRecord type,Category category, TypeBalance balance, Month month)
        {
            // Nalezené záznamy
            ObservableCollection<FinanceRecord> findRecords = new ObservableCollection<FinanceRecord>();
            // Pomocná kolekce pro uložení mezivýsledku
            ObservableCollection<FinanceRecord> helpRecords = new ObservableCollection<FinanceRecord>();

            // Uživatel zadal měsíc pro vyhledávání
            if (month != Month.Other)
            {
                // Prochází záznamy a ukládá ty, které se shodují v měsící
                foreach (FinanceRecord record in Records)
                {
                    if (record.Date.Month == (int)month + 1)
                    {
                        findRecords.Add(record);
                    }
                }
            }
            // Uživatel zadal kategorii - pokud zadal i měsíc, vezmou se výsledky z toho hledání a v nich se hledá shoda s kategoriíí 
            // v opačném případě se prochází kompletní kolekce záznamů
            if (category != Category.Other)
            {
                // Získaná kolekce z předchozí podmínky je uložena do jiné proměnné a původní se naváže na prázdnou kolekci
                if (findRecords.Count != 0)
                {
                    helpRecords = findRecords;
                    findRecords = new ObservableCollection<FinanceRecord>();
                }
                    
                else
                    helpRecords = Records;
                // Pokud se shoduje kategorie záznamu, přidá se do kolekce
                foreach (FinanceRecord record in helpRecords)
                {
                    if (record.Category == category)
                        findRecords.Add(record);
                }
            }
            // Uživatel zadal typ balancu - Bankovní účet nebo hotovost
            if(balance != TypeBalance.Other)
            {
                if (findRecords.Count != 0)
                {
                    helpRecords = findRecords;
                    findRecords = new ObservableCollection<FinanceRecord>();
                }
                else
                    helpRecords = Records;

                foreach (FinanceRecord record in helpRecords)
                {
                    // Shoduje se typ zůstatku podle nastavení filtru
                    if(record.TypeBalance == balance)
                    {
                        findRecords.Add(record);
                    }
                }
            }
            // Výdaje nebo příjmy
            if (type != TypeRecord.Other)
            {
                if (findRecords.Count != 0)
                {
                    helpRecords = findRecords;
                    findRecords = new ObservableCollection<FinanceRecord>();
                }
                else
                    helpRecords = Records;

                foreach (FinanceRecord record in helpRecords)
                {
                    // Porovnání podle toho zda se jedná o Výdaj nebo o Příjem
                    if (record.TypeRecord == type)
                    {
                        findRecords.Add(record);
                    }
                }
            }
            findRecord = findRecords;
        }

        #region Enter,Remove record
        /// <summary>
        /// Metoda pro zadání nového finančního záznamu nebo jeho úpravu, při zadání nového umožňuje zadání všech parametrů nebo pouze povinných
        /// </summary>
        /// <param name="operation">0 - Nový záznam, 1 - upravit stávající</param>
        /// <param name="price">Částka</param>
        /// <param name="name">Název transakce</param>
        /// <param name="date">Datum transakce</param>
        /// <param name="place">Místo uskutečnění transakce</param>
        /// <param name="description">Popis</param>
        /// <param name="type">Výdaj / příjem</param>
        /// <param name="category">Kategorie výdaje</param>
        /// <param name="full">Zda se zadávají všechny parametry</param>
        /// <param name="record">Stávající záznam k upravení</param>
        public void EnterEditRecord(byte operation ,int price, string name, DateTime date,string place, string description, TypeRecord type, 
                                    Category category,TypeBalance balance,bool full, FinanceRecord record)
        {
            // Zapsání nové transakce v plné výši, uživatel vyplnil všechna pole
            if (operation == 0 && full)
            {
                Records.Add(new FinanceRecord(price, name, date, place, description, type, category, balance));
            }
            // Zapsání nové transakce v nekompletní podobě, uživatel nezadal všechna pole  
            else if (operation == 0 && !full)
            {
                Records.Add(new FinanceRecord(price, name, date, type, category, balance));
            }
            // Úpravy zůstatků podle nově zadané transakce
            if (operation == 0 && type == TypeRecord.Costs)
            {
                if ((Balance[(int)balance] - price) > 0)
                    Balance[(int)balance] -= price;
            }
            else if (operation == 0 && type == TypeRecord.Income)
            {
                if ((Balance[(int)balance] + price) > 0)
                    Balance[(int)balance] += price;
            }

            // Úprava stávající transakce 
            if (operation == 1)
            {
                // Získání hodnoty v ENUM, tedy pozici v daném enumu 0 - bankovní účet / 1 - hotovost
                int typeBalance_old = (int)record.TypeBalance;
                int typeBalance_new = (int)balance;
                // Původní záznam je výdaj
                if (record.TypeRecord == TypeRecord.Costs)
                {
                    // Přičte se původní suma ke starému zůstaku 
                    Balance[typeBalance_old] += record.Price;
                    // Podle toho zda se změnil i typ (Výdaj , příjem) tak se podle toho přičte / odečte
                    if (type == TypeRecord.Costs)
                        Balance[typeBalance_new] -= price;
                    else
                        Balance[typeBalance_new] += price;
                }
                // Původní záznam je příjem
                else if (record.TypeRecord == TypeRecord.Income)
                {
                    // Odečte se od původního zůstatku
                    Balance[typeBalance_old] -= record.Price;
                    if (type == TypeRecord.Costs)
                        Balance[typeBalance_new] -= price;
                    else
                        Balance[typeBalance_new] += price;
                }
                // Změním všechny atribity podle nově zadaných hodnot
                record.Price = price;
                record.Name = name;
                record.Date = date;
                record.Place = place;
                record.Description = description;
                // Změním všechny ENUM členy na nové zadání
                record.TypeRecord = type;
                record.Category = category;
                record.TypeBalance = balance;
            }
            SaveFinance(pathFinance);
            SaveBalance(pathBalance);

            SaveFinance(pathFinancePraceD);
            SaveBalance(pathBalancePraceD);
            
        }

        /// <summary>
        /// Metoda pro vymazání záznamu z kolekce
        /// </summary>
        /// <param name="record"></param>
        public void RemoveRecord(FinanceRecord record)
        {
            // Jedná se o výdaj - k danému zůstatku se přičte suma transakce, aby bylo odebrání výdaje korektní
            if(record.TypeRecord == TypeRecord.Costs)
                Balance[(int)record.TypeBalance] += record.Price;
            else if(record.TypeRecord == TypeRecord.Income)
                Balance[(int)record.TypeBalance] -= record.Price;

            if (Records.Contains(record))
                Records.Remove(record);

            SaveFinance(pathFinance);
            SaveBalance(pathBalance);

            SaveFinance(pathFinancePraceD);
            SaveBalance(pathBalancePraceD);
            

            
        }
        #endregion

        /// <summary>
        /// Spočítá veškeré parametry pro vyhodnocení statistiky za měsíc či rok - denní útrata, měsíční výdaje - příjmy apod
        /// </summary>
        /// <param name="month">Měsíc pro který se počítá statistika</param>
        /// <param name="year">Rok pro který se počítá statistika, vyplnit vždy i pro měsíc</param>
        /// <param name="type">0 - Výpočet dat pro MĚSÍC *** 1 - Výpočet dat pro ROK</param>
        /// <returns>Měsíc 10 hodnot ** Rok 17 hodnot ** Příjmy, Výdaje, Rozdíl, Denní a Týdenní
        /// ,* Výdaje dle týdnů 5,* Výdajů dle měsíce 12</returns>
        public List<double> CalculateStatisticParametres(Month month, int year, byte type)
        {
            List<double> ResultReport = new List<double>();
            // Roční parametry
            double YearIncome = 0;
            double YearCosts = 0;
            double YearDifferent = 0;
            // Měsíční parametry
            double MonthIncome = 0;
            double MonthCost = 0;
            double MonthDifferent = 0;
            // Roční ukazatele - denní útrata, průměrná týdení útrata, měsíční útrata
            double Year_DayAverageCosts = 0;
            double Year_WeekAverageCosts = 0;
            // Měsíční ukazatele - denní útrata, týdenní průměrná útrata
            double Month_DayAverageCosts = 0;
            double Month_WeekAverageCosts = 0;
            // Výdaje v konkrétních týdnech
            double[] WeekCosts = new double[5];
            double[] MonthCosts = new double[12];
            List<DateTime> weeks = new List<DateTime>();
            if (type == 0)
            {
                weeks = Week.GetWeek(((int)month + 1), year);
            }
            // Prochází se všechny finanční záznamy     
            foreach (FinanceRecord record in Records)
            {
                // Požadavek na výpočet pro měsíc, a měsíc záznamu je shodný s počítaným
                if(type == 0 && record.Date.Year == year)
                {
                    // Příjmy
                    if(record.Date.Month == (int)month + 1 && record.TypeRecord == TypeRecord.Income)
                    {
                        MonthIncome += record.Price;
                    }
                    // Výdaje
                    else if(record.Date.Month == (int)month + 1 && record.TypeRecord == TypeRecord.Costs)
                    {
                        MonthCost += record.Price;
                    }
                    DateTime day = record.Date;
                    // Jedná se o výdaj a měsíc je buď ten vybraný nebo předchozí, nebo budoucí (kvůli tomu, že ve výpisu je první týden od minulého měsíce a poslední zasahuje
                    // někdy do dalšího měsíce)                                 Aktuální měsíc                  Předchozí měsíc                     Budoucí měsíc
                    if(record.TypeRecord == TypeRecord.Costs && (record.Date.Month == (int)month + 1 || record.Date.Month == (int)month) || record.Date.Month == (int)month + 2)
                    {
                        // Výdaje pro každý konkrétní týden v měsíci
                        if (day >= weeks[0] && day <= weeks[1])
                            WeekCosts[0] += record.Price;
                        else if (day >= weeks[2] && day <= weeks[3])
                            WeekCosts[1] += record.Price;
                        else if (day >= weeks[4] && day <= weeks[5])
                            WeekCosts[2] += record.Price;
                        else if (day >= weeks[6] && day <= weeks[7])
                            WeekCosts[3] += record.Price;
                        // Ošetření pro případ kdy budou v měsíci pouze 4 týdny
                        if (weeks.Count == 10)
                        {
                            if (day >= weeks[8] && day <= weeks[9])
                                WeekCosts[4] += record.Price;
                        }
                    }

                }
                // Výpočet parametrů pro ROK
                else if (record.Date.Year == year && type == 1)
                {
                    if (record.TypeRecord == TypeRecord.Income)
                    {
                        YearIncome += record.Price;
                    }
                    else if (record.TypeRecord == TypeRecord.Costs)
                    {
                        YearCosts += record.Price;
                        MonthCosts[record.Date.Month - 1] += record.Price;
                    }
                    
                }
            }
            // Průměrné výdaje - den, týden a měsíc pro roční výpočty
            if(type == 0 && MonthIncome != 0 && MonthCost != 0)
            {
                MonthDifferent = MonthIncome - MonthCost;
                Month_DayAverageCosts = Math.Round(MonthCost / DateTime.DaysInMonth(year, (int)month + 1));
                Month_WeekAverageCosts = Math.Round(MonthCost / 4.35);
            }
            // Průměrné výdaje - den, týden pro měsíční výpočty
            else if(type == 1 && YearIncome != 0 && YearCosts != 0)
            {
                YearDifferent = YearIncome - YearCosts;
                Year_DayAverageCosts = Math.Round(YearCosts / 365.25);
                Year_WeekAverageCosts = Math.Round(YearCosts / 52.18);
            }
            // Uložení všech získaných hodnot do kolekce, podle toho zda se počítá ROK či Měsíc
            // Měsíc - kolekce obsahuje 10 záznamů
            // Rok - kolekce obsahuje 6 hodnot
            if (type == 0)
            {
                ResultReport.Add(MonthIncome);
                ResultReport.Add(MonthCost);
                ResultReport.Add(MonthDifferent);


                ResultReport.Add(Month_DayAverageCosts);
                ResultReport.Add(Month_WeekAverageCosts);
                foreach (double d in WeekCosts)
                    ResultReport.Add(d);
            }

            if (type == 1)
            {
                ResultReport.Add(YearIncome);
                ResultReport.Add(YearCosts);
                ResultReport.Add(YearDifferent);
                ResultReport.Add(Year_DayAverageCosts);
                ResultReport.Add(Year_WeekAverageCosts);
                foreach(double cost in MonthCosts)
                    ResultReport.Add(cost);
            }
            return ResultReport;
        }


        #region ZObrazovacíMetodyCANVAS

        /// <summary>
        /// Metoda pro vypracování všech transakncí do grafické podoby. Transakce také seřadí sestupně
        /// </summary>
        /// <returns>List obsahující pole (obdelnik, castka, datum, nazev, popis)</returns>
        /// <param name="type">0 - Příjmy a výdaje | 1 - Příjmy | 2 - Výdaje</param> | 3 - VybranáKolekceMimo</param>
        public List<FinanceGraphicRecord> ViewFinanceResults(byte type)
        {
            List<FinanceGraphicRecord> result = new List<FinanceGraphicRecord>();
            IEnumerable<FinanceRecord> sortRecords = Records.OrderByDescending(a => a.Date);
            if (type == 3 && findRecord.Count != 0)
                sortRecords = findRecord.OrderByDescending(a => a.Date);
            
            foreach (FinanceRecord rec in sortRecords)
            {
                // Příjmy a výdaje
                if (type == 0)
                    result.Add(new FinanceGraphicRecord(rec));
                // Příjmy
                else if (rec.TypeRecord == TypeRecord.Income && type == 1)
                    result.Add(new FinanceGraphicRecord(rec));
                // Výdaje
                else if (rec.TypeRecord == TypeRecord.Costs && type == 2)
                    result.Add(new FinanceGraphicRecord(rec));
                else if (type == 3 && findRecord.Count != 0)
                    result.Add(new FinanceGraphicRecord(rec));
            }
            return result;

        }

        /// <summary>
        /// Vyjádření zůstatků na účtě a hotovosti
        /// </summary>
        /// <returns>List, který obsahuje (Obdelnik, obdelnik, nazev, castka, linie)</returns>
        public int[] ShowBalance()
        {
            int[] balances = new int[2];
            // Bankovní účet
            balances[0] = Balance[0];
            // Hotovost
            balances[1] = Balance[1];

            return balances;
        }


        /// <summary>
        /// Metoda pro konstrukci Grafu výdajů v Kategoriích (sloupcový graf)
        /// </summary>
        /// <returns>Kolekci objektů obsahující Sloupec grafu, Název kategorie a Hodnota kategorie</returns>
        public List<object[]> ConstructChartColumn(Month month,int year)
        {
            List<object[]> shapes = new List<object[]>();
            double[] categorySum = new double[8];
            foreach(FinanceRecord record in Records)
            {
                // Záznam se shoduje se vstupem v měsíci a roce + jedná se o výdaj
                if(record.TypeRecord != TypeRecord.Income && record.Date.Month == (int)(month + 1) && record.Date.Year == year)
                {
                    categorySum[(int)record.Category] += record.Price;
                }
            }
            // Proměnná pro uložení celkových nákladů ve všech kategoriích
            double totalSum = categorySum.Sum();
            int i = 0;
            // Pole, které obsahuje výdaje v daných 8 kategoriích
            double[] categoryPrice = new double[8];
            foreach (int category in categorySum)
            {

                categoryPrice[i] = categorySum[i];
                // Získání poměru dané kategorie, tedy jak vysoký sloupec to bude
                categorySum[i] = (category / totalSum) * 500;
                // Zaokrouhlení
                categorySum[i] = Math.Floor(categorySum[i]);
                categoryPrice[i] = Math.Floor(categoryPrice[i]);
                i++;     
            }
            string[] categoryNames = { "Jídlo", "Zábava", "Pití", "Auto", "Pojištění", "Bydlení", "Oblečení", "Ostatní", };
            string[] columnNames = { "jidlo", "zabava", "piti", "auto", "pojisteni", "bydleni", "obleceni", "ostatni", };
            for (int j = 0; j < categorySum.Count(); j++)
            {
                Rectangle column = new Rectangle
                {
                    Width = 60,
                    Height = categorySum[j],
                    Fill = Brushes.Lime,
                    Name = columnNames[j]
                };

                TextBlock descColumn = new TextBlock
                {
                    Text = categoryNames[j],
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                };

                TextBlock costCategory = new TextBlock
                {
                    Text = categoryPrice[j].ToString(),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black,
                };
                // Sloučí objekty pro jeden sloupec do POLE objektů
                object[] oneColumn = new object[3];

                oneColumn[0] = column;
                oneColumn[1] = descColumn;
                oneColumn[2] = costCategory;
                shapes.Add(oneColumn);
            }
            return shapes;
        }


        /// <summary>
        /// Metoda vracející záznamy z dané kategorie, aby bylo možné je vykreslit v KATEGORIÍCH  ve vyskakovacím oknu
        /// </summary>
        /// <param name="category">Kategorie pro kterou se vytváří grafické menu</param>
        /// <returns>Kolekci objektů složenou z TEXT BLOCKU</returns>
        public List<FinanceRecord> ConstructInfoMenu(Category category,Month month, int year)
        {
            List<object[]> shapes = new List<object[]>();
            List<FinanceRecord> selectRecords = new List<FinanceRecord>();

            foreach (FinanceRecord record in Records.OrderByDescending(a => a.Date))
            {
                // Shoda v kategorii, jedná se o výdaj a shoda v měsíci a roce
                if (record.Category == category && record.TypeRecord != TypeRecord.Income 
                    && record.Date.Month == (int)month+1 && record.Date.Year == year)
                {
                    // Přidání do kolekce vybraný záznam
                    selectRecords.Add(record);
                }
            }
            return selectRecords;
        }
        #endregion

        #region Save / Load
        /// <summary>
        /// Metoda pro uložení kolekce balance do souboru XML
        /// </summary>
        private void SaveBalance(string path)
        {
            XmlSerializer serializer = new XmlSerializer(Balance.GetType());

            using (StreamWriter writter = new StreamWriter(path))
            {
                writter.WriteLine(Balance[0]);
                writter.WriteLine(Balance[1]);
                writter.WriteLine(FinanceRecord.Id);
            }

        }

        /// <summary>
        /// Načtení XML souboru obsahující uložené balance v kolekci BALANCE
        /// </summary>
        public void LoadBalance()
        {
            XmlSerializer serializer = new XmlSerializer(Balance.GetType());

            if (File.Exists(pathBalance))
            {
                using (StreamReader reader = new StreamReader(pathBalance))
                {
                    Balance = new ObservableCollection<int>();
                    Balance.Add(int.Parse(reader.ReadLine()));
                    Balance.Add(int.Parse(reader.ReadLine()));
                    FinanceRecord.Id = (int.Parse(reader.ReadLine()));
                }
            }
            else
                Balance = new ObservableCollection<int>();
        }



        /// <summary>
        /// Metoda pro uložení kolekce finančních záznamů do souboru XML
        /// </summary>
        public void SaveFinance(string path)
        {
            XmlSerializer serializer = new XmlSerializer(Records.GetType());

            using (StreamWriter writter = new StreamWriter(path))
            {
                serializer.Serialize(writter, Records);
            }

        }

        /// <summary>
        /// Načtení XML souboru obsahující uložené finanční záznamy v kolekci RECORDS
        /// </summary>
        public void LoadFinance()
        {
            XmlSerializer serializer = new XmlSerializer(Records.GetType());

            if (File.Exists(pathFinance))
            {
                using (StreamReader reader = new StreamReader(pathFinance))
                {
                    Records = (ObservableCollection<FinanceRecord>)serializer.Deserialize(reader);
                }
            }
            else
                Records = new ObservableCollection<FinanceRecord>();
        }

        #endregion
    }

}
