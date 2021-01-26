using log4net;
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

namespace Stefanini.Transverse
{
    [Serializable]
    public class StefaniniExcepcion : Exception
    {
        #region Constructores
        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        private static ILog DefLog(ILog log)
        {
            if (log == null)
                return LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            return log;
        }

        public StefaniniExcepcion(string message, ILog log) : base(message) { WriteLog(message, log); }

        public StefaniniExcepcion(string message, Exception e, ILog log) : base(message, e) { WriteLog(e, log); }

        [SecurityCritical()]
        protected StefaniniExcepcion(SerializationInfo info, StreamingContext context, ILog log) : base(info, context) { WriteLog(info.ToString(), log); }
        #endregion

        #region Metodos

        public static void WriteLog(string message, ILog log = null)
        {
            var logAux = DefLog(log);
            logAux.Error(message);
        }

        public static void WriteLog(Exception ex, ILog log = null)
        {
            var logAux = DefLog(log);

            if (ex != null)
            {
                Exception exception = GetException(ex);
                string type = string.Concat("[Tipo: ", exception.GetType().Name, "]");
                string message = string.Concat("[Mensaje: ", exception.Message, "]");
                string source = string.Concat("[Origen: ", exception.Source, "]");
                string stackTrace = string.Concat("[StackTrace: ", exception.StackTrace, "]");

                logAux.Error(string.Concat(type, "\n", message, "\n", source, "\n", stackTrace, "\n"));
            }
        }

        public static string GetExcepcionMessage(Exception exception)
        {
            return GetException(exception).Message;
        }

        public static Exception GetException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("e");

            while (exception.InnerException != null)
                exception = exception.InnerException;

            return exception;
        }

        public StefaniniExcepcion()
        {
        }

        public StefaniniExcepcion(string message) : base(message)
        {
        }

        public StefaniniExcepcion(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}
