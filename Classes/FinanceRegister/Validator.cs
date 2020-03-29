using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Třída sloužící pro validaci vstupních dat pro Finanční blok (komunikuje s View - tedy formuláři)
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// Událost kliknutí na transakci pro změnu. Předá jako sender třídu FinanceGraphicRecord
        /// </summary>
        public event EventHandler RectangleButtonClick;
        /// <summary>
        /// Událost, která informuje o tom, že jsou práve zobrazovány filtrovaná data
        /// </summary>
        public event EventHandler ViewSelectData;
        /// <summary>
        /// Vnitřní instance admina 
        /// </summary>
        private FinanceAdmin admin;
        /// <summary>
        /// Kolekce pro uložení IDs, jež jsou vykresleny na první stranu CANVAS
        /// </summary>
        private List<int> printAtFirstPage;
        /// <summary>
        /// Plátno na které se vykreslují data 
        /// </summary>
        private Canvas canvas;
        /// <summary>
        /// Kolekce, která obsahuje seznam 11 roků - vždy aktuální rok + 5 předchozích + 5 budoucích
        /// </summary>
        public List<int> Years { get; private set; }
        /// <summary>
        /// Informace o tom, jaký měsíc byl naposled vybrát ve formuláři
        /// </summary>
        private Month categoryStatistic_ActualSelectMonth = (Month)DateTime.Now.Month - 1;
        /// <summary>
        /// Informace o tom, jaký rok byl naposled vybrán ve formuláři
        /// </summary>
        private int categoryStatistic_ActualSelectYear = DateTime.Now.Year;
        /// <summary>
        /// Poslední záznam vykreslený na první straně
        /// </summary>
        public FinanceGraphicRecord LastPageRecord { get; private set; }
        /// <summary>
        /// Dnešní datum
        /// </summary>
        public DateTime TodayDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        /// <summary>
        /// Základní konstruktor - vytvoří instanci správce a definuje výčet roků - Tento +- 5 let
        /// </summary>
        public Validator()
        {
            admin = new FinanceAdmin();
            Years = new List<int>();
            for(int i = -5; i < 5; i++)
            {
                Years.Add(DateTime.Now.Year + i);
            }
            
        }
        /// <summary>
        /// Nastavení CANVASU, tak aby každá metoda nemusela přijímat plátno na vstupu
        /// </summary>
        /// <param name="canvas"></param>
        public void DefineCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// Spočítá veškeré parametry pro vyhodnocení statistiky za měsíc či rok - denní útrata, měsíční výdaje - příjmy apod
        /// </summary>
        /// <param name="month">Měsíc ve kterém se spočítá statistika</param>
        /// <param name="yearString">Rok ve kterém se spočítá statistika</param>
        /// <param name="type">0 - Výpočet pro měsíc ** 1 - Výpočet pro rok</param>
        public void CalculateStatisticParametres(int month, string yearString, byte type)
        {
            int.TryParse(yearString, out int year);
            // Získám měsíc ve výčtovém typu
            Month monthType = Month.Other;
            if(month >= 0 && month <= 12)
            {
                monthType = (Month)month;
            }
            // Kolekce, která obsahuje první a konečný den zkoumaných týdnů
            List<DateTime> weeks = Week.GetWeek(month + 1, year);
            List<double> result = new List<double>();
            // Data pro měsíc nebo pro rok
            if (monthType != Month.Other && type == 0)
                result = admin.CalculateStatisticParametres(monthType, year, type);
            else if (type == 1)
                result = admin.CalculateStatisticParametres(Month.Other, year, type);
            // Vzdálenosti záznamů od sebe 
            int headHeight = 35;
            // Výška jednoho řádků 
            int heightChange = 22;
            // Pomocná proměnná, která obsahuje pozici pro vykreslení nového objektu
            int sumHeight = 0;
            // Výška předělů ve vykreslení - tmavě zelené bloky
            int splitHeight = 0;
            // Pomocné incrementy
            int n = 0;
            int monthInc = 0;
            int inc = 0;
            canvas.Children.Clear();
            string[] names = new string[] { "Příjmy", "Výdaje", "Bilance", "Den", "Týden", "Týden", "Týden", "Týden", "Týden", "Týden",
                "Leden","Únor","Březen","Duben","Květen","Červen","Červenec","Srpen","Září","Říjen","Listopad","Prosinec"};
            // Pro každou získanou honotu ze správce se vykreslí řádek - modrý a bílý obdelník + název a suma 
            for (int i = 0; i < result.Count; i++)
            {
                sumHeight = i * headHeight;
                if (i > 4)
                {
                    if(type == 0)
                    {
                        heightChange = 50;
                        sumHeight = 5 * 35 + ((i - 5) * 55);
                    }
                    else if(type == 1)
                    {
                        heightChange = 22;
                        sumHeight = i * 35;
                    }
                }
                   
                if(type == 1 && i == 5)
                {
                    monthInc += 5;
                }
                // Získání grafických objektů pro vykreslení na plátno - generuje je statická třída GRAPHIC
                (Rectangle left, Rectangle right, TextBlock name, TextBlock price) =
                      Graphic.BlueWhitePanel(names[i+monthInc], result[i].ToString(), new int[] { 200, 100 }, new int[] { heightChange, 22 }, new int[] { 3, 3 });
                // Přiřezení objektů na plátno
                canvas.Children.Add(left);
                canvas.Children.Add(right);
                canvas.Children.Add(name);
                canvas.Children.Add(price);
                // Vykreslení tmavě zeleného předělů v definovaných místech plátna
                if(i == 0 || i == 3 || i == 5)
                {
                    string[] splitNames = new string[] { "Základní informace", "Průměrné výdaje", "Týdení výdaje", "Měsíční výdaje"};
                    (Rectangle split_rectangle, TextBlock split_text) =
                        Graphic.SplitPanel(30, 425, 3, new SolidColorBrush(Color.FromArgb(255, 20,82,87)), splitNames[n]);
                    n++;
                    // Pro vykreslení ROKU - přeskočí název Týdenní výdaje a použije Měsíční výdaje
                    if (type == 1 && n == 2)
                        n++;

                    canvas.Children.Add(split_rectangle);
                    canvas.Children.Add(split_text);
                    // Tmavě zelený obdelník, který odděluje bloky
                    Canvas.SetLeft(split_rectangle, 5); Canvas.SetTop(split_rectangle, 8 + sumHeight + splitHeight);
                    // Popis předělovacího bloku - NADPIS SEKCE
                    Canvas.SetLeft(split_text, 150); Canvas.SetTop(split_text, 10 + sumHeight + splitHeight);
                    splitHeight += 35;
                }
                // Podmínka pro vykreslení datumů pro týdny
                if (type == 0 && i > 4)
                {            
                    TextBlock weekDate = new TextBlock
                    {
                        FontSize = 15,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Black,
                        Text = "- " + weeks[inc].ToShortDateString() + " - " + weeks[inc + 1].ToShortDateString()
                    };

                    canvas.Children.Add(weekDate);
                    Canvas.SetLeft(weekDate, 20); Canvas.SetTop(weekDate, 35 + sumHeight + splitHeight);
                    if (inc < result.Count - 1)
                        inc += 2;
                }

                // Obdelnik pod typem zůstatku -- Světle modrá
                Canvas.SetLeft(left, 10); Canvas.SetTop(left, 10 + sumHeight + splitHeight);
                // Obdelnik pod hodnotou zůstatku -- bílý
                Canvas.SetLeft(right, 230); Canvas.SetTop(right, 10 + sumHeight + splitHeight);
                // Typ zůstatku Bankovní účet nebo Hotovost
                Canvas.SetLeft(name, 15); Canvas.SetTop(name, 10 + sumHeight + splitHeight);
                // Hodnota zůstatku v dané platformě
                Canvas.SetLeft(price, 290); Canvas.SetTop(price, 10 + sumHeight + splitHeight);
            }
            
        }

        /// <summary>
        /// Metoda, která vyhledá záznamy podle zadaných parametrů, buď podle jedno nebo až tří
        /// </summary>
        /// <param name="category">Kategorie výdajů</param>
        /// <param name="balance">Typ zůstatku bankovní účet nebo hotovost</param>
        /// <param name="month">Měsíc kdy byla transakce uskutečněna</param>
        public void FindFinanceRecords(TypeRecord type,Category category, TypeBalance balance, Month month)
        {
            admin.FindRecords(type,category, balance, month);
            ViewGraphicFinance(3, true, false);
        }

        /// <summary>
        /// Obsluha události, která se vyvolá událostí ve FinanceGraphicRecord
        /// </summary>
        /// <param name="sender">FinanceGraphicRecord třída</param>
        /// <param name="e"></param>
        public void RectangleClick(object sender, EventArgs e)
        {
            // Vyvolám událost třídy Validator, na kterou reaguje formulář
            RectangleButtonClick(sender,EventArgs.Empty);
        }

        #region ValidaceEnum
        /// <summary>
        /// Metoda pro validování ID comboBoxu a měsíců
        /// </summary>
        /// <param name="idCategory">Id comboBoxu pro Kategorii výdajů</param>
        /// <param name="idType">Id comboBoxu pro Typ</param>
        /// <param name="idBalance">Typ zůstatku účet / hotovost</param>
        /// <param name="idMonth">Měsíce v roce 0 - Leden, 11 - Prosinec, ostatní Month.Any případně</param>
        /// <returns></returns>
        public (TypeRecord type, Category category, TypeBalance balance, Month month) 
            EnumValidator(int idCategory, int idType, int idBalance, int idMonth)
        {
            Category selectCategory = Category.Other;
            TypeRecord selectType;
            TypeBalance balance;
            Month selectMonth;
            if (idCategory != -1 && idType != 1)
            {
                selectCategory = (Category)idCategory;
            }
            else
                selectCategory = Category.Other;

            if (idMonth >= 0 && idMonth <= 11)
                selectMonth = (Month)idMonth;
            else
                selectMonth = Month.Other;

            if (idType == 0)
                selectType = TypeRecord.Costs;
            else if (idType == 1)
                selectType = TypeRecord.Income;
            else
                selectType = TypeRecord.Other;

            if (idBalance == 0)
                balance = TypeBalance.BankAccount;
            else if (idBalance == 1)
                balance = TypeBalance.Cash;
            else
                balance = TypeBalance.Other;

            return (selectType,selectCategory,balance,selectMonth);
        }

        /// <summary>
        /// Obrácený type EnumValidator - na základě zadaných Enums vytvoří pole ID daných prvků
        /// </summary>
        /// <param name="type">Type transakce Výdaj - Příjem</param>
        /// <param name="category">Kategorie výdajů</param>
        /// <param name="balance">Typ zůstatku Bankovní účet - Hotovost</param>
        /// <returns>[Typ transakce, Kategorie, Typ zůstatku pro platbu]</returns>
        public int[] EnumValidatorReverse(TypeRecord type, Category category, TypeBalance balance)
        {
            int[] ids = new int[3];

            if ((int)category >= 0 && (int)category <= 6)
                ids[0] = (int)category;
            else
                ids[0] = 7;

            if (type == TypeRecord.Costs)
                ids[1] = 0;
            else if (type == TypeRecord.Income)
                ids[1] = 1;
            else
                ids[1] = -1;

            if (balance == TypeBalance.BankAccount)
                ids[2] = 0;
            else if(balance == TypeBalance.Cash)
                ids[2] = 1;
            else
                ids[2] = -1;

            return ids;
        }
        #endregion

        #region ChartCategory
        /// <summary>
        /// Metoda pro konstrukci Grafu výdajů v Kategoriích (sloupcový graf)
        /// </summary>
        public void ConstructChartColumn(int month, string yearString)
        {
            canvas.Children.Clear();
            // Převedení hodnoty roku v ComboBoxu na int
            int.TryParse(yearString, out int year);
            // Získání objektů na canvas ze správce
            List<object[]> rectangle = admin.ConstructChartColumn((Month)month,year);
            // Vzdálenosti popisu kategoriíí od leva
            int[] descriptionDistance = { 20, 90, 175, 245, 310, 390, 460, 540 };
            // Vzdálenost sumy výdajů
            int[] costDistance = { 25, 100, 180, 250, 325, 405, 475, 550 };
            categoryStatistic_ActualSelectMonth = (Month)month;
            categoryStatistic_ActualSelectYear = year;
            // Projde všech 8 sloupců a vykreslí graf
            for (int i = 0; i < 8; i++)
            {
                Rectangle column = rectangle[i][0] as Rectangle;
                TextBlock description = rectangle[i][1] as TextBlock;
                TextBlock costCategory = rectangle[i][2] as TextBlock;
                // Přidání obsluhy události kliknutí na sloupec grafu
                column.MouseMove += Column_MouseMove;
                column.MouseLeave += Column_MouseLeave;
                // Přiřazení na plátno
                canvas.Children.Add(column);
                canvas.Children.Add(description);
                canvas.Children.Add(costCategory);
                // Zarovnání na plátnu
                Canvas.SetBottom(column, 50); Canvas.SetLeft(column, i * 75 + 10);
                Canvas.SetBottom(description, 20); Canvas.SetLeft(description, descriptionDistance[i]);
                Canvas.SetBottom(costCategory, column.Height + 60); Canvas.SetLeft(costCategory, costDistance[i]);
            } 
        }
        
        /// <summary>
        /// Obsluha při opuštění myši jednoho ze sloupců GRAFU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Column_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Vrátím barvu sloupce na původní, která byla před najetím na sloupec
            (sender as Rectangle).Fill = Brushes.Lime;
            DestructInfoMenu();
        }

        /// <summary>
        /// Obsluha reagující na najetí myši na sloupec GRAFU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Column_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Změním barvu sloupce na který uživatel ukazuje myší
            (sender as Rectangle).Fill = Brushes.Red;
            ConstructInfoWindow((sender as Rectangle).Name);
        }

        /// <summary>
        /// Smaže plátno a vykreslí pouze GRAF, zmizí tedy vyskakovací okno
        /// </summary>
        private void DestructInfoMenu()
        {
            // Smazání plátna
            canvas.Children.Clear();
            // Opětovné vykreslení grafu bez přítomnosti vyskakovacího okna
            ConstructChartColumn((int)categoryStatistic_ActualSelectMonth, categoryStatistic_ActualSelectYear.ToString());
        }
     
        /// <summary>
        /// Metoda konstruující vyskakovací menu u grafického znázornění výdajů v Kategoriích
        /// </summary>
        /// <param name="categoryName"></param>
        private void ConstructInfoWindow(string categoryName)
        {
            Category category = Category.Other;
            // Validace názvu kategorie, uložené v názvu obdelníku, který znázorňuje daný sloupec
            switch (categoryName)
            {
                case "jidlo":
                    category = Category.Food;
                    break;
                case "zabava":
                    category = Category.Funny;
                    break;
                case "piti":
                    category = Category.Drink;
                    break;
                case "auto":
                    category = Category.Car;
                    break;
                case "pojisteni":
                    category = Category.Insurance;
                    break;
                case "bydleni":
                    category = Category.Housing;
                    break;
                case "obleceni":
                    category = Category.Cloth;
                    break;
                case "ostatni":
                    category = Category.Other;
                    break;
            }
            // Třída GRAPHIC vytvoří dva obdelníky + popis a vrátí jej
            (Rectangle menu, Rectangle topRectangle, TextBlock desc)  = 
                Graphic.CategoryChartInfoWindow(200, 35, 585, 3, categoryName);
            // Získané objekty jsou zarovnány na CANVAS
            canvas.Children.Add(menu);
            canvas.Children.Add(topRectangle);
            canvas.Children.Add(desc);
            
            Canvas.SetBottom(menu, 500); Canvas.SetLeft(menu, 10);
            Canvas.SetBottom(topRectangle, 670); Canvas.SetLeft(topRectangle, 10);
            Canvas.SetBottom(desc, 675); Canvas.SetLeft(desc, 100);
            // Ze správce získám kolekci záznamů, jež odpovídají dané kategorii
            List<FinanceRecord> selectRecords = admin.ConstructInfoMenu(category,categoryStatistic_ActualSelectMonth, categoryStatistic_ActualSelectYear);
            int recordDistance = 20;
            int columnDistance = 100;
            int leftDistanceColumn = 15;
            int j = 0;
            // Získané záznamy projdu a pro každý vytvořím grafickou realizaci
            foreach(FinanceRecord record in selectRecords)
            {
                string name = "";
                if (record.Name.Count() < 17)
                    name = record.Name;
                else
                    name = record.Name.Substring(0, 14) + "..";

                if(j == 8)
                {
                    leftDistanceColumn = 300;
                    j = 0;
                }
                (TextBlock recordName, TextBlock recordPrice, TextBlock recordDate) = 
                    Graphic.FinanceRecordRowWrite(14, name, record.Price.ToString(), record.Date.ToShortDateString());

                canvas.Children.Add(recordName);
                canvas.Children.Add(recordPrice);
                canvas.Children.Add(recordDate);
                Canvas.SetBottom(recordName, 660 - recordDistance * j - 20); Canvas.SetLeft(recordName, leftDistanceColumn);
                Canvas.SetBottom(recordPrice, 660 - recordDistance * j - 20); Canvas.SetLeft(recordPrice, leftDistanceColumn  + columnDistance + 30);
                Canvas.SetBottom(recordDate, 660 - recordDistance * j - 20); Canvas.SetLeft(recordDate, leftDistanceColumn + columnDistance * 2);
                j++;
            }
        }
        #endregion

        #region DrawMetods
        /// <summary>
        /// Přidá na canvas vykreslené transakce, uložené ve tříde FinanceAdmin
        /// </summary>
        /// <param name="canvas">Plátno na které bude zakresleno</param>
        /// <param name="selectRecords">0 - Příjmy a Výdaje, 1 - Příjmy, 2 - Výdaje, 3 - VybranáKolekceMimo</param>
        /// <param name="firstPage">Ano / ne zda zobrazit první stranu</param>
        /// <param name="nextPage">Ano / ne zda zobrazit druhou stranu</param>
        public void ViewGraphicFinance(byte selectRecords,bool firstPage , bool nextPage)
        {
            // Grafická reprezentace hlavičky, kde jsou zobrazeny zůstatky na účtu a v hotovosti
            int[] graphicHead = admin.ShowBalance();
            List<FinanceGraphicRecord> graphicEvent = admin.ViewFinanceResults(0);
            List<FinanceGraphicRecord> graphicSelectEvent = new List<FinanceGraphicRecord>();
            List<FinanceGraphicRecord> graphic = new List<FinanceGraphicRecord>();
            canvas.Children.Clear();
            // Získám ze správce kolekci, která obsahuje všechny prvky pro příjmy nebo výdaje
            if (selectRecords == 1)
                graphicSelectEvent = admin.ViewFinanceResults(1);
            else if (selectRecords == 2)
                graphicSelectEvent = admin.ViewFinanceResults(2);
            else if (selectRecords == 3)
                graphicSelectEvent = admin.ViewFinanceResults(3);
            // Smažu kolekci, která obsahuje ID záznamů na první straně, jelikož je vyžadován výpis první strany
            if (firstPage)
                printAtFirstPage = new List<int>();

            // Vyvolám událost, která stanovuje, že se budou zobrazovat filtrovaná data
            if (graphicSelectEvent.Count != 0 && selectRecords == 3)
                ViewSelectData(null, EventArgs.Empty);

            // Vzdálenosti na Canvasu od vrchu, vzdálenost transakcí, odstup hlavičky o transakcí, výška hlavičky
            int[] top = { 10, 12, 38, 55 };
            //int eventDistance = 70;
            int eventDistance = 56;
            int headDistance = 25;
            int headHeight = 70;
            int i = 0;
            // Vytvoří na plátně horní řádky, tedy dva obdelníky ve kterých je (Bankovní účet nebo Hotovost)
            // a dále hodnota v dané variantě v Kč
            string[] balanceNames = { "Bankovní účet", "Hotovost" };

            Line linea = new Line
            {
                X1 = 0,
                Y1 = 75,
                X2 = 335,
                Y2 = 75,
                Stroke = Brushes.Gray,
                StrokeThickness = 4
            };
            canvas.Children.Add(linea);
            for (int j = 0; j < 2; j++)
            {
                (Rectangle left, Rectangle right, TextBlock name, TextBlock price) = 
                    Graphic.BlueWhitePanel(balanceNames[j],graphicHead[j].ToString(), new int[] { 150, 150 }, new int[] { 20, 20 }, new int[] { 3, 3 });
                canvas.Children.Add(left);
                canvas.Children.Add(right);
                canvas.Children.Add(name);
                canvas.Children.Add(price);

                // Obdelnik pod typem zůstatku -- Světle modrá
                Canvas.SetLeft(left, 10); Canvas.SetTop(left, 10 + j * headDistance);
                // Obdelnik pod hodnotou zůstatku -- bílý
                Canvas.SetLeft(right, 180); Canvas.SetTop(right, 10 + j * headDistance);
                // Typ zůstatku Bankovní účet nebo Hotovost
                Canvas.SetLeft(name, 15); Canvas.SetTop(name, 10 + j * headDistance);
                // Hodnota zůstatku v dané platformě
                Canvas.SetLeft(price, 240); Canvas.SetTop(price, 10 + j * headDistance);                
            }
            
            // Přiřazení správné kolekce pro vykreslení, tak aby byla metoda univerzální a mohla vykreslit
            // - Příjmy a výdaje
            // - Příjmy
            // - Výdaje podle zadání v formuláře
            if (selectRecords == 0)
                graphic = graphicEvent;
            else if (selectRecords == 1)
                graphic = graphicSelectEvent;
            else if (selectRecords == 2)
                graphic = graphicSelectEvent;
            else if (selectRecords == 3)
                graphic = graphicSelectEvent;
               
            bool printNextPage = false;

            foreach (FinanceGraphicRecord graphicRecord in graphic)
            {
                // Třída FinanceGraphicRecord má událost, která se vyvolá při kliknutí na jeden z 
                // jejích Rectanglů
                graphicRecord.RectangleEvent += RectangleClick;


                if (nextPage && LastPageRecord.FinanceRecord.ID == graphicRecord.FinanceRecord.ID)
                    printNextPage = true;

                if (eventDistance * i > 600)
                {
                    LastPageRecord = graphicRecord;
                    break;
                }
                // Pokud vykresluji na první stranu, tak ukládám všechny ID, která tam vykreslím, tak aby
                // nebyly vykresleni také na stranu druhou
                if (firstPage)
                    printAtFirstPage.Add(graphicRecord.FinanceRecord.ID);
                // Vykreslení útvarů
                //  - jedná se o první stranu
                //  - jedná se o druhou a záznam není vykreslen již na straně první
                if(firstPage || printNextPage && !printAtFirstPage.Contains(graphicRecord.FinanceRecord.ID))
                {
                    // Přidám geometrické útvary a další první na CANVAS
                    canvas.Children.Add(graphicRecord.RectangleTop);
                    canvas.Children.Add(graphicRecord.RectangleDown);
                    canvas.Children.Add(graphicRecord.RectangleLeft);
                    canvas.Children.Add(graphicRecord.Price);
                    canvas.Children.Add(graphicRecord.Date);
                    canvas.Children.Add(graphicRecord.Title);

                    // Následně tyto prvky zarovnám na plátně podle požadavků
                    // Spodni obdelník
                    Canvas.SetLeft(graphicRecord.RectangleDown, 10);
                    Canvas.SetTop(graphicRecord.RectangleDown, headHeight + top[0] + 25 + i * eventDistance);
                    // Horni obdelník
                    Canvas.SetLeft(graphicRecord.RectangleTop, 10);
                    Canvas.SetTop(graphicRecord.RectangleTop, headHeight + top[0] + i * eventDistance);
                    // Levý obdelník
                    Canvas.SetLeft(graphicRecord.RectangleLeft, 10);
                    Canvas.SetTop(graphicRecord.RectangleLeft, headHeight + top[0] + 25 + i * eventDistance);

                    // Částka
                    Canvas.SetLeft(graphicRecord.Price, 12);
                    Canvas.SetTop(graphicRecord.Price, headHeight + top[1] + i * eventDistance);
                    // Datum
                    Canvas.SetLeft(graphicRecord.Date, 240);
                    Canvas.SetTop(graphicRecord.Date, headHeight + top[1] + i * eventDistance);
                    // Název
                    Canvas.SetLeft(graphicRecord.Title, 30);
                    Canvas.SetTop(graphicRecord.Title, headHeight + top[2] + i * eventDistance);
                    // Zvyšuji, aby další záznam byl zarovnán pod ten předchozí
                    i++;
                }
            }
        }

        #endregion

        #region Enter, Remove records, SAVE and LOAD data
        /// <summary>
        /// Validace zadaných údajů do formuláře, tak aby bylo možné vytvořit nový záznam v databázi
        /// </summary>
        /// <param name="price">Částka</param>
        /// <param name="name">Název transakce</param>
        /// <param name="date">Datum transakce</param>
        /// <param name="description">Popis transakce</param>
        /// <param name="place">Místo transakce</param>
        /// <param name="type">Výdaj / příjem</param>
        /// <param name="category">Kategorie výdajů</param>
        public void EnterFinanceRecord(string price, string name, string date, string description, string place, TypeRecord type, 
            Category category, FinanceRecord record,TypeBalance balance)
        {
            // Pomocné proměnné pro výstup z parsování INT a DATETIME
            DateTime helpDate = DateTime.Today.Date;
            int helpPrice = 0;
            // Ošetření vyjímek, tedy pokud uživatel zadá nesmysl, aby byl upozorněn a program nespadl
            if (int.TryParse(price, out helpPrice))
            {
                if (helpPrice < 0)
                    throw new ArgumentException("Zadal jsi částku menší než 0!");
            }
            else
                throw new ArgumentException("Zadal jsi částku ve špatném formátu.");

            if (DateTime.TryParse(date, out helpDate))
            {
                if (helpDate > DateTime.Now)
                    throw new ArgumentException("Zadal jsi datum z budoucnosti");
            }
            else
                throw new ArgumentException("Zadal jsi datum ve špatném formátu. Má vypadat jako 12.10.2020");

            if (name.Count() < 3)
                throw new ArgumentException("Zadal jsi název transakce kratší než 3, jsi si jist ?");

            if(record == null)
            {
                if (description == "" && place == "")
                    admin.EnterEditRecord(0, helpPrice, name, helpDate, "", "", type, category,balance, false, null);
                else
                    admin.EnterEditRecord(0, helpPrice, name, helpDate, place, description, type, category,balance, true, null);
            }
            else
            {
                admin.EnterEditRecord(1, helpPrice, name, helpDate, place, description, type, category,balance, true, record);
            }
        }

        /// <summary>
        /// Zavolá metodu z vniřní třídy, která slouží pro uložení dat do TXT souboru
        /// </summary>
        /// <param name="type">FALSE - forma pro EXCEL, TRUE - forma pro WORD</param>
        public void PrintFinanceRecords(bool type)
        {
            admin.PrintFinanceRecords(type);
        }

        /// <summary>
        /// Zavolá metodu ze správce, která odstraní vybraný záznam z databáze
        /// </summary>
        /// <param name="record">Záznam, který se má vymazat</param>
        public void RemoveFinanceRecord(FinanceRecord record)
        {
            admin.RemoveRecord(record);
        }

        /// <summary>
        /// Načtení finančních transakcí
        /// </summary>
        public void LoadFinance()
        {
            admin.LoadFinance();
        }
        
        /// <summary>
        /// Načtení zůstatků na účtě a v hotovosti
        /// </summary>
        public void LoadBalance()
        {
            admin.LoadBalance();
        }

        #endregion
    }
}
