using System;
using System.Collections.Generic;
using System.Text;

namespace AccountManagementSystem.Service
{
	public static class DigitsGenerator
	{
		public static int GetUniqueDigits()
		{
            var random = new Random((int)DateTime.Now.Ticks);
			var result = random.Next(10000000, 1000000000);
			return result;
        }
	}
}
