using aracKiralamaDeneme.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly IDbConnection _connection;

        public Repository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<T>> GetAllAsync(string spName)
        {
            return await _connection.QueryAsync<T>(spName, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> GetByIdAsync(string spName, object parameters)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(
                spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddAsync(string spName, object parameters)
        {
            return await _connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(string spName, object parameters)
        {
            return await _connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(string spName, object parameters)
        {
            return await _connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
