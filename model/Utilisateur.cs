using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur
    /// </summary>
    public class Utilisateur
    {
        public string Login { get; set; }
        public string Pwd { get; set; }
        public string IdService { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Utilisateur
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pwd"></param>
        /// <param name="idService"></param>
        public Utilisateur(string login, string pwd, string idService)
        {
            this.Login = login;
            this.Pwd = pwd;
            this.IdService= idService;
        }
       
    }
}
