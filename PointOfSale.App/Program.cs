using Microsoft.Extensions.Configuration;
namespace PointOfSale.App
{
    public class Program
    {
        private static IConfigurationRoot configuration;
        private static double[] bills;
        private static double[] coins;
        private static string currencyName;

        public static void Main(string[] args)
        {
            Init();
            ExecuteTask();
        }

        public static void ExecuteTask()
        {
            CashRegister cashRegister = CashRegister.SetUpCashRegister(bills, coins);

            Item[] items =
            {
                new Item() { Name = "Item 1", Price = 100.00d },
                new Item() { Name = "Item 2", Price = 20.15d },
                new Item() { Name = "Item 3", Price = 84.37d },
                new Item() { Name = "Item 4", Price = 10.00d },
                new Item() { Name = "Item 5", Price = .17d }
            };

            var totalAmount = cashRegister.GenerateTotalPriceItems(items);

            Console.WriteLine($"Total To Pay: {totalAmount}");

            var cash = SetBillsAndCoinsToPay();
            var change = cashRegister.Pay(totalAmount, cash);

            ShowAmoutOfChange(change);

            Console.ReadLine();
        }
        public static Cash[] SetBillsAndCoinsToPay()
        {
            var cashList = new List<Cash>();
            Console.WriteLine("Insert the number of bills by value to Pay");
            for (int i = 0; i < bills.Length; i++)
            {
                Console.Write($"{bills[i]}: ");
                int quantity = GetQuantityOfCashToUse();
                if (quantity > 0)
                {
                    cashList.Add(Cash.Create(bills[i], quantity, CashType.Bill));
                }
            }
            Console.WriteLine("Insert the number of coins by value to Pay");
            for (int i = 0; i < coins.Length; i++)
            {
                Console.Write($"{coins[i]}: ");
                int quantity = GetQuantityOfCashToUse();
                if (quantity > 0)
                {
                    cashList.Add(Cash.Create(coins[i], quantity, CashType.Coin));
                }
            }

            return cashList.ToArray();
        }

        private static int GetQuantityOfCashToUse()
        {
            bool error = false;
            int quantity = -1;
            while (quantity < 0)
            {
                if (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0)
                {
                    quantity = -1;
                    Console.WriteLine("Please insert a valid positive number");
                }
            }

            return quantity;
        }

        private static void ShowAmoutOfChange(Cash[] change)
        {
            double changeText = Math.Round(change.Sum(c => c.Quantity * c.Value), 2);
            Console.WriteLine(@$"Your change is: {changeText}({currencyName})");
            Console.WriteLine(@"Value | Quantity");
            foreach(var cash in change)
            {
                Console.WriteLine($"{string.Format("{0:C}", cash.Value)}: {cash.Quantity}");
            }
        }

        private static void Init()
        {
            SetConfigurationBuilder();
            SetCash();
        }

        private static void SetConfigurationBuilder()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = configurationBuilder.Build();
        }

        private static void SetCash()
        {
            bills = GetCashCurrency(SettingsKeys.APP_SETTINGS_BILLS_KEY);
            coins = GetCashCurrency(SettingsKeys.APP_SETTINGS_COINS_KEY);
            currencyName = configuration.GetSection($"{SettingsKeys.APP_SETTINGS_SECTION}:{SettingsKeys.APP_SETTINGS_CURRENCY_NAME_KEY}").Get<string>()!;
        }
        private static double[] GetCashCurrency(string cashType)
        {
            var section = configuration.GetSection($"{SettingsKeys.APP_SETTINGS_SECTION}:{cashType}");
            return section.Get<double[]>()!.OrderByDescending(c => c).ToArray()!;
        }

    }
}
