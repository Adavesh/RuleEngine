using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RuleEngine.Data
{
    /// <summary>
    /// Helper class to perform database operations - Mini Entity Framework
    /// </summary>
    public class DatabaseManager
    {
        private static string ConnectionString;

        /// <summary>
        /// Static constructor to initialize the connection string.
        /// </summary>
        static DatabaseManager()
        {
            ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\adavesh.managaon\source\repos\RuleEngine\RuleEngine\Data\RuleEngine.mdf;Integrated Security=True";
        }

        /// <summary>
        /// Saved the given <see cref="Rule" instance to database./>
        /// </summary>
        /// <param name="rule">Rule to be saved into the DB</param>
        public static void AddRule(Rule rule)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var command = conn.CreateCommand())
                {
                    conn.Open();
                    command.CommandText = "AddNewRule";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@signal", rule.SignalSource);
                    command.Parameters.AddWithValue("@condition", rule.Condition);
                    command.Parameters.AddWithValue("@targetValue", rule.TargetValue);
                    command.Parameters.AddWithValue("@valueType", rule.ValueType);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Removes the given <see cref="Rule" instance to database./>
        /// </summary>
        /// <param name="rule">Rule to be removed from the DB</param>
        public static void RemoveRule(Rule rule)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var command = conn.CreateCommand())
                {
                    conn.Open();
                    command.CommandText = "RemoveRule";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@signal", rule.SignalSource));
                    command.Parameters.Add(new SqlParameter("@condition", rule.Condition));
                    command.Parameters.Add(new SqlParameter("@targetValue", rule.TargetValue));
                    command.Parameters.Add(new SqlParameter("@value", rule.ValueType));
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Reads all data from Rules table and returns.
        /// </summary>
        /// <returns>List of <see cref="Rule"/> instances.</returns>
        public static List<Rule> GetAllRules()
        {
            List<Rule> rules = new List<Rule>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var command = conn.CreateCommand())
                {
                    conn.Open();
                    command.CommandText = "GetAllRules";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string signal = Convert.ToString(reader["SignalSource"])?.Trim();
                            string condition = Convert.ToString(reader["Condition"])?.Trim();
                            string targetVal = Convert.ToString(reader["TargetValue"])?.Trim();
                            string valueType = Convert.ToString(reader["ValueType"])?.Trim();

                            rules.Add(new Rule(signal, condition, targetVal, valueType));
                        }
                    }
                }
            }   

            return rules;
        }

        /// <summary>
        /// Removes all the Rules from the database.
        /// </summary>
        public static void RemoveAllRules()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var command = conn.CreateCommand())
                {
                    conn.Open();
                    command.CommandText = "RemoveAllRules";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
