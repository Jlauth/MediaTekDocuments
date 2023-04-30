using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier AbonnementEcheance hérite de Commande.
    /// </summary>
    public class AbonnementEcheance : Commande
    {
        /// <summary>
        /// Propriété DateFinAbonnement de AbonnementEcheance.
        /// </summary>
        public DateTime DateFinAbonnement { get; set; }

        /// <summary>
        /// Propriété IdRevue de AbonnementEcheance.
        /// </summary>
        public string IdRevue { get; set; }

        /// <summary>
        /// Propriété TitreRevue de AbonnementEcheance.
        /// </summary>
        public string TitreRevue { get; set; }

        /// <summary>
        /// Constructeur de la classe métier AbonnementEcheance.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant de la commande.</param>
        /// <param name="dateCommande">Date de la commande.</param>
        /// <param name="montant">Montant de la commande.</param>
        /// <param name="DateFinAbonnement">Date de fin d'abonnement.</param>
        /// <param name="idRevue">Identifiant de la revue.</param>
        /// <param name="titreRevue">Titre de la revue.</param>
        public AbonnementEcheance(string id, DateTime dateCommande, double montant, DateTime DateFinAbonnement, string idRevue, string titreRevue) :
            base(id, dateCommande, montant)
        {
            this.DateFinAbonnement = DateFinAbonnement;
            this.IdRevue = idRevue;
            this.TitreRevue= titreRevue;
        }
    }
}
