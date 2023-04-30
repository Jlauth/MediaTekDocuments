using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier ExemplaireDetail.
    /// </summary>
    public class ExemplaireDetail
    {
        /// <summary>
        /// Propriété Numéro de ExemplaireDetail.
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Propriété DateAchat de ExemplaireDetail.
        /// </summary>
        public DateTime DateAchat { get; set; }

        /// <summary>
        /// Propriété Photo de ExemplaireDetail.
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Propriété IdEtat de ExemplaireDetail.
        /// </summary>
        public string IdEtat { get; set; }

        /// <summary>
        /// Propriété LibelleEtat de ExemplaireDetail.
        /// </summary>
        public string LibelleEtat { get; set; }

        /// <summary>
        /// Propriété Id de ExemplaireDetail.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Constructeur de la classe ExemplaireDetail.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="numero">Numéro de l'exemplaire.</param>
        /// <param name="dateAchat">Date d'achat de l'exemplaire.</param>
        /// <param name="photo">Photo de l'exemplaire.</param>
        /// <param name="idEtat">Identifiant de l'état.</param>
        /// <param name="libelleEtat">Libelle de l'état.</param>
        /// <param name="idDocument">Identifiant du document.</param>
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
