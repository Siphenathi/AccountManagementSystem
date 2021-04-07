using AccountManagementSystem.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagementSystem.Service.Interface
{
	public interface IAccountRepository
	{
		Task<int> AddAccountAsync(Account account);
		Task<int> DeleteAccountAsync(int code);
		Task<IEnumerable<Account>> GetAllAccountsAsync();
		Task<Account> GetAccountAsync(int code);
		Task<Account> GetAccountWithParentKey(int code);
		Task<bool> AccountExist(int parentCode);
		Task<int> UpdateAccountAsync(Account account);
	}
}
