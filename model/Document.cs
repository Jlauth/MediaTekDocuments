
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        public string Id { get; set; }
        public string Titre { get; set; }
        public string Image { get; set; }
        public string IdGenre { get; set; }
        public string Genre { get; set; }
        public string IdPublic { get; set; }
        public string Public { get; set; }
        public string IdRayon { get; set; }
        public string Rayon { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Document
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            this.Id = id;
            this.Titre = titre;
            this.Image = image;
            this.IdGenre = idGenre;
            this.Genre = genre;
            this.IdPublic = idPublic;
            this.Public = lePublic;
            this.IdRayon = idRayon;
            this.Rayon = rayon;
        }
    }
}
