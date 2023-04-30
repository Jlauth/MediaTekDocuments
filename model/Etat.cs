
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier État (état d'usure d'un document).
    /// </summary>
    public class Etat
    {
        /// <summary>
        /// Propriété Id de État.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Propriété Libelle de État.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Constructeur de la classe métier État.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant de l'état.</param>
        /// <param name="libelle">Libelle de l'état.</param>
        public Etat(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos.
        /// </summary>
        /// <returns>Libelle au format String.</returns>
        public override string ToString()
        {
            return this.Libelle;
        }

    }
}
