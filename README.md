# dynamic-oracle-procedure-call
C# Function for calling any oracle database procedure with input and output valus

you have to use Oracle.ManagedDataaccess package from NuGet Packages

Install-Package Oracle.ManagedDataAccess -Version 12.1.21

How to use the function :

 var parData = new List<Tuple<string, OracleDbType, ParameterDirection, dynamic>>();

 parData.Add(new Tuple<string, OracleDbType, ParameterDirection, dynamic>("ParameterName", OracleDbType.Int32, ParameterDirection.Input, "ParameterValue"));
 parData.Add(new Tuple<string, OracleDbType, ParameterDirection, dynamic>("ParameterName", OracleDbType.Int32, ParameterDirection.Output, "string.Empty OR Null ""));
  List<Tuple<string, dynamic>> ReqDataResult = Helper.DBProcedure("Connection string for oracle DB", "PackageName.ProcedureName", parData);
