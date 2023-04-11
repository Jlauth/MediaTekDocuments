using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement hérite de Commande
    /// </summary>
    public class Abonnement : Commande
    {
        /// <summary>
        /// propriété DateFinAbonnement
        /// </summary>
        public DateTime DateFinAbonnement { get; set; }

        /// <summary>
        /// Propriété IdRevue
        /// </summary>
        public string IdRevue { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Abonnement
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="DateFinAbonnement"></param>
        /// <param name="idRevue"></param>
        public Abonnement(string id, DateTime dateCommande, double montant, DateTime DateFinAbonnement, string idRevue) :
            base(id, dateCommande, montant)
        {
            this.DateFinAbonnement = DateFinAbonnement;
            this.IdRevue = idRevue;
        }
    }
}
