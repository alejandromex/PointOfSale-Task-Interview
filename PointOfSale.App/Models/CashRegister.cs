namespace PointOfSale.App.Models
{
    public class CashRegister
    {
        private double[] billsAccepted;
        private double[] coinsAccepted;
        private Cash[] amount;

        private CashRegister(double[] billsAccepted, double[] coinsAccepted)
        {
            this.billsAccepted = billsAccepted.OrderByDescending(c => c).ToArray();
            this.coinsAccepted = coinsAccepted.OrderByDescending(c => c).ToArray();
        }

        public static CashRegister SetUpCashRegister(double[] billsAccepted, double[] coinsAccepted)
        {
            return new CashRegister(billsAccepted, coinsAccepted);
        }

        public double GenerateTotalPriceItems(Item[] items)
        {
            return items.Sum(x => x.Price);
        }

        public Cash[] Pay(double totalToPay, Cash[] cash)
        {
            double cashAmount = cash.Sum(c => c.Quantity * c.Value);
            if(totalToPay > cashAmount)
            {
                throw new NotEnoughCashToPayException();
            }
            if(totalToPay == cashAmount)
            {
                return Array.Empty<Cash>();
            }

            cashAmount -= totalToPay;
            return CalculateChange(cashAmount);
        }

        public Cash[] CalculateChange(double total)
        {
            List<Cash> cash = new();
            total = Math.Round(total, 2);
            int totalInCoins = (int)(total * 100);

            for (int i = 0; i < billsAccepted.Length; i++)
            {
                int billInCoins = (int)billsAccepted[i] * 100;
                int value = totalInCoins / billInCoins;
                if(value > 0)
                {
                    cash.Add(Cash.Create(billsAccepted[i], value, CashType.Bill));
                }
                totalInCoins -= value * billInCoins;
            }

            for (int i = 0; i < coinsAccepted.Length; i++)
            {
                int coins = (int)(coinsAccepted[i] * 100);
                int value = totalInCoins / coins;
                if(value > 0)
                {
                    cash.Add(Cash.Create(coinsAccepted[i], value, CashType.Bill));
                }
                totalInCoins -= value * coins;
            }

            return cash.ToArray();
        }
    }
}
