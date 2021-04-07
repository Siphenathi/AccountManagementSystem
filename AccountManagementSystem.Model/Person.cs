using System.ComponentModel.DataAnnotations;

namespace AccountManagementSystem.Model
{
	public class Person
	{
		public int Code { get; set; }
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		public string Id_Number { get; set; }
	}
}
