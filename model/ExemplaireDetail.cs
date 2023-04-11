using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier ExemplaireDetail
    /// </summary>
    public class ExemplaireDetail
    {
        /// <summary>
        /// Propriété Numéro de ExemplaireDetail
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Propriété DateAchat de ExemplaireDetail
        /// </summary>
        public DateTime DateAchat { get; set; }

        /// <summary>
        /// Propriété Photo de ExemplaireDetail
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Propriété IdEtat de ExemplaireDetail
        /// </summary>
        public string IdEtat { get; set; }

        /// <summary>
        /// Propriété LibelleEtat de ExemplaireDetail
        /// </summary>
        public string LibelleEtat { get; set; }

        /// <summary>
        /// Propriété Id de ExemplaireDetail
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Constructeur de la classe ExemplaireDetail
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="dateAchat"></param>
        /// <param name="photo"></param>
        /// <param name="idEtat"></param>
        /// <param name="libelleEtat"></param>
        /// <param name="idDocument"></param>
        public ExemplaireDetail(int numero, DateTime dateAchat, string photo, string idEtat, string libelleEtat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.LibelleEtat = libelleEtat;
            this.Id = idDocument;
        }
    }
}
