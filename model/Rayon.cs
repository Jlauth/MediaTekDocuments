
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Rayon (rayon de classement du document) hérite de Catégorie.
    /// </summary>
    public class Rayon : Categorie
    {
        /// <summary>
        /// Constructeur de la classe Rayon hérite de Catégorie.
        /// </summary>
        /// <param name="id">Identifiant du rayon.</param>
        /// <param name="libelle">Libelle du rayon.</param>
        public Rayon(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
