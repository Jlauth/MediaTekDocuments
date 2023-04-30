
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Livre hérite de LivreDvd : contient des propriétés spécifiques aux livres.
    /// </summary>
    public class Livre : LivreDvd
    {
        /// <summary>
        /// Propriété ISBN de Livre.
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// Propriété Auteur de Livre.
        /// </summary>
        public string Auteur { get; set; }

        /// <summary>
        /// Propriété Collection de Livre.
        /// </summary>
        public string Collection { get; set; }

        /// <summary>
        /// Constructeur de la classe métier Livre.
        /// Valorise les propriétés de cette classe.
        /// </summary>
        /// <param name="id">Identifiant du livre.</param>
        /// <param name="titre">Titre du livre.</param>
        /// <param name="image">Image du livre.</param>
        /// <param name="isbn">ISBN du livre.</param>
        /// <param name="auteur">Auteur du livre.</param>
        /// <param name="collection">Collection du livre.</param>
        /// <param name="idGenre">Identifiant du genre.</param>
        /// <param name="genre">Libelle du genre.</param>
        /// <param name="idPublic">Identifiant du public.</param>
        /// <param name="lePublic">Libelle du public.</param>
        /// <param name="idRayon">Identifiant du rayon.</param>
        /// <param name="rayon">Libelle du rayon.</param>
        public Livre(string id, string titre, string image, string isbn, string auteur, string collection,
             string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Isbn = isbn;
            this.Auteur = auteur;
            this.Collection = collection;
        }
    }
}
