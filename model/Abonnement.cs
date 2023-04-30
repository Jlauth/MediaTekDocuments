using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement hérite de Commande.
    /// </summary>
    public class Abonnement : Commande
    {
        /// <summary>
        /// Propriété DateFinAbonnement de Abonnement.
        /// </summary>
        public DateTime DateFinAbonnement { get; set; }

        /// <summary>
        /// Propriété IdRevue de Abonnement.
        /// </summary>
        public string IdRevue { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Abonnement.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement.</param>
        /// <param name="dateCommande">Date de commande.</param>
        /// <param name="montant">Montant de la commande.</param>
        /// <param name="DateFinAbonnement">Date de fin de l'abonnement.</param>
        /// <param name="idRevue">Identifiant de la revue.</param>
        public Abonnement(string id, DateTime dateCommande, double montant, DateTime DateFinAbonnement, string idRevue) :
            base(id, dateCommande, montant)
        {
            this.DateFinAbonnement = DateFinAbonnement;
            this.IdRevue = idRevue;
        }
    }
}
