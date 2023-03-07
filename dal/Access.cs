using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Windows.Forms;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        private const string url = "http://localhost/rest_mediatekdocuments/";

        /// <summary>
        /// Adresse de l'API
        /// </summary>
        private static readonly string uriApi = url;
        /// <summary>
        /// Instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// Instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// Méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// Méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// Méthode HTTP pour update
        /// <summary>
        private const string PUT = "PUT";
        /// <summary>
        /// Méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";

        /// Méthode privée pour créer un singleton
        /// Initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les documents à partir de la BDD
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Document> GetAllDocuments(string idDocument)
        {
            List<Document> lesDocuments = TraitementRecup<Document>(GET, "document/" + idDocument);
            return lesDocuments;
        }
        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre");
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon");
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public");
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre");
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd");
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue");
            return lesRevues;
        }

        /// <summary>
        /// Retourne les suivis à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            IEnumerable<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi");
            return new List<Suivi>(lesSuivis);
        }

        /// <summary>
        /// Retournes toutes les commandes de livres à partir de la BDD
        /// </summary>
        /// <returns></returns>
        public List<CommandeDocument> GetAllCommandesLivres()
        {
            List<CommandeDocument> lesCommandesLivres = TraitementRecup<CommandeDocument>(GET, "commandeslivres");
            return lesCommandesLivres;
        }

        /// <summary>
        /// Retourne les commandes d'un livre ou dvd à partir de la BDD
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Commande Document</returns>
        public List<CommandeDocument> GetCommandedDocument(string idDocument)
        {
            List<CommandeDocument> lesCommandesDocument = TraitementRecup<CommandeDocument>(GET, "commandedocument/" + idDocument);
            return lesCommandesDocument;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue à partir de la BDD
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + idDocument);
            return lesExemplaires;
        }

        /// <summary>
        /// Ajout d'un document dans la BDD
        /// </summary>
        /// <param name="document">document à ajouter</param>
        /// <returns>true si ajout a pu se faire (retour != null)</returns>
        public bool CreerDocument(Document document)
        {
            String jsonCreerDocument = JsonConvert.SerializeObject(document);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Document> liste = TraitementRecup<Document>(POST, "document/" + jsonCreerDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'un livre dans la BDD
        /// </summary>
        /// <param name="livre">livre à ajouter</param>
        /// <returns>true si ajout a pu se faire (retour != null)</returns>
        public bool CreerLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre);
            Console.WriteLine(jsonLivre);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur) 
                List<Livre> liste = TraitementRecup<Livre>(POST, "livre/" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Ajout d'un dvd dans la BDD
        /// </summary>
        /// <param name="dvd">dvd à ajouter</param>
        /// <returns>true si ajout a pu se faire (retour != null)</returns>
        public bool CreerDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd);
            Console.WriteLine(jsonDvd);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Dvd> liste = TraitementRecup<Dvd>(POST, "dvd/" + jsonDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'une revenue dans la BDD
        /// </summary>
        /// <param name="revue">revue à ajouter</param>
        /// <returns>true si ajout a pu se faire (retour != null)</returns>
        public bool CreerRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Revue> liste = TraitementRecup<Revue>(POST, "revue/" + jsonRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'un exemplaire en BDD
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire/" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'une commande de livre ou dvd en BDD
        /// </summary>
        /// <param name="commandeDocument">livre ou dvd commandé</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCommandeDocument = JsonConvert.SerializeObject(commandeDocument, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(POST, "commandedocument/" + jsonCommandeDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'un livre en BDD
        /// </summary>
        /// <param name="livre">le livre à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre);
            Console.WriteLine(jsonLivre);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Livre> liste = TraitementRecup<Livre>(PUT, "livre/" + livre.Id + "/" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'un dvd en BDD
        /// </summary>
        /// <param name="dvd">le dvd à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Dvd> liste = TraitementRecup<Dvd>(PUT, "dvd/" + dvd.Id + "/" + jsonDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'une revue en BDD
        /// </summary>
        /// <param name="revue">la revue à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Revue> liste = TraitementRecup<Revue>(PUT, "revue/" + revue.Id + "/" + jsonRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification d'une commande document en BDD
        /// </summary>
        /// <param name="commandeDocument">la commande document à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCommandeDocument = JsonConvert.SerializeObject(commandeDocument);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(PUT, "commandedocument/" + commandeDocument.Id + "/" + jsonCommandeDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un livre en BDD
        /// </summary>
        /// <param name="livre">le livre à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Livre> liste = TraitementRecup<Livre>(DELETE, "livre/" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un dvd en BDD
        /// </summary>
        /// <param name="dvd">le dvd à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Dvd> liste = TraitementRecup<Dvd>(DELETE, "dvd/" + jsonDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'une revue en BDD
        /// </summary>
        /// <param name="dvd">la revue à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue);
            Console.WriteLine(jsonRevue);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Revue> liste = TraitementRecup<Revue>(DELETE, "revue/" + jsonRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'une commande document en BDD
        /// </summary>
        /// <param name="commandeDocument">la commande document à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCommandeDocument = JsonConvert.SerializeObject(commandeDocument);
            Console.WriteLine(jsonCommandeDocument);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(DELETE, "commandedocument/" + jsonCommandeDocument); 
                return (liste != null);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message)
        {
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("Code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + e.Message);
                Environment.Exit(0);
            }
            return liste;
        }




        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}
