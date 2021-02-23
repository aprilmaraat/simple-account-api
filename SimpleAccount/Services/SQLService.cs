using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace SimpleAccount.Services
{
    public class SQLService
    {
        private static string _contentRootPath;
        private static string connectionString;

        ///// <summary>
        ///// Initializes database service settings
        ///// </summary>
        ///// <param name="configuration">Application Configuration Settings</param>
        public static void Init(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("HaydenDb");
        }

        /// <summary>
        /// Sets the content root path.
        /// </summary>
        /// <param name="contentRootPath">Root path of the project.</param>
        public static void SetContentRootPath(string contentRootPath)
        {
            _contentRootPath = contentRootPath;
        }

        /// <summary>
        /// Result of the execution of a migration
        /// </summary>
        public class SQLMigrationResults
        {
            /// <summary>
            /// Indicates if the executions was a success or failure.
            /// </summary>
            public bool Success { get; set; }
            /// <summary>
            /// When the execution is a failure, this is the exception
            /// </summary>
            public Exception Error { get; set; }
        }

        /// <summary>
        /// Result of the execution of a database query.
        /// </summary>
        public class DatabaseResult
        {
            /// <summary>
            /// Indicates if the executions was a success or failure.
            /// </summary>
            public bool Success { get; set; }
            /// <summary>
            /// When the execution is a failure, this is the exception
            /// </summary>
            public Exception Exception { get; set; }
            /// <summary>
            /// The number of row affected
            /// </summary>
            public int RowsAffected { get; set; }
        }
        /// <summary>
        /// Runs the SQL queries in the migration files
        /// </summary>
        /// <returns><see cref="SQLMigrationResults"/></returns>
        public static async Task<SQLMigrationResults> RunMigration()
        {
            string path = Path.Combine(_contentRootPath, "SQL");

            if (!Directory.Exists(path))
                return new SQLMigrationResults
                {
                    Success = false,
                    Error = new Exception("No directory or SQL files found.")
                };

            IEnumerable<string> sqlFiles = Directory.GetFiles(path, "*.SQL", SearchOption.AllDirectories)
                .OrderBy(x => x);

            bool success = true;
            List<Exception> errors = new List<Exception>();

            foreach (string sqlFile in sqlFiles)
            {
                try
                {
                    DatabaseResult result = await ExecQueryFromFile(sqlFile);
                    if (result.Success) continue;
                    success = false;
                    Console.WriteLine(result.Exception.Message);
                    errors.Add(result.Exception);
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine(ex.Message);
                    errors.Add(ex);
                }
            }

            if (success)
                return new SQLMigrationResults
                {
                    Success = true,
                    Error = null
                };

            var error = new AggregateException("Error recreating SQL Objects", errors);
            Console.WriteLine(error.Message);

            return new SQLMigrationResults
            {
                Success = true,
                Error = error
            };

        }

        /// <summary>
        /// Helper method to support executing SQL queries from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns><see cref="DatabaseResult"/></returns>
        public static async Task<DatabaseResult> ExecQueryFromFile(string fileName)
        {
            string sqlContent = await File.ReadAllTextAsync(fileName);
            DatabaseResult dbResult = await ExecQuery(sqlContent, null);

            if (dbResult.Success)
                return new DatabaseResult { Success = true };

            return new DatabaseResult
            {
                Success = false,
                Exception = new Exception($"Error executing query in {fileName}", dbResult.Exception),
            };
        }

        /// <summary>
        /// Executes a query on the database
        /// </summary>
        /// <param name="bID">The ID of the Business the query is pertaining to for logging.</param>
        /// <param name="sql">The SQL query to be executed.</param>
        /// <param name="model">The object passed into the query as the parameters.</param>
        /// <returns><see cref="DatabaseResult"/></returns>
        public static async Task<DatabaseResult> ExecQuery(string sql, object parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    bool success = true;
                    string badSql = null;
                    Exception error = null;
                    int rows = 0;

                    try
                    {
                        rows = await connection.ExecuteAsync(sql, parameters, transaction);
                    }
                    catch (Exception err)
                    {
                        badSql = sql;
                        success = false;
                        error = err;
                    }

                    if (!success)
                    {
                        transaction.Rollback();
                        return new DatabaseResult
                        {
                            Success = false,
                            Exception = error
                        };
                    }

                    transaction.Commit();
                    return new DatabaseResult { Success = true, RowsAffected = rows };
                }

                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
