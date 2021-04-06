using AccountManagementSystem.Model;
using AccountManagementSystem.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementSystem.Host.Controllers
{
	[Route("api/[controller]")]
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
			return !allPeople.Any() ? NotFound() : new ActionResult<IEnumerable<Person>>(allPeople);
		}
	}
}
