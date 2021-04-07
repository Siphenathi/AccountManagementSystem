using AccountManagementSystem.Model;
using AccountManagementSystem.Service.Interface;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace AccountManagementSystem.Service.Tests
{
	[TestFixture]
	public class TestAccountService
	{
		private const string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=AMSDatabase";
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
		public async Task GetAllAccounts_WhenCalled_ShouldReturnAllAccounts()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);

			//Act
			var actual = await sut.GetAllAccountsAsync();
			var dataLength = Enumerable.Count(actual);

			//Assert
			Assert.IsTrue(dataLength > 0, "Database must not be empty");
			Assert.IsTrue(dataLength >= 50, "Database records should be 50 or more");
		}

		[Test]
		public async Task GetAccount_WhenCalled_ShouldReturnAccount()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);

			//Act
			var actual = await sut.GetAccountAsync(1);

			//Assert
			actual.Should().NotBeNull();
		}

		[Test]
		public async Task GetAccountWithParentKey_WhenCalled_ShouldReturnAccount()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);

			//Act
			var actual = await sut.GetAccountWithParentKey(1);

			//Assert
			actual.Should().NotBeNull();
		}

		[Test]
		public void GetAccount_WhenCalledWithNonExistenceCode_ShouldThrowException()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);

			//Act
			var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetAccountAsync(1264949526));

			//Assert
			Assert.AreEqual("Account with Code [1264949526] could not be found.", exception.Message);
		}

		[Test]
		public async Task AddAccount_WhenCalledWithAccount_ShouldSaveAccount()
		{
			var expectedNumberOfRowsToBeAffected = 1;
			var sut = CreateAccountRepository(_connectionString);
			var account = new Account
			{
				Person_Code = 1,
				Account_Number = "123456789x",
				Outstanding_Balance = 5080.94567m
			};

			//Act
			var actual = await sut.AddAccountAsync(account);

			//Assert
			actual.Should().Be(expectedNumberOfRowsToBeAffected);
		}

		[Test]
		public async Task UpdateAccount_WhenCalledWithExistingAccount_ShouldUpdateAccount()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);
			var account = new Account
			{
				Code = 1,
				Person_Code = 1,
				Account_Number = "123456789",
				Outstanding_Balance = 5080.00m
			};

			//act
			var actual = await sut.UpdateAccountAsync(account);

			//Assert
			actual.Should().Be(1);
		}

		[Test]
		public void UpdateAccount_WhenCalledWithNonExistentAccount_ShouldThrowArException()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);
			var account = new Account
			{
				Code = 11111,
				Person_Code = 70,
				Account_Number = "123456789",
				Outstanding_Balance = 5080.94567m
			};

			//act
			var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateAccountAsync(account));

			//Assert
			Assert.AreEqual("Account with Code [11111] could not be found.", exception.Message);
		}

		[Test]
		public void DeleteAccount_WhenCalledWithNonExistentCode_ShouldThrowException()
		{
			//Arrange 
			var sut = CreateAccountRepository(_connectionString);
			var code = 1070;

			//Act
			var actual = Assert.ThrowsAsync<KeyNotFoundException>(() => sut.DeleteAccountAsync(code));

			//Assert
			actual.Message.Should().Be("Account with Code [1070] could not be found.");
		}

		[Test]
		public async Task DeleteAccount_WhenCalledWithCode_ShouldDeleteAccount()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);
			var numberOfRowsAffected = 1;
			var code = 29;

			//Act
			var actual = await sut.DeleteAccountAsync(code);

			//Assert
			actual.Should().Be(numberOfRowsAffected);
		}

		[Test]
		public async Task AccountExist_WhenCalledWithExistentParentCode_ShouldTrueAccountExist()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);
			var parentCode = 29;

			//Act
			var actual = await sut.AccountExist(parentCode);

			//Assert
			actual.Should().Be(true);
		}

		[Test]
		public async Task AccountExist_WhenCalledWithNonExistentParentCode_ShouldFalseAccountDoesNotExist()
		{
			//Arrange
			var sut = CreateAccountRepository(_connectionString);
			var parentCode = 200000;

			//Act
			var actual = await sut.AccountExist(parentCode);

			//Assert
			actual.Should().Be(false);
		}

		private IAccountRepository CreateAccountRepository(string connectionString)
		{
			IAccountRepository accountRepository = new AccountRepository(connectionString);
			return accountRepository;
		}
	}
}
