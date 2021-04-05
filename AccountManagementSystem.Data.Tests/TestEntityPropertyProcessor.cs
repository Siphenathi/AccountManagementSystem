using AccountManagementSystem.Data.TransferObject;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace AccountManagementSystem.Data.Tests
{
	[TestFixture]
	public class TestEntityPropertyProcessor
	{
		[Test]
		public void RemoveProperties_GivenNullListOfProperties_ShouldReturnErrorMessage()
		{
			//Arrange
			//Act
			var actual = EntityPropertyProcessor.RemoveProperties(null, "girlfriend");

			//Assert
			actual.Error.Message.Should().Be("Invalid list of Properties entered.");
		}

		[Test]
		public void RemoveProperties_GivenEmptyListOfProperties_ShouldReturnErrorMessage()
		{
			//Arrange
			//Act
			var actual = EntityPropertyProcessor.RemoveProperties(new List<string>(), "girlfriend");

			//Assert
			actual.Error.Message.Should().Be("list of Properties entered is empty.");
		}

		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void RemoveProperties_GivenInvalidPropertyToBeRemove_ShouldReturnErrorMessage(string recordToRemove)
		{
			//Arrange
			var listOfProperties = new List<string> { "car", "house", "girlfriend" };

			//Act
			var actual = EntityPropertyProcessor.RemoveProperties(listOfProperties, recordToRemove);

			//Assert
			actual.Error.Message.Should().Be("Invalid property name, check your property names.");
		}

		[Test]
		public void RemoveProperties_GivenListOfColumnsAndNonExistentColumn_ShouldRemoveEntityProperty()
		{
			//Arrange
			var listOfProperties = new List<string> { "car", "house", "girlfriend" };

			//Act
			var actual = EntityPropertyProcessor.RemoveProperties(listOfProperties, "soccer");

			//Assert
			actual.Error.Message.Should().Be("property name soccer is not found in the entity provided.");
		}

		[Test]
		public void RemoveProperties_GivenListOfPropertiesAnd1ExistentPropertyNameAnd1NonExistentPropertyNameToBeRemove_ShouldRemoveRecord()
		{
			//Arrange
			var listOfProperties = new List<string> { "car", "house", "girlfriend" };

			//Act
			var actual = EntityPropertyProcessor.RemoveProperties(listOfProperties, "girlfriend","UnknownProperty");

			//Assert
			actual.Error.Message.Should().Be("property name UnknownProperty is not found in the entity provided.");
		}

		[Test]
		public void RemoveProperties_GivenListOfPropertiesAndPropertyToBeRemove_ShouldRemoveRecord()
		{
			//Arrange
			var listOfProperties = new List<string> { "car", "house", "girlfriend" };

			//Act
			var actual = EntityPropertyProcessor.RemoveProperties(listOfProperties, "girlfriend");

			//Assert
			Assert.AreEqual(2, actual.Properties.Count);
		}

		[Test]
		public void GetEntityProperties_WhenCalledWithNullProperties_ShouldReturnListOfEntityProperties()
		{
			//Arrange
			//Act
			var actual = EntityPropertyProcessor.GetEntityProperties(null);

			//Assert
			actual.Should().BeEquivalentTo(new List<string>());
		}

		[Test]
		public void GetEntityProperties_WhenCalledWithValidInput_ShouldReturnListOfEntityProperties()
		{
			//Arrange
			var expectedListOfProperties = new List<string> { "Id", "Name", "Age" };

			//Act
			var actual = EntityPropertyProcessor.GetEntityProperties(typeof(TestingObject).GetProperties());

			//Assert
			actual.Should().BeEquivalentTo(expectedListOfProperties);
		}

		[Test]
		public void GetAggregatedTableAndModelFields_WhenCalled_ShouldReturnAggregatedTableAndModelFields()
		{
			//Arrange
			var expectedListOfProperties = new EntityPropertyProcessorResponse 
			{ 
				TableFields = "Id,Name,Age", 
				ModelFields = "@Id, @Name, @Age"
			};

			//Act
			var actual = EntityPropertyProcessor.GetAggregatedTableAndModelFields<TestingObject>();

			//Assert
			actual.Should().BeEquivalentTo(expectedListOfProperties);
		}

		[Test]
		public void GetAggregatedTableAndModelFields_WhenCalledWithPropertyName_ShouldReturnAggregatedFieldsWithoutTheKey()
		{
			//Arrange
			var propertyToBeRemoved = "Id";
			var expectedList = new EntityPropertyProcessorResponse
			{
				TableFields = "Name,Age",
				ModelFields = "@Name, @Age"
			};

			//Act
			var actual = EntityPropertyProcessor.GetAggregatedTableAndModelFields<TestingObject>(propertyToBeRemoved);

			//Assert
			actual.Should().BeEquivalentTo(expectedList);
		}
	}
}