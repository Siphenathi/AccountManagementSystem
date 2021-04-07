using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountManagementSystem.Service.Tests
{
	[TestFixture]
	public class TestDigitsGenerator
	{
		[Test]
		public void GetUniqueDigits_WhenCalled_ShouldGenerateUniqueDigits()
		{
			//Arrange
			var listOfNumbers = Enumerable.Empty<int>();

			//Act
			listOfNumbers = GetListOfUniqueDigits(listOfNumbers);

			//Assert
			Assert.IsTrue(listOfNumbers.Distinct().Count() == 30);
		}

		private static IEnumerable<int> GetListOfUniqueDigits(IEnumerable<int> listOfNumbers)
		{
			for (int x = 0; x < 30; x++)
			{
				listOfNumbers = listOfNumbers.Concat(new[] { DigitsGenerator.GetUniqueDigits() });
			}

			return listOfNumbers;
		}
	}
}
