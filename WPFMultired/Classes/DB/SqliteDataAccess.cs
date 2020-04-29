using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using WPFMultired.DataModel;
using WPFMultired.Models;
using WPFMultired.Services.Object;

namespace WPFMultired.Classes.DB
{
    static class SqliteDataAccess
    {

        public static TRANSACTION GetTRANSACTION(int idTransaction)
        {
            try
            {
                var query = string.Concat("SELECT * FROM 'TRANSACTION' WHERE TRANSACTION_ID = ", idTransaction);
                return Select<TRANSACTION>(query).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TRANSACTION> GetTransactionNotNotific()
        {
            try
            {
                var query = "SELECT * FROM 'TRANSACTION' WHERE STATE_NOTIFICATION = 0";
                return Select<TRANSACTION>(query);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TRANSACTION> GetTransactionPending()
        {
            try
            {
                var query = "SELECT * FROM 'TRANSACTION' WHERE STATE = 0 AND STATE_TRANSACTION_ID != " + (int)ETransactionState.Initial;
                return Select<TRANSACTION>(query);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TRANSACTION_DETAIL> GetDetailsTransaction()
        {
            try
            {
                var query = "SELECT * FROM 'TRANSACTION_DETAIL' WHERE STATE = 0";
                return Select<TRANSACTION_DETAIL>(query);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool UpdateConfiguration(CONFIGURATION_PAYDAD config)
        {
            try
            {
                var query = "DELETE FROM CONFIGURATION_PAYDAD";
                var configuration = Execute<object>(query, null);

                query = "INSERT INTO CONFIGURATION_PAYDAD (" +
                        "USER_API, " +
                        "PASSWORD_API," +
                        "USER, " +
                        "PASSWORD, " +
                        "TYPE, " +
                        "ID_PAYPAD, " +
                        "ID_SESSION, " +
                        "TOKEN_API) VALUES (" +
                        "@USER_API, " +
                        "@PASSWORD_API, " +
                        "@USER, " +
                        "@PASSWORD, " +
                        "@TYPE, " +
                        "@ID_PAYPAD, " +
                        "@ID_SESSION, " +
                        "@TOKEN_API)";
                Execute<CONFIGURATION_PAYDAD>(query, config);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int SaveTransaction(TRANSACTION transaction)
        {
            try
            {
                var data = new ITRANSACTION
                {
                    TYPE_TRANSACTION_ID = transaction.TYPE_TRANSACTION_ID,
                    PAYER_ID = transaction.PAYER_ID,
                    STATE_TRANSACTION_ID = transaction.STATE_TRANSACTION_ID,
                    TOTAL_AMOUNT = transaction.TOTAL_AMOUNT,
                    DATE_END = transaction.DATE_END,
                    TRANSACTION_ID = transaction.TRANSACTION_ID,
                    RETURN_AMOUNT = transaction.RETURN_AMOUNT,
                    INCOME_AMOUNT = transaction.INCOME_AMOUNT,
                    PAYPAD_ID = transaction.PAYER_ID,
                    DATE_BEGIN = transaction.DATE_BEGIN,
                    STATE_NOTIFICATION = transaction.STATE_NOTIFICATION,
                    STATE = transaction.STATE,
                    DESCRIPTION = transaction.DESCRIPTION,
                    TRANSACTION_REFERENCE = transaction.TRANSACTION_REFERENCE
                };
                var query = "INSERT INTO 'TRANSACTION' (" +
                            "TRANSACTION_ID, " +
                            "PAYPAD_ID," +
                            "TYPE_TRANSACTION_ID, " +
                            "DATE_BEGIN, " +
                            "DATE_END, " +
                            "TOTAL_AMOUNT, " +
                            "INCOME_AMOUNT, " +
                            "RETURN_AMOUNT, " +
                            "DESCRIPTION, " +
                            "PAYER_ID, " +
                            "STATE_TRANSACTION_ID, " +
                            "STATE_NOTIFICATION, " +
                            "STATE " +
                            "TRANSACTION_REFERENCE) VALUES (" +
                            "@TRANSACTION_ID, " +
                            "@PAYPAD_ID, " +
                            "@TYPE_TRANSACTION_ID, " +
                            "@DATE_BEGIN, " +
                            "@DATE_END, " +
                            "@TOTAL_AMOUNT, " +
                            "@INCOME_AMOUNT, " +
                            "@RETURN_AMOUNT, " +
                            "@DESCRIPTION, " +
                            "@PAYER_ID, " +
                            "@STATE_TRANSACTION_ID, " +
                            "@STATE_NOTIFICATION, " +
                            "@STATE " +
                            "TRANSACTION_REFERENCE) ";

                var idTransaccion = Execute<ITRANSACTION>(query, data);

                query = "INSERT INTO TRANSACTION_DESCRIPTION (" +
                            "TRANSACTION_ID, " +
                            "REFERENCE," +
                            "AMOUNT, " +
                            "OBSERVATION, " +
                            "STATE) VALUES (" +
                            "@TRANSACTION_ID, " +
                            "@REFERENCE, " +
                            "@AMOUNT, " +
                            "@OBSERVATION, " +
                            "@STATE)";

                foreach (var description in transaction.TRANSACTION_DESCRIPTION)
                {
                    description.TRANSACTION_ID = idTransaccion;
                    Execute<TRANSACTION_DESCRIPTION>(query, description);
                }

                return GetTRANSACTION((int)transaction.TRANSACTION_ID).ID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static TRANSACTION UpdateTransaction(Transaction transaction)
        {
            try
            {
                TRANSACTION data = GetTRANSACTION(transaction.IdTransactionAPi);

                if (data != null)
                {
                    data.TOTAL_AMOUNT = transaction.Payment.PayValue;
                    data.INCOME_AMOUNT = transaction.Payment.ValorIngresado;
                    data.RETURN_AMOUNT = transaction.Payment.ValorDispensado;
                    data.DESCRIPTION = "Transaccion finalizada correctamente";
                    data.STATE_TRANSACTION_ID = (int)transaction.State;
                    data.DATE_END = DateTime.Now.ToString();
                    data.TRANSACTION_REFERENCE = transaction.consecutive;
                    data.STATE_NOTIFICATION = transaction.StateNotification;

                    if (transaction.State != ETransactionState.ErrorService)
                    {
                        data.STATE_NOTIFICATION = 1;
                    }

                    var query = "UPDATE 'TRANSACTION' SET INCOME_AMOUNT = @INCOME_AMOUNT, " +
                                "RETURN_AMOUNT = @RETURN_AMOUNT, " +
                                "DESCRIPTION = @DESCRIPTION, " +
                                "STATE_TRANSACTION_ID = @STATE_TRANSACTION_ID, " +
                                "DATE_END = @DATE_END, " +
                                "STATE_NOTIFICATION = @STATE_NOTIFICATION " +
                                "TRANSACTION_REFERENCE = @TRANSACTION_REFERENCE" +
                                "STATE_NOTIFICATION = @STATE_NOTIFICATION" +
                                "WHERE TRANSACTION_ID = " + transaction.IdTransactionAPi;

                    Execute<TRANSACTION>(query, data);

                    return data;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static void UpdateTransactionState(TRANSACTION transaction, int type)
        {
            try
            {
                var query = "UPDATE 'TRANSACTION' SET ";

                if (type == 1)
                {
                    query += "STATE_NOTIFICATION = " + transaction.STATE_NOTIFICATION + " WHERE TRANSACTION_ID = " + transaction.TRANSACTION_ID;
                }
                else
                if (type == 2)
                {
                    query += "STATE = " + transaction.STATE + " WHERE TRANSACTION_ID = " + transaction.TRANSACTION_ID;
                }
                else
                {
                    query += "STATE = " + transaction.STATE +
                        "STATE_TRANSACTION_ID = " + transaction.STATE_TRANSACTION_ID +
                        " WHERE TRANSACTION_ID = " + transaction.TRANSACTION_ID;

                }

                Execute<TRANSACTION>(query, null);
            }
            catch (Exception ex)
            {

            }
        }

        public static void UpdateTransactionDetailState(TRANSACTION_DETAIL transactionDetail)
        {
            try
            {
                var query = "UPDATE 'TRANSACTION_DETAIL' SET STATE = " + transactionDetail.STATE + " WHERE TRANSACTION_DETAIL_ID = " + transactionDetail.TRANSACTION_ID;
                Execute<TRANSACTION_DETAIL>(query, null);
            }
            catch (Exception ex)
            {

            }
        }

        public static object SaveLog(object log, ELogType type)
        {
            try
            {
                string query = "";
                object data = null;
                if (type == ELogType.General)
                {
                    data = new PAYPAD_LOG
                    {
                        REFERENCE = ((RequestLog)log).Reference,
                        DESCRIPTION = ((RequestLog)log).Description,
                        STATE = true
                    };

                    query = "INSERT INTO PAYPAD_LOG (" +
                                    "REFERENCE," +
                                    "DESCRIPTION, " +
                                    "STATE) VALUES (" +
                                    "@REFERENCE, " +
                                    "@DESCRIPTION, " +
                                    "@STATE)";
                }
                else if (type == ELogType.Error)
                {
                    data = (ERROR_LOG)log;

                    query = "INSERT INTO ERROR_LOG (" +
                                    "NAME_CLASS, " +
                                    "NAME_FUNCTION," +
                                    "MESSAGE_ERROR, " +
                                    "DESCRIPTION, " +
                                    "DATE, " +
                                    "TYPE," +
                                    "STATE) VALUES (" +
                                    "@NAME_CLASS, " +
                                    "@NAME_FUNCTION, " +
                                    "@MESSAGE_ERROR, " +
                                    "@DESCRIPTION, " +
                                    "@DATE, " +
                                    "@TYPE, " +
                                    "@STATE)";
                }
                else
                {
                    var logDevice = (RequestLogDevice)log;

                    data = new DEVICE_LOG
                    {
                        TRANSACTION_ID = logDevice.TransactionId,
                        CODE = logDevice.Code,
                        DATETIME = logDevice.Date,
                        DESCRIPTION = logDevice.Description
                    };

                    query = "INSERT INTO DEVICE_LOG (" +
                                    "TRANSACTION_ID, " +
                                    "DESCRIPTION," +
                                    "DATETIME, " +
                                    "CODE) VALUES (" +
                                    "@TRANSACTION_ID, " +
                                    "@DESCRIPTION, " +
                                    "@DATETIME, " +
                                    "@CODE)";
                }
                if (!string.IsNullOrEmpty(query) && data != null)
                {
                    return Execute<object>(query, data);
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static int SaveTransactionDetail(RequestTransactionDetails detail, int state)
        {
            try
            {
                var tRANSACTION_DETAIL = new TRANSACTION_DETAIL
                {
                    TRANSACTION_ID = detail.TransactionId,
                    DENOMINATION = detail.Denomination,
                    QUANTITY = detail.Quantity,
                    OPERATION = detail.Operation,
                    CODE = detail.Code,
                    DESCRIPTION = detail.Description,
                    STATE = state
                };

                var query = "INSERT INTO TRANSACTION_DETAIL (" +
                       "TRANSACTION_ID, " +
                       "CODE," +
                       "DENOMINATION, " +
                       "OPERATION, " +
                       "QUANTITY, " +
                       "DESCRIPTION, " +
                       "STATE) VALUES (" +
                       "@TRANSACTION_ID, " +
                       "@CODE, " +
                       "@DENOMINATION, " +
                       "@OPERATION, " +
                       "@QUANTITY, " +
                       "@DESCRIPTION, " +
                       "@STATE)";

                return Execute<TRANSACTION_DETAIL>(query, tRANSACTION_DETAIL);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool InsetConsoleError(PAYPAD_CONSOLE_ERROR error)
        {
            try
            {
                var query = "INSERT INTO PAYPAD_CONSOLE_ERROR (" +
                        "PAYPAD_ID, " +
                        "ERROR_ID," +
                        "ERROR_LEVEL_ID, " +
                        "DEVICE_PAYPAD_ID, " +
                        "DESCRIPTION, " +
                        "DATE, " +
                        "OBSERVATION, " +
                        "STATE) VALUES (" +
                        "@PAYPAD_ID, " +
                        "@ERROR_ID, " +
                        "@ERROR_LEVEL_ID, " +
                        "@DEVICE_PAYPAD_ID, " +
                        "@DESCRIPTION, " +
                        "@DATE, " +
                        "@OBSERVATION, " +
                        "@STATE)";
                Execute<PAYPAD_CONSOLE_ERROR>(query, error);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static IDbConnection LoadConnectionString(string id = "ConnectionString")
        {
            return new SQLiteConnection(@"" + Utilities.GetConfiguration(id, false).ToString());
        }

        public static List<T> Select<T>(string query)
        {
            List<T> result = default(List<T>);
            try
            {
                using (IDbConnection connection = LoadConnectionString())
                {
                    try
                    {
                        result = connection.Query<T>(query).ToList();
                    }
                    catch (InvalidOperationException ex)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static int Execute<T>(string query, T data)
        {
            object result = 0;
            try
            {
                using (IDbConnection connection = LoadConnectionString())
                {
                    try
                    {
                        if (data == null)
                        {
                            result = (int)connection.Execute(query);
                        }
                        else
                        {
                            result = connection.Execute(query, data);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return (int)result;
        }
    }
}
