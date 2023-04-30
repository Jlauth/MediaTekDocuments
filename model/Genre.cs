
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Genre : hérite de Catégorie.
    /// </summary>
    public class Genre : Categorie
    {
        /// <summary>
        /// Constructeur de la classe Genre.
        /// </summary>
        /// <param name="id">Identifiant du genre.</param>
        /// <param name="libelle">Libelle du genre.</param>
        public Genre(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
