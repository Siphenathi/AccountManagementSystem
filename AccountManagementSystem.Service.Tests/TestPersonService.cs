﻿using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;
using System.Collections.Generic;
using AccountManagementSystem.Model;
using System;
using System.Transactions;
using AccountManagementSystem.Service.Interface;

namespace AccountManagementSystem.Service.Tests
{
	[TestFixture]
	public class TestPersonService
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
		public async Task GetAllPeople_WhenCalled_ShouldReturnAllPeople()
		{
			//Arrange
			var sut = CreatePersonRepository(_connectionString);

			//Act
			var actual = await sut.GetAllPeopleAsync();
			var dataLength = Enumerable.Count(actual);

			//Assert
			Assert.IsTrue(dataLength > 0, "Database must not be empty");
			Assert.IsTrue(dataLength >= 50, "Database records should be 50 or more");
		}

		[Test]
		public async Task GetPerson_WhenCalled_ShouldReturnPerson()
		{
			//Arrange
			var sut = CreatePersonRepository(_connectionString);

			//Act
			var actual = await sut.GetPerson(1);

			//Assert
			actual.Should().NotBeNull();
		}

		[Test]
		public void GetPerson_WhenCalledWithNonExistenceCode_ShouldThrowException()
		{
			//Arrange
			var sut = CreatePersonRepository(_connectionString);

			//Act
			var exception = Assert.ThrowsAsync<KeyNotFoundException>(() =>  sut.GetPerson(1264949526));

			//Assert
			Assert.AreEqual("Person with Code [1264949526] could not be found.", exception.Message);
		}

		[Test]
		public async Task AddPerson_WhenCalledWithPerson_ShouldSavePerson()
		{
			var expectedNumberOfRowsToBeAffected = 1;
			var sut = CreatePersonRepository(_connectionString);
			var person = new Person 
			{
				Name = "Siphenathi",
				Surname = "Pantshwa",
				Id_Number = "9501045404082"
			};

			//Act
			var actual = await sut.AddPersonAsync(person);

			//Assert
			actual.Should().Be(expectedNumberOfRowsToBeAffected);
		}

		[Test]
		public async Task UpdatePerson_WhenCalledWithExistingPerson_ShouldUpdatePerson()
		{
			//Arrange
			var sut = CreatePersonRepository(_connectionString);
			var person = new Person
			{
				Code=70,
				Name = "Nathi",
				Surname = "Pantshwa Hlanga",
				Id_Number = "9x01045404082"
			};

			//act
			var actual = await sut.UpdatePersonAsync(person);

			//Assert
			actual.Should().Be(1);
		}

		[Test]
		public void UpdatePerson_WhenCalledWithNonExistentPerson_ShouldThrowArException()
		{
			//Arrange
			var sut = CreatePersonRepository(_connectionString);
			var person = new Person
			{
				Code = 7000,
				Name = "Nathi",
				Surname = "Pantshwa Hlanga",
				Id_Number = "9x01045404082"
			};

			//act
			var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdatePersonAsync(person));

			//Assert
			Assert.AreEqual("Person with Code [7000] could not be found.", exception.Message);
		}

		[Test]
		public void DeletePerson_WhenCalledWithNonExistentCode_ShouldThrowException()
		{
			//Arrange 
			var sut = CreatePersonRepository(_connectionString);
			var code = 1000;

			//Act
			var actual = Assert.ThrowsAsync<KeyNotFoundException>(() => sut.DeletePersonAsync(code));

			//Assert
			actual.Message.Should().Be("Person with Code [1000] could not be found.");
		}

		[Test]
		public async Task DeletePerson_WhenCalledWithCode_ShouldDeletePerson()
		{
			//Arrange 
			var sut = CreatePersonRepository(_connectionString);
			var numberOfRowsAffected = 1;
			var code = 70;

			//Act
			var actual = await sut.DeletePersonAsync(code);

			//Assert
			actual.Should().Be(numberOfRowsAffected);
		}

		private static IPersonRepository CreatePersonRepository(string connectionString)
		{
			IPersonRepository personRepository = new PersonRepository(connectionString);
			return personRepository;
		}
	}
}
