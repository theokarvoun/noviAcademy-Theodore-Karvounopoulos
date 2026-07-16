using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Tests.Entities
{
    public class WalletTests
    {
        [Fact]
        public void Constructor_ProperInput_InitializedCorrectly()
        {
            //Arrange
            Guid playerId = Guid.NewGuid();
            Currency currency = Currency.USD;
            //Act
            var wallet = new Wallet(playerId, currency);
            //Assert
            Assert.Equal(wallet.PlayerId, playerId);
            Assert.Equal(wallet.Currency, currency);
        }

        [Fact]
        public void Deposit_ValidInput_Success()
        {
            decimal amount = 100;
            var wallet = new Wallet(Guid.NewGuid(), Currency.USD);
            decimal balance = wallet.Balance;
            wallet.Deposit(amount);
            balance += amount;
            Assert.Equal(wallet.Balance,balance);

        }

        [Theory]
        [InlineData(-100)]
        [InlineData(0)]
        public void Deposit_NotPositiveAmount_InvalidAmountException(decimal amount)
        {
            var wallet = new Wallet(Guid.NewGuid(), Currency.USD);
            Action deposit = () => wallet.Deposit(amount);
            Assert.Throws<InvalidAmountException>(deposit);
        }
    }
}
