
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Public (public concerné par le document) hérite de Catégorie
    /// </summary>
    public class Public : Categorie
    {
   
        /// <summary>
        /// Constructeur de la classe Public de Catégorie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Public(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
