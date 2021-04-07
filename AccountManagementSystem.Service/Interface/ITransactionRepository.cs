using AccountManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementSystem.Service.Interface
{
	public interface ITransactionRepository
	{
		Task<int> AddTransactionAsync(Transaction transaction);
		Task<int> DeleteTransactionAsync(int code);
		Task<Transaction> GetTransaction(int code);
		Task<IEnumerable<Transaction>> GetTransactionsWithParentKeyAsync(int code);
		Task<int> UpdateTransactionAsync(Transaction transaction);
	}
}
