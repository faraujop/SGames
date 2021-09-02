using System;
using SocialGames.TechnicalTest.Games.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace SocialGames.TechnicalTest.Games.Interface
{
    /// <summary>
    /// Interfase para las reglas de negocio
    /// </summary>
    public interface ISocialGamesTT
    {

        Task<List<CharIndex>> PlayGame(string idgame, string endpoint);
    }

    
}
