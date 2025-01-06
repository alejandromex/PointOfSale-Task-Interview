using PointOfSale.App.Enums;
using System.Xml.Linq;

namespace PointOfSale.App.Models
{
    public class Cash
    {
        private double value { get; set; }
        public int Quantity { get; private set; }
        public CashType Type { get; private set; }

        private Cash() { }

        public double Value
        {
            get { return Math.Round(value, 2); }
        }

        public static Cash Create(double value, int quantity, CashType type)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentNullException.ThrowIfNull(type);

            var cash = new Cash()
            {
                value = value,
                Quantity = quantity,
                Type = type
            };
            return cash;
        }
    }
}
