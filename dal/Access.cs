using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Serilog;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto.Paddings;
using System.Security.Policy;
using static System.Net.WebRequestMethods;
using Serilog.Formatting.Json;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// Chaîne de connection référence externe
        /// </summary>
        private static readonly string connectionName = "MediaTekDocuments.Properties.Settings.mediatek86ConnectionString";
        /// <summary>
        /// Adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// Instance unique de la classe
        /// </summary>
        private static Access instance;
        /// <summary>
        /// Instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api;
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
            String connectionString;
            try
            {
                connectionString = GetConnectionStringByName(connectionName);
                //connectionString = "admin:adminpwd";
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File(new JsonFormatter(), "logs/log.txt",
                    rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                api = ApiRest.GetInstance(uriApi, connectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Log.Fatal("Access.Access catch connectionString={0} error={1}", connectionName, e);
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
        /// Récupération de la chaîne de connexion
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string GetConnectionStringByName(string name)
        {
            string returnValue = null;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings != null)
            {
                returnValue = settings.ConnectionString;
            }
            return returnValue;
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
        /// Retourne tous les états document à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Etat</returns>
        public List<Etat> GetAllEtats()
        {
            IEnumerable<Etat> lesEtatsDocument = TraitementRecup<Etat>(GET, "etat");
            return new List<Etat>(lesEtatsDocument);
        }

        /// <summary>
        /// Retourne les exemplaires à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetAllExemplaires()
        {
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplairesdocument");
            return lesExemplaires;
        }

        /// <summary>
        /// Retourne les commandes document à partir de la BDD
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandesDocuments(string idDocument)
        {
            List<CommandeDocument> lesCommandesDocuments = TraitementRecup<CommandeDocument>(GET, "commandesdocuments/" + idDocument);
            return lesCommandesDocuments;
        }

        /// <summary>
        /// Retourne tous les services à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Service</returns>
        public List<Service> GetService()
        {
            List<Service> lesServices = TraitementRecup<Service>(GET, "service");
            return lesServices;
        }

        /// <summary>
        /// Retourne les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            List<Exemplaire> lesExemplairesDocument = TraitementRecup<Exemplaire>(GET, "exemplairesdocument/" + idDocument);
            return lesExemplairesDocument;
        }

        /// <summary>
        /// Retourne le détail des exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets ExemplaireDetail</returns>
        public List<ExemplaireDetail> GetExemplairesDetailsDocument(string idDocument)
        {
            List<ExemplaireDetail> lesDetailsExemplairesDocuments = TraitementRecup<ExemplaireDetail>(GET, "detaildocument/" + idDocument);
            return lesDetailsExemplairesDocuments;
        }

        /// <summary>
        /// Retourne les abonnements d'une revue à partir de la BDD
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            List<Abonnement> lesAbonnementsRevues = TraitementRecup<Abonnement>(GET, "abonnementsrevue/" + idRevue);
            return lesAbonnementsRevues;
        }

        /// <summary>
        /// Retourne tous les abonnements à échéance
        /// </summary>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<EcheanceAbonnement> GetAbonnementsEcheance()
        {
            List<EcheanceAbonnement> lesAbonnementsEcheances = TraitementRecup<EcheanceAbonnement>(GET, "echeancessabos");
            return lesAbonnementsEcheances;
        }

        /// <summary>
        /// Récupère l'utilisateur cible en BDD
        /// </summary>
        /// <param name="utilisateur">l'utilisateur cible</param>
        /// <returns>true si l'utilisateur a été trouvé (retour != null)</returns>
        public string ControleAuthentification(Utilisateur utilisateur)
        {
            String jsonRecupererUtilisateur = JsonConvert.SerializeObject(utilisateur);
            try
            {
                List<Utilisateur> utilisateurs = TraitementRecup<Utilisateur>(GET, "utilisateur/" + jsonRecupererUtilisateur);
                if (utilisateurs != null && utilisateurs.Count > 0)
                {
                    // récupération de l'utilisateur authentifié
                    Utilisateur utilisateurCible = utilisateurs[0];
                    // mise à jour l'objet utilisateur avec l'idService
                    utilisateur.IdService = utilisateurCible.IdService;
                    return utilisateur.IdService;
                }
                else
                {
                    return utilisateur.IdService = null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ControleAuthentification catch jsonRecupererUtilisateur={0} error={1}", jsonRecupererUtilisateur, ex);
                return utilisateur.IdService = null;
            }
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
                Log.Error("Access.CreerDocument catch jsonCreerDocument={0} error={1}", jsonCreerDocument, ex);
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
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur) 
                List<Livre> liste = TraitementRecup<Livre>(POST, "livre/" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerLivre catch jsonLivre={0} error={1}", jsonLivre, ex);
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
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Dvd> liste = TraitementRecup<Dvd>(POST, "dvd/" + jsonDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerDvd catch jsonDvd={0} error={1}", jsonDvd, ex);
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
                Log.Error("Access.CreerRevue catch jsonRevue={0} error={1}", jsonRevue, ex);
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
                Log.Error("Access.CreerExemplaire catch jsonExemplaire={0} error={1}", jsonExemplaire, ex);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'un abonnement en BDD
        /// </summary>
        /// <param name="abonnement">objet abonnement</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            String jsonAbonnement = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "abonnement/" + jsonAbonnement);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerAbonnement catch jsonAbonnement={0} error={1}", jsonAbonnement, ex);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'une commande document en BDD
        /// </summary>
        /// <param name="commandeDocument">objet commande document</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCreerCommandeDocument = JsonConvert.SerializeObject(commandeDocument, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(POST, "commandedocument/" + jsonCreerCommandeDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerCommandeDocument catch jsonCreerDocument={0} error={1}", jsonCreerCommandeDocument, ex);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'une abonnement de revue en BDD
        /// </summary>
        /// <param name="abonnementRevue">revue concernée</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerAbonnementRevue(string id, DateTime dateFinAbonnement, string idRevue)
        {
            String jsonDateCommande = JsonConvert.SerializeObject(dateFinAbonnement, new CustomDateTimeConverter());
            String jsonCreerAbonnement = "{\"id\":\"" + id + "\", \"dateFinAbonnement\" : " + jsonDateCommande + ", \"idRevue\" :  \"" + idRevue + "\"}";
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "abonnement/" + jsonCreerAbonnement);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerAbonnementRevue catch jsonCreerAbonnement={0} error={1}", jsonCreerAbonnement, ex);
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
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Livre> liste = TraitementRecup<Livre>(PUT, "livre/" + livre.Id + "/" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ModifierLivre catch jsonLivre={0} error={1}", jsonLivre, ex);
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
                Log.Error("Access.ModifierDvd catch jsonDvd={1} error={1}", jsonDvd, ex);
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
                Log.Error("Access.ModifierRevue catch jsonRevue={0} error={1}", jsonRevue, ex);
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
            Console.WriteLine(jsonCommandeDocument);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(PUT, "commandedocument/" + commandeDocument.Id + "/" + jsonCommandeDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ModifierCommandeDocument catch jsonCommandeDocument={0} error={1}", jsonCommandeDocument, ex);
            }
            return false;
        }

        /// <summary>
        /// Modification d'une commande revue en BDD
        /// </summary>
        /// <param name="abonnementRevue">la revue à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierCommandeRevue(Abonnement abonnementRevue)
        {
            String jsonCommandeRevue = JsonConvert.SerializeObject(abonnementRevue);
            Console.WriteLine(jsonCommandeRevue);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Abonnement> liste = TraitementRecup<Abonnement>(PUT, "commanderevue/" + abonnementRevue.Id + "/" + jsonCommandeRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ModifierCommandeRevue catch jsonCommandeRevue={0} error={1}", jsonCommandeRevue, ex);
            }
            return false;
        }

        /// <summary>
        /// Modification d'un exemplaire en BDD
        /// </summary>
        /// <param name="exemplaire">exemplaire à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierExemplaire(Exemplaire exemplaire)
        {
            String jsonModifierExemplaire = JsonConvert.SerializeObject(exemplaire);
            Console.WriteLine(jsonModifierExemplaire);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(PUT, "modifierexemplaire/" + exemplaire.Numero + "/" + jsonModifierExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ModifierExemplaire catch jsonModifierExemplaire={0} error={1}", jsonModifierExemplaire, ex);
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
                Log.Error("Access.SupprimerLivre catch jsonLivre={0} error={1}", jsonLivre, ex);
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
                Log.Error("Access.SupprimerDvd catch jsonDvd={0} error={1}", jsonDvd, ex);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'une revue en BDD
        /// </summary>
        /// <param name="revue">la revue à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Revue> liste = TraitementRecup<Revue>(DELETE, "revue/" + jsonRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerRevue catch jsonRevue={0} error={1}", jsonRevue, ex);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un exemplaire en BDD
        /// </summary>
        /// <param name="exemplaire">l'exemplaire à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = "{\"id\":\"" + exemplaire.Id + "\",\"numero\":\"" + exemplaire.Numero + "\"}";
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(DELETE, "exemplaire/" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerExemplaire catch jsonExemplaire={0} error={1}", jsonExemplaire, ex);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'une commande document en BDD
        /// </summary>
        /// <param name="commandeDocument">le document concerné</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerCommandeDocument(Commande commandeDocument)
        {
            String jsonCommandeDocument = JsonConvert.SerializeObject(commandeDocument);
            Console.WriteLine(jsonCommandeDocument);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Commande> liste = TraitementRecup<Commande>(DELETE, "commandedocument/" + jsonCommandeDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerCommandeDocument catch jsonCommandeDocument={0} error={1}", jsonCommandeDocument, ex);
            }
            return false;
        }

        /// <summary>
        /// Suppresion d'une commande revue en BDD
        /// </summary>
        /// <param name="abonnementRevue">la revue concernée</param>
        /// <returns></returns>
        public bool SupprimerAbonnement(Abonnement abonnement)
        {
            String jsonSupprimerAbonnement = JsonConvert.SerializeObject(abonnement);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Abonnement> liste = TraitementRecup<Abonnement>(DELETE, "abonnement/" + jsonSupprimerAbonnement);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerAbonnement catch jsonSupprimerAbonnement={0} error={1}", jsonSupprimerAbonnement, ex);
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
                    Log.Error("Access.TraitementRecup catch code={0} error={1}", code);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + ex.Message);
                Log.Error("Access.TraitementRecup catch liste={0} error={1}", liste, ex);
                Environment.Exit(0);
            }
            return liste;
        }

        internal Utilisateur GetUser(object login)
        {
            throw new NotImplementedException();
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
