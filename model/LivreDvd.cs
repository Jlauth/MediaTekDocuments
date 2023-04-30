
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier LivreDvd hérite de Document.
    /// </summary>
    public abstract class LivreDvd : Document
    {
        /// <summary>
        /// Constructeur de la classe LivreDvd hérite de Document.
        /// </summary>
        /// <param name="id">Identifiant du document.</param>
        /// <param name="titre">Titre du document.</param>
        /// <param name="image">Image du document.</param>
        /// <param name="idGenre">Identifiant du genre.</param>
        /// <param name="genre">Libelle du genre.</param>
        /// <param name="idPublic">Identifiant du public.</param>
        /// <param name="lePublic">Libelle du public.</param>
        /// <param name="idRayon">Identifiant du rayon.</param>
        /// <param name="rayon">Libelle du rayon.</param>
        protected LivreDvd(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
        }
    }
}
