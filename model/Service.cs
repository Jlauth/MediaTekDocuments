using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Services
    /// </summary>
    public class Service
    {
        public string Id { get; }
        public string Libelle { get; }

        /// <summary>
        /// Constructeur de la classe métier Service
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Service(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

    }
}
