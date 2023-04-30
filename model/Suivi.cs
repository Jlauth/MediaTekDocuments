using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi.
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// Propriété Id de Suivi.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Propriété Libelle de Suivi.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Suivi.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant du suivi.</param>
        /// <param name="libelle">Libelle du suivi.</param>
        public Suivi(string id, string libelle)
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
