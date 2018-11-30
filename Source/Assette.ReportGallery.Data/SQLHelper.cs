//===============================================================================
// Microsoft Data Access Application Block for .NET
// http://msdn.microsoft.Com/library/en-us/dnbda/html/daab-rm.asp
//
// SQLHelper.cs
//
// This file contains the implementations of the SqlHelper and SqlHelperParameterCache
// classes.
//
// For more information see the Data Access Application Block Implementation Overview. 
// 
//===============================================================================
// Copyright (C) 2000-2001 Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//==============================================================================

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using BTA.EPM.Common;

namespace Assette.PA.Data
{
	/// <summary>
	/// The SqlHelper class is intended to encapsulate high performance, scalable best practices for 
	/// Common uses of SqlClient.
	/// </summary>
	public sealed class SqlHelper
	{
	   

		private static string GetContextConnectionString()
		{
#if DEBUG
            return "Data Source = 192.168.180.41;Initial Catalog = DEVGALR;User id = sa;Password =hammer;";
#else

			//Get the connection string from the current context.           
			return BTAConnectionManager.Connection.ClientConnectionString;
			//return "Data Source = gbs-sl-dev02;Initial Catalog = WS20BRK;User id = sa;Password = ;";
#endif
		}

		#region private utility methods & constructors

		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created with "new SqlHelper()".
		private SqlHelper() {}



		/// <summary>
		/// This method is used to attach array of SqlParameters to a Sqlcommand.
		/// 
		/// This method will assign a value of DbNull to any parameter with a direction of
		/// InputOutput and a value of null.  
		/// 
		/// This behavior will prevent default values from being used, but
		/// this will be the less Common case than an intended pure output parameter (derived as InputOutput)
		/// where the user provided no input value.
		/// </summary>
		/// <param name="Command">The Command to which the parameters will be added</param>
		/// <param name="CommandParameters">an array of SqlParameters tho be added to Command</param>
		private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
		{
			foreach (SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}
				
				command.Parameters.Add(p);
			}
		}

		/// <summary>
		/// This method assigns an array of values to an array of SqlParameters.
		/// </summary>
		/// <param name="CommandParameters">array of SqlParameters to be assigned values</param>
		/// <param name="parameterValues">array of objects holding the values to be assigned</param>
		private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
		{
			if ((commandParameters == null) || (parameterValues == null)) 
			{
				//do nothing if we get no data
				return;
			}

			// we must have the same number of values as we pave parameters to put them in
			if (commandParameters.Length != parameterValues.Length)
			{
				throw new ArgumentException("Parameter count does not match Parameter Value count.");
			}

			//iterate through the SqlParameters, assigning the values from the corresponding position in the 
			//value array
			for (int i = 0, j = commandParameters.Length; i < j; i++)
			{
				commandParameters[i].Value = parameterValues[i];
			}
		}

		/// <summary>
		/// This method opens (if necessary) and assigns a connection, transaction, Command type and parameters 
		/// to the provided command.
		/// </summary>
		/// <param name="Command">the SqlCommand to be prepared</param>
		/// <param name="connection">a valid SqlConnection, on which to execute this Command</param>
		/// <param name="transaction">a valid SqlTransaction, or 'null'</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParameters to be associated with the Command or 'null' if no parameters are required</param>
		private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			//if the provided connection is not open, we will open it
			if (connection.State != ConnectionState.Open)
			{
				connection.Open();
			}

			//associate the connection with the Command
			command.Connection = connection;
			
			//set the Command Timeout
			command.CommandTimeout = 1800;
			//set the Command text (stored procedure name or SQL statement)
			command.CommandText = commandText;

			//if we were provided a transaction, assign it.
			if (transaction != null)
			{
				command.Transaction = transaction;
			}

			//set the Command type
			command.CommandType = commandType;

			//attach the Command parameters if they are provided
			if (commandParameters != null)
			{
				AttachParameters(command, commandParameters);
			}

