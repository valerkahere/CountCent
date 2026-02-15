namespace CountCent.Model
{
    public class DataPoint
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public DataPoint()
        {
            Amount = 0;
            Date = DateTime.Now;
        }

        public DataPoint(decimal amount)
        {
            Amount = amount;
            Date = DateTime.Now;
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
