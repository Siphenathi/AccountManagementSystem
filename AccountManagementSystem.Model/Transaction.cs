using System;
using System.Collections.Generic;
using System.Text;

namespace AccountManagementSystem.Model
{
	public class Transaction
	{
		public int Code { get; set; }
		public int Account_Code { get; set; }
		public DateTime Transaction_Date { get; set; }
		public DateTime Capture_Date { get; set; }
		public decimal Amount { get; set; }
		public string Description { get; set; }
	}
}
