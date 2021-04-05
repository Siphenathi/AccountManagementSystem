using AccountManagementSystem.Data.TransferObject;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AccountManagementSystem.Data
{
	public static class EntityPropertyProcessor
	{
		public static EntityPropertyProcessorResponse GetAggregatedTableAndModelFields<TEntity>(params string[] propertiesToBeRemoved)
		{
			var entityPropertyRemovalResponse = propertiesToBeRemoved.Any() ?
				RemoveProperties(GetEntityProperties(typeof(TEntity).GetProperties()), propertiesToBeRemoved) :
				new EntityPropertyRemovalResponse { Properties = GetEntityProperties(typeof(TEntity).GetProperties()) };

			if (entityPropertyRemovalResponse.Error != null) 
				return new EntityPropertyProcessorResponse { Error = entityPropertyRemovalResponse.Error };

				var tableFields = string.Join(",", entityPropertyRemovalResponse.Properties);
			var modelFields = $"@{ string.Join(", @", entityPropertyRemovalResponse.Properties) }";
			return new EntityPropertyProcessorResponse { TableFields = tableFields, ModelFields = modelFields};
		}

		public static EntityPropertyRemovalResponse RemoveProperties(List<string> listOfProperties, params string[] propertiesToBeRemoved)
		{
			foreach(var property in propertiesToBeRemoved)
			{
				var inputIsNotValid = InputIsNotValid(listOfProperties, property);
				if (inputIsNotValid.Item1)
					return new EntityPropertyRemovalResponse { Error = new Error { Message = inputIsNotValid.Item2 } };				

				var propertyIndex = listOfProperties.FindIndex(word => word.Equals(property, System.StringComparison.CurrentCultureIgnoreCase));
				if (propertyIndex == -1) return new EntityPropertyRemovalResponse { Error = new Error { Message = $"property name {property} is not found in the entity provided." }};
				listOfProperties.RemoveAt(propertyIndex);
			};

			return new EntityPropertyRemovalResponse { Properties = listOfProperties };
		}

		public static List<string> GetEntityProperties(IEnumerable<PropertyInfo> listOfProperties)
		{
			if (listOfProperties == null) return new List<string>();

			var entityProperties = (from prop in listOfProperties
									let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
									where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
									select prop.Name).ToList();
			return entityProperties;
		}

		private static (bool, string) InputIsNotValid(List<string> listOfProperties, string property)
		{
			if (listOfProperties == null) return (true, "Invalid list of Properties entered.");
			if (listOfProperties.Count == 0) return (true, "list of Properties entered is empty.");
			if (string.IsNullOrWhiteSpace(property)) return (true, "Invalid property name, check your property names.");
			return (false, null);
		}
	}
}
