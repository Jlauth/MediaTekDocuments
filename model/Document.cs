
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les informations communes à tous les documents : Livre, Revue, DVD).
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Propriété Id de Document.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Propriété Titre de Document.
        /// </summary>
        public string Titre { get; set; }

        /// <summary>
        /// Propriété Image de Document.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Propriété IdGenre de Document.
        /// </summary>
        public string IdGenre { get; set; }

        /// <summary>
        /// Propriété Genre de Document.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Propriété IdPublic de Document.
        /// </summary>
        public string IdPublic { get; set; }

        /// <summary>
        /// Propriété Public de Document.
        /// </summary>
        public string Public { get; set; }

        /// <summary>
        /// Propriété IdRayon de Document.
        /// </summary>
        public string IdRayon { get; set; }

        /// <summary>
        /// Propriété Rayon de Document.
        /// </summary>
        public string Rayon { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Document.
        /// Valorise les propriétés de cette classe.
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
