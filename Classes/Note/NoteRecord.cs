using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All4Me
{
    /// <summary>
    /// Třída reprezentující jednu poznámku
    /// </summary>
    public class NoteRecord
    {
        public static int Id { get; set; }
        /// <summary>
        /// Identifikátor poznámky
        /// </summary>
        public int ID { get;  set; }
        /// <summary>
        /// Název poznámky
        /// </summary>
        public string Name { get;  set; }
        /// <summary>
        /// Samotný text poznámky
        /// </summary>
        public string Text { get;  set; }
        /// <summary>
        /// Datum vytvoření poznámky
        /// </summary>
        public DateTime Date { get;  set; }
        /// <summary>
        /// Číslo strany, na které se nachází poznámka 1 - 5
        /// </summary>
        public byte Page { get; set; }

        /// <summary>
        /// Základní konstruktor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="date"></param>
        public NoteRecord(string name, string text, DateTime date, byte page)
        {
            Name = name;
            Text = text;
            Date = date;
            Page = page;
            ID = Id;

            Id++;
        }

        /// <summary>
        /// Prázdný konstruktor pro ukládání pomocí XMLserializer
        /// </summary>
        public NoteRecord()
        {

        }
    }
}
