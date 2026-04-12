using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountCent.Model
{
    // custom table name
    [SQLite.Table("DataPoints")]
    public class DataPoint
    {
        [PrimaryKey]
        [AutoIncrement]

        // custom column names
        [SQLite.Column("id")]
        public int Id { get; set; }

        // second here because want csv format like
        // id date_added amount

        [SQLite.Column("date_added")]
        public DateTime Date { get; set; }

        [SQLite.Column("amount")]
        public decimal Amount { get; set; }

        

        public DataPoint()
        {
            Amount = 0;
            Date = DateTime.UtcNow;
        }

        public DataPoint(decimal amount)
        {
            Amount = amount;
            Date = DateTime.UtcNow;
        }

        public DataPoint(decimal amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }

        public override string ToString()
        {
            // Alternative with a date
            // return $"{Amount} - {Date.ToShortDateString()}";
            return $"{Amount:C1}";
        }
    }
}
