using System.Collections.Generic;

namespace AccountManagementSystem.Data.TransferObject
{
	public class EntityPropertyRemovalResponse
	{
		public List<string> Properties { get; set; }
		public Error Error { get; set;}
	}
}
