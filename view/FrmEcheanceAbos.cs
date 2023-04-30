using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechTalk.SpecFlow.Tracing;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Classe d'affichage des abonnements arrivant à échéance.
    /// </summary>
    public partial class FrmEcheancesAbos : Form
    {
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgEcheancesAbos = new BindingSource();
        private readonly List<AbonnementEcheance> lesEcheancesAbos;

        /// <summary>
        /// Constructeur de la classe FrmEcheancesAbos.
        /// </summary>
        public FrmEcheancesAbos()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            lesEcheancesAbos = controller.GetAbonnementsEcheance();
            RemplirEcheancesAbos(lesEcheancesAbos);
        }

        /// <summary>
        /// Rempli le grid avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="echeanceAbonnements">Liste des abonnements arrivant à échéance.</param>
        private void RemplirEcheancesAbos(List<AbonnementEcheance> echeanceAbonnements)
        {
            bdgEcheancesAbos.DataSource = echeanceAbonnements;
            dgvEcheancesAbosListe.DataSource = bdgEcheancesAbos;
            dgvEcheancesAbosListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEcheancesAbosListe.Columns["DateFinAbonnement"].HeaderText = "Date de fin d'abonnement";
            dgvEcheancesAbosListe.Columns["TitreRevue"].HeaderText = "Titre de la revue";
            dgvEcheancesAbosListe.Columns["Montant"].Visible = false;
            dgvEcheancesAbosListe.Columns["Id"].Visible = false;
            dgvEcheancesAbosListe.Columns["DateCommande"].Visible = false;
            dgvEcheancesAbosListe.Columns["IdRevue"].Visible = false;
        }

        /// <summary>
        /// Ferme la fenêtre d'authentification.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOkEcheancesAbos_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// Tri sur les colonnes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvEcheancesAbos_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvEcheancesAbosListe.Columns[e.ColumnIndex].HeaderText;
            List<AbonnementEcheance> sortedList = new List<AbonnementEcheance>();
            switch (titreColonne)
            {
                case "Date de fin d'abonnement":
                    sortedList = lesEcheancesAbos.OrderBy(o => o.DateFinAbonnement).ToList();
                    break;
                case "TitreRevue":
                    sortedList = lesEcheancesAbos.OrderBy(o => o.TitreRevue).ToList();
                    break;
            }
            RemplirEcheancesAbos(sortedList);
        }
    }
}
