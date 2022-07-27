using Afiniti.Framework.LoggingTracing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Configuration;

namespace Common
{
    public static class LoggingUtility
    {
        private static readonly StringCollection EMessages = new StringCollection();

        public static void LogException(Exception pEx, string pString = "CommercialGateAPI")
        {
            WriteMessage(pEx, pString);
            Log.WriteLog(EMessages, pString);
        }


        //public static void LogTrace(string pAction, StringCollection pMsgs)
        //{
        //    string loggingFlag = WebConfigurationManager.AppSettings["EnableLogging"];
        //    if (loggingFlag == "1")
        //    {
        //        Log.WriteLog(pMsgs, pAction);
        //    }
        //}

        private static void WriteMessage(Exception ex, String callerName = "CommercialGateAPI")
        {
            StringBuilder sbTrace = new StringBuilder();
            sbTrace.AppendLine("-----------------------------------------------------------------------------");
            sbTrace.AppendLine("Caller : " + callerName);

            sbTrace.AppendLine(DateTime.Now.ToString(CultureInfo.InvariantCulture)).AppendLine("");

            sbTrace.AppendLine(ex.GetType().ToString());

            sbTrace.AppendLine(ex.Message);
            sbTrace.AppendLine(ex.StackTrace);


            EMessages.Add(sbTrace.ToString());
            Exception inner = ex.InnerException;

            if (inner != null)
            {
                WriteMessage(inner, callerName);
            }
        }

        public static object BeforeCall(string operationName, object[] inputs)
        {
            ApplicationTrace.Log("---------------------------------------------------------------------------", Status.Started);
            ApplicationTrace.Log("Method Name:  " + operationName, Status.Started);

            foreach (var inObj in inputs)
            {
                PropertyInfo[] properties = inObj.GetType().GetProperties();
                ApplicationTrace.Log("Parameter Name:  " + inObj.GetType().Name, Status.Started);

                if (inObj != null && properties.Count() == 0)
                {
                    ApplicationTrace.Log(inObj.ToString(), Status.Started);
                }
                else
                {
                    foreach (PropertyInfo property in properties)
                    {
                        ApplicationTrace.Log("Property Name:  " + property.Name, Status.Started);
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            PropertyInfo[] propInner = property.PropertyType.GetProperties();
                            foreach (PropertyInfo propInners in propInner)
                            {
                                ParameterInfo[] paramInfo = propInners.GetIndexParameters();
                                if (paramInfo.Count() > 0)
                                {
                                    object gInObj = property.GetValue(inObj);

                                    PropertyInfo[] propList = propInners.PropertyType.GetProperties();
                                    if (gInObj == null)
                                    {
                                        ApplicationTrace.Log("Property Value:  No value available", Status.Started);
                                    }
                                    else
                                    {
                                        foreach (var propIn in gInObj as IEnumerable)
                                        {
                                            foreach (var gList in propList)
                                            {
                                                ApplicationTrace.Log("Property Name:  " + gList.Name, Status.Started);
                                                ApplicationTrace.Log("Property Value:  " + gList.GetValue(propIn), Status.Started);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (property.PropertyType.IsClass && inObj.GetType().IsGenericType && inObj.GetType().GetGenericTypeDefinition() == typeof(List<>))
                        {
                            PropertyInfo[] propInner = property.PropertyType.GetProperties();
                            foreach (var propIn in inObj as IEnumerable)
                            {
                                foreach (var gList in propInner)
                                {
                                    ApplicationTrace.Log("Property Name:  " + gList.Name, Status.Started);
                                    ApplicationTrace.Log("Property Value:  " + gList.GetValue(propIn), Status.Started);
                                }
                            }
                        }
                        else if (inObj.GetType().IsGenericType && inObj.GetType().GetGenericTypeDefinition() == typeof(List<>))
                        {
                            foreach (var propIn in inObj as IEnumerable)
                            {
                                ApplicationTrace.Log("Property Value:  " + propIn, Status.Started);
                            }
                        }
                        else
                        {
                            if (inObj == null || inObj.ToString() == "")
                            {
                                ApplicationTrace.Log("Property Value:  No value available", Status.Started);
                            }
                            else if (inObj.GetType().Equals(typeof(string)))
                            {
                                ApplicationTrace.Log("Property Value:  " + inObj.ToString(), Status.Started);
                            }
                            else if (property.GetValue(inObj) != null)
                            {
                                ApplicationTrace.Log("Property Value:  " + property.GetValue(inObj), Status.Started);
                            }
                            else
                            {
                                ApplicationTrace.Log("Property Value:  No value available", Status.Started);
                            }
                        }
                    }
                }
            }

            ApplicationTrace.Log("---------------------------------------------------------------------------", Status.Completed);

            return null;
        }

        public static void AfterCall(string operationName)
        {
            ApplicationTrace.Log("---------------------------------------------------------------------------", Status.Started);
            ApplicationTrace.Log("Method Name:  " + operationName, Status.Completed);
            ApplicationTrace.Log("---------------------------------------------------------------------------", Status.Completed);
        }
    }

}
