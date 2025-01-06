
using PointOfSale.App.Exceptions;

namespace PointOfSale.Tests
{
    public class CashRegisterTest
    {
        double[] billsAccepted = { 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 };
        double[] coinsAccepted = { 0.01, 0.05, 0.10, 0.25, 0.50 };


        [Fact]
        public void Verify_SumPrices_IsCorrect()
        {
            CashRegister cashRegister = CashRegister.SetUpCashRegister(billsAccepted, coinsAccepted);
            Item[] items =
            {
                new Item() { Name = "Item 1", Price = 50.20d },
                new Item() { Name = "Item 2", Price = 30.40d },
                new Item() { Name = "Item 3", Price = 19.40d }
            };

            double totalPrice = cashRegister.GenerateTotalPriceItems(items);

            Assert.Equal(100d, totalPrice);
        }

        [Fact]
        public void CashRegister_Pay_ThrowsNotEnoughtCashToPayException()
        {
            CashRegister cashRegister = CashRegister.SetUpCashRegister(billsAccepted, coinsAccepted);
            double totalToPay = 100d;
            Cash[] cash =
            {
                Cash.Create(50, 1, App.Enums.CashType.Bill),
                Cash.Create(20, 2, App.Enums.CashType.Bill),
                Cash.Create(1, 9, App.Enums.CashType.Bill),
            };


            Assert.Throws<NotEnoughCashToPayException>(() =>
            {
                cashRegister.Pay(totalToPay, cash);
            });
        }

        [Fact]
        public void CashRegsiter_PayExact_ReturnEmptyChange()
        {
            CashRegister cashRegister = CashRegister.SetUpCashRegister(billsAccepted, coinsAccepted);
            double totalToPay = 100d;
            Cash[] cash =
            {
                Cash.Create(50, 1, App.Enums.CashType.Bill),
                Cash.Create(20, 2, App.Enums.CashType.Bill),
                Cash.Create(1, 10, App.Enums.CashType.Bill),
            };

            var change = cashRegister.Pay(totalToPay, cash);

            Assert.Empty(change);
        }

        [Fact]
        public void CashRegsiter_PayMore_ReturnCorrectChange()
        {
            CashRegister cashRegister = CashRegister.SetUpCashRegister(billsAccepted, coinsAccepted);
            double totalToPay = 100d;
            // In my cash I got one more bill of 20 and another of 1, the function should return me the change of 21
            Cash[] cash =
            {
                Cash.Create(50, 1, App.Enums.CashType.Bill),
                Cash.Create(20, 3, App.Enums.CashType.Bill),
                Cash.Create(1, 11, App.Enums.CashType.Bill),
            };
            Cash[] expectedChange =
            {
                Cash.Create(20, 1, App.Enums.CashType.Bill),
                Cash.Create(1, 1, App.Enums.CashType.Bill),
            };

            var change = cashRegister.Pay(totalToPay, cash);

            Assert.Equivalent(expectedChange, change);
        }
    }
}