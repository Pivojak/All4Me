using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace All4Me
{
    /// <summary>
    /// Správce pro sekci Poznámkového bloku
    /// </summary>
    public class NoteAdmin
    {
        /// <summary>
        /// Kliknutí na oblast poznámky - událost převzata od grafickéPodoby - NoteGraphicRecord třída
        /// </summary>
        public event EventHandler GraphicRecord_RectangleButtonClick;
        /// <summary>
        /// Plátno na které se vykreslují objekty
        /// </summary>
        private Canvas canvas;
        /// <summary>
        /// Základní část cesty k uložení na C do appData
        /// </summary>
        private string pathBase;
        /// <summary>
        /// Cesta pro uložení IDs
        /// </summary>
        private string pathId;
        /// <summary>
        /// Cesta pro uložení samotných poznámek
        /// </summary>
        private string pathRecords;
        /// <summary>
        /// Kolekce obsahující poznámky
        /// </summary>
        public List<NoteRecord> Records { get; private set; }
        /// <summary>
        /// Příznak, poslední navštívené strany
        /// </summary>
        private byte lastPageView = 1;
        /// <summary>
        /// 1 - Prvni strana ** 5 - Pata strana
        /// </summary>
        public byte[] PageColor { get; private set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="canvas"></param>
        public NoteAdmin(Canvas canvas)
        {
            this.canvas = canvas;
            Records = new List<NoteRecord>();
            PageColor = new byte[6];

            try
            {
                // Získání cesty do složky All4Me v AppData na disku C
                pathBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "All4Me");
                if (Directory.Exists(pathBase))
                    Directory.CreateDirectory(pathBase);
            }
            catch
            {
                MessageBox.Show("Nepodarilo se vytvorit soubor", "Pozor", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            pathId = Path.Combine(pathBase, "NoteID.xml");
            pathRecords = Path.Combine(pathBase, "NoteRecords.xml");
        }
        /// <summary>
        /// Nastavení barvy pro danou stránku
        /// </summary>
        /// <param name="page"></param>
        /// <param name="color"></param>
        public void SetPageColor(int page, byte color)
        {
            if(page >= 1 && page <= 5 && color >= 1 && color <= 5)
            {
                PageColor[page] = color;
                ConstructGraphicRecord(lastPageView);
            }
        }

        /// <summary>
        /// Metoda, která vykreslí Grafickou podobu poznámek na Canvas
        /// </summary>
        public void ConstructGraphicRecord(byte page)
        {
            lastPageView = page;
            canvas.Children.Clear();
            int[] posTOP = new int[] { 0, 0, 260, 260 };
            int[] posLEFT = new int[] { 0, 270, 0, 270 };
            int i = 0;
            foreach (NoteRecord record in Records)
            {
                if(record.Page == page)
                {

                    NoteGraphicRecord graphic = new NoteGraphicRecord(record,PageColor[page]);
                    // Počet znaků celkem v textu
                    int charCount = 0;
                    // Skutečný počet znaků - protože řádek končí např po 10 znacích
                    int charCount_real = 0;
                    // Počet znaků na řádku
                    int charRowCount = 0;
                    // Počet řádků
                    int rowCount = 0;
                    // Příznak, který povoluje připsání teček pro značení plného TEXTU
                    bool longText = false;
                    string text = graphic.Text.Text;
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

                        if (charRowCount == 34)
                        {
                           rowCount++;
                           charCount_real += charRowCount;
                           // Délka textu na řádku je 34 znaků - MAXIMUM pro řádek, proto se
                           // text zalomí 
                           text = text.Insert(charCount_real, "\n");
                           
                           charRowCount = 0;
                        }

                        if(rowCount == 11 || charCount == 335)
                        {
                            // Omezení celkového počtu znaků na blok poznámky - nebo řádků
                            text = text.Substring(0, charCount - 1);
                            longText = true;
                            break;
                        }
                        charCount++;
                        charRowCount++;

                    }
                    // Text je příliš dlouhý, tak aby se přebytečná část zrušila pro výpis
                    // a místo toho se přidá následující textový řetězec
                    if (longText)
                    {
                        text += "\n... NEXT ...";   
                    }
                    graphic.Text.Text = text;
                    // Přidání obsluhy na událost, jež vyvolá Grafická poznámka - kliknutí na obdelník
                    graphic.RectangleButtonClick += Graphic_RectangleButtonClick;
                    // Přidání objektů na plátno
                    canvas.Children.Add(graphic.RectangleDown);
                    canvas.Children.Add(graphic.RectangleTop);
                    canvas.Children.Add(graphic.Name);
                    canvas.Children.Add(graphic.Text);
                    canvas.Children.Add(graphic.Date);
                    // zarovnání TEXT BLOCKU a RECTANGLU na plátno
                    Canvas.SetTop(graphic.RectangleDown, 10 + posTOP[i]); Canvas.SetLeft(graphic.RectangleDown, 10 + posLEFT[i]);
                    Canvas.SetTop(graphic.RectangleTop, 10 + posTOP[i]); Canvas.SetLeft(graphic.RectangleTop, 10 + posLEFT[i]);
                    Canvas.SetTop(graphic.Name, 15 + posTOP[i]); Canvas.SetLeft(graphic.Name, 15 + posLEFT[i]);
                    Canvas.SetTop(graphic.Text, 40 + posTOP[i]); Canvas.SetLeft(graphic.Text, 20 + posLEFT[i]);
                    Canvas.SetTop(graphic.Date, 235 + posTOP[i]); Canvas.SetLeft(graphic.Date, 180 + posLEFT[i]);

                    i++;
                    if (i == 4)
                        i = 0;
                }
            }
        }
        /// <summary>
        /// Obsluha události kliknutí na poznámku, vyvolává ji Grafická poznámka na základni události kliknutí na text nebo obdelník
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Graphic_RectangleButtonClick(object sender, EventArgs e)
        {
            // Vyvolám událost této třídy, která oznámí, že proběhlo kliknutí a aby mohl formulář reagovat
            GraphicRecord_RectangleButtonClick(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Metoda pro uložení / úpravu poznámky do kolecke
        /// </summary>
        /// <param name="type">0 - Nový záznam ** 1 - Úprava</param>
        /// <param name="name">Název poznámky</param>
        /// <param name="description">Text poznámky</param>
        /// <param name="date">Datum zadání poznámky</param>
        /// <param name="record">Záznam, který má být upraven</param>
        public void EnterEditRecord(byte type, string name, string description, NoteRecord record)
        {
            if (name.Count() <= 3)
                throw new ArgumentException("Zadal jsi název poznámky kratší než 4 znaky. Jsi si jist?");
            if (description.Count() <= 3)
                throw new ArgumentException("Zadal jsi text poznámky kratší než 4 znaky. Jsi si jist?");
            if (name == "" || description == "")
                throw new ArgumentException("Nezadal jsi žádný název nebo text poznámky");
            int[] pageCount = new int[6];
            // Strana na které je místo pro novou poznámka - počítáno od 1.
            byte nextFreePlacePage = 1;
            foreach (NoteRecord rec in Records)
            {
                // Inkrementuji počet záznamů na straně
                pageCount[rec.Page]++;
            }
            for (int i = 1; i < 6; i++)
            {
                // Pokud je na straně max 3 záznamy, vystavím FLAG - že daná strana má místo
                if (pageCount[i] < 4)
                {
                    nextFreePlacePage = (byte)i;
                    break;
                }
            }
            if (type == 0)
                Records.Add(new NoteRecord(name, description, DateTime.Now.Date, nextFreePlacePage));
            else if (type == 1 && record != null)
            {
                record.Name = name;
                record.Text = description;
            }
            else
                throw new ArgumentException("Něco se nepodařilo a záznam nebyl přidán / upraven. Zkus to prosím znovu.");

            SaveId_Color();
            SaveRecord();
        }

        /// <summary>
        /// Odebrání záznamu z kolekce
        /// </summary>
        public void RemoveRecord(NoteRecord record)
        {
            if (Records.Contains(record))
            {
                Records.Remove(record);
            }
            SaveId_Color();
            SaveRecord();
        }

        /// <summary>
        /// Uložení ID poznámke + barev použitých na jednotlivých stranách
        /// </summary>
        public void SaveId_Color()
        {
            using(StreamWriter sw = new StreamWriter(pathId))
            {
                sw.WriteLine(NoteRecord.Id);
                for(int i = 1; i < 6; i++)
                    sw.WriteLine(PageColor[i]);
            }
        }

        /// <summary>
        /// Uložení všech poznámek do APP data 
        /// </summary>
        public void SaveRecord()
        {
            XmlSerializer serializer = new XmlSerializer(Records.GetType());

            using(StreamWriter writter = new StreamWriter(pathRecords))
            {
                serializer.Serialize(writter, Records);
            }
        }

        /// <summary>
        /// Načtení ID poznámke + barev použitých na jednotlivých stranách
        /// </summary>
        public void LoadId_Color()
        {
            if (File.Exists(pathId))
            {
                using (StreamReader sr = new StreamReader(pathId))
                {
                    NoteRecord.Id = int.Parse(sr.ReadLine());
                    for (int i = 1; i < 6; i++)
                        PageColor[i] = byte.Parse(sr.ReadLine());
                }
            }
            else
            {
                NoteRecord.Id = 0;
                for (int i = 1; i < 6; i++)
                    PageColor[i] = 0;

            }


        }

        /// <summary>
        /// Načtení všech poznámek do APP data 
        /// </summary>
        public void LoadRecord()
        {
            // Serializuje data do formátu XML
            XmlSerializer serializer = new XmlSerializer(Records.GetType());

            if (File.Exists(pathRecords))
            {
                using (StreamReader reader = new StreamReader(pathRecords))
                {
                    Records = (List<NoteRecord>)serializer.Deserialize(reader);
                }
            }
            else
                Records = new List<NoteRecord>();
            
        }
    }
}
