using AccountManagementSystem.Host.Controllers;
using AccountManagementSystem.Model;
using AccountManagementSystem.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagementSystem.Host.Tests
{
	[TestFixture]
	public class TestPersonController
	{
		[Test]
		public async Task GetPeople_WhenCalled_ShouldCallPeople()
		{
			//Arrange
			var personRepository = Substitute.For<IPersonRepository>();
			personRepository.GetAllPeopleAsync().Returns(new List<Person> {
				new Person
				{
					Name ="Name",
					Surname = "Surname",
					Id_Number = "Id_Number"
				}
			});

			var personControler = new PersonController(personRepository);

			//Act
			_ = await personControler.GetPeopleAsync();

			//Assert
			await personRepository.Received(1).GetAllPeopleAsync();
			
		}

		[Test]
		public async Task GetPerson_WhenCalled_ShouldCallPerson()
		{
			//Arrange
			var personRepository = Substitute.For<IPersonRepository>();
			personRepository.GetPersonAsync(Arg.Any<int>()).Returns(Task.FromResult(
				new Person
				{
					Name ="Name",
					Surname = "Surname",
					Id_Number = "Id_Number"
				}));

			var personControler = new PersonController(personRepository);

			//Act
			_ = await personControler.GetPerson(2);

			//Assert
			await personRepository.Received(1).GetPersonAsync(Arg.Any<int>());
		}

		[Test]
		public async Task SavePerson_WhenCalledWithPerson_ShouldCallAddPerson()
		{
			//Arrange
			var personRepository = Substitute.For<IPersonRepository>();
			personRepository.AddPersonAsync(Arg.Any<Person>()).Returns(1);

			var personControler = new PersonController(personRepository);

			//Act
			_ = await personControler.SavePerson(new Person { Surname = "Surname", Id_Number="sdwdddwvw"});

			await personRepository.Received(1).AddPersonAsync(Arg.Any<Person>());
		}

		private static T GetObjectResultContent<T>(ActionResult<T> result)
		{
			if (result.Result != null)
				return (T)((ObjectResult)result.Result).Value;
			return result.Value;
		}

	}
}
