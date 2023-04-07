
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Etat (état d'usure d'un document)
    /// </summary>
    public class Etat
    {
        public string Id { get; set; }
        public string Libelle { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Etat
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Etat(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns>Libelle</returns>
        public override string ToString()
        {
            return this.Libelle;
        }

    }
}
