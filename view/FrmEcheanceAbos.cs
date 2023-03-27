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
        private readonly List<AbonnementEcheance> lesEcheancesAbos;
        public FrmEcheancesAbos()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            lesEcheancesAbos = controller.GetAbonnementsEcheance();
            RemplirEcheancesAbos();

        }

        private void RemplirEcheancesAbos()
        {
            bdgEcheancesAbos.DataSource = lesEcheancesAbos;
            dgvEcheancesAbosListe.DataSource = bdgEcheancesAbos;
            dgvEcheancesAbosListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEcheancesAbosListe.Columns["DateFinAbonnement"].HeaderText = "Date de fin d'abonnement";
            dgvEcheancesAbosListe.Columns["IdRevue"].HeaderText = "Id Revue";
            dgvEcheancesAbosListe.Columns["Montant"].Visible = false;
            dgvEcheancesAbosListe.Columns["Id"].Visible = false;
            dgvEcheancesAbosListe.Columns["DateCommande"].Visible = false;
        }
    }
}
