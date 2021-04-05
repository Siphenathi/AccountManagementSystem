using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagementSystem.Data
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetAsync(int id, string primaryKeyName);
		Task<int> InsertAsync(T entity, params string[] namesOfPropertiesToBeExcluded);
		Task<int> UpdateAsync(string primaryKeyName, T entity, params string[] namesOfPropertiesToBeExcluded);
		Task<int> DeleteAsync(int id, string primaryKeyName);		
	}
}
