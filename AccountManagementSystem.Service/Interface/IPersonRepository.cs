using AccountManagementSystem.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagementSystem.Service.Interface
{
	public interface IPersonRepository
	{
		Task<int> AddPersonAsync(Person person);
		Task<int> DeletePersonAsync(int code);
		Task<IEnumerable<Person>> GetAllPeopleAsync();
		Task<Person> GetPerson(int code);
		Task<int> UpdatePersonAsync(Person person);
	}
}