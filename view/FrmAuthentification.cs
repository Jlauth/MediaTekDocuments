using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechTalk.SpecFlow.Assist;

namespace MediaTekDocuments.view
{
    public partial class FrmAuthentification : Form
    {
        #region Commun
        private readonly FrmAuthentificationController controller;

        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmAuthentificationController();
        }
        #endregion

        #region Authentification
        /// <summary>
        /// Vide les champs d'authentification
        /// </summary>
        private void ViderChampsAuth()
        {
            txbAuthentificationLogin.Text = "";
            txbAuthentificationPwd.Text = "";
        }

        private void BtnConnexion_Click(object sender, EventArgs e)
        {
            string login = txbAuthentificationLogin.Text.ToString();
            string pwdNoH = txbAuthentificationPwd.Text.ToString();
            string idService = "";
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pwdNoH))
                {
                    MessageBox.Show("Tous les champs doivent être remplis", "INFORMATION");
                    ViderChampsAuth();
                }
                else
                {
                    string pwdWH = Sha256(pwdNoH);
                    Utilisateur utilisateur = new Utilisateur(login, pwdWH, idService);
                    idService = controller.ControleAuthentification(utilisateur);
                    if (idService != null)
                    {
                        if (idService == "50003")
                        {
                            MessageBox.Show("Vos droits ne sont pas suffisants pour accèder à cette application.", "INFORMATION");
                            ViderChampsAuth();
                        }
                        else
                        {
                            FrmMediatek frmMediatek = new FrmMediatek(idService);
                            frmMediatek.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mot de passe et/ou login incorrects", "ERREUR");
                        ViderChampsAuth();
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViderChampsAuth();
            }
        }

        /// <summary>
        /// Hashe le mot de passe
        /// </summary>
        /// <param name="randomString">la valeur à hashe</param>
        /// <returns></returns>
        static string Sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(System.Text.Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        #endregion
    }
}
