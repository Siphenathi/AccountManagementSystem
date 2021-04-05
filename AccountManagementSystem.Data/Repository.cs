﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Data.SqlClient;

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
			using IDbConnection connection = GetConnection();
			var result = await connection.QueryAsync<TEntity>($"Select * from {_tableName}");

			return result;
		}

		public async Task<TEntity> GetAsync(int id, string primaryKeyName)
		{
			using IDbConnection connection = GetConnection();
			var getRecordQuery = $"Select * from {_tableName} where {primaryKeyName}=@Id";
			var result = await connection.QuerySingleOrDefaultAsync<TEntity>(getRecordQuery, new { Id=id });

			if (result == null)
				throw new KeyNotFoundException($"{_tableName} with {primaryKeyName} [{id}] could not be found.");
			return result;
		}

		public async Task<int> InsertAsync(TEntity entity, params string [] namesOfPropertiesToBeExcluded)
		{
			var entityPropertyProcessorResponse = EntityPropertyProcessor.GetFormattedQueryStatementBody<TEntity>(QueryStatement.InsertQuery, namesOfPropertiesToBeExcluded);
			if (entityPropertyProcessorResponse.Error != null)
				throw new Exception(entityPropertyProcessorResponse.Error.Message);

			var insertQuery = $"Insert into {_tableName} {entityPropertyProcessorResponse.Result}";
			using IDbConnection connection = GetConnection();
			return await connection.ExecuteAsync(insertQuery, entity);
		}

		public async Task<int> UpdateAsync(string primaryKeyName, TEntity entity, params string[] namesOfPropertiesToBeExcluded)
		{
			var entityPropertyProcessorResponse = EntityPropertyProcessor.GetFormattedQueryStatementBody<TEntity>(QueryStatement.UpdateQuery, namesOfPropertiesToBeExcluded);
			if (entityPropertyProcessorResponse.Error != null)
				throw new Exception(entityPropertyProcessorResponse.Error.Message);

			var updateQuery = $"update dbo.{_tableName} set {entityPropertyProcessorResponse.Result} where {primaryKeyName}=@{primaryKeyName}";
			using IDbConnection connection = GetConnection();
			return await connection.ExecuteAsync(updateQuery, entity);
		}
		public async Task<int> DeleteAsync(int id, string primaryKeyName)
		{
			var deleteQuery = $"delete from {_tableName} where {primaryKeyName}=@Id";
			using IDbConnection connection = GetConnection();
			return await connection.ExecuteAsync(deleteQuery, new { Id = id });
		}
		private SqlConnection GetConnection()
		{
			return new SqlConnection(_connectionString);
		}
	}
}
