
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier DVD hérite de LivreDvd : contient des propriétés spécifiques aux DVD.
    /// </summary>
    public class Dvd : LivreDvd
    {
        /// <summary>
        /// Propriété Durée de DVD.
        /// </summary>
        public int Duree { get; set; }

        /// <summary>
        /// Propriété Réalisateur de DVD.
        /// </summary>
        public string Realisateur { get; set; }

        /// <summary>
        /// Propriété Synopsis de DVD.
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        /// Constructeur de la classe métier DVD héritant de LivreDvd.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant du DVD.</param>
        /// <param name="titre">Titre du DVD.</param>
        /// <param name="image">Image du DVD.</param>
        /// <param name="duree">Durée du DVD.</param>
        /// <param name="realisateur">Réalisateur du DVD.</param>
        /// <param name="synopsis">Synopsis du DVD.</param>
        /// <param name="idGenre">Identifiant du genre.</param>
        /// <param name="genre">Libelle du genre.</param>
        /// <param name="idPublic">Identifiant du public.</param>
        /// <param name="lePublic">Libelle du public.</param>
        /// <param name="idRayon">Identifiant du rayon.</param>
        /// <param name="rayon">Libelle du rayon.</param>
        public Dvd(string id, string titre, string image, int duree, string realisateur, string synopsis,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Duree = duree;
            this.Realisateur = realisateur;
            this.Synopsis = synopsis;
        }

    }
}
