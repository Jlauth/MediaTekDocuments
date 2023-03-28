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
    public partial class FrmEcheancesAbos : Form
    {
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgEcheancesAbos = new BindingSource();
        private readonly List<EcheanceAbonnement> lesEcheancesAbos;
        public FrmEcheancesAbos()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            lesEcheancesAbos = controller.GetAbonnementsEcheance();
            RemplirEcheancesAbos(lesEcheancesAbos);
        }

        private void RemplirEcheancesAbos(List<EcheanceAbonnement> echeanceAbonnements)
        {
            bdgEcheancesAbos.DataSource = echeanceAbonnements;
            dgvEcheancesAbosListe.DataSource = bdgEcheancesAbos;
            dgvEcheancesAbosListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEcheancesAbosListe.Columns["DateFinAbonnement"].HeaderText = "Date de fin d'abonnement";
            dgvEcheancesAbosListe.Columns["IdRevue"].HeaderText = "Id Revue";
            dgvEcheancesAbosListe.Columns["Montant"].Visible = false;
            dgvEcheancesAbosListe.Columns["Id"].Visible = false;
            dgvEcheancesAbosListe.Columns["DateCommande"].Visible = false;
            dgvEcheancesAbosListe.Columns["IdRevue"].Visible = false;
        }

        private void BtnOkEcheancesAbos_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvEcheancesAbos_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvEcheancesAbosListe.Columns[e.ColumnIndex].HeaderText;
            List<EcheanceAbonnement> sortedList = new List<EcheanceAbonnement>();
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
