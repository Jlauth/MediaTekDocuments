
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les informations communes à tous les documents : Livre, Revue, DVD)
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Propriété Id de Document
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Propriété Titre de Document
        /// </summary>
        public string Titre { get; set; }

        /// <summary>
        /// Propriété Image de Document
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Propriété IdGenre de Document
        /// </summary>
        public string IdGenre { get; set; }

        /// <summary>
        /// Propriété Genre de Document
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Propriété IdPublic de Document
        /// </summary>
        public string IdPublic { get; set; }

        /// <summary>
        /// Propriété Public de Document
        /// </summary>
        public string Public { get; set; }

        /// <summary>
        /// Propriété IdRayon de Document
        /// </summary>
        public string IdRayon { get; set; }

        /// <summary>
        /// Propriété Rayon de Document
        /// </summary>
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
#pragma warning disable S107 // Methods should not have too many parameters
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
#pragma warning restore S107 // Methods should not have too many parameters
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
