using System;
using System.Diagnostics;
using SocialGames.TechnicalTest.Games.Interface;
using SocialGames.TechnicalTest.Games.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SocialGames.TechnicalTest.Games.Concrete
{
    /// <summary>
    /// Clase para el modelo de negocio
    /// Reglas :
    ///     1.- El keyId debe ser siempre "eltesorodejava"
    ///     2.- Debe generar un archivo de log para las transacciones correctas con el formato:
    ///                 Fecha / hora de la ejecucion
    ///                 Endopint
    ///                 Tiempo de ejecuccion
    ///                 Mensaje
    ///     3.- El tiempo de respuesta debe tener un delay de 500ms
    ///     4.- Debe regresar en un arreglo de Char el caracter y el indice de el Keygame hasta encontrar la primera o
    /// </summary>
    public class SocialGamesTT : ISocialGamesTT
    {
        readonly Stopwatch watch;
        const string keyGame = "ELTESORODEJAVA";
        const int delay = 500;
        public SocialGamesTT()
        {
            watch = Stopwatch.StartNew();
        }

        

        public async Task<List<CharIndex>> PlayGame(string idgame, string endpoint)
        {

            Functions.Logs log = new Functions.Logs();

            return await Task.Run(() =>
           {

               List<CharIndex> charIndex = new List<CharIndex>();

               if (idgame.ToUpper() == keyGame) // Validar que el key del game sea "eltesorodejava", se hace CASE INSENSITIVE (pueden ponerlo en mayusculas o minusuculas)
                {
                   try
                   {
                       IEnumerable<char> firstO = idgame.ToUpper().TakeWhile(c => c != 'O');
                       int index = 0;
                       foreach (char c in firstO)
                       {
                           charIndex.Add(new CharIndex { Index = index, Char = c });
                           index++;
                       }

                       watch.Stop();
                        /** si el tiempo de ejecucion fur menor a 500ms hacer la pausa para completar los 500ms en caso de requerirlo 
                         *  En el Log se guarda el tiempo real de ejecucion **/
                       if (delay - int.Parse(watch.ElapsedMilliseconds.ToString()) > 0)
                       {
                           Task.Delay(delay - int.Parse(watch.ElapsedMilliseconds.ToString())).Wait();
                       }
                       var x = log.Log(endpoint, watch.Elapsed.ToString(), "200");




                   }
                   catch (Exception ex)
                   {
                        //Log Errores
                        var x = log.Log(endpoint, watch.Elapsed.ToString(), ex.Message);

                   }

               }
               else
               {
                    //Log de errores
                    var x = log.Log(endpoint, watch.Elapsed.ToString(), "keygame no es valido");
                   throw new Exception("keygame no es valido");
               }
               return charIndex;
           });
        }
    }
}
