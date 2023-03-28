﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement hérite de Commande
    /// </summary>
    public class EcheanceAbonnement : Commande
    {
        public DateTime DateFinAbonnement { get; set; }
        public string IdRevue { get; set; }
        public string TitreRevue { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Abonnement
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="DateFinAbonnement"></param>
        /// <param name="idRevue"></param>
        public EcheanceAbonnement(string id, DateTime dateCommande, double montant, DateTime DateFinAbonnement, string idRevue, string titreRevue) :
            base(id, dateCommande, montant)
        {
            this.DateFinAbonnement = DateFinAbonnement;
            this.IdRevue = idRevue;
            this.TitreRevue= titreRevue;
        }
    }
}
