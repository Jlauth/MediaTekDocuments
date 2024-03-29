﻿using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek.
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données.
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données.
        /// </summary>
        public FrmMediatekController() => access = Access.GetInstance();

        /// <summary>
        /// Getter sur les abonnements arrivant à échéance.
        /// </summary>
        /// <returns>Liste d'objets Abonnement.</returns>
        public List<AbonnementEcheance> GetAbonnementsEcheance() => access.GetAbonnementsEcheance();

        /// <summary>
        /// Récupère le document concerné dans la BDD.
        /// </summary>
        /// <param name="idDocument">Le document recherché.</param>
        /// <returns></returns>
        public List<Document> GetDocuments(string idDocument) => access.GetAllDocuments(idDocument);

        /// <summary>
        /// Getter sur la liste des genres.
        /// </summary>
        /// <returns>Liste d'objets Genre.</returns>
        public List<Categorie> GetAllGenres() => access.GetAllGenres();

        /// <summary>
        /// Getter sur la liste des livres.
        /// </summary>
        /// <returns>Liste d'objets Livre.</returns>
        public List<Livre> GetAllLivres() => access.GetAllLivres();

        /// <summary>
        /// Getter sur la liste des DVD.
        /// </summary>
        /// <returns>Liste d'objets DVD.</returns>
        public List<Dvd> GetAllDvd() => access.GetAllDvd();

        /// <summary>
        /// Getter sur la liste des revues.
        /// </summary>
        /// <returns>Liste d'objets Revue.</returns>
        public List<Revue> GetAllRevues() => access.GetAllRevues();

        /// <summary>
        /// Getter sur les rayons.
        /// </summary>
        /// <returns>Liste d'objets Rayon.</returns>
        public List<Categorie> GetAllRayons() => access.GetAllRayons();

        /// <summary>
        /// Getter sur les publics.
        /// </summary>
        /// <returns>Liste d'objets Public.</returns>
        public List<Categorie> GetAllPublics() => access.GetAllPublics();

        /// <summary>
        /// Getter sur les suivis
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis() => access.GetAllSuivis();

        /// <summary>
        /// Récupère les états des documents.
        /// </summary>
        /// <returns>Liste d'objets État.</returns>
        public List<Etat> GetAllEtats() => access.GetAllEtats();

        /// <summary>
        /// Getter sur les exemplaires.
        /// </summary>
        /// <returns>Liste d'objets Exemplaire.</returns>
        public List<Exemplaire> GetAllExemplaires() => access.GetAllExemplaires();

        /// <summary>
        /// Récupère les commandes d'un document de type livre ou DVD.
        /// </summary>
        /// /// <param name="idDocument">ID du document concerné.</param>
        /// <returns>Liste d'objets Commande.</returns>
        public List<CommandeDocument> GetCommandesDocument(string idDocument) => access.GetCommandesDocuments(idDocument);

        /// <summary>
        /// Récupère le numéro de commande d'un document.
        /// </summary>
        /// <param name="id">ID de la commande concernée.</param>
        /// <returns>Liste d'objets Commande.</returns>
        public List<Commande> GetCommandeId(string id) => access.GetCommandeId(id);

        /// <summary>
        /// Récupère les exemplaires d'un document.
        /// </summary>
        /// <param name="idDocument">ID du document concerné.</param>
        /// <returns>Liste d'objets Exemplaire.</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument) => access.GetExemplairesDocument(idDocument);

        /// <summary>
        /// Récupère le détail des exemplaires d'un document.
        /// </summary>
        /// <param name="idDocument">ID du document concerné.</param>
        /// <returns>Liste d'objets ExemplaireDetail.</returns>
        public List<ExemplaireDetail> GetExemplaireDetailsDocument(string idDocument) => access.GetExemplairesDetailsDocument(idDocument);

        /// <summary>
        /// Récupère le numéro de parution dans le cas d'une revue.
        /// </summary>
        /// <param name="numero">Numéro de parution de la revue concernée</param>
        /// <returns></returns>
        public List<Exemplaire> GetNumeroParution(string numero) => access.GetNumeroParution(numero);

        /// <summary>
        /// Récupère les abonnements d'une revue 
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>Liste d'objets Commande</returns>
        public List<Abonnement> GetAbonnementsRevue(string idRevue) => access.GetAbonnementsRevue(idRevue);

        /// <summary>
        /// Crée un document dans la BDD
        /// </summary>
        /// <param name="document">l'objet document concerné</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerDocument(Document document) => access.CreerDocument(document);

        /// <summary>
        /// Crée un DVD dans la BDD
        /// </summary>
        /// <param name="dvd">l'objet DVD concerné</param>
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
        /// Créer une commande dans la BDD
        /// </summary>
        /// <param name="abonnement">l'objet commandé</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerAbonnement(Abonnement abonnement) => access.CreerAbonnement(abonnement);

        /// <summary>
        /// Créer une commande document dans la BDD
        /// </summary>
        /// <param name="commandeDocument">le document commandé</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument) => access.CreerCommandeDocument(commandeDocument);

        /// <summary>
        /// Créer un abonnement revue en BDD
        /// </summary>
        /// <param name="id">id de l'abonnement</param>
        /// <param name="dateFinAbonnement">date de fin d'abonnement</param>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns></returns>
        public bool CreerAbonnementRevue(string id, DateTime dateFinAbonnement, string idRevue) => access.CreerAbonnementRevue(id, dateFinAbonnement, idRevue);

        /// <summary>
        /// Modifie un livre dans la BDD
        /// </summary>
        /// <param name="livre">l'objet Livre concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierLivre(Livre livre) => access.ModifierLivre(livre);

        /// <summary>
        /// Modifier un DVD dans la BDD
        /// </summary>
        /// <param name="dvd">l'objet DVD concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierDvd(Dvd dvd) => access.ModifierDvd(dvd);

        /// <summary>
        /// Modifier une revue dans la BDD
        /// </summary>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierRevue(Revue revue) => access.ModifierRevue(revue);

        /// <summary>
        /// Modifier la commande d'un livre dans la BDD
        /// </summary>
        /// <param name="commandeDocument">l'objet commande document concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierCommandeDocument(CommandeDocument commandeDocument) => access.ModifierCommandeDocument(commandeDocument);

        /// <summary>
        /// Modifier la commande d'une revue dans la BDD
        /// </summary>
        /// <param name="abonnementRevue">l'objet commande revue concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierCommandeRevue(Abonnement abonnementRevue) => access.ModifierCommandeRevue(abonnementRevue);

        /// <summary>
        /// Modifier un exemplaire dans la BDD
        /// </summary>
        /// <param name="exemplaire">l'objet exemplaire concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierExemplaire(Exemplaire exemplaire) => access.ModifierExemplaire(exemplaire);

        /// <summary>
        /// Supprimer un livre dans la BDD
        /// </summary>
        /// <param name="livre">l'objet livre concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerLivre(Livre livre) => access.SupprimerLivre(livre);

        /// <summary>
        /// Supprimer un DVD dans la BDD
        /// </summary>
        /// <param name="dvd">l'objet DVD concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerDvd(Dvd dvd) => access.SupprimerDvd(dvd);

        /// <summary>
        /// Supprimer une revue dans la BDD
        /// </summary>
        /// <param name="revue">l'objet revue concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerRevue(Revue revue) => access.SupprimerRevue(revue);

        /// <summary>
        /// Supprimer un exemplaire dans la BDD
        /// </summary>
        /// <param name="exemplaire">l'objet exemplaire concerné</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool SupprimerExemplaire(Exemplaire exemplaire) => access.SupprimerExemplaire(exemplaire);
        
        /// <summary>
        /// Supprimer une commande document dans la BDD
        /// </summary>
        /// <param name="commandeDocument">l'objet commande document concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerCommandeDocument(Commande commandeDocument) => access.SupprimerCommandeDocument(commandeDocument);

        /// <summary>
        /// Supprimer un abonnement dans la BDD
        /// </summary>
        /// <param name="abonnement">l'abonnement concerné</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerAbonnement(Abonnement abonnement) => access.SupprimerAbonnement(abonnement);

    }
}
