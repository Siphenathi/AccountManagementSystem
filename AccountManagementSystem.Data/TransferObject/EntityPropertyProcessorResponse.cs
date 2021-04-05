using System;
using System.Text;

namespace AccountManagementSystem.Data.TransferObject
{
	public class EntityPropertyProcessorResponse
	{
		public string TableFields { get; set; }
		public string ModelFields { get; set; }
		public Error Error { get; set; }
	}
}
