
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Revue hérite de Document : contient des propriétés spécifiques aux revues
    /// </summary>
    public class Revue : Document
    {
        public string Periodicite { get; set; }
        public int DelaiMiseADispo { get; set; }

        /// <summary>
        /// Constructeur de la classe Revue hérite Document
        /// Valorise les propriétés de cette classe
        /// </summary>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon,
            string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Periodicite = periodicite;
            this.DelaiMiseADispo = delaiMiseADispo;
        }

    }
}
