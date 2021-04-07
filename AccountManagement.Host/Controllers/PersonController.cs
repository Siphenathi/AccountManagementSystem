using AccountManagementSystem.Model;
using AccountManagementSystem.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementSystem.Host.Controllers
{
	[ApiController]
	public class PersonController : ControllerBase
	{
		private readonly IPersonRepository _personRepository;

		public PersonController(IPersonRepository personRepository)
		{
			_personRepository = personRepository;
		}

		[HttpGet]
		[Route("api/v1/[controller]")]
		public async Task<ActionResult<IEnumerable<Person>>> GetPeopleAsync()
		{
			var allPeople = await _personRepository.GetAllPeopleAsync();
			return !allPeople.Any() ? StatusCode(404, "No people found yet!") : new ActionResult<IEnumerable<Person>>(allPeople);
		}

		[HttpGet]
		[Route("api/v1/[controller]/Get/{code}")]
		public async Task<ActionResult<Person>> GetPerson(int code)
		{
			try
			{
				var person = await _personRepository.GetPersonAsync(code);
				return StatusCode(200, person);
			}
			catch (Exception exception)
			{
				if (exception.GetType().Name == "KeyNotFoundException")
					return NotFound(exception.Message);
				return StatusCode(500, exception.Message);
			}
		}

		[HttpPost]
		[Route("api/v1/[controller]/Save")]
		public async Task<ActionResult> SavePerson(Person person)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(person.Id_Number)) return StatusCode(400, "ID Number is required!");
				var numberOfRowsAffected = await _personRepository.AddPersonAsync(person);
				return StatusCode(200, $"{numberOfRowsAffected} number of row(s) affected.");
			}
			catch (Exception exception)
			{
				return StatusCode(500, exception.Message);
			}
		}

		[HttpPut]
		[Route("api/v1/[controller]/Update")]
		public async Task<ActionResult> UpdatePerson(Person person)
		{
			try
			{
				var numberOfRowsAffected = await _personRepository.UpdatePersonAsync(person);
				return StatusCode(202, $"{numberOfRowsAffected} number of row(s) affected.");
			}
			catch (Exception exception)
			{
				if (exception.GetType().Name == "KeyNotFoundException")
					return NotFound(exception.Message);
				return StatusCode(304, exception.Message);
			}
		}

		[HttpDelete]
		[Route("api/v1/[controller]/Delete/{code}")]
		public async Task<ActionResult> DeletePerson(int code)
		{
			try
			{
				var numberOfRowsAffected = await _personRepository.DeletePersonAsync(code);
				return StatusCode(202, $"{numberOfRowsAffected} number of row(s) affected.");
			}
			catch (Exception exception)
			{
				if (exception.GetType().Name == "KeyNotFoundException")
					return NotFound(exception.Message);
				return StatusCode(500, exception.Message);
			}
		}
	}
}
