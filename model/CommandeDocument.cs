using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument hérite de Commande
    /// </summary>
    public class CommandeDocument : Commande
    {
        public int NbExemplaire { get; set; }
        public string IdLivreDvd { get; set; }
        public string IdSuivi { get; set; }
        public string LibelleSuivi { get; set; }

        /// <summary>
        /// Constructeur de la classe métier CommandeDocument
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="idSuivi"></param>
        /// <param name="libelleSuivi"></param>
        /// <param name="nbExemplaire"></param>
        public CommandeDocument(string id, DateTime dateCommande, double montant, int nbExemplaire, string idLivreDvd, string idSuivi, string libelleSuivi)
            : base(id, dateCommande, montant)
        {
            this.NbExemplaire = nbExemplaire;
            this.IdSuivi = idSuivi;
            this.LibelleSuivi = libelleSuivi;
            this.IdLivreDvd = idLivreDvd;
        }
    }
}
