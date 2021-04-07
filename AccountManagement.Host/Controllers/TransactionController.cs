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
	public class TransactionController : ControllerBase
	{
		private readonly ITransactionRepository _transactionRepository;
		private readonly IAccountRepository _accountRepository;

		public TransactionController(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
		{
			_transactionRepository = transactionRepository;
			_accountRepository = accountRepository;
		}

		[HttpGet]
		[Route("api/v1/[controller]/GetAccountTransaction/{accountCode}")]
		public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsAsync(int accountCode)
		{
			var accountTransaction = await _transactionRepository.GetTransactionsWithParentKeyAsync(accountCode);
			return !accountTransaction.Any() ? StatusCode(404, $"No transactions associated with Account Code : {accountCode}")
				: new ActionResult<IEnumerable<Transaction>>(accountTransaction);
		}

		[HttpGet]
		[Route("api/v1/[controller]/Get/{code}")]
		public async Task<ActionResult<Transaction>> GetTransaction(int code)
		{
			try
			{
				var transaction = await _transactionRepository.GetTransaction(code);
				return StatusCode(200, transaction);
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
		public async Task<ActionResult> SaveTransaction(Transaction transaction)
		{
			try
			{
				var transactionIsNotValid = TransactionIsNotValid(transaction);
				if (transactionIsNotValid.Item1)
					return StatusCode(400, transactionIsNotValid.Item2);

				var numberOfRowsAffected = await HandleTransactions(transaction, TransactionType.Insert);
				return StatusCode(200, $"{numberOfRowsAffected} number of row(s) affected.");
			}
			catch (Exception exception)
			{
				return StatusCode(500, exception.Message);
			}
		}

		[HttpPut]
		[Route("api/v1/[controller]/Update")]
		public async Task<ActionResult> UpdateTransaction(Transaction transaction)
		{
			try
			{
				var transactionIsNotValid = TransactionIsNotValid(transaction);
				if (transactionIsNotValid.Item1)
					return StatusCode(400, transactionIsNotValid.Item2);

				var numberOfRowsAffected = await HandleTransactions(transaction, TransactionType.Update);
				return StatusCode(200, $"{numberOfRowsAffected} number of row(s) affected.");
			}
			catch (Exception exception)
			{
				return StatusCode(500, exception.Message);
			}
		}

		[HttpDelete]
		[Route("api/v1/[controller]/Delete/{code}")]
		public async Task<ActionResult> DeleteTransaction(int code)
		{
			try
			{
				var transaction = await _transactionRepository.GetTransaction(code);
				var numberOfRowsAffected = await _transactionRepository.DeleteTransactionAsync(code);
				await HandleTransactions( new Transaction { Account_Code = transaction.Account_Code, Amount = transaction.Amount }, TransactionType.Delete);

				return StatusCode(200, $"{numberOfRowsAffected} number of row(s) affected.");
			}
			catch (Exception exception)
			{
				return StatusCode(500, exception.Message);
			}
		}

		private async Task<int> HandleTransactions(Transaction transaction, TransactionType transactionType)
		{
			using var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeAsyncFlowOption.Enabled);
			var numberOfRowsAffected = 0;
			var account = await _accountRepository.GetAccountAsync(transaction.Account_Code);
			if (transactionType == TransactionType.Insert)
			{
				transaction.Capture_Date = DateTime.Now;
				numberOfRowsAffected = await _transactionRepository.AddTransactionAsync(transaction);
				account.Outstanding_Balance += transaction.Amount;
			}			
			else if(transactionType == TransactionType.Update)
			{
				var outDatedTransaction = await _transactionRepository.GetTransaction(transaction.Code);
				numberOfRowsAffected = await _transactionRepository.UpdateTransactionAsync(transaction);
				account.Outstanding_Balance -= outDatedTransaction.Amount;
				account.Outstanding_Balance += transaction.Amount;
			}
			else if(transactionType == TransactionType.Delete)
			{
				account.Outstanding_Balance -= transaction.Amount;
			}

			await _accountRepository.UpdateAccountAsync(account);
			transactionScope.Complete();
			return numberOfRowsAffected;
		}

		private static (bool, string) TransactionIsNotValid(Transaction transaction)
		{
			if (transaction.Transaction_Date > DateTime.Now)
				return (true, "Transaction date cannot be in future!");
			if (transaction.Account_Code == 0)
				return (true, "Please provide valid account code!");
			if(transaction.Amount <= 0)
				return (true, "Invalid transaction amount!");
			return (false, null);
		}
	}
}