			return;
		}


		#endregion private utility methods & constructors

		#region ExecuteNonQuery

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored prcedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
            //create a Command and prepare it for execution
            SqlCommand cmd = new SqlCommand();

            string[] commands = commandText.Split(new string[] { "GO\r\n", "GO ", "GO\t" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in commands)
            {
                PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, c, commandParameters);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
            // detach the SqlParameters from the Command object, so they can be used again.
            cmd.Parameters.Clear();
            return 0;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a Command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
			
			//finally, execute the command.
			int retval = cmd.ExecuteNonQuery();
			
			// detach the SqlParameters from the Command object, so they can be used again.
			cmd.Parameters.Clear();
			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified 
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the Command</returns>
		public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
			}
		}


		#endregion ExecuteNonQuery

		#region ExecuteDataSet

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteDataset(cn, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
		}
		
		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a Command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);
			
			//create the DataAdapter & DataSet
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();

			//fill the DataSet using default values for DataTable names, etc.
			da.Fill(ds);
			
			// detach the SqlParameters from the Command object, so they can be used again.			
			cmd.Parameters.Clear();
			
			//return the dataset
			return ds;						
		}
		
		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
		}
		
		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a Command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
			
			//create the DataAdapter & DataSet
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();

			//fill the DataSet using default values for DataTable names, etc.
			da.Fill(ds);
			
			// detach the SqlParameters from the Command object, so they can be used again.
			cmd.Parameters.Clear();
			
			//return the dataset
			return ds;
		}
		
		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
			}
		}

		#endregion ExecuteDataSet
		
		#region ExecuteReader

		/// <summary>
		/// this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
		/// we can set the appropriate CommandBehavior when calling ExecuteReader()
		/// </summary>
		private enum SqlConnectionOwnership	
		{
			/// <summary>Connection is owned and managed by SqlHelper</summary>
			Internal, 
			/// <summary>Connection is owned and managed by the caller</summary>
			External
		}

		/// <summary>
		/// Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
		/// </summary>
		/// <remarks>
		/// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
		/// 
		/// If the caller provided the connection, we want to leave it to them to manage.
		/// </remarks>
		/// <param name="connection">a valid SqlConnection, on which to execute this Command</param>
		/// <param name="transaction">a valid SqlTransaction, or 'null'</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParameters to be associated with the Command or 'null' if no parameters are required</param>
		/// <param name="connectionOwnership">indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
		/// <returns>SqlDataReader containing the results of the Command</returns>
		private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
		{	
			//create a Command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);
			
			//create a reader
			SqlDataReader dr;

			// call ExecuteReader with the appropriate CommandBehavior
			if (connectionOwnership == SqlConnectionOwnership.External)
			{
				dr = cmd.ExecuteReader();
			}
			else
			{
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			
			// detach the SqlParameters from the Command object, so they can be used again.
			cmd.Parameters.Clear();
			
			return dr;
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection
			SqlConnection cn = new SqlConnection(connectionString);
			cn.Open();

			try
			{
				//call the private overload that takes an internally owned connection in place of the connection string
				return ExecuteReader(cn, null, commandType, commandText, commandParameters,SqlConnectionOwnership.Internal);
			}
			catch
			{
				//if we fail to return the SqlDatReader, we need to close the connection ourselves
				cn.Close();
				throw;
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//pass through the call to the private overload using a null transaction value and an externally owned connection
			return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

				AssignParameterValues(commandParameters, parameterValues);

				return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteReader(connection, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//pass through to private overload, indicating that the connection is owned by the caller
			return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a SqlDataReader containing the resultset generated by the Command</returns>
		public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

				AssignParameterValues(commandParameters, parameterValues);

				return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
			}
		}

		#endregion ExecuteReader

		#region BTA

		#region ExecuteDataset
		public static DataSet ExecuteDataset(CommandType commandType, string commandText)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteDataset(BTAConnectionManager.Transaction, commandType, commandText, (SqlParameter[])null);
			else
				return ExecuteDataset(GetContextConnectionString(), commandType, commandText, (SqlParameter[])null);
		}

		public static DataSet ExecuteDataset(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteDataset(BTAConnectionManager.Transaction, commandType, commandText, commandParameters);
			else
				return ExecuteDataset(GetContextConnectionString(), commandType, commandText, commandParameters);
		}

		public static DataSet ExecuteDataset(string spName, params object[] parameterValues)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteDataset(BTAConnectionManager.Transaction, spName, parameterValues);
			else
				return ExecuteDataset(GetContextConnectionString(), spName, parameterValues);
		}
		#endregion ExecuteDataset

		#region ExecuteReader
		public static SqlDataReader ExecuteReader(CommandType commandType, string commandText)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteReader(BTAConnectionManager.Transaction, commandType, commandText, (SqlParameter[])null);
			else
				return ExecuteReader(GetContextConnectionString(), commandType, commandText, (SqlParameter[])null);
		}

		public static SqlDataReader ExecuteReader(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteReader(BTAConnectionManager.Transaction, commandType, commandText, commandParameters);
			else
				return ExecuteReader(GetContextConnectionString(), commandType, commandText, commandParameters);
		}

		public static SqlDataReader ExecuteReader(string spName, params object[] parameterValues)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteReader(BTAConnectionManager.Transaction, spName, parameterValues);
			else
				return ExecuteReader(GetContextConnectionString(), spName, parameterValues);
		}
		#endregion ExecuteReader

		#region ExecuteNonQuery
		public static int ExecuteNonQuery(CommandType commandType, string commandText)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteNonQuery(BTAConnectionManager.Transaction, commandType, commandText, (SqlParameter[])null);
			else
				return ExecuteNonQuery(GetContextConnectionString(), commandType, commandText, (SqlParameter[])null);
		}

		public static int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteNonQuery(BTAConnectionManager.Transaction, commandType, commandText, commandParameters);
			else
				return ExecuteNonQuery(GetContextConnectionString(), commandType, commandText, commandParameters);
		}

		public static int ExecuteNonQuery(string spName, params object[] parameterValues)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteNonQuery(BTAConnectionManager.Transaction, spName, parameterValues);
			else
				return ExecuteNonQuery(GetContextConnectionString(), spName, parameterValues);
		}
		#endregion ExecuteNonQuery

		#region ExecuteScalar
		public static object ExecuteScalar(CommandType commandType, string commandText)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteScalar(BTAConnectionManager.Transaction, commandType, commandText);
			else
				return ExecuteScalar(GetContextConnectionString(), commandType, commandText);
		}
		public static object ExecuteScalar(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteScalar(BTAConnectionManager.Transaction, commandType, commandText, commandParameters);
			else
				return ExecuteScalar(GetContextConnectionString(), commandType, commandText, commandParameters);
		}
		public static object ExecuteScalar(string spName, params object[] parameterValues)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteScalar(BTAConnectionManager.Transaction, spName, parameterValues);
			else
				return ExecuteScalar(GetContextConnectionString(), spName, parameterValues);
		}
		#endregion ExecuteScalar

		#region ExecuteXmlReader
		public static XmlReader ExecuteXmlReader(CommandType commandType, string commandText)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteXmlReader(BTAConnectionManager.Transaction, commandType, commandText, (SqlParameter[])null);
			else
				return ExecuteXmlReader(GetContextConnectionString(), commandType, commandText, (SqlParameter[])null);
		}

		public static XmlReader ExecuteXmlReader(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteXmlReader(BTAConnectionManager.Transaction, commandType, commandText, commandParameters);
			else
				return ExecuteXmlReader(GetContextConnectionString(), commandType, commandText, commandParameters);
		}

		public static XmlReader ExecuteXmlReader(string spName, params object[] parameterValues)
		{
			if (BTAConnectionManager.IsInTransaction)
				return ExecuteXmlReader(BTAConnectionManager.Transaction, spName, parameterValues);
			else
				return ExecuteXmlReader(GetContextConnectionString(), spName, parameterValues);
		}
		#endregion ExecuteXmlReader

		#endregion BTA

		#region ExecuteScalar

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteScalar(cn, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a Command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);
			
			//execute the Command & return the results
			object retval = cmd.ExecuteScalar();
			
			// detach the SqlParameters from the Command object, so they can be used again.
			cmd.Parameters.Clear();
			return retval;
			
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a Command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
			
			//execute the Command & return the results
			object retval = cmd.ExecuteScalar();
			
			// detach the SqlParameters from the Command object, so they can be used again.
			cmd.Parameters.Clear();
			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the Command</returns>
		public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
			}
		}

		#endregion ExecuteScalar	

		#region ExecuteXmlReader

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteXmlReader(connectionString, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteXmlReader(cn, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(string connectionString, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteXmlReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteXmlReader(connectionString, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command using "FOR XML AUTO"</param>
		/// <returns>an XmlReader containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command using "FOR XML AUTO"</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>an XmlReader containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a Command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
		   
			PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters);
			
			//create the DataAdapter & DataSet
			XmlReader retval = cmd.ExecuteXmlReader();
			
			// detach the SqlParameters from the Command object, so they can be used again.
			cmd.Parameters.Clear();
			return retval;
			
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure using "FOR XML AUTO"</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an XmlReader containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command using "FOR XML AUTO"</param>
		/// <returns>an XmlReader containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="CommandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command using "FOR XML AUTO"</param>
		/// <param name="CommandParameters">an array of SqlParamters used to execute the Command</param>
		/// <returns>an XmlReader containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a Command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
			
			//create the DataAdapter & DataSet
			XmlReader retval = cmd.ExecuteXmlReader();
			
			// detach the SqlParameters from the Command object, so they can be used again.
			cmd.Parameters.Clear();
			return retval;			
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the Command</returns>
		public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if ((parameterValues != null) && (parameterValues.Length > 0)) 
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters, parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
			}
				//otherwise we can just call the SP without params
			else 
			{
				return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
			}
		}


		#endregion ExecuteXmlReader
	}

	/// <summary>
	/// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
	/// ability to discover parameters for stored procedures at run-time.
	/// </summary>
	public sealed class SqlHelperParameterCache
	{
		#region private methods, variables, and constructors

		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created with "new SqlHelperParameterCache()".
		private SqlHelperParameterCache() {}

		private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// resolve at run time the appropriate set of SqlParameters for a stored procedure
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
		/// <returns></returns>
		private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
		{
			using (SqlConnection cn = new SqlConnection(connectionString)) 
			using (SqlCommand cmd = new SqlCommand(spName,cn))
			{
				cn.Open();
				cmd.CommandType = CommandType.StoredProcedure;

				SqlCommandBuilder.DeriveParameters(cmd);

				if (!includeReturnValueParameter) 
				{
					cmd.Parameters.RemoveAt(0);
				}

				SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];;

				cmd.Parameters.CopyTo(discoveredParameters, 0);

				return discoveredParameters;
			}
		}

		//deep copy of cached SqlParameter array
		private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
		{
			SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

			for (int i = 0, j = originalParameters.Length; i < j; i++)
			{
				clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
			}

			return clonedParameters;
		}

		#endregion private methods, variables, and constructors

		#region caching functions

		/// <summary>
		/// add parameter array to the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <param name="CommandParameters">an array of SqlParamters to be cached</param>
		public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
		{
			//string hashKey = connectionString + ":" + commandText;
			string hashKey = string.Concat(connectionString , ":" , commandText);

			paramCache[hashKey] = commandParameters;
		}

		/// <summary>
		/// retrieve a parameter array from the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="CommandText">the stored procedure name or T-SQL Command</param>
		/// <returns>an array of SqlParamters</returns>
		public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
		{
			//string hashKey = connectionString + ":" + commandText;
			string hashKey = string.Concat(connectionString, ":", commandText);

			SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];
			
			if (cachedParameters == null)
			{			
				return null;
			}
			else
			{
				return CloneParameters(cachedParameters);
			}
		}

		#endregion caching functions

		#region Parameter Discovery Functions

		/// <summary>
		/// Retrieves the set of SqlParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an array of SqlParameters</returns>
		public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
		{
			return GetSpParameterSet(connectionString, spName, false);
		}

		/// <summary>
		/// Retrieves the set of SqlParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
		/// <returns>an array of SqlParameters</returns>
		public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
		{
			string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter":"");
			
			SqlParameter[] cachedParameters;
			
			cachedParameters = (SqlParameter[])paramCache[hashKey];

			if (cachedParameters == null)
			{			
				cachedParameters = (SqlParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
			}
			
			return CloneParameters(cachedParameters);
		}

		#endregion Parameter Discovery Functions

	}
}
