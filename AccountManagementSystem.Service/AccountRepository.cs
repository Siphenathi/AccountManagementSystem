using AccountManagementSystem.Data;
using AccountManagementSystem.Model;
using AccountManagementSystem.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementSystem.Service
{
	public class AccountRepository : IAccountRepository
	{
		private readonly IRepository<Account> _accountRepository;
		private const string _tableName = "Accounts";
		private const string _primaryKeyName = "Code";
		private const string _parentKeyName = "Person_Code";
		public AccountRepository(string connectionString)
		{
			_accountRepository = new Repository<Account>(_tableName, connectionString);
		}

		public async Task<IEnumerable<Account>> GetAllAccountsAsync()
		{
			return await _accountRepository.GetAllAsync();
		}

		public async Task<Account> GetAccountAsync(int code)
		{
			return await _accountRepository.GetAsync(code, _primaryKeyName);
		}

		public async Task<Account> GetAccountWithParentKey(int parentCode)
		{
			return await _accountRepository.GetAsync(parentCode, _parentKeyName);
		}

		public async Task<bool> AccountExist(int parentCode)
		{
			var accounts = await _accountRepository.GetAllAsync();
			var account = accounts.ToList().Find(x => x.Person_Code == parentCode);
			return account != null;
		}

		public async Task<int> AddAccountAsync(Account account)
		{
			account.Account_Number = DigitsGenerator.GetUniqueDigits().ToString();
			return await _accountRepository.InsertAsync(account, _primaryKeyName);
		}

		public async Task<int> UpdateAccountAsync(Account account)
		{
			var numberOfRowsAffected = await _accountRepository.UpdateAsync(_primaryKeyName, account, _primaryKeyName, _parentKeyName, "Account_Number");
			if (numberOfRowsAffected == 0) throw new KeyNotFoundException($"{_tableName[0..^1]} with {_primaryKeyName} [{account.Code}] could not be found.");
			return numberOfRowsAffected;
		}

		public async Task<int> DeleteAccountAsync(int code)
		{
			var numberOfRowsAffected = await _accountRepository.DeleteAsync(code, _primaryKeyName);
			if (numberOfRowsAffected == 0) throw new KeyNotFoundException($"{_tableName[0..^1]} with {_primaryKeyName} [{code}] could not be found.");
			return numberOfRowsAffected;
		}
	}
}
