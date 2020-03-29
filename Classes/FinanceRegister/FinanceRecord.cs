using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All4Me
{
    /// <summary>
    /// Kategorie výdajů, tak aby bylo možné třídit výdaje podle typu 0 - jídlo až 6 oblečení ** 7 - ostatní 
    /// </summary>
    public enum Category
    {
        Food,
        Funny,
        Drink,
        Car,
        Insurance,
        Housing,
        Cloth,
        Other
    }

    /// <summary>
    /// Typ transakce VÝDAJE - 0  / PŘÍJMY - 1 / OSTATNI - 2
    /// </summary>
    public enum TypeRecord
    {
        Income,
        Costs,
        Other
        
    }

    /// <summary>
    /// 0 - bankovní účet, 1 - hotovost, 2 - Ostatní - záložní varianta
    /// </summary>
    public enum TypeBalance
    {
        BankAccount,
        Cash,
        Other
    }

    /// <summary>
    /// Měsíce v roce 0 - Leden až 11 - Prosinec, 12 - případně žádný
    /// </summary>
    public enum Month
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,

        August,
        September,
        October,
        November,
        December,

        Other
    }
    
    public class FinanceRecord
    {
        public static int Id { get; set; }

        /// <summary>
        /// Jedinečný identifikátor každé transakce
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Utracená částka
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// Název transakce
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Místo útráty
        /// </summary>
        public string Place { get; set; }
        /// <summary>
        /// Detailnější popis transakce
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Výdaj / příjem
        /// </summary>
        public TypeRecord TypeRecord { get; set; }
        /// <summary>
        /// Druh finance, tedy kategorie (jídlo, bydlení atd.)
        /// </summary>
        public Category Category { get; set; }

        public TypeBalance TypeBalance { get; set; }
        /// <summary>
        /// Datum uskutečnění transakce
        /// </summary>
        public DateTime Date { get; set;  }

        //public FinanceGraphicRecord GraphicRecord { get; set; }

        /// <summary>
        /// Základní konstruktor s povinnými údaji
        /// </summary>
        /// <param name="price">Utracená / získaná částka</param>
        /// <param name="name">Název transakce</param>
        /// <param name="date">Datum uskutečnění transakce</param>
        /// <param name="type">Výdaj / příjem</param>
        /// <param name="category">Kategorie výdaje</param>
        public FinanceRecord(int price,string name, DateTime date, TypeRecord type, Category category
            ,TypeBalance balance)
        {
            Price = price;
            Name = name;
            Date = date;
            TypeRecord = type;
            Category = category;
            TypeBalance = balance;
            ID = Id;
            Id++;
        }


        /// <summary>
        /// Kompletní konstruktor pro zadání všech parametrů
        /// </summary>
        /// <param name="price">Utracená / získaná částka</param>
        /// <param name="name">Název transakce</param>
        /// <param name="date">Datum uskutečnění transakce</param>
        /// <param name="place">Místo uskutečněné transakce (obchod, město apod)</param>
        /// <param name="description">Detailní popis k transakci</param>
        /// <param name="type">Výdaj / příjem</param>
        /// <param name="category">Kategorie výdaje</param>
        public FinanceRecord(int price, string name, DateTime date, string place, string description, TypeRecord type, 
            Category category, TypeBalance balance)
        {
            Price = price;
            Name = name;
            Date = date;
            Place = place;
            Description = description;
            TypeRecord = type;
            Category = category;
            TypeBalance = balance;
            ID = Id;
            Id++;
        }

        /// <summary>
        /// Prázdný konstruktor pro ukládání souboru
        /// </summary>
        public FinanceRecord()
        {

        }

    }
}
