
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Revue hérite de Document : contient des propriétés spécifiques aux revues.
    /// </summary>
    public class Revue : Document
    {
        /// <summary>
        /// Propriété Périodicité de Revue.
        /// </summary>
        public string Periodicite { get; set; }

        /// <summary>
        /// Propriété DelaiMiseADispo de Revue.
        /// </summary>
        public int DelaiMiseADispo { get; set; }

        /// <summary>
        /// Constructeur de la classe Revue hérite Document.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant de la revue.</param>
        /// <param name="titre">Titre de la revue.</param>
        /// <param name="image">Image de la revue.</param>
        /// <param name="idGenre">Identifiant du genre.</param>
        /// <param name="genre">Libelle du genre.</param>
        /// <param name="idPublic">Identifiant du public.</param>
        /// <param name="lePublic">Libelle du public.</param>
        /// <param name="idRayon">Identifiant du rayon.</param>
        /// <param name="rayon">Libelle du rayon.</param>
        /// <param name="periodicite">Périodicité de la revue.</param>
        /// <param name="delaiMiseADispo">Délai de mise à disposition de la revue.</param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon,
            string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Periodicite = periodicite;
            this.DelaiMiseADispo = delaiMiseADispo;
        }
    }
}
