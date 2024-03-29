﻿
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Catégorie (réunit les informations des classes Public, Genre et Rayon).
    /// </summary>
    public class Categorie
    {
        /// <summary>
        /// Propriété Id de Catégorie.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Propriété Libelle de Catégorie.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Catégorie.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie.</param>
        /// <param name="libelle">Libelle de la catégorie.</param>
        public Categorie(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combobox.
        /// </summary>
        /// <returns>Libelle au format String.</returns>
        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
