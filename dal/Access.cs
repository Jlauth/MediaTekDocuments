using MediaTekDocuments.manager;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace MediaTekDocuments.dal
/*
Auteur : [votre nom]
Date de création : [la date de création du code]
Description : [une description concise de ce que fait ce code]

Modifications :
[la liste des modifications effectuées avec les dates et les auteurs]
*/
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// Chaîne de connexion référence externe
        /// </summary>
        private static readonly string connectionName = "MediaTekDocuments.Properties.Settings.mediatek86ConnectionString";

        /// <summary>
        /// Adresse de l'API
        /// Switch de la déclaration au besoin (local ou distant)
        /// </summary>
        private static readonly string uriApi = "http://hafsatc.cluster027.hosting.ovh.net/rest_mediatekdocuments/";
        //private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";

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
        /// </summary>
        private const string PUT = "PUT";

        /// <summary>
        /// Méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// Initialise l'accès à l'API
        /// Switch de l'initialisation de connectionString au besoin pour SpecFlow
        /// </summary>
        private Access()
        {
            String connectionString;
            try
            {
                connectionString = GetConnectionStringByName(connectionName);
                // connectionString = "admin:adminpwd"; pour SpecFlow                       
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
        /// Création et retour de l'instance unique de la classe.
        /// </summary>
        /// <returns>Instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Récupération de la chaîne de connexion.
        /// </summary>
        /// <param name="name">Nom de la chaîne de connexion à récupérer.</param>
        /// <returns>La chaîne de connexion si elle existe, sinon null.</returns>
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
        /// Retourne tous les documents à partir de la BDD.
        /// </summary>
        /// <param name="idDocument">ID du document concerné.</param>
        /// <returns>Liste d'objets Document.</returns>
        public List<Document> GetAllDocuments(string idDocument)
        {
            List<Document> lesDocuments = TraitementRecup<Document>(GET, "document/" + idDocument);
            String jsonTest = JsonConvert.SerializeObject(lesDocuments);
            Console.WriteLine("document concerné = " + jsonTest);
            return lesDocuments;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets Genre.</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre");
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets Rayon.</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon");
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets Public.</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public");
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets Livre.</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre");
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les DVD à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets DVD.</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd");
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets Revue.</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue");
            return lesRevues;
        }

        /// <summary>
        /// Retourne les suivis à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets Suivi.</returns>
        public List<Suivi> GetAllSuivis()
        {
            IEnumerable<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi");
            return new List<Suivi>(lesSuivis);
        }

        /// <summary>
        /// Retourne tous les états document à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets État.</returns>
        public List<Etat> GetAllEtats()
        {
            IEnumerable<Etat> lesEtatsDocument = TraitementRecup<Etat>(GET, "etat");
            return new List<Etat>(lesEtatsDocument);
        }

        /// <summary>
        /// Retourne les exemplaires à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets Exemplaire.</returns>
        public List<Exemplaire> GetAllExemplaires()
        {
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplairesdocument");
            return lesExemplaires;
        }

        /// <summary>
        /// Retourne les commandes document à partir de la BDD.
        /// </summary>
        /// <param name="idDocument">ID du document concerné.</param>
        /// <returns>Liste d'objets CommandeDocument.</returns>
        public List<CommandeDocument> GetCommandesDocuments(string idDocument)
        {
            List<CommandeDocument> lesCommandesDocuments = TraitementRecup<CommandeDocument>(GET, "commandesdocuments/" + idDocument);
            return lesCommandesDocuments;
        }

        /// <summary>
        /// Retourne ID commande à partir de la BDD.
        /// </summary>
        /// <param name="id">ID de la commande concernée.</param>
        /// <returns>List d'objets Commande.</returns>
        public List<Commande> GetCommandeId(string id)
        {
            List<Commande> idCommande = TraitementRecup<Commande>(GET, "commandeid/" + id);
            return idCommande;
        }

        /// <summary>
        /// Retourne tous les services à partir de la BDD.
        /// </summary>
        /// <returns>Liste d'objets Service.</returns>
        public List<Service> GetService()
        {
            List<Service> lesServices = TraitementRecup<Service>(GET, "service");
            return lesServices;
        }

        /// <summary>
        /// Retourne les exemplaires d'un document.
        /// </summary>
        /// <param name="idDocument">ID du document concerné.</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            List<Exemplaire> lesExemplairesDocument = TraitementRecup<Exemplaire>(GET, "exemplairesdocument/" + idDocument);
            return lesExemplairesDocument;
        }

        /// <summary>
        /// Retourne le détail des exemplaires d'un document.
        /// </summary>
        /// <param name="idDocument">ID du document concerné.</param>
        /// <returns>Liste d'objets ExemplaireDetail.</returns>
        public List<ExemplaireDetail> GetExemplairesDetailsDocument(string idDocument)
        {
            List<ExemplaireDetail> lesDetailsExemplairesDocuments = TraitementRecup<ExemplaireDetail>(GET, "detaildocument/" + idDocument);
            return lesDetailsExemplairesDocuments;
        }

        /// <summary>
        /// Retourne le numéro de parution d'un exemplaire de type Revue.
        /// </summary>
        /// <param name="numero">Le numéro de parution de la revue.</param>
        /// <returns>Liste d'objets Exemplaire.</returns>
        public List<Exemplaire> GetNumeroParution(string numero)
        {
            List<Exemplaire> leNumeroDeParution = TraitementRecup<Exemplaire>(GET, "numeroparution/" + numero);
            String checkParution = JsonConvert.SerializeObject(leNumeroDeParution);
            Console.WriteLine(checkParution);
            return leNumeroDeParution;
        }

        /// <summary>
        /// Retourne les abonnements d'une revue à partir de la BDD.
        /// </summary>
        /// <param name="idRevue">IDde la revue concernée.</param>
        /// <returns>Liste d'objets Abonnement.</returns>
        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            List<Abonnement> lesAbonnementsRevues = TraitementRecup<Abonnement>(GET, "abonnementsrevue/" + idRevue);
            return lesAbonnementsRevues;
        }

        /// <summary>
        /// Retourne tous les abonnements à échéance.
        /// </summary>
        /// <returns>Liste d'objets Abonnement.</returns>
        public List<AbonnementEcheance> GetAbonnementsEcheance()
        {
            List<AbonnementEcheance> lesAbonnementsEcheances = TraitementRecup<AbonnementEcheance>(GET, "echeancessabos");
            return lesAbonnementsEcheances;
        }

        /// <summary>
        /// Récupère l'utilisateur cible en BDD.
        /// </summary>
        /// <param name="utilisateur">Utilisateur cible.</param>
        /// <returns>true si l'utilisateur a été trouvé (retour != null).</returns>
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
        /// Ajout d'un document dans la BDD.
        /// </summary>
        /// <param name="document">Document à ajouter.</param>
        /// <returns>true si ajout a pu se faire (retour != null).</returns>
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
        /// Ajout d'un livre dans la BDD.
        /// </summary>
        /// <param name="livre">Livre à ajouter.</param>
        /// <returns>true si ajout a pu se faire (retour != null).</returns>
        public bool CreerLivre(Livre livre)
        {
            String jsonCreerLivre = JsonConvert.SerializeObject(livre);
            Console.WriteLine("jsonCreerLivre = " + jsonCreerLivre);
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur) 
                List<Livre> liste = TraitementRecup<Livre>(POST, "livre/" + jsonCreerLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerLivre catch jsonCreerLivre={0} error={1}", jsonCreerLivre, ex);
            }
            return false;
        }

        /// <summary>
        /// Ajout d'un DVD dans la BDD.
        /// </summary>
        /// <param name="dvd">DVD à ajouter</param>
        /// <returns>true si ajout a pu se faire (retour != null).</returns>
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
        /// Ajout d'une revenue dans la BDD.
        /// </summary>
        /// <param name="revue">Revue à ajouter.</param>
        /// <returns>true si ajout a pu se faire (retour != null).</returns>
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
        /// Ajout d'un exemplaire en BDD.
        /// </summary>
        /// <param name="exemplaire">Exemplaire à insérer.</param>
        /// <returns>true si l'insertion a pu se faire (retour != null).</returns>
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
        /// Ajout d'un abonnement en BDD.
        /// </summary>
        /// <param name="abonnement">Abonnement à insérer.</param>
        /// <returns>true si l'insertion a pu se faire (retour != null).</returns>
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
        /// Ajout d'une commande de type document en BDD.
        /// </summary>
        /// <param name="commandeDocument">Objet CommandeDocument</param>
        /// <returns>true si l'insertion a pu se faire (retour != null).</returns>
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
        /// Créer un abonnement d'une revue en BDD.
        /// </summary>
        /// <param name="id">ID de l'abonnement.</param>
        /// <param name="dateFinAbonnement">Date de fin d'abonnement.</param>
        /// <param name="idRevue">ID lié à la revue.</param>
        /// <returns>true si l'insertion a pu se faire (retour != null).</returns>
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
        /// Modification d'un livre en BDD.
        /// </summary>
        /// <param name="livre">Livre à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null).</returns>
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
        /// Modification d'un DVD en BDD.
        /// </summary>
        /// <param name="dvd">DVD à modifier.</param>
        /// <returns>true si la modification a pu se faire (retour != null).</returns>
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
        /// Modification d'une revue en BDD.
        /// </summary>
        /// <param name="revue">Revue à modifier.</param>
        /// <returns>true si la modification a pu se faire (retour != null).</returns>
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
        /// Modification d'une commande document en BDD.
        /// </summary>
        /// <param name="commandeDocument">ComandeDocument à modifier.</param>
        /// <returns>true si la modification a pu se faire (retour != null).</returns>
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
        /// Modification d'une commande de type revue en BDD.
        /// </summary>
        /// <param name="abonnementRevue">Revue à modifier.</param>
        /// <returns>true si la modification a pu se faire (retour != null).</returns>
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
        /// Modification d'un exemplaire en BDD.
        /// </summary>
        /// <param name="exemplaire">Exemplaire à modifier.</param>
        /// <returns>true si la modification a pu se faire (retour != null).</returns>
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
        /// Suppression d'un livre en BDD.
        /// </summary>
        /// <param name="livre">Livre à supprimer.</param>
        /// <returns>true si la suppression a pu se faire (retour != null).</returns>
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
        /// Suppression d'un DVD en BDD
        /// </summary>
        /// <param name="dvd">DVD à supprimer.</param>
        /// <returns>true si la suppression a pu se faire (retour != null).</returns>
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
        /// Suppression d'une revue en BDD.
        /// </summary>
        /// <param name="revue">Revue à supprimer.</param>
        /// <returns>true si la suppression a pu se faire (retour != null).</returns>
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
        /// Suppression d'un exemplaire en BDD.
        /// </summary>
        /// <param name="exemplaire">Exemplaire à supprimer.</param>
        /// <returns>true si la suppression a pu se faire (retour != null).</returns>
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

        /// <summary>²
        /// Suppression d'une commande document en BDD.
        /// </summary>
        /// <param name="commandeDocument">CommandeDocument concernée.</param>
        /// <returns>true si la suppression a pu se faire (retour != null).</returns>
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
        /// Suppression d'une commande de type revue en BDD.
        /// </summary>
        /// <param name="abonnement">Abonnement concerné.</param>
        /// <returns>true si la suppression a pu se faire (retour != null).</returns>
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
        /// Effectue le traitement de récupération du retour de l'API et convertit le JSON en une liste d'objets pour les requêtes de type SELECT (GET).
        /// </summary>
        /// <typeparam name="T">Type de l'objet à récupérer.</typeparam>
        /// <param name="methode">Verbe HTTP (GET, POST, PUT, DELETE).</param>
        /// <param name="message">Informations envoyées à l'API.</param>
        /// <returns>Une liste d'objets récupérés (ou une liste vide).</returns>
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

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date.
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens.
        /// Classe trouvée sur le site :
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
