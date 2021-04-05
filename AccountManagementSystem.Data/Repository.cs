﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.Linq;
using System.ComponentModel;

namespace AccountManagementSystem.Data
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private readonly string _tableName;
		private readonly string _connectionString;
		public Repository(string tableName, string connectionString)
		{
			_tableName = tableName;
			_connectionString = connectionString;
		}
		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			using IDbConnection connection = new SqlConnection(_connectionString);
			var result = await connection.QueryAsync<TEntity>($"Select * from {_tableName}");

			return result;
		}

		public async Task<TEntity> GetAsync(int code, string primaryKeyName)
		{
			using IDbConnection connection = new SqlConnection(_connectionString);
			var getRecordQuery = $"Select * from {_tableName} where {primaryKeyName}=@Id";
			var result = await connection.QuerySingleOrDefaultAsync<TEntity>(getRecordQuery, new { Id=code });

			if (result == null)
				throw new KeyNotFoundException($"{_tableName} with {primaryKeyName} [{code}] could not be found.");
			return result;
		}

		public async Task<int> InsertAsync(TEntity entity, string autoGeneratedKeyName)
		{
			var entityPropertyProcessorResponse = EntityPropertyProcessor.GetAggregatedTableAndModelFields<TEntity>(autoGeneratedKeyName);
			var insertQuery = $"Insert into {_tableName} ({entityPropertyProcessorResponse.TableFields}) VALUES ({entityPropertyProcessorResponse.ModelFields})";
			using IDbConnection connection = new SqlConnection(_connectionString);
			var numberOfRowAffected = await connection.ExecuteAsync(insertQuery, entity);

			return numberOfRowAffected;
		}

		public Task UpdateAsync(TEntity entity)
		{
			throw new NotImplementedException();
		}
		public Task DeleteRowAsync(int id)
		{
			throw new NotImplementedException();
		}
	}
}
