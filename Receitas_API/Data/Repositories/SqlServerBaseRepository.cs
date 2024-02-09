using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Receitas_API.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Receitas_API.Data.Repositories
{
    public abstract class SQLServerBaseRepository<T> : IBaseRepository<T>
    {
        protected string connectionString;
        protected IDbConnection con;
        private readonly SqlConnectionConfiguration _configuration;
        private readonly ILogger _logger;

        public SQLServerBaseRepository(SqlConnectionConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            connectionString = _configuration.Value;
            _logger = logger;
        }

        public virtual long Insert(T entity)
        {
            var spInsert = $"usp_{typeof(T).Name}_Create";
            long RecordInserted = 0;
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var regs = connection.Execute(spInsert, entity, commandType: CommandType.StoredProcedure);
                    RecordInserted = (long)GetLastId();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return RecordInserted;
        }

        public virtual void DeleteById(int Id)
        {
            var spDelete = $"usp_{typeof(T).Name}_Delete";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Execute(spDelete, new { Id }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
            }
        }

        public virtual void Update(T entity)
        {
            var spUpdate = $"usp_{typeof(T).Name}_Update";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Execute(spUpdate, entity, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
            }
        }

        public virtual IEnumerable<T> Query(string where = null)
        {

            bool CallSP = string.IsNullOrEmpty(where);
            var query = $"SELECT * FROM {typeof(T).Name} ";

            if (!string.IsNullOrWhiteSpace(where))
            {
                query += $"WHERE {where}";
            }
            else
            {
                query = $"usp_{typeof(T).Name}_GetAll";
            }

            using (var connection = new SqlConnection(connectionString))
            {
                if (!CallSP)
                    return connection.Query<T>(query);
                else
                    return connection.Query<T>(query, commandType: CommandType.StoredProcedure);
            }
        }

        public virtual T Query_ById(int Id)
        {
            var spQueryId = $"usp_{typeof(T).Name}_GetSingle";


            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<T>(spQueryId, new { Id },
                    commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();
            }
        }

        public virtual bool EntradaExiste_BD(string campo, string str2Search)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var output = Query($"{campo} = '{str2Search}'");
                if (output.Count() > 0)
                    return true;
            }
            return false;
        }

        public bool TableHasData()
        {
            var spQueryAll = $"usp_{typeof(T).Name}_GetAll";

            using (var connection = new SqlConnection(connectionString))
            {
                //string sql = $"SELECT * FROM {typeof(T).Name}";
                int result = connection.Query<int>(spQueryAll, commandType: CommandType.StoredProcedure).Count();
                return result > 0;
            }
        }

        public int GetFirstId()
        {
            if (TableHasData())
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = $"SELECT TOP 1 Id FROM {typeof(T).Name} ORDER BY Id ";
                    int result = connection.Query<int>(sql).SingleOrDefault();
                    return result;
                }
            }
            else
                return 0;
        }

        public int GetLastId()
        {
            if (TableHasData())
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = $"SELECT Id FROM {typeof(T).Name} ORDER BY Id DESC";
                    int lastId = connection.Query<int>(sql).FirstOrDefault();
                    return lastId;
                }
            }
            else
                return 0;
        }

        public bool RecInUse(int Id)
        {
            var uSP = $"usp_{typeof(T).Name}_RecInUse_ById";
            using (var connection = new SqlConnection(connectionString))
            {
                bool exists = connection.Query<bool>(uSP, new { Id }, commandType: CommandType.StoredProcedure)
                    .SingleOrDefault();
                return exists;
            }
        }

    

        public int GetIdByDescription(string Descricao)
        {
            var uSP = $"usp_{typeof(T).Name}_GetId_ByDescription";
            using (var connection = new SqlConnection(connectionString))
            {
                int iID = connection.Query<int>(uSP, new { Descricao }, commandType: CommandType.StoredProcedure)
                    .SingleOrDefault();
                return iID;
            }
        }

        public string GetDescriptionById(int Id)
        {
            var uSP = $"usp_{typeof(T).Name}_GetDescription_ById";
            using (var connection = new SqlConnection(connectionString))
            {
                string sDescricao = connection.Query<string>(uSP, new { Id }, commandType: CommandType.StoredProcedure)
                    .SingleOrDefault();
                return sDescricao;
            }
        }

        public bool RecInUse(string Descricao)
        {
            var uSP = $"usp_{typeof(T).Name}_RecInUse_ByDescription";
            using (var connection = new SqlConnection(connectionString))
            {
                bool exists = connection.Query<bool>(uSP, new { Descricao }, commandType: CommandType.StoredProcedure)
                    .SingleOrDefault();
                return exists;
            }
        }

        public List<T> Listar()
        {
            var uSP = $"usp_{typeof(T).Name}_GetAll";
            using (var connection = new SqlConnection(connectionString))
            {
                var recordList = connection.Query<T>(uSP, commandType: CommandType.StoredProcedure)
                    .ToList();
                return recordList;
            }
        }

        public IEnumerable<T> ListByDescription(string Descricao)
        {
            var uSP = $"usp_{typeof(T).Name}_ListByDescription";
            using (var connection = new SqlConnection(connectionString))
            {
                var recordList = connection.Query<T>(uSP, new { Descricao }, commandType: CommandType.StoredProcedure)
                    .ToList();
                return recordList;
            }
        }

        public IDataReader ListByDescription_Reader(string Descricao)
        {
            var uSP = $"usp_{typeof(T).Name}_ListByDescription";
            var connection = new SqlConnection(connectionString);

            var recordList = connection.ExecuteReader(uSP, new { Descricao }, commandType: CommandType.StoredProcedure);

            return recordList;
        }

        public IEnumerable<T> Listar_ID(int Id)
        {
            var uSP = $"usp_{typeof(T).Name}_ListById";
            using (var connection = new SqlConnection(connectionString))
            {
                var recordList = connection.Query<T>(uSP, new { Id }, commandType: CommandType.StoredProcedure)
                    .ToList();
                return recordList;
            }
        }

        public IDataReader Listar_ID_DR(int Id)
        {
            var uSP = $"usp_{typeof(T).Name}_ListById";
            var connection = new SqlConnection(connectionString);

            var recordList = connection.ExecuteReader(uSP, new { Id }, commandType: CommandType.StoredProcedure);
            return recordList;
        }
    }
}
