using AccountManagementSystem.Model;
using AccountManagementSystem.Service.Interface;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace AccountManagementSystem.Service.Tests
{
	[TestFixture]
	public class TestTransactionService
	{
		private const string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=AMSContext";
		private TransactionScope scope;

		[SetUp]
		public void SetUp()
		{
			scope = new TransactionScope();
		}

		[TearDown]
		public void TearDown()
		{
			scope.Dispose();
		}

		[Test]
		public async Task GetAllTransactions_WhenCalled_ShouldReturnAllTransactions()
		{
			//Arrange
			var sut = CreateTransactionRepository(_connectionString);

			//Act
			var actual = await sut.GetAllTransactionsAsync();
			var dataLength = Enumerable.Count(actual);

			//Assert
			Assert.IsTrue(dataLength > 0, "Database must not be empty");
			Assert.IsTrue(dataLength >= 45, "Database records should be 50 or more");
		}

		[Test]
		public async Task GetTransaction_WhenCalled_ShouldReturnTransaction()
		{
			//Arrange
			var sut = CreateTransactionRepository(_connectionString);

			//Act
			var actual = await sut.GetTransaction(1);

			//Assert
			actual.Should().NotBeNull();
		}

		[Test]
		public async Task GetTransactionWithParentKey_WhenCalled_ShouldReturnTransaction()
		{
			//Arrange
			var sut = CreateTransactionRepository(_connectionString);

			//Act
			var actual = await sut.GetTransactionWithParentKey(1);

			//Assert
			actual.Should().NotBeNull();
		}

		[Test]
		public void GetTransaction_WhenCalledWithNonExistenceCode_ShouldThrowException()
		{
			//Arrange
			var sut = CreateTransactionRepository(_connectionString);

			//Act
			var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetTransaction(1264949526));

			//Assert
			Assert.AreEqual("Transaction with Code [1264949526] could not be found.", exception.Message);
		}

		[Test]
		public async Task AddTransaction_WhenCalledWithTransaction_ShouldSaveTransaction()
		{
			var expectedNumberOfRowsToBeAffected = 1;
			var sut = CreateTransactionRepository(_connectionString);
			var transaction = new Model.Transaction
			{
				Account_Code = 1,
				Transaction_Date = DateTime.Parse("2021/03/28 9:45:37 PM"),
				Capture_Date = DateTime.Parse("2021/03/28 9:45:37 PM"),
				Amount = 10000.00m,
				Description = "ATM Withdraw"
			};

			//Act
			var actual = await sut.AddTransactionAsync(transaction);

			//Assert
			actual.Should().Be(expectedNumberOfRowsToBeAffected);
		}

		[Test]
		public async Task UpdateTransaction_WhenCalledWithExistingTransaction_ShouldUpdateAccount()
		{
			//Arrange
			var sut = CreateTransactionRepository(_connectionString);
			var transaction = new Model.Transaction
			{
				Code = 1,
				Account_Code = 1,
				Transaction_Date = DateTime.Parse("2021/03/28 9:45:37 PM"),
				Capture_Date = DateTime.Parse("2021/03/28 9:45:37 PM"),
				Amount = 10000.00m,
				Description = "ATM Withdraw"
			};

			//act
			var actual = await sut.UpdateTransactionAsync(transaction);

			//Assert
			actual.Should().Be(1);
		}

		[Test]
		public void UpdateTransaction_WhenCalledWithNonExistentTransaction_ShouldThrowArException()
		{
			//Arrange
			var sut = CreateTransactionRepository(_connectionString);
			var transaction = new Model.Transaction
			{
				Code = 1000,
				Account_Code = 1,
				Transaction_Date = DateTime.Parse("2021/03/28 9:45:37 PM"),
				Capture_Date = DateTime.Parse("2021/03/28 9:45:37 PM"),
				Amount = 10000.00m,
				Description = "Cashback Withdraw"
			};

			//act
			var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateTransactionAsync(transaction));

			//Assert
			Assert.AreEqual("Transaction with Code [1000] could not be found.", exception.Message);
		}

		[Test]
		public void DeleteTransaction_WhenCalledWithNonExistentCode_ShouldThrowException()
		{
			//Arrange 
			var sut = CreateTransactionRepository(_connectionString);
			var code = 1070;

			//Act
			var actual = Assert.ThrowsAsync<KeyNotFoundException>(() => sut.DeleteTransactionAsync(code));

			//Assert
			actual.Message.Should().Be("Transaction with Code [1070] could not be found.");
		}

		[Test]
		public async Task DeleteTransaction_WhenCalledWithCode_ShouldDeleteTransaction()
		{
			//Arrange
			var sut = CreateTransactionRepository(_connectionString);
			var numberOfRowsAffected = 1;
			var code = 1;

			//Act
			var actual = await sut.DeleteTransactionAsync(code);

			//Assert
			actual.Should().Be(numberOfRowsAffected);
		}

		private ITransactionRepository CreateTransactionRepository(string connectionString)
		{
			ITransactionRepository transactionRepository = new TransactionRepository(connectionString);
			return transactionRepository;
		}
	}
}
