
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Genre : hérite de Catégorie
    /// </summary>
    public class Genre : Categorie
    {
        /// <summary>
        /// Constructeur de la classe Genre
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Genre(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
