using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmAuthentification
    /// </summary>
    class FrmAuthentificationController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmAuthentificationController() => access = Access.GetInstance();

        public String ControleAuthentification(Utilisateur utilisateur) => access.ControleAuthentification(utilisateur);

    }
}
