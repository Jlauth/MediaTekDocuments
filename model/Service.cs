using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Service.
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Propriété Id de Service.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Propriété Libelle de Service.
        /// </summary>
        public string Libelle { get; }

        /// <summary>
        /// Constructeur de la classe métier Service.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant du service.</param>
        /// <param name="libelle">Libelle du service.</param>
        public Service(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

    }
}
