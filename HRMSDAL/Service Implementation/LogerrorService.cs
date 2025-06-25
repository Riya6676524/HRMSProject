//using HRMSModels;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HRMSUtility;

//namespace HRMSDAL
//{
//    public class LogErrorService
//    //{
//        public static void SaveError(ExceptionLogModel model)
//        {

//            return ExceptionHandler.Handle(() =>
//            {
//                using (SqlConnection con = DBHelper.GetConnection())
//                using (SqlCommand cmd = DBHelper.CreateCommand("", con))
//                {


//                    SqlDataReader reader = DBHelper.ExecuteReader(cmd);
//                    if (reader.Read())
//                    {

//                    }

//                    reader.Close();
//                }
//            },
//            defaultValue: null);

//        }
//    }


//< !--<? xml version = "1.0" encoding = "utf-8" ?>
//< log4net >

//    < appender name = "DbAppender" type = "log4net.Appender.AdoNetAppender" >

//        < bufferSize value = "1" />

//        < connectionType value = "System.Data.SqlClient.SqlConnection, System.Data" />

//        < connectionString value = "Data Source=Harsh\SQLEXPRESS;Initial Catalog=HRMSDB;Integrated Security=True" />

//        < commandText >
//INSERT INTO ExceptionLogs
//(Emp_ID, ExceptionMessage, ExceptionType, StackTrace, LineNumber, MethodName, Source, InnerException)

//            VALUES
//            (@empId, @message, @exceptionType, @stacktrace, @linenumber, @methodname, @source, @innerException)
//        </ commandText >


//        < parameter >

//            < parameterName value = "@empId" />

//            < dbType value = "Int32" />

//            < layout type = "log4net.Layout.PatternLayout" >

//                < conversionPattern value = "%property{Emp_ID}" />

//            </ layout >

//        </ parameter >


//        < parameter >

//            < parameterName value = "@message" />

//            < dbType value = "String" />

//            < layout type = "log4net.Layout.PatternLayout" >

//                < conversionPattern value = "%message" />

//            </ layout >

//        </ parameter >


//        < parameter >

//            < parameterName value = "@exceptionType" />

//            < dbType value = "String" />

//            < layout type = "log4net.Layout.PatternLayout" >

//                < conversionPattern value = "%exception{type}" />

//            </ layout >

//        </ parameter >


//        < parameter >

//            < parameterName value = "@stacktrace" />

//            < dbType value = "String" />

//            < layout type = "log4net.Layout.PatternLayout" >

//                < conversionPattern value = "%exception{stacktrace}" />

//            </ layout >

//        </ parameter >


//        < parameter >

//            < parameterName value = "@linenumber" />

//            < dbType value = "Int32" />

//            < layout type = "log4net.Layout.PatternLayout" >

//                < conversionPattern value = "%property{LineNumber}" />

//            </ layout >

//        </ parameter >


//        < parameter >

//            < parameterName value = "@methodname" />

//            < dbType value = "String" />

//            < layout type = "log4net.Layout.PatternLayout" >

//                < conversionPattern value = "%property{MethodName}" />

//            </ layout >

//        </ parameter >


//        < parameter >

//            < parameterName value = "@source" />

//            < dbType value = "String" />

//            < layout type = "log4net.Layout.PatternLayout" >

//                < conversionPattern value = "%property{Source}" />

//            </ layout >

//        </ parameter >


//        < parameter >

//            < parameterName value = "@innerException" />

//            < dbType value = "String" />

//            < layout type = "log4net.Layout.PatternLayout" >

//                < conversionPattern value = "%exception{inner}" />

//            </ layout >

//        </ parameter >

//    </ appender >


//    < root >

//        < level value = "ERROR" />

//        < appender - ref ref= "DbAppender" />

//    </ root >
//</ log4net > -->