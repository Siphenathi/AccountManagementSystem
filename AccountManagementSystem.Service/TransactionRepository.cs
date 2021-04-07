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
	public class TransactionRepository : ITransactionRepository
	{
		private readonly IRepository<Transaction> _transactionRepository;
		private const string _tableName = "Transactions";
		private const string _primaryKeyName = "Code";
		private const string _parentKeyName = "Account_Code";
		public TransactionRepository(string connectionString)
		{
			_transactionRepository = new Repository<Transaction>(_tableName, connectionString);
		}

		public async Task<Transaction> GetTransaction(int code)
		{
			return await _transactionRepository.GetAsync(code, _primaryKeyName);
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsWithParentKeyAsync(int parentCode)
		{
			var accountTransactions = await _transactionRepository.GetAllAsync(parentCode, _parentKeyName);
			return accountTransactions;
		}

		public async Task<int> AddTransactionAsync(Transaction transaction)
		{
			return await _transactionRepository.InsertAsync(transaction, _primaryKeyName);
		}

		public async Task<int> UpdateTransactionAsync(Transaction transaction)
		{
			var numberOfRowsAffected = await _transactionRepository.UpdateAsync(_primaryKeyName, transaction, _primaryKeyName, _parentKeyName, "Capture_Date");
			if (numberOfRowsAffected == 0) throw new KeyNotFoundException($"{_tableName[0..^1]} with {_primaryKeyName} [{transaction.Code}] could not be found.");
			return numberOfRowsAffected;
		}

		public async Task<int> DeleteTransactionAsync(int code)
		{
			var numberOfRowsAffected = await _transactionRepository.DeleteAsync(code, _primaryKeyName);
			if (numberOfRowsAffected == 0) throw new KeyNotFoundException($"{_tableName[0..^1]} with {_primaryKeyName} [{code}] could not be found.");
			return numberOfRowsAffected;
		}
	}
}
