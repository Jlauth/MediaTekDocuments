using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande Document hérite de Commande.
    /// </summary>
    public class CommandeDocument : Commande
    {
        /// <summary>
        /// Propriété NbExemplaire de CommandeDocument.
        /// </summary>
        public int NbExemplaire { get; set; }

        /// <summary>
        /// Propriété IdLivreDvd de CommandeDocument.
        /// </summary>
        public string IdLivreDvd { get; set; }

        /// <summary>
        /// Propriété IdSuivi de CommandeDocument.
        /// </summary>
        public string IdSuivi { get; set; }

        /// <summary>
        /// Propriété LibelleSuivi de CommandeDocument.
        /// </summary>
        public string LibelleSuivi { get; set; }

        /// <summary>
        /// Constructeur de la classe métier CommandeDocument.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant de la commande.</param>
        /// <param name="dateCommande">Date de la commande.</param>
        /// <param name="montant">Montant de la commande.</param>
        /// <param name="nbExemplaire">Nombre d'exemplaires commandés.</param>
        /// <param name="idLivreDvd">Identifiant de type livres ou DVD.</param>
        /// <param name="idSuivi">Identifiant du suivi.</param>
        /// <param name="libelleSuivi">Libelle du suivi.</param>
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
