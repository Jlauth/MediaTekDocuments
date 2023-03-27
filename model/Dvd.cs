
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Dvd hérite de LivreDvd : contient des propriétés spécifiques aux dvd
    /// </summary>
    public class Dvd : LivreDvd
    {
        public int Duree { get; set; }
        public string Realisateur { get; set; }
        public string Synopsis { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Dvd héritant de LivreDvd
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="duree"></param>
        /// <param name="realisateur"></param>
        /// <param name="synopsis"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
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
