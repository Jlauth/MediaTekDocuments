using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Exemplaire.
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// Propriété Numéro de Exemplaire.
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Propriété DateAchat de Exemplaire.
        /// </summary>
        public DateTime DateAchat { get; set; }

        /// <summary>
        /// Propriété Photo de Exemplaire.
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Propriété IdEtat de Exemplaire.
        /// </summary>
        public string IdEtat { get; set; }

        /// <summary>
        /// Propriété Id de Exemplaire.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Constructeur de la classe Exemplaire.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="numero">Numéro de l'exemplaire.</param>
        /// <param name="dateAchat">Date d'achat de l'exemplaire.</param>
        /// <param name="photo">Photo de l'exemplaire.</param>
        /// <param name="idEtat">Identifiant de l'état.</param>
        /// <param name="idDocument">Identifiant du document.</param>
        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.Id = idDocument;
        }
    }
}
