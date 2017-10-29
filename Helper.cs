using System;

namespace oracle_caller
{
    public class Helper
    {
		public static dynamic DBProcedure(string conString, string FunctionName, List<Tuple<string, OracleDbType, ParameterDirection, dynamic>> Paramerters)
		{

			using (OracleConnection conn = new OracleConnection(conString))
			{
				conn.Open();
				OracleTransaction transaction;
				// Start a local transaction
				transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
				// Assign transaction object for a pending local transaction
				OracleCommand cmd = new OracleCommand();
				cmd.Connection = conn;
				cmd.InitialLONGFetchSize = 1000;
				cmd.Transaction = transaction;
				OracleDataAdapter da = new OracleDataAdapter(cmd);

				cmd.BindByName = true;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = FunctionName;

				List<string> outputPar = new List<string>();

				List<Tuple<string, dynamic>> outputParVals = new List<Tuple<string, dynamic>>();
				try
				{
					foreach (var item in Paramerters)
					{
						cmd.Parameters.Add(item.Item1, item.Item2, 32767);
						if (item.Item3 == ParameterDirection.Input)
						{
							cmd.Parameters[item.Item1].Value = item.Item4;
						}
						else
						{
							outputPar.Add(item.Item1);
						}
						cmd.Parameters[item.Item1].Direction = item.Item3;
					}

					da.SelectCommand = cmd;

					DataTable dt = new DataTable();
					da.Fill(dt);

					if (outputPar != null)
					{
						if (dt != null)
						{
							if (dt.Rows.Count > 0)
							{
								outputParVals.Add(new Tuple<string, dynamic>("Cusror", dt));
							}
						}

						foreach (string item in outputPar)
						{
							outputParVals.Add(new Tuple<string, dynamic>(item, cmd.Parameters[item].Value));
						}

						transaction.Commit();
						return outputParVals;
					}
					else
					{
						transaction.Rollback();
						outputParVals.Add(new Tuple<string, dynamic>("Result", 0));
						return outputParVals;
					}
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					outputParVals.Add(new Tuple<string, dynamic>("Error", ex.Message));
					return outputParVals;
				}
			}
		}
    }
}
