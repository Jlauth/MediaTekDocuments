 using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Propriété Id de Commande
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Propriété DateCommande de Commande
        /// </summary>
        public DateTime DateCommande { get; set; }
        
        /// <summary>
        /// Propriété Montant de Commande
        /// </summary>
        public double Montant { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Commande
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        public Commande(string id, DateTime dateCommande, double montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
}
