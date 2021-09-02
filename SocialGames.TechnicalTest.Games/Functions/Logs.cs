using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SocialGames.TechnicalTest.Games.Functions
{
    #region modelodelogs
        /// <summary>
        /// Estructura del modelo para generar logs
        /// </summary>
        public class LogModel
        {
            /// <summary>
            /// Fecha / Hora del Log
            /// </summary>
            public string Date { get; set; }
            /// <summary>
            /// Modelo del detalle del log
            /// </summary>
            public LogTrace LogTrace { get; set; }
         
        }
        /// <summary>
        /// Modelo del detalle del log
        /// </summary>
        public class LogTrace
        {
            /// <summary>
            /// URL del endpoint que se esta evaluando
            /// </summary>
            public string EndPoint { get; set; }
            /// <summary>
            /// Tiempo de respuesta de la peticion
            /// </summary>
            public string ResponseTime { get; set; }
            /// <summary>
            /// Mensaje en caso de exito 200, cualquier otro se considera error y se muestra el mensaje referente al error
            /// </summary>
            public string Message { get; set; }
        }
    #endregion
    public class Logs
    {
       
        static string fileNameLog;
        static string fileNameError ;
        static FileInfo logFile;
        static FileInfo logFileError;

        /// <summary>
        /// faraujo 28/08/2021
        /// Constructor de la clase de funciones Estabele los nombres de los archivos de Log y Errores
        /// Los crea si no existen de acuerdo al formato descrito en el metodo privado CreateLogsFiles
        /// </summary>
        public Logs()
        {
      
            fileNameLog = DateTime.Now.ToString("yyyyMMdd").ToString() + ".txt";
            fileNameError = DateTime.Now.ToString("yyyyMMdd").ToString() + "_Error.txt";

            logFile = new FileInfo(fileNameLog);
            logFileError = new FileInfo(fileNameError);

            if (!CreateLogsFiles(logFile))
                throw new Exception("Error al crear el archivo Log");
            if (!CreateLogsFiles(logFileError))
                throw new Exception("Error al crear el archivo Log de Errores");

        }

        /// <summary>
        /// faraujo 28/08/2021
        /// Crea los archivos de texto para los Logs, por el momento en la misma carpeta (no especifica el diseño guardarlos en una ruta especifica
        /// Los nombres es la fecha en que se estan procesando las transacciones, se crearan archivos por dia con el formato:
        ///         YYYYMMDD.txt para el archivo de transacciones
        ///         YYYYMMDD_Error.txt para el archivo de errores
        ///
        /// Ver opciones de poder usar librerias externas como SERILOG para tener mas informacion.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private bool CreateLogsFiles(FileInfo fileInfo)
        {
            try
            {
                if (!fileInfo.Exists)
                {
                    using (FileStream fs = fileInfo.Create())
                    {
                        Byte[] txt = new UTF8Encoding(true).GetBytes("Archivo creado - " + DateTime.Now.ToString());
                        fs.Write(txt, 0, txt.Length);
                        fs.Flush();
                    }
                    // Poner un separador entre la fecha de creacion y los datos de Log
                    using (StreamWriter fs = fileInfo.AppendText())
                    {
                        fs.WriteLine("**********************************************************************");
                        fs.Flush();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// faraujo 28/08/2001
        /// Escribir en los archivos de log (transacciones o errores)
        /// </summary>
        /// <param name="endpoint"> Url que se esta logando </param>
        /// <param name="responsetime"> Tiempo de resptes  </param>
        /// <param name="message">200 Escribe en Log de transacciones, cualquier otro escribe en log de errores</param>
        /// <returns></returns>
        public  bool Log (string endpoint, string responsetime,  string message)
        {
            try
            {
                LogModel logModel = new LogModel
                {
                    Date = DateTime.Now.ToString(),
                    LogTrace = new LogTrace
                    {
                        EndPoint = endpoint,
                        ResponseTime = responsetime,
                        Message=message
                    }
                };
                //Se define cual es el archivo a usar si el mensaje es 200 OK usa el log, cualquier otro usa el Log de errores

                FileInfo file = message == "200" ? logFile : logFileError;
                using (StreamWriter fs = file.AppendText())
                {
                    fs.WriteLine( JsonConvert.SerializeObject(logModel));
                    fs.Flush();
                }

                
                return true;
            }
            catch (Exception ex)
            {
                //escribir en el log de errores
                Log(endpoint, responsetime, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        
    }
}
