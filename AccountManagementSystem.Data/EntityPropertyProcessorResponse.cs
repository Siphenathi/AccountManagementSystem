using System;
using System.Collections.Generic;
using System.Text;

namespace AccountManagementSystem.Data
{
	public class EntityPropertyProcessorResponse
	{
		public string TableFields { get; set; }
		public string ModelFields { get; set; }
	}

	public class ListProcessorResponse
	{
		public List<string> List { get; set; }
		public Error Error { get; set;}
	}

	public class Error
	{
		public string Message { get; set; }
	}
}
