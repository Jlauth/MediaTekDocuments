using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController() => access = Access.GetInstance();

        /// <summary>
        /// Getter sur les documents
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Document> GetDocuments(string idDocument) => access.GetAllDocuments(idDocument);

        /// <summary>
        /// Getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres() => access.GetAllGenres();

        /// <summary>
        /// Getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres() => access.GetAllLivres();

        /// <summary>
        /// Getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd() => access.GetAllDvd();

        /// <summary>
        /// Getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues() => access.GetAllRevues();

        /// <summary>
        /// Getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons() => access.GetAllRayons();

        /// <summary>
        /// Getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics() => access.GetAllPublics();

        /// <summary>
        /// Getter sur les suivis
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis() => access.GetAllSuivis();


        /// <summary>
        /// Getter sur les commandes livres
        /// </summary>
        /// <returns>Liste d'objets Commande Document</returns>
        public List<CommandeDocument> GetAllCommandesLivres() => access.GetAllCommandesLivres();

        /// <summary>
        /// Récupère les commandes d'un document de type livre ou dvd
        /// </summary>
        /// /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Document</returns>
        public List<CommandeDocument> GetCommandeDocument(string idDocument) => access.GetCommandedDocument(idDocument);

        /// <summary>
        /// Récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument) => access.GetExemplairesRevue(idDocument);


        /// <summary>
        /// Crée un document dans la BDD
        /// </summary>
        /// <param name="document">l'objet document concerné</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerDocument(Document document) => access.CreerDocument(document);

        /// <summary>
        /// Crée un dvd dans la BDD
        /// </summary>
        /// <param name="dvd">l'objet dvd concerné</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerDvd(Dvd dvd) => access.CreerDvd(dvd);

        /// <summary>
        /// Crée un livre dans la BDD
        /// </summary>
        /// <param name="livre">l'objet livre concerné</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerLivre(Livre livre) => access.CreerLivre(livre);

        /// <summary>
        /// Créer une revue dans la BDD
        /// </summary>
        /// <param name="revue">l'objet revue concerné</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerRevue(Revue revue) => access.CreerRevue(revue);

        /// <summary>
        /// Créer un exemplaire d'une revue dans la BDD
        /// </summary>
        /// <param name="exemplaire">l'objet Exemplaire concerné</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire) => access.CreerExemplaire(exemplaire);

        /// <summary>
        /// Créer la commande d'un livre ou dvd en BDD
        /// </summary>
        /// <param name="commandeDocument">l'objet livre ou dvd concerné</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument) => access.CreerCommandeDocument(commandeDocument);

        /// <summary>
        /// Modifie un livre dans la BDD
        /// </summary>
        /// <param name="livre">l'objet Livre concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierLivre(Livre livre) => access.ModifierLivre(livre);

        /// <summary>
        /// Modifier un dvd dans la BDD
        /// </summary>
        /// <param name="dvd">l'objet Dvd concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierDvd(Dvd dvd) => access.ModifierDvd(dvd);

        /// <summary>
        /// Modifier une revue dans la BDD
        /// </summary>
        /// <param name="dvd">l'objet Revue concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierRevue(Revue revue) => access.ModifierRevue(revue);

        /// <summary>
        /// Modifier la commande d'un livre dans la BDD
        /// </summary>
        /// <param name="commandeDocument">l'objet commande document concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierCommandeDocument(CommandeDocument commandeDocument) => access.ModifierCommandeDocument(commandeDocument);

        /// <summary>
        /// Supprimer un livre dans la BDD
        /// </summary>
        /// <param name="livre">l'objet livre concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerLivre(Livre livre) => access.SupprimerLivre(livre);

        /// <summary>
        /// Supprimer un dvd dans la BDD
        /// </summary>
        /// <param name="dvd">l'objet dvd concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerDvd(Dvd dvd) => access.SupprimerDvd(dvd);

        /// <summary>
        /// Supprimer une revue dans la BDD
        /// </summary>
        /// <param name="revue">l'objet revue concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerRevue(Revue revue) => access.SupprimerRevue(revue);

        /// <summary>
        /// Supprimer une commande document dans la BDD
        /// </summary>
        /// <param name="commandeDocument">l'objet commande document concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerCommandeDocument(CommandeDocument commandeDocument) => access.SupprimerCommandeDocument(commandeDocument);
    }
}
