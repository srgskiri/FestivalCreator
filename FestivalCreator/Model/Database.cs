﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalCreator.Model
{
    class Database
    {
        public static ConnectionStringSettings ConnectionString //referentie toevoegen om "ConnectionStringSettings" te vinden/gebruiken
        {
            get { return ConfigurationManager.ConnectionStrings["ConnectionString"]; }
        }

        private static DbConnection GetConnection()
        {
            DbConnection con = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateConnection();
            con.ConnectionString = ConnectionString.ConnectionString;
            con.Open();
            return con;
        }

        public static void ReleaseConnection(DbConnection con)
        {
            if (con != null)
            {
                con.Close();
                con = null;
            }
        }

        private static DbCommand BuildCommand(string sql, params DbParameter[] parameters) //"paramas" = optioneel
        {
            DbCommand command = GetConnection().CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        public static DbDataReader GetData(string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                command = BuildCommand(sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (reader != null)
                    reader.Close();

                if (command != null)
                    ReleaseConnection(command.Connection);

                throw;
            }

        }

        public static int ModifyData(string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(sql, parameters);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (command != null)
                    ReleaseConnection(command.Connection);

                return 0;

                throw;
            }
        }

        public static DbParameter AddParameter(string key, string value)
        {
            DbParameter par = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateParameter();
            par.ParameterName = key;
            par.Value = value;
            return par;
        }

        #region "transaction"

        public static DbTransaction BeginTransaction()
        {
            DbConnection con = null;

            try
            {
                con = GetConnection();
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReleaseConnection(con);
                throw;
            }
        }

        private static DbCommand BuildCommand(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = trans.Connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        public static DbDataReader GetData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                command = BuildCommand(trans, sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (reader != null)
                    reader.Close();

                if (command != null)
                    ReleaseConnection(command.Connection);

                throw;
            }

        }

        public static int ModifyData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(trans, sql, parameters);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (command != null)
                    ReleaseConnection(command.Connection);

                return 0;

                throw;
            }
        }

        #endregion
    }
}
