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
	public class AccountController : ControllerBase
	{
		private readonly IAccountRepository _accountRepository;

		public AccountController(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		[HttpGet]
		[Route("api/v1/[controller]")]
		public async Task<ActionResult<IEnumerable<Account>>> GetAccountsAsync()
		{
			var allAcounts = await _accountRepository.GetAllAccountsAsync();
			return !allAcounts.Any() ? StatusCode(404, "No accounts found yet!") : new ActionResult<IEnumerable<Account>>(allAcounts);
		}

		[HttpGet]
		[Route("api/v1/[controller]/Get/{code}")]
		public async Task<ActionResult<Account>> GetAccount(int code)
		{
			try
			{
				var account = await _accountRepository.GetAccountAsync(code);
				return StatusCode(200, account);
			}
			catch (Exception exception)
			{
				if (exception.GetType().Name == "KeyNotFoundException")
					return NotFound(exception.Message);
				return StatusCode(500, exception.Message);
			}
		}

		[HttpGet]
		[Route("api/v1/[controller]/GetAccountWithParentKey/{parentCode}")]
		public async Task<ActionResult<Account>> GetAccountWithParentKey(int parentCode)
		{
			try
			{
				var account = await _accountRepository.GetAccountWithParentKey(parentCode);
				return StatusCode(200, account);
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
		public async Task<ActionResult> SaveAccount(Account account)
		{
			try
			{
				if (await _accountRepository.AccountExist(account.Person_Code))
					return StatusCode(400, "User already got an account!");

				account.Outstanding_Balance = 0;
				var numberOfRowsAffected = await _accountRepository.AddAccountAsync(account);
				return StatusCode(200, $"{numberOfRowsAffected} number of row(s) affected.");
			}
			catch (Exception exception)
			{
				return StatusCode(500, exception.Message);
			}
		}

		[HttpDelete]
		[Route("api/v1/[controller]/Delete/{code}")]
		public async Task<ActionResult> DeleteAccount(int code)
		{
			try
			{
				var numberOfRowsAffected = await _accountRepository.DeleteAccountAsync(code);
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
