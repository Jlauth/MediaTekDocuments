using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande Document hérite de Commande
    /// </summary>
    public class CommandeDocument : Commande
    {
        /// <summary>
        /// Propriété NbExemplaire de CommandeDocument
        /// </summary>
        public int NbExemplaire { get; set; }

        /// <summary>
        /// Propriété IdLivreDvd de CommandeDocument
        /// </summary>
        public string IdLivreDvd { get; set; }

        /// <summary>
        /// Propriété IdSuivi de CommandeDocument
        /// </summary>
        public string IdSuivi { get; set; }

        /// <summary>
        /// Propriété LibelleSuivi de CommandeDocument
        /// </summary>
        public string LibelleSuivi { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Commande Document
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="idLivreDvd"></param>
        /// <param name="idSuivi"></param>
        /// <param name="libelleSuivi"></param>
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
