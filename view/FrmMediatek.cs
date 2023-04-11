using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage principale
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgSuivis = new BindingSource();
        private readonly BindingSource bdgCommandesLivres = new BindingSource();
        private readonly BindingSource bdgCommandesDvd = new BindingSource();
        private readonly BindingSource bdgCommandesRevues = new BindingSource();
        private readonly BindingSource bdgExemplairesLivreListe = new BindingSource();
        private readonly BindingSource bdgExemplairesDvdListe = new BindingSource();
        private readonly BindingSource bdgExemplairesRevueListe = new BindingSource();
        private readonly BindingSource bdgEtats = new BindingSource();

        private List<Livre> lesLivres = new List<Livre>();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Revue> lesRevues = new List<Revue>();
        private List<CommandeDocument> lesCommandesDocuments = new List<CommandeDocument>();
        private List<Abonnement> lesAbonnementsRevues = new List<Abonnement>();
        private List<ExemplaireDetail> lesDetailsExemplairesDocument = new List<ExemplaireDetail>();

        private bool DefaultLivre = true;
        private bool DefaultDvd = true;
        private bool DefaultRevue = true;
        private bool DefaultParution = true;
        private bool DefaultCmdLivre = true;
        private bool DefaultCmdDvd = true;
        private bool DefaultCmdRevue = true;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        public FrmMediatek(string idService)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            if (idService != null)
            {
                if (idService == "50001" || idService == "50000")
                {
                    FrmEcheancesAbos frmEcheancesAbos = new FrmEcheancesAbos();
                    frmEcheancesAbos.ShowDialog();

                }
                else if (idService == "50002")
                {
                    AfficherInfosServicePrets();
                }
            }
        }

        /// <summary>
        /// Constructeur dédié à SpecFlow
        /// </summary>
        public FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Afficher uniquement les pages livres, DVD et revues
        /// Si service prêts
        /// </summary>
        private void AfficherInfosServicePrets()
        {
            tabOngletsApplication.TabPages.Remove(tabReceptionRevue);
            tabOngletsApplication.TabPages.Remove(tabCmdLivres);
            tabOngletsApplication.TabPages.Remove(tabCmdDvd);
            tabOngletsApplication.TabPages.Remove(tabCmdRevues);
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Rempli le combo suivi
        /// </summary>
        /// <param name="lesSuivis">liste des objets de type Suivi</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboSuivi(List<Suivi> lesSuivis, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesSuivis;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Remplir le combo état
        /// </summary>
        /// <param name="lesEtats">liste des objets de type État</param>
        /// <param name="bdg">bindingsource content les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboEtat(List<Etat> lesEtats, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesEtats;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres

        /// <summary>
        /// Ouverture de l'onglet Livres 
        /// Appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            if (DefaultLivre)
            {
                lesLivres = controller.GetAllLivres();
                RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
                RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
                RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
                RemplirComboEtat(controller.GetAllEtats(), bdgEtats, cbxExemplairesLivreEtat);
                RemplirLivresListeComplete();
                VideExemplairesLivreInfos();
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste des livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            if (livres != null)
            {
                bdgLivresListe.DataSource = livres;
                dgvLivresListe.DataSource = bdgLivresListe;
                dgvLivresListe.Columns["isbn"].Visible = false;
                dgvLivresListe.Columns["idRayon"].Visible = false;
                dgvLivresListe.Columns["idGenre"].Visible = false;
                dgvLivresListe.Columns["idPublic"].Visible = false;
                dgvLivresListe.Columns["image"].Visible = false;
                dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvLivresListe.Columns["id"].DisplayIndex = 0;
                dgvLivresListe.Columns["titre"].DisplayIndex = 1;
            }
        }

        /// <summary>
        /// Remplir le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesDetailsExemplaires">liste des exemplaires revue</param>
        private void RemplirExemplairesLivreListe(List<ExemplaireDetail> lesDetailsExemplaires)
        {
            if (lesDetailsExemplaires != null)
            {
                bdgExemplairesLivreListe.DataSource = lesDetailsExemplaires;
                dgvExemplairesLivreListe.DataSource = bdgExemplairesLivreListe;
                dgvExemplairesLivreListe.Columns["Photo"].Visible = false;
                dgvExemplairesLivreListe.Columns["Id"].Visible = false;
                dgvExemplairesLivreListe.Columns["IdEtat"].Visible = false;
                dgvExemplairesLivreListe.Columns["LibelleEtat"].HeaderText = "État";
                dgvExemplairesLivreListe.Columns["DateAchat"].HeaderText = "Date d'achat";
                dgvExemplairesLivreListe.Columns["Numero"].HeaderText = "Numéro";
                dgvExemplairesLivreListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvExemplairesLivreListe.Columns["DateAchat"].DisplayIndex = 0;
                dgvExemplairesLivreListe.Columns["LibelleEtat"].DisplayIndex = 1;
            }
            else
            {
                dgvExemplairesLivreListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (DefaultLivre && !txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                    AfficherExemplairesLivre();

                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titreRevue matche avec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (DefaultLivre && !txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, ré-affichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresIdGenre.Text = livre.IdGenre;
            txbLivresGenre.Text = livre.Genre;
            txbLivresIdPublic.Text = livre.IdPublic;
            txbLivresPublic.Text = livre.Public;
            txbLivresIdRayon.Text = livre.IdRayon;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Affiche les exemplaires du livre sélectionné via idDocument
        /// </summary>
        private void AfficherExemplairesLivre()
        {
            string idDocument = txbLivresNumRecherche.Text;
            lesDetailsExemplairesDocument = controller.GetExemplaireDetailsDocument(idDocument);
            RemplirExemplairesLivreListe(lesDetailsExemplairesDocument);

        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// Annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du Livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresIdGenre.Text = "";
            txbLivresGenre.Text = "";
            txbLivresIdPublic.Text = "";
            txbLivresPublic.Text = "";
            txbLivresIdRayon.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Vide les zones d'information des exemplaires livre
        /// </summary>
        private void VideExemplairesLivreInfos()
        {
            bdgExemplairesLivreListe.DataSource = null;
            cbxExemplairesLivreEtat.Enabled = false;
            cbxExemplairesLivreEtat.SelectedIndex = -1;
            txbLibelleEtatLivre.Text = "";
        }

        /// <summary>
        /// Vide la totalité des informations livres
        /// Désactive la synchro datagrid
        /// </summary>
        private void VideLivresInfosTotal()
        {
            VideLivresInfos();
            VideLivresZones();
            dgvLivresListe.Rows.Clear();
        }

        /// <summary>
        /// Change l'état des champs informations détaillées
        /// Afin de pouvoir les éditer ou non
        /// </summary>
        private void SetReadOnlyLivre(bool isReadOnly)
        {
            txbLivresNumero.ReadOnly = isReadOnly;
            txbLivresIsbn.ReadOnly = isReadOnly;
            txbLivresTitre.ReadOnly = isReadOnly;
            txbLivresAuteur.ReadOnly = isReadOnly;
            txbLivresCollection.ReadOnly = isReadOnly;
            txbLivresGenre.ReadOnly = isReadOnly;
            txbLivresPublic.ReadOnly = isReadOnly;
            txbLivresRayon.ReadOnly = isReadOnly;
            txbLivresImage.ReadOnly = isReadOnly;
        }

        /// <summary>
        /// Passe les champs en readonly
        /// </summary>
        private void DesactiverChampsInfosLivre()
        {
            SetReadOnlyLivre(true);
        }

        /// <summary>
        /// Passe les champs en mode édition
        /// </summary>
        private void ActiverChampsInfosLivre()
        {
            SetReadOnlyLivre(false);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion côté Livres
        /// </summary>
        /// <param name="acces">True ou False</param>
        private void AccesInfosLivresGrpBox(bool acces)
        {
            grpLivresInfos.Enabled = acces;
        }

        /// <summary>
        /// Tri sur les colonnes datagrid livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Tri sur les colonnes datagrid exemplaires livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesLivreListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvExemplairesLivreListe.Columns[e.ColumnIndex].HeaderText;
            List<ExemplaireDetail> sortedList = new List<ExemplaireDetail>();
            switch (titreColonne)
            {
                case "Numéro":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.Numero).ToList();
                    break;
                case "Date d'achat":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "État":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.LibelleEtat).ToList();
                    break;
            }
            RemplirExemplairesLivreListe(sortedList);
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultLivre && cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idGenre en fonction du genre select en cbx
        /// Uniquement en mode édition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenreEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenreEdit.SelectedIndex >= 0)
            {
                Genre genre = (Genre)cbxLivresGenreEdit.SelectedItem;
                txbLivresIdGenre.Text = genre.Id;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultLivre && cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idPublic en fonction du public select en cbx
        /// Uniquement en mode édition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublicEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublicEdit.SelectedIndex >= 0)
            {
                Public lePublic = (Public)cbxLivresPublicEdit.SelectedItem;
                txbLivresIdPublic.Text = lePublic.Id;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultLivre && cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idRayon en fonction du rayon select en cbx
        /// En mode édition uniquement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayonEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayonEdit.SelectedIndex >= 0)
            {
                Rayon rayon = (Rayon)cbxLivresRayonEdit.SelectedItem;
                txbLivresIdRayon.Text = rayon.Id;
            }
        }

        /// <summary>
        /// Récupère l'idEtat en fonction de l'exemplaire select en cbx
        /// Ainsi qu'un check dev pour l'id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxExemplairesLivreEtat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxExemplairesLivreEtat.SelectedIndex >= 0)
            {
                Etat etat = (Etat)cbxExemplairesLivreEtat.SelectedItem;
                cbxExemplairesLivreEtat.Text = etat.Id;
                txbCheckIdEtatExLivre.Text = cbxExemplairesLivreEtat.Text;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// Affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// Affiche l'état de l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesLivreListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvExemplairesLivreListe.CurrentCell != null)
            {
                try
                {
                    ExemplaireDetail exemplairesDetail = (ExemplaireDetail)bdgExemplairesLivreListe.List[bdgExemplairesLivreListe.Position];
                    txbLibelleEtatLivre.Text = exemplairesDetail.LibelleEtat;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Actualiser l'UI pour ajouter un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouterLivre_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre grid et édition
            DefaultLivre = false;
            // Vider tous les champs et activer les champs informations détaillées
            VideLivresInfosTotal();
            ActiverChampsInfosLivre();
            AccesInfosLivresGrpBox(true);
            // Liste des contrôles à masquer
            Control[] hide = {
                btnAjouterLivre, btnModifierLivre, btnSupprimerLivre,
                cbxLivresGenres, cbxLivresPublics, cbxLivresRayons
            };
            // Masquer tous les contrôles de la liste
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Liste des contrôles à afficher
            Control[] show = {
                btnAnnulerLivre, btnValiderAjouterLivre,
                txbLivresIdGenre, txbLivresIdPublic, txbLivresIdRayon,
                cbxLivresGenreEdit, cbxLivresPublicEdit, cbxLivresRayonEdit,
                lblLivresIdGenre, lblLivresIdPublic, lblLivresIdRayon
            };
            // Afficher tous les contrôles de la liste
            foreach (Control control in show)
            {
                control.Show();
            }
            // Remplir les cbx ajoutés avec la BDD
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayonEdit);
            // Focus sur le champ "ID livre" et vider les champs des ID catégories
            txbLivresNumero.Focus();
            txbLivresIdGenre.Clear();
            txbLivresIdRayon.Clear();
            txbLivresIdPublic.Clear();
        }

        /// <summary>
        /// Ajout d'un livre en BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderAjoutLivre_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumero.Text.Equals("") && cbxLivresGenreEdit.SelectedIndex != -1 && cbxLivresPublicEdit.SelectedIndex != -1 && cbxLivresPublicEdit.SelectedIndex != -1)
            {
                try
                {
                    string id = txbLivresNumero.Text;
                    string titre = txbLivresTitre.Text;
                    string image = txbLivresImage.Text;
                    string isbn = txbLivresIsbn.Text;
                    string auteur = txbLivresAuteur.Text;
                    string collection = txbLivresCollection.Text;
                    string idGenre = txbLivresIdGenre.Text;
                    string genre = cbxLivresGenreEdit.SelectedItem.ToString();
                    string idPublic = txbLivresIdPublic.Text;
                    string lePublic = cbxLivresPublicEdit.SelectedItem.ToString();
                    string idRayon = txbLivresIdRayon.Text;
                    string rayon = cbxLivresRayonEdit.SelectedItem.ToString();
                    Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    if (controller.CreerLivre(livre))
                    {
                        MessageBox.Show("Le livre " + titre + " vient d'être ajouté.");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur sur l'insertion d'un nouveau livre");
                }
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires");
            }
            RestaurationConfigLivres();
            TabLivres_Enter(sender, e);
        }

        /// <summary>
        /// Annule l'ajout d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show("Annulation de la modification en cours", "Information");
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
        }

        /// <summary>
        /// Annule la modification d'un exemplaire livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerExemplairesLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show("Annulation de la modification en cours", "Information");
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'un Livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierLivre_Click(object sender, EventArgs e)
        {
            // Désactive le lien avec le grid en mode édition
            DefaultLivre = false;
            // Liste des contrôles à afficher
            Control[] show = {
            btnValiderModifierLivre, btnAnnulerLivre,
            cbxLivresPublicEdit, cbxLivresGenreEdit, cbxLivresRayonEdit
            };
            foreach (Control control in show)
            {
                control.Show();
            }
            // Liste des contrôles à cacher
            Control[] hide =
            {
                btnModifierLivre, btnSupprimerLivre, btnAjouterLivre,
                cbxLivresGenres, cbxLivresPublics, cbxLivresRayons
            };
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Remplir les cbx ajoutés avec la BDD
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayonEdit);
            // Activer le groupe de contrôles "Informations Livres"
            ActiverChampsInfosLivre();
            // Focus sur le champ "ISBN" et vider les champs des ID
            txbLivresNumero.ReadOnly = true;
            txbLivresIsbn.Focus();
        }

        /// <summary>
        /// Active les champs nécessaires à la modification de l'état d'un exemplaire livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierExemplairesLivre_Click(object sender, EventArgs e)
        {
            // Liste des contrôles à afficher
            Control[] show = { btnModifierExemplairesLivreOk, btnAnnulerExemplairesLivre };
            foreach (Control control in show)
            {
                control.Show();
            }
            // Liste des contrôles à masquer
            Control[] hide = { btnSupprimerExemplairesLivre, btnModifierExemplairesLivre };
            foreach (Control control in hide)
            {
                control.Hide();
            }
            cbxExemplairesLivreEtat.Enabled = true;
        }

        /// <summary>
        /// Valide ou non la modification d'un livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierLivreOk_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumero.Text.Equals("") && cbxLivresGenreEdit.SelectedIndex != -1 && cbxLivresPublicEdit.SelectedIndex != -1 && cbxLivresRayonEdit.SelectedIndex != -1)
            {
                try
                {
                    string id = txbLivresNumero.Text;
                    string titre = txbLivresTitre.Text;
                    string image = txbLivresImage.Text;
                    string isbn = txbLivresIsbn.Text;
                    string auteur = txbLivresAuteur.Text;
                    string collection = txbLivresCollection.Text;
                    string idGenre = txbLivresIdGenre.Text;
                    string genre = cbxLivresGenreEdit.SelectedItem.ToString();
                    string idPublic = txbLivresIdPublic.Text;
                    string lePublic = cbxLivresPublicEdit.SelectedItem.ToString();
                    string idRayon = txbLivresIdRayon.Text;
                    string rayon = cbxLivresRayonEdit.SelectedItem.ToString();
                    Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    if (MessageBox.Show(this, "Confirmez-vous la modification livre " + livre.Titre + " ?", "INFORMATION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierLivre(livre);
                        MessageBox.Show("Modification du livre " + livre.Titre + " réussie", "Succès");
                    }
                    else
                    {
                        MessageBox.Show("Modification du livre " + livre.Titre + " annulée", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un livre", "Information");
            }
            RestaurationConfigLivres();
            TabLivres_Enter(sender, e);
        }

        /// <summary>
        /// Valide ou non la modification de l'état d'un exemplaire livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierExemplairesLivreOk_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumero.Text.Equals("") && cbxExemplairesLivreEtat.SelectedIndex != -1)
            {
                try
                {
                    ExemplaireDetail exemplairesDet = (ExemplaireDetail)bdgExemplairesLivreListe.List[bdgExemplairesLivreListe.Position];
                    int numero = exemplairesDet.Numero;
                    DateTime dateAchat = exemplairesDet.DateAchat;
                    string photo = exemplairesDet.Photo;
                    string idEtat = txbCheckIdEtatExLivre.Text;
                    string id = txbLivresNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de l'état de l'exemplaire " + exemplaire.Numero + " ?", "Information",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierExemplaire(exemplaire);
                        MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " réussie", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " annulée", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire", "Information");
            }
            RestaurationConfigLivres();
            TabLivres_Enter(sender, e);
        }

        /// <summary>
        /// Supprimer un livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerLivre_Click(object sender, EventArgs e)
        {
            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            if (livre != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression du livre " + livre.Titre + " ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerLivre(livre);
                        MessageBox.Show("Suppression du livre " + livre.Titre + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression du livre " + livre.Titre + " annulée");
                    }

                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de ce livre, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucun livre sélectionné");
            }
            RestaurationConfigLivres();
            TabLivres_Enter(sender, e);
        }

        /// <summary>
        /// Supprime un exemplaire livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerExemplairesLivre_Click(object sender, EventArgs e)
        {
            ExemplaireDetail exemplairesDet = (ExemplaireDetail)bdgExemplairesLivreListe.List[bdgExemplairesLivreListe.Position];
            int numero = exemplairesDet.Numero;
            DateTime dateAchat = exemplairesDet.DateAchat;
            string photo = exemplairesDet.Photo;
            string idEtat = txbCheckIdEtatExLivre.Text;
            string id = txbLivresNumero.Text;
            Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);
            if (exemplaire != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de l'exemplaire n°" + exemplaire.Numero + " ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerExemplaire(exemplaire);
                        MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " annulée");
                    }

                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de cet exemplaire, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucun exemplaire sélectionné");
            }
            RestaurationConfigLivres();
            TabLivres_Enter(sender, e);
        }


        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Restaure l'onglet Livres à son état initial
        /// </summary>
        private void RestaurationConfigLivres()
        {
            // Activer le lien avec le grid
            DefaultLivre = true;
            // Liste des contrôles à masquer
            Control[] hide = { txbLivresIdGenre, txbLivresIdPublic, txbLivresIdRayon,
                lblLivresIdGenre, lblLivresIdPublic, lblLivresIdRayon,
                cbxLivresGenreEdit, cbxLivresPublicEdit, cbxLivresRayonEdit,
                btnAnnulerLivre, btnValiderAjouterLivre, btnAnnulerExemplairesLivre, btnModifierExemplairesLivreOk
            };
            foreach (Control control in hide)
            {
                control.Hide();
            }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterLivre, btnModifierLivre, btnSupprimerLivre,
                btnModifierExemplairesLivre, btnSupprimerExemplairesLivre,
                cbxLivresGenres, cbxLivresPublics, cbxLivresRayons
            };
            foreach (Control control in show)
            {
                control.Show();
            }
            DesactiverChampsInfosLivre();

        }
        #endregion

        #region Onglet DVD
        /// <summary>
        /// Ouverture de l'onglet DVD
        /// Appel des méthodes pour remplir le datagrid des DVD et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabDvd_Enter(object sender, EventArgs e)
        {
            if (DefaultDvd)
            {
                lesDvd = controller.GetAllDvd();
                RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
                RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
                RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
                RemplirComboEtat(controller.GetAllEtats(), bdgEtats, cbxExemplairesDvdEtat);
                RemplirDvdListeComplete();
                VideExemplairesDvdInfos();
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="dvds">liste de DVD</param>
        private void RemplirDvdListe(List<Dvd> dvds)
        {
            bdgDvdListe.DataSource = dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesDetailsExemplaires">liste des exemplaires du DVD cible</param>
        private void RemplirExemplairesDvdListe(List<ExemplaireDetail> lesDetailsExemplaires)
        {
            bdgExemplairesDvdListe.DataSource = lesDetailsExemplaires;
            dgvExemplairesDvdListe.DataSource = bdgExemplairesDvdListe;
            dgvExemplairesDvdListe.Columns["Photo"].Visible = false;
            dgvExemplairesDvdListe.Columns["Id"].Visible = false;
            dgvExemplairesDvdListe.Columns["IdEtat"].Visible = false;
            dgvExemplairesDvdListe.Columns["LibelleEtat"].HeaderText = "État";
            dgvExemplairesDvdListe.Columns["DateAchat"].HeaderText = "Date d'achat";
            dgvExemplairesDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Recherche et affichage du DVD dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> dvds = new List<Dvd>() { dvd };
                    RemplirDvdListe(dvds);
                    AfficherExemplairesDvd();
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des DVD dont le titreRevue matche avec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdsParTitre;
                lesDvdsParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdsParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, ré-affichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du DVD sélectionné
        /// </summary>
        /// <param name="dvd">le DVD</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdIdGenre.Text = dvd.IdGenre;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdIdPublic.Text = dvd.IdPublic;
            txbDvdPublic.Text = dvd.Public;
            txbDvdIdRayon.Text = dvd.IdRayon;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Affiche les exemplaires du DVD sélectionné via idDocument
        /// </summary>
        private void AfficherExemplairesDvd()
        {
            string idDocument = txbDvdNumRecherche.Text;
            lesDetailsExemplairesDocument = controller.GetExemplaireDetailsDocument(idDocument);
            RemplirExemplairesDvdListe(lesDetailsExemplairesDocument);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du DVD
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdIdGenre.Text = "";
            txbDvdGenre.Text = "";
            txbDvdIdPublic.Text = "";
            txbDvdPublic.Text = "";
            txbDvdIdRayon.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Vide les zones d'affichages des informations exemplaires DVD
        /// </summary>
        private void VideExemplairesDvdInfos()
        {
            bdgExemplairesDvdListe.DataSource = null;
            cbxExemplairesDvdEtat.Enabled = false;
            cbxExemplairesDvdEtat.SelectedIndex = -1;
            txbLibelleEtatDvd.Text = "";
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultDvd && cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère de l'idGenre en fonction du genre select cbx
        /// Uniquement en mode édition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdGenreEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenreEdit.SelectedIndex >= 0)
            {
                Genre genre = (Genre)cbxDvdGenreEdit.SelectedItem;
                txbDvdIdGenre.Text = genre.Id;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultDvd && cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idPublic en fonction du public select en cbx
        /// Uniquement en mode édition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdPublicEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublicEdit.SelectedIndex >= 0)
            {
                Public lePublic = (Public)cbxDvdPublicEdit.SelectedItem;
                txbDvdIdPublic.Text = lePublic.Id;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultDvd && cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idEtat en fonction de l'exemplaire select en cbx
        /// Ainsi qu'un check version dev pour l'id
        /// En mode édition uniquement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxExemplairesDvdEtat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxExemplairesDvdEtat.SelectedIndex >= 0)
            {
                Etat etat = (Etat)cbxExemplairesDvdEtat.SelectedItem;
                cbxExemplairesDvdEtat.Text = etat.Id;
                txbCheckIdEtatExDvd.Text = cbxExemplairesDvdEtat.Text;
            }
        }

        /// <summary>
        /// Récupère l'idRayon en fonction du rayon select en cbx
        /// En mode édition uniquement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxDvdRayonEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayonEdit.SelectedIndex >= 0)
            {
                Rayon rayon = (Rayon)cbxDvdRayonEdit.SelectedItem;
                txbDvdIdRayon.Text = rayon.Id;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// Affiche l'état de l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvExemplairesDvdListe.CurrentCell != null)
            {
                try
                {
                    ExemplaireDetail exemplaireDetail = (ExemplaireDetail)bdgExemplairesDvdListe.List[bdgExemplairesDvdListe.Position];
                    txbLibelleEtatDvd.Text = exemplaireDetail.LibelleEtat;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Actualiser l'UI pour ajouter un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouterDvd_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre grid et édition
            DefaultDvd = false;
            // Vider tous les champs et activer les champs informations détaillées
            VideDvdInfosTotal();
            ActiverChampsInfosDvd();
            AccesInformationsDvdGroupBox(true);
            // Liste des contrôles à masquer
            Control[] hide = {
            btnAjouterDvd, btnModifierDvd, btnSupprimerDvd,
            cbxDvdGenres, cbxDvdPublics, cbxDvdRayons
            };
            // Masquer tous les contrôles de la liste
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Liste des contrôles à afficher
            Control[] show = {
                btnAnnulerAjouterDvd, btnValiderAjouterDvd,
                txbDvdIdGenre, txbDvdIdPublic, txbDvdIdRayon,
                cbxDvdGenreEdit, cbxDvdPublicEdit, cbxDvdRayonEdit,
                lblDvdIdGenre, lblDvdIdPublic, lblDvdIdRayon
            };
            // Afficher tous les contrôles de la liste
            foreach (Control control in show)
            {
                control.Show();
            }
            // Remplir les cbx ajoutés avec la BDD
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayonEdit);
            // Focus sur le champ "ID DVD" et vider les champs des ID catégorie
            txbDvdNumero.Focus();
            txbDvdIdGenre.Clear();
            txbDvdIdRayon.Clear();
            txbDvdIdPublic.Clear();

        }

        /// <summary>
        /// Ajout d'un DVD si non présent en BDD
        /// Ou refuse le cas échéant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderAjoutDvd_Click(object sender, EventArgs e)
        {

            if (!txbDvdNumero.Text.Equals("") && !txbDvdDuree.Text.Equals("") && cbxDvdGenreEdit.SelectedIndex != -1 && cbxDvdPublicEdit.SelectedIndex != -1 && cbxDvdRayonEdit.SelectedIndex != -1)
            {
                try
                {
                    string id = txbDvdNumero.Text;
                    string titre = txbDvdTitre.Text;
                    string image = txbDvdImage.Text;
                    int duree = int.Parse(txbDvdDuree.Text);
                    string realisateur = txbDvdRealisateur.Text;
                    string synopsis = txbDvdSynopsis.Text;
                    string idGenre = txbDvdIdGenre.Text;
                    string genre = cbxDvdGenreEdit.SelectedItem.ToString();
                    string idPublic = txbDvdIdPublic.Text;
                    string lePublic = cbxDvdPublicEdit.SelectedItem.ToString();
                    string idRayon = txbDvdIdRayon.Text;
                    string rayon = cbxDvdRayonEdit.SelectedItem.ToString();
                    Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    if (controller.CreerDvd(dvd))
                    {
                        MessageBox.Show("Le DVD " + titre + " vient d'être ajouté.");
                        RestaurationConfigDvd();
                        TabDvd_Enter(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur sur l'insertion d'un nouveau DVD" + ex.Message);
                    RestaurationConfigDvd();
                    TabDvd_Enter(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires");
            }
            RestaurationConfigDvd();
            TabDvd_Enter(sender, e);
        }

        /// <summary>
        /// Annule l'ajout d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerAjoutDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show("Annulation de la modification en cours", "Information");
                RestaurationConfigDvd();
                TabDvd_Enter(sender, e);
            }
        }

        /// <summary>
        /// Annule la modification de l'état d'un exemplaire DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerExemplairesDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show("Annulation de la modification en cours", "Information");
                RestaurationConfigDvd();
                TabDvd_Enter(sender, e);
            }
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierDvd_Click(object sender, EventArgs e)
        {
            DefaultDvd = false;
            Control[] show = {
            btnValiderModifierDvd, btnAnnulerModifierDvd,
            cbxDvdPublicEdit, cbxDvdGenreEdit, cbxDvdRayonEdit,
            };
            foreach (Control control in show)
            {
                control.Show();
            }

            Control[] hide =
            {
                btnValiderAjouterDvd, btnModifierDvd, btnSupprimerDvd, btnAjouterDvd,
                cbxDvdGenres, cbxDvdPublics, cbxDvdRayons
            };
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Remplir les cbx ajoutés avec la BDD
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayonEdit);
            // Activer le groupe de contrôles "Informations DVD"
            ActiverChampsInfosDvd();
            // Focus sur le champ "Durée" et vider les champs des ID
            txbDvdNumero.ReadOnly = true;
            txbDvdDuree.Focus();
        }

        /// <summary>
        /// Affiche les champs nécessaires à la modification de l'état d'un exemplaire DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierExemplairesDvd_Click(object sender, EventArgs e)
        {
            // Liste des contrôles à afficher
            Control[] show = { btnModifierExemplairesDvdOk, btnAnnulerExemplairesDvd };
            foreach (Control control in show)
            {
                control.Show();
            }
            // Liste des contrôles à masquer
            Control[] hide = { btnModifierExemplairesDvd, btnSupprimerExemplairesDvd };
            foreach (Control control in hide)
            {
                control.Hide();
            }
            cbxExemplairesDvdEtat.Enabled = true;
        }

        /// <summary>
        /// Valide la modification d'un livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderModifierDvd_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumero.Text.Equals("") && cbxDvdGenreEdit.SelectedIndex != -1 && cbxDvdPublicEdit.SelectedIndex != -1 && cbxDvdRayonEdit.SelectedIndex != -1)
            {
                try
                {
                    string id = txbDvdNumero.Text;
                    string titre = txbDvdTitre.Text;
                    string image = txbDvdImage.Text;
                    int duree = int.Parse(txbDvdDuree.Text);
                    string realisateur = txbDvdRealisateur.Text;
                    string synopsis = txbDvdSynopsis.Text;
                    string idGenre = txbDvdIdGenre.Text;
                    string genre = cbxDvdGenreEdit.SelectedItem.ToString();
                    string idPublic = txbDvdIdPublic.Text;
                    string lePublic = cbxDvdPublicEdit.SelectedItem.ToString();
                    string idRayon = txbDvdIdRayon.Text;
                    string rayon = cbxDvdRayonEdit.SelectedItem.ToString();
                    Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    if (MessageBox.Show(this, "Confirmez-vous la modification du DVD " + dvd.Titre + " ?", "INFORMATION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierDvd(dvd);
                        MessageBox.Show("Modification du DVD " + dvd.Titre + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Modification du DVD " + dvd.Titre + " annulée");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un DVD valide");
            }
            RestaurationConfigDvd();
            TabDvd_Enter(sender, e);
        }

        /// <summary>
        /// Valide ou non la modification de l'état d'un exemplaire revue dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExemplairesDvdModifierOk_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumero.Text.Equals("") && cbxExemplairesDvdEtat.SelectedIndex != -1)
            {
                try
                {
                    ExemplaireDetail exemplaireDetail = (ExemplaireDetail)bdgExemplairesDvdListe.List[bdgExemplairesDvdListe.Position];
                    int numero = exemplaireDetail.Numero;
                    DateTime dateAchat = exemplaireDetail.DateAchat;
                    string photo = exemplaireDetail.Photo;
                    string idEtat = txbCheckIdEtatExDvd.Text;
                    string id = txbDvdNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de l'état de l'exemplaire n°" + exemplaire.Numero + " ?", "Information",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierExemplaire(exemplaire);
                        MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " réussie", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " annulée", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire", "Information");
            }
            RestaurationConfigDvd();
            TabDvd_Enter(sender, e);
        }

        /// <summary>
        /// Supprimer un DVD dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerDvd_Click(object sender, EventArgs e)
        {
            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
            if (dvd != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression du DVD " + dvd.Titre + " ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerDvd(dvd);
                        MessageBox.Show("Suppression du DVD " + dvd.Titre + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression du DVD " + dvd.Titre + " annulée");
                    }

                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de ce DVD, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucun livre sélectionné");
            }
            RestaurationConfigDvd();
            TabDvd_Enter(sender, e);
        }

        /// <summary>
        /// Supprimer un exemplaire DVD dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerExemplairesDvd_Click(object sender, EventArgs e)
        {
            ExemplaireDetail exemplairesDet = (ExemplaireDetail)bdgExemplairesDvdListe.List[bdgExemplairesDvdListe.Position];
            int numero = exemplairesDet.Numero;
            DateTime dateAchat = exemplairesDet.DateAchat;
            string photo = exemplairesDet.Photo;
            string idEtat = txbCheckIdEtatExDvd.Text;
            string id = txbDvdNumero.Text;
            Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);
            if (exemplaire != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de l'exemplaire n°" + exemplaire.Numero + " ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerExemplaire(exemplaire);
                        MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " annulée");
                    }

                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de cet exemplaire, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucun exemplaire sélectionné");
            }
            RestaurationConfigDvd();
            TabDvd_Enter(sender, e);
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des DVD
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// Vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Vide la totalité des informations DVD
        /// Désactive la synchro datagrid
        /// </summary>
        private void VideDvdInfosTotal()
        {
            VideDvdInfos();
            VideDvdZones();
            dgvDvdListe.Rows.Clear();
        }

        /// <summary>
        /// Restaure l'UI DVD initiale
        /// </summary>
        private void RestaurationConfigDvd()
        {
            DefaultDvd = true;
            // Liste des contrôles à masquer
            Control[] hide = { txbDvdIdGenre, txbDvdIdPublic, txbDvdIdRayon,
                lblDvdIdGenre, lblDvdIdPublic, lblDvdIdRayon,
                cbxDvdGenreEdit, cbxDvdPublicEdit, cbxDvdRayonEdit,
                btnValiderAjouterDvd, btnValiderModifierDvd,  btnAnnulerAjouterDvd,  btnAnnulerModifierDvd,
                btnAnnulerExemplairesDvd, btnModifierExemplairesDvdOk
            };
            // Masquer tous les contrôles de la liste
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterDvd, btnModifierDvd, btnSupprimerDvd, btnModifierExemplairesDvd, btnSupprimerExemplairesDvd,
                cbxDvdGenres, cbxDvdPublics, cbxDvdRayons
            };
            // Afficher tous les contrôles de la liste
            foreach (Control control in show)
            {
                control.Show();
            }
            DesactiverChampsInfosDvd();
        }

        /// <summary>
        /// Change l'état des champs informations détaillées
        /// </summary>
        private void SetReadOnlyDvd(bool isReadOnly)
        {
            txbDvdNumero.ReadOnly = isReadOnly;
            txbDvdDuree.ReadOnly = isReadOnly;
            txbDvdTitre.ReadOnly = isReadOnly;
            txbDvdRealisateur.ReadOnly = isReadOnly;
            txbDvdSynopsis.ReadOnly = isReadOnly;
            txbDvdGenre.ReadOnly = isReadOnly;
            txbDvdPublic.ReadOnly = isReadOnly;
            txbDvdRayon.ReadOnly = isReadOnly;
            txbDvdImage.ReadOnly = isReadOnly;
        }

        /// <summary>
        /// Désactive l'accès aux champs DVD
        /// </summary>
        private void DesactiverChampsInfosDvd()
        {
            SetReadOnlyDvd(true);
        }

        /// <summary>
        /// Active l'accès aux champs DVD
        /// </summary>
        private void ActiverChampsInfosDvd()
        {
            SetReadOnlyDvd(false);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion côté DVD
        /// </summary>
        /// <param name="acces"></param>
        private void AccesInformationsDvdGroupBox(bool acces)
        {
            grpDvdInfos.Enabled = acces;
        }

        /// <summary>
        /// Tri sur les colonnes datagrid DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Durée":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Réalisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        /// <summary>
        /// Tri sur les colonnes datagrid DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvExemplairesLivreListe.Columns[e.ColumnIndex].HeaderText;
            List<ExemplaireDetail> sortedList = new List<ExemplaireDetail>();
            switch (titreColonne)
            {
                case "Numéro":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.Numero).ToList();
                    break;
                case "Date d'achat":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "État":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.LibelleEtat).ToList();
                    break;
            }
            RemplirExemplairesDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// Appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabRevues_Enter(object sender, EventArgs e)
        {
            if (DefaultRevue)
            {
                lesRevues = controller.GetAllRevues();
                RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
                RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
                RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
                RemplirComboEtat(controller.GetAllEtats(), bdgEtats, cbxExemplairesRevueEtat);
                RemplirRevuesListeComplete();
                VideExemplairesRevueInfos();
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            if (revues != null)
            {
                bdgRevuesListe.DataSource = revues;
                dgvRevuesListe.DataSource = bdgRevuesListe;
                dgvRevuesListe.Columns["idRayon"].Visible = false;
                dgvRevuesListe.Columns["idGenre"].Visible = false;
                dgvRevuesListe.Columns["idPublic"].Visible = false;
                dgvRevuesListe.Columns["image"].Visible = false;
                dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvRevuesListe.Columns["id"].DisplayIndex = 0;
                dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="detailExemplaires">liste des exemplaires revue</param>
        private void RemplirExemplairesRevueListe(List<ExemplaireDetail> detailExemplaires)
        {
            if (detailExemplaires != null)
            {
                bdgExemplairesRevueListe.DataSource = detailExemplaires;
                dgvExemplairesRevueListe.DataSource = bdgExemplairesRevueListe;
                dgvExemplairesRevueListe.Columns["Photo"].Visible = false;
                dgvExemplairesRevueListe.Columns["Id"].Visible = false;
                dgvExemplairesRevueListe.Columns["IdEtat"].Visible = false;
                dgvExemplairesRevueListe.Columns["LibelleEtat"].HeaderText = "État";
                dgvExemplairesRevueListe.Columns["DateAchat"].HeaderText = "Date d'achat";
                dgvExemplairesRevueListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvExemplairesRevueListe.Columns["DateAchat"].DisplayIndex = 0;
                dgvExemplairesRevueListe.Columns["LibelleEtat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesRevueListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (DefaultRevue && !txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                    AfficherExemplairesRevue();
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titreRevue matche avec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, ré-affichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Affiche les exemplaires de la revue sélectionnée via idDocument
        /// </summary>
        private void AfficherExemplairesRevue()
        {
            string idDocument = txbRevuesNumRecherche.Text;
            lesDetailsExemplairesDocument = controller.GetExemplaireDetailsDocument(idDocument);
            RemplirExemplairesRevueListe(lesDetailsExemplairesDocument);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la revue
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Vide les zones d'information des exemplaires revue
        /// </summary>
        private void VideExemplairesRevueInfos()
        {
            bdgExemplairesRevueListe.DataSource = null;
            cbxExemplairesRevueEtat.Enabled = false;
            cbxExemplairesRevueEtat.SelectedIndex = -1;
            txbLibelleEtatRevue.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes datagrid exemplaires revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesRevueList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvExemplairesRevueListe.Columns[e.ColumnIndex].HeaderText;
            List<ExemplaireDetail> sortedList = new List<ExemplaireDetail>();
            switch (titreColonne)
            {
                case "Numéro":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.Numero).ToList();
                    break;
                case "Date d'achat":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "État":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.LibelleEtat).ToList();
                    break;
            }
            RemplirExemplairesRevueListe(sortedList);
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultRevue && cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idGenre en fonction du genre select en cbx
        /// Uniquement en mode édition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesGenreEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenreEdit.SelectedIndex >= 0)
            {
                Genre genre = (Genre)cbxRevuesGenreEdit.SelectedItem;
                txbRevuesIdGenre.Text = genre.Id;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultRevue && cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idPublic en fonction du public select en cbx
        /// Uniquement en mode édition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesPublicEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublicEdit.SelectedIndex >= 0)
            {
                Public lePublic = (Public)cbxRevuesPublicEdit.SelectedItem;
                txbRevuesIdPublic.Text = lePublic.Id;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultRevue && cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idRayon en fonction du rayon select en cbx
        /// En mode édition uniquement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxRevuesRayonEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayonEdit.SelectedIndex >= 0)
            {
                Rayon rayon = (Rayon)cbxRevuesRayonEdit.SelectedItem;
                txbRevuesIdRayon.Text = rayon.Id;
            }
        }

        /// <summary>
        /// Récupère l'idEtat en fonction de l'exemplaire select en cbx
        /// Ainsi qu'un check version dev pour l'id
        /// En mode édition uniquement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxExemplairesRevueEtat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxExemplairesRevueEtat.SelectedIndex >= 0)
            {
                Etat etat = (Etat)cbxExemplairesRevueEtat.SelectedItem;
                cbxExemplairesRevueEtat.Text = etat.Id;
                txbCheckIdEtatExRevue.Text = cbxExemplairesRevueEtat.Text;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// Affiche l'état de l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesRevueListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvExemplairesRevueListe.CurrentCell != null)
            {
                try
                {
                    ExemplaireDetail exemplairesDet = (ExemplaireDetail)bdgExemplairesRevueListe.List[bdgExemplairesRevueListe.Position];
                    txbLibelleEtatRevue.Text = exemplairesDet.LibelleEtat;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à l'ajout d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouterRevue_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre grid et édition
            DefaultRevue = false;

            // Vider tous les champs et activer les champs informations détaillées
            VideRevuesInfosTotal();
            ActiverChampsInfosRevues();
            AccesInformationsRevuesGroupBox(true);

            // Liste des contrôles à masquer
            Control[] hide = {
                btnAjouterRevue, btnModifierRevue, btnSupprimerRevue,
                cbxRevuesGenres, cbxRevuesPublics, cbxRevuesRayons
            };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAnnulerRevue, btnValiderAjouterRevue,
                txbRevuesIdGenre, txbRevuesIdPublic, txbRevuesIdRayon,
                cbxRevuesGenreEdit, cbxRevuesPublicEdit, cbxRevuesRayonEdit,
                lblRevueIdGenre, lblRevueIdPublic, lblRevueIdRayon
            };
            foreach (Control control in show) { control.Show(); }

            // Remplir les cbx ajoutés avec la BDD
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayonEdit);

            // Focus sur le champ "ID livre" et vider les champs des ID catégories
            txbRevuesNumero.Focus();
            txbRevuesIdGenre.Clear();
            txbRevuesIdRayon.Clear();
            txbRevuesIdPublic.Clear();
        }

        /// <summary>
        /// Ajout d'une revue si non présente en BDD
        /// Ou refuse le cas échéant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderAjoutRevue_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumero.Text.Equals("") && cbxRevuesGenreEdit.SelectedIndex != -1 && cbxRevuesPublicEdit.SelectedIndex != -1 && cbxRevuesRayonEdit.SelectedIndex != -1)
            {
                try
                {
                    string id = txbRevuesNumero.Text;
                    string titre = txbRevuesTitre.Text;
                    string image = txbRevuesImage.Text;
                    string idGenre = txbRevuesIdGenre.Text;
                    string genre = cbxRevuesGenreEdit.SelectedItem.ToString();
                    string idPublic = txbRevuesIdPublic.Text;
                    string lePublic = cbxRevuesPublicEdit.SelectedItem.ToString();
                    string idRayon = txbRevuesIdRayon.Text;
                    string rayon = cbxRevuesRayonEdit.SelectedItem.ToString();
                    string periodicite = txbRevuesPeriodicite.Text;
                    int delaiMiseADispo = int.Parse(txbRevuesDateMiseADispo.Text);
                    Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);
                    if (controller.CreerRevue(revue))
                    {
                        MessageBox.Show("La revue " + revue + " vient d'être ajoutée");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur sur l'insertion d'une nouvelle revue");
                }
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires");
            }
            RestaurationConfigRevues();
            TabRevues_Enter(sender, e);
        }

        /// <summary>
        /// Annule l'ajout d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerAjoutRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
               MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show("Annulation de la modification en cours", "Information");
                RestaurationConfigRevues();
                TabRevues_Enter(sender, e);
            }
        }

        /// <summary>
        /// Annule la modification d'un exemplaire revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerExemplairesRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
               MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show("Annulation de la modification en cours", "Information");
                RestaurationConfigRevues();
                TabRevues_Enter(sender, e);
            }
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierRevue_Click(object sender, EventArgs e)
        {
            DefaultRevue = false;

            // Liste des contrôles à afficher
            Control[] show = {btnValiderModifierRevue, btnAnnulerRevue,
            cbxRevuesPublicEdit, cbxRevuesGenreEdit, cbxRevuesRayonEdit, };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide ={ btnModifierRevue, btnSupprimerRevue, btnAjouterRevue,
                cbxRevuesGenres, cbxRevuesPublics, cbxRevuesRayons};
            foreach (Control control in hide) { control.Hide(); }

            // Remplir les cbx ajoutés avec la BDD
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayonEdit);

            // Activer le groupe de contrôles "Informations Revues"
            ActiverChampsInfosRevues();
            txbRevuesNumero.ReadOnly = true;
            txbRevuesTitre.Focus();
        }

        /// <summary>
        /// Active les champs nécessaires à la modification de l'état d'un exemplaire revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierExemplairesRevue_Click(object sender, EventArgs e)
        {
            // Liste des contrôles à afficher
            Control[] show = { btnModifierExemplairesRevueOk, btnAnnulerExemplairesRevue };
            foreach (Control control in show)
            {
                control.Show();
            }
            // Liste des contrôles à masquer
            Control[] hide = { btnSupprimerExemplairesRevue, btnModifierExemplairesRevue };
            foreach (Control control in hide)
            {
                control.Hide();
            }
            cbxExemplairesRevueEtat.Enabled = true;
        }

        /// <summary>
        /// Valide la modification d'une revue dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderModifierRevue_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumero.Text.Equals("") && cbxRevuesGenreEdit.SelectedIndex != -1 && cbxRevuesPublicEdit.SelectedIndex != -1 && cbxRevuesRayonEdit.SelectedIndex != -1)
            {
                try
                {
                    string id = txbRevuesNumero.Text;
                    string titre = txbRevuesTitre.Text;
                    string image = txbRevuesImage.Text;
                    string idGenre = txbRevuesIdGenre.Text;
                    string genre = cbxRevuesGenreEdit.SelectedItem.ToString();
                    string idPublic = txbRevuesIdPublic.Text;
                    string lePublic = cbxRevuesPublicEdit.SelectedItem.ToString();
                    string idRayon = txbRevuesIdRayon.Text;
                    string rayon = cbxRevuesRayonEdit.SelectedItem.ToString();
                    string periodicite = txbRevuesPeriodicite.Text;
                    int delaiMiseADispo = int.Parse(txbRevuesDateMiseADispo.Text);
                    Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de la revue " + revue.Titre + " ?", "INFORMATION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierRevue(revue);
                        MessageBox.Show("Modification de la revue " + revue.Titre + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Modification de la revue " + revue.Titre + " annulée");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une revue valide");
            }
            RestaurationConfigRevues();
            TabRevues_Enter(sender, e);
        }

        /// <summary>
        /// Valide ou non la modification de l'état d'un exemplaire revue dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierExemplairesRevueOk_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals("") && cbxExemplairesRevueEtat.SelectedIndex != -1)
            {
                try
                {
                    ExemplaireDetail exemplairesDet = (ExemplaireDetail)bdgExemplairesRevueListe.List[bdgExemplairesRevueListe.Position];
                    int numero = exemplairesDet.Numero;
                    DateTime dateAchat = exemplairesDet.DateAchat;
                    string photo = exemplairesDet.Photo;
                    string idEtat = txbCheckIdEtatExRevue.Text;
                    string id = txbRevuesNumRecherche.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de l'état de l'exemplaire " + exemplaire.Numero + " ?", "Information",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierExemplaire(exemplaire);
                        MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " réussie", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " annulée", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire", "Information");
            }
            RestaurationConfigRevues();
            TabRevues_Enter(sender, e);
        }

        /// <summary>
        /// Supprimer une revue dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerRevue_Click(object sender, EventArgs e)
        {
            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
            if (revue != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de la revue " + revue.Titre + " ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerRevue(revue);
                        MessageBox.Show("Suppression de la revue " + revue.Titre + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression de la revue " + revue.Titre + " annulée");
                    }

                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de cette revue, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucune revue sélectionnée");
            }
            RestaurationConfigRevues();
            TabRevues_Enter(sender, e);
        }

        /// <summary>
        /// Supprimer un exemplaire revue dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerExemplairesRevue_Click(object sender, EventArgs e)
        {
            ExemplaireDetail exemplairesDet = (ExemplaireDetail)bdgExemplairesRevueListe.List[bdgExemplairesRevueListe.Position];
            int numero = exemplairesDet.Numero;
            DateTime dateAchat = exemplairesDet.DateAchat;
            string photo = exemplairesDet.Photo;
            string idEtat = txbCheckIdEtatExRevue.Text;
            string id = txbRevuesNumero.Text;
            Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);
            if (exemplaire != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de l'exemplaire n°" + exemplaire.Numero + " ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerExemplaire(exemplaire);
                        MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " annulée");
                    }

                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de cet exemplaire, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucun exemplaire sélectionné");
            }
            RestaurationConfigRevues();
            TabRevues_Enter(sender, e);
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// Vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Vide la totalité des informations revues
        /// Désactive la synchro datagrid
        /// </summary>
        private void VideRevuesInfosTotal()
        {
            VideRevuesInfos();
            VideRevuesZones();
            dgvRevuesListe.Rows.Clear();
        }

        /// <summary>
        /// Restaure l'onglet Revues à son état initial
        /// </summary>
        private void RestaurationConfigRevues()
        {
            // Activer le lien avec le grid
            DefaultRevue = true;
            // Liste des contrôles à masquer
            Control[] hide = { txbRevuesIdGenre, txbRevuesIdPublic, txbRevuesIdRayon,
                lblRevueIdGenre, lblRevueIdPublic, lblRevueIdRayon,
                cbxRevuesGenreEdit, cbxRevuesPublicEdit, cbxRevuesRayonEdit,
                btnAnnulerRevue, btnValiderAjouterRevue, btnValiderModifierRevue,
                btnAnnulerExemplairesRevue, btnModifierExemplairesRevueOk
            };
            // Masquer tous les contrôles de la liste
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterRevue, btnModifierRevue, btnSupprimerRevue,
                btnModifierExemplairesRevue, btnSupprimerExemplairesRevue,
                cbxRevuesGenres, cbxRevuesPublics, cbxRevuesRayons
            };
            // Afficher tous les contrôles de la liste
            foreach (Control control in show)
            {
                control.Show();
            }
            DesactiverChampsInfosRevues();
        }

        /// <summary>
        /// Change l'état des champs informations détaillées
        /// Afin de pas pouvoir les éditer
        /// </summary>
        private void SetReadOnlyRevues(bool isReadOnly)
        {
            txbRevuesNumero.ReadOnly = isReadOnly;
            txbRevuesTitre.ReadOnly = isReadOnly;
            txbRevuesGenre.ReadOnly = isReadOnly;
            txbRevuesPublic.ReadOnly = isReadOnly;
            txbRevuesRayon.ReadOnly = isReadOnly;
            txbRevuesImage.ReadOnly = isReadOnly;
            txbRevuesDateMiseADispo.ReadOnly = isReadOnly;
            txbRevuesPeriodicite.ReadOnly = isReadOnly;
        }

        /// <summary>
        /// Désactive l'accès aux champs revue
        /// </summary>
        private void DesactiverChampsInfosRevues()
        {
            SetReadOnlyRevues(true);
        }

        /// <summary>
        /// Active l'accès aux champs revue
        /// </summary>
        private void ActiverChampsInfosRevues()
        {
            SetReadOnlyRevues(false);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion côté revues
        /// </summary>
        /// <param name="acces">True ou False</param>
        private void AccesInformationsRevuesGroupBox(bool acces)
        {
            grpRevuesInfos.Enabled = acces;

        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Périodicité":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Parutions
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabReceptionRevue_Enter(object sender, EventArgs e)
        {
            if (DefaultParution)
            {
                lesRevues = controller.GetAllRevues();
                RemplirComboEtat(controller.GetAllEtats(), bdgEtats, cbxParutionsEtat);
                bdgExemplairesListe.DataSource = null;
                cbxParutionsEtat.Enabled = false;
                cbxParutionsEtat.SelectedIndex = -1;
                txbReceptionRevueNumero.Text = "";
                txbLibelleEtatParution.Text = "";

            }
        }

        /// <summary>
        /// Remplit le datagrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesDetailsExemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<ExemplaireDetail> lesDetailsExemplaires)
        {
            if (lesDetailsExemplaires != null)
            {
                bdgExemplairesListe.DataSource = lesDetailsExemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["IdEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["Id"].Visible = false;
                dgvReceptionExemplairesListe.Columns["Photo"].Visible = false;
                dgvReceptionExemplairesListe.Columns["DateAchat"].HeaderText = "Date d'achat";
                dgvReceptionExemplairesListe.Columns["LibelleEtat"].HeaderText = "État";
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvReceptionExemplairesListe.Columns["Numéro"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["DateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue sont aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocument = txbReceptionRevueNumero.Text;
            lesDetailsExemplairesDocument = controller.GetExemplaireDetailsDocument(idDocument);
            RemplirReceptionExemplairesListe(lesDetailsExemplairesDocument);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = System.Drawing.Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("Numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("Le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<ExemplaireDetail> sortedList = new List<ExemplaireDetail>();
            switch (titreColonne)
            {
                case "Numéro":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "Date d'achat":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "État":
                    sortedList = lesDetailsExemplairesDocument.OrderBy(o => o.LibelleEtat).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// Affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                ExemplaireDetail exemplaireDetail = (ExemplaireDetail)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                txbLibelleEtatParution.Text = exemplaireDetail.LibelleEtat;
                string image = exemplaireDetail.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = System.Drawing.Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        /// <summary>
        /// Récupère l'idEtat en fonction de l'exemplaire select  en cbx
        /// Ainsi qu'un check dev pour l'id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxParutionsEtat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxParutionsEtat.SelectedIndex >= 0)
            {
                Etat etat = (Etat)cbxParutionsEtat.SelectedItem;
                cbxParutionsEtat.Text = etat.Id;
                txbParutionsEtatCheckId.Text = cbxParutionsEtat.Text;
            }
        }

        /// <summary>
        /// Active les champs nécessaire à la modification de l'état d'une parution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierParutions_Click(object sender, EventArgs e)
        {
            // Liste des contrôles à afficher
            Control[] show = { btnModifierParutionOk, btnAnnulerParution };
            foreach (Control control in show)
            {
                control.Show();
            }
            // Liste des contrôles à masquer
            Control[] hide = { btnModifierParution, btnSupprimerParution };
            foreach (Control control in hide)
            {
                control.Hide();
            }
            cbxParutionsEtat.Enabled = true;
        }

        /// <summary>
        /// Valide ou non la modification de l'état d'une parution dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierParutionsOk_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Equals("") && cbxParutionsEtat.SelectedIndex >= 0)
            {
                try
                {
                    ExemplaireDetail exemplairesDet = (ExemplaireDetail)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                    int numero = exemplairesDet.Numero;
                    DateTime dateAchat = exemplairesDet.DateAchat;
                    string photo = exemplairesDet.Photo;
                    string idEtat = txbParutionsEtatCheckId.Text;
                    string id = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de l'état de la parution " + exemplaire.Numero + " ?", "Information",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierExemplaire(exemplaire);
                        MessageBox.Show("Modification de l'état de la parution n°" + exemplaire.Numero + " réussie", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Modification de l'état de la parution n°" + exemplaire.Numero + " annulée", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une parution", "Information");
            }
            RestaurationConfigParutions();
            TabReceptionRevue_Enter(sender, e);
        }

        /// <summary>
        /// Annule la modification d'une parution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerParution_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show("Annulation de la modification en cours", "Information");
                RestaurationConfigParutions();
                TabReceptionRevue_Enter(sender, e);
            }
        }

        /// <summary>
        /// Supprimer une parution dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerParution_Click(object sender, EventArgs e)
        {
            ExemplaireDetail exemplairesDet = (ExemplaireDetail)bdgExemplairesListe.List[bdgExemplairesListe.Position];
            int numero = exemplairesDet.Numero;
            DateTime dateAchat = exemplairesDet.DateAchat;
            string photo = exemplairesDet.Photo;
            string idEtat = txbParutionsEtatCheckId.Text;
            string id = txbReceptionRevueNumero.Text;
            Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, id);
            if (exemplaire != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de la parution n°" + exemplaire.Numero + " ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerExemplaire(exemplaire);
                        MessageBox.Show("Suppression de la parution n°" + exemplaire.Numero + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression de la parution n°" + exemplaire.Numero + " annulée");
                    }

                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de cette parution, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucune parution sélectionnée");
            }
            RestaurationConfigParutions();
            TabReceptionRevue_Enter(sender, e);
        }

        /// <summary>
        /// Restaurer l'onglet Parutions des revues à son état initial
        /// </summary>
        private void RestaurationConfigParutions()
        {
            // Activer le lien avec le grid
            DefaultParution = true;
            // Liste des contrôles à masquer
            btnModifierParutionOk.Visible = false;
            btnAnnulerParution.Visible = false;
            // Liste des contrôles à afficher
            btnModifierParution.Visible = true;
            btnSupprimerParution.Visible = true;
        }
        #endregion

        #region Onglet Commandes Livres

        private string recupSuivi;
        /// <summary>
        /// Ouverture de l'onglet Commandes de Livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandesLivres_Enter(object sender, EventArgs e)
        {
            if (DefaultCmdLivre)
            {
                lesLivres = controller.GetAllLivres();
                RemplirComboSuivi(controller.GetAllSuivis(), bdgSuivis, cbxCmdLivreSuivi);
                dgvCmdLivreListe.DataSource = null;
                ViderCmdLivreInfosTotal();
            }
        }

        /// <summary>
        /// Remplit le datagrid commande livre avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesDocument ">liste des livres commandés</param>
        private void RemplirCmdLivreListe(List<CommandeDocument> lesCommandesDocument)
        {
            bdgCommandesLivres.DataSource = lesCommandesDocument;
            dgvCmdLivreListe.DataSource = bdgCommandesLivres;
            dgvCmdLivreListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCmdLivreListe.Columns["IdLivreDvd"].Visible = false;
            dgvCmdLivreListe.Columns["IdSuivi"].Visible = false;
            dgvCmdLivreListe.Columns["Id"].Visible = false;
            dgvCmdLivreListe.Columns["DateCommande"].DisplayIndex = 0;
            dgvCmdLivreListe.Columns["DateCommande"].HeaderText = "Date commande";
            dgvCmdLivreListe.Columns["Montant"].DisplayIndex = 1;
            dgvCmdLivreListe.Columns["NbExemplaire"].DisplayIndex = 2;
            dgvCmdLivreListe.Columns["NbExemplaire"].HeaderText = "Nb exemplaires";
            dgvCmdLivreListe.Columns["LibelleSuivi"].HeaderText = "Suivi";
        }

        /// <summary>
        /// Recherche et affiche les informations du livre renseigné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreNumRecherche_Click(object sender, EventArgs e)
        {
            if (DefaultCmdLivre && !txbCmdLivreNumRecherche.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbCmdLivreNumRecherche.Text.ToString()));
                if (livre != null)
                {
                    AfficherCmdLivreInfos(livre);
                    RemplirCmdLivreListe(lesCommandesDocuments);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer un numéro de livre valide");
                TabCommandesLivres_Enter(sender, e);
            }
        }

        /// <summary>
        /// Affiche les informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficherCmdLivreInfos(Livre livre)
        {
            txbCmdLivreIdLivreDvd.Text = livre.Id;
            txbCmdLivreIsbn.Text = livre.Isbn;
            txbCmdLivreTitre.Text = livre.Titre;
            txbCmdLivreAuteur.Text = livre.Auteur;
            txbCmdLivreCollection.Text = livre.Collection;
            txbCmdLivreGenre.Text = livre.Genre;
            txbCmdLivrePublic.Text = livre.Public;
            txbCmdLivreRayon.Text = livre.Rayon;
            string image = livre.Image;
            try
            {
                pcbCmdLivre.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbCmdLivre.Image = null;
            }
            AfficherCmdLivreInfoCmdGrid();
        }

        /// <summary>
        /// Récupère et affiche les infos de commande du livre dans le datagrid
        /// </summary>
        public void AfficherCmdLivreInfoCmdGrid()
        {
            string idDocument = txbCmdLivreNumRecherche.Text;
            lesCommandesDocuments = controller.GetCommandesDocument(idDocument);
            RemplirCmdLivreListe(lesCommandesDocuments);
        }

        /// <summary>
        /// Récupère les informations de commande d'un livre
        /// Et initialise les éléments correspondants
        /// </summary>
        /// <param name="commandeDocument"></param>
        private void AfficherCmdLivreInfosCmd(CommandeDocument commandeDocument)
        {
            txbCmdLivreIdCmd.Text = commandeDocument.Id;
            cbxCmdLivreSuivi.Text = commandeDocument.LibelleSuivi;
            txbCmdLivreNbExemplaires.Text = commandeDocument.NbExemplaire.ToString();
            txbCmdLivreMontant.Text = commandeDocument.Montant.ToString();
            dtpCmdLivre.Value = commandeDocument.DateCommande;
        }

        /// <summary>
        /// Affiche le détail de la commande livre en fonction de sa position datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdLivreListe_SelectionChanged(object sender, EventArgs e)
        {
            if (DefaultCmdLivre && dgvCmdLivreListe.CurrentCell != null)
            {
                try
                {
                    CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesLivres.List[bdgCommandesLivres.Position];
                    AfficherCmdLivreInfosCmd(commandeDocument);
                }
                catch
                {
                    ViderCmdLivreInfosTotal();
                }
            }
            else
            {
                ViderCmdLivreInfosTotal();
            }
        }
        /// <summary>
        /// Vide les zones d'affichage des informations du Livre
        /// </summary>
        private void ViderCmdLivreInfos()
        {
            txbCmdLivreAuteur.Text = "";
            txbCmdLivreCollection.Text = "";
            txbCmdLivreTitre.Text = "";
            txbCmdLivreIsbn.Text = "";
            txbCmdLivreIdLivreDvd.Text = "";
            txbCmdLivreGenre.Text = "";
            txbCmdLivrePublic.Text = "";
            txbCmdLivreRayon.Text = "";
            txbCmdLivreNumRecherche.Text = "";
            pcbCmdLivre.Image = null;
        }

        /// <summary>
        /// Vide les zones d'affichage des infos de commande du livre sélectionné
        /// </summary>
        private void ViderCmdLivreInfosCmd()
        {
            txbCmdLivreIdCmd.Text = "";
            txbCmdLivreNbExemplaires.Text = "";
            txbCmdLivreMontant.Text = "";
            cbxCmdLivreSuivi.SelectedIndex = -1;
            txbCmdLivreNumRecherche.Text = "";
            dtpCmdLivre.Value = DateTime.Now;

        }

        /// <summary>
        /// Vide la totalité des infos commande et livre
        /// </summary>
        private void ViderCmdLivreInfosTotal()
        {
            ViderCmdLivreInfos();
            ViderCmdLivreInfosCmd();
            DesactiverChampsInfosCmdLivre();
            txbCmdLivreNumRecherche.Text = "";
            cbxCmdLivreSuivi.Enabled = false;
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion des commandes livres    
        /// </summary>
        /// <param name="access">true ou false</param>
        private void AccesInfosCmdLivreGrpBox(bool access)
        {
            grpCmdLivreInfos.Enabled = access;
        }

        /// <summary>
        /// Change l'état des champs informations détaillées commande d'un livre
        /// </summary>
        private void SetReadOnlyCmdLivre(bool isReadOnly)
        {
            txbCmdLivreMontant.ReadOnly = isReadOnly;
            txbCmdLivreNbExemplaires.ReadOnly = isReadOnly;
            txbCmdLivreIdSuivi.ReadOnly = isReadOnly;
            txbCmdLivreIdCmd.ReadOnly = isReadOnly;
        }
        
        /// <summary>
        /// Activer les champs commande livre
        /// </summary>
        private void ActiverChampsInfosCmdLivre()
        {
            SetReadOnlyCmdLivre(false);
            dtpCmdLivre.Enabled = true;
        }

        /// <summary>
        /// Désactive les champs commande livre
        /// </summary>
        private void DesactiverChampsInfosCmdLivre()
        {
            SetReadOnlyCmdLivre(true);
            dtpCmdLivre.Enabled = false;
        }

        /// <summary>
        /// Récupère l'id du suivi en fonction de la sélection du cbx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxCmdLivreLibelleSuivi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdLivreSuivi.SelectedIndex >= 0)
            {
                Suivi suivi = (Suivi)cbxCmdLivreSuivi.SelectedItem;
                txbCmdLivreIdSuivi.Text = suivi.Id;
            }
        }

        /// <summary>
        /// Restaure les paramètres Commandes Livres à son état initial
        /// </summary>
        public void RestaureConfigCmdLivres()
        {
            // Activer le lien avec le grid
            DefaultCmdLivre = true;
            dgvCmdLivreListe.Enabled = true;

            // Désactive l'accès à la gestion commande et le vide
            DesactiverChampsInfosCmdLivre();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdLivreCmd, btnCmdLivreModifier, btnCmdLivreSupprimer };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide = { btnCmdLivreCmdOk, btnCmdLivreModifierOk, btnCmdLivreAnnuler };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Activer les champs nécessaires à la création d'une commande livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreCmd_Click(object sender, EventArgs e)
        {
            // Désactive le datagrid
            DefaultCmdLivre = false;
            dgvCmdLivreListe.Enabled = false;

            // Active l'accès à la gestion de la commande
            AccesInfosCmdLivreGrpBox(true);
            ActiverChampsInfosCmdLivre();
            ViderCmdLivreInfosCmd();
            cbxCmdLivreSuivi.SelectedIndex = 0;

            // Liste des contrôles à masquer
            Control[] hide = { btnCmdLivreCmd, btnCmdLivreModifier, btnCmdLivreSupprimer };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = { btnCmdLivreCmdOk, btnCmdLivreAnnuler };
            foreach (Control control in show) { control.Show(); }
        }

        /// <summary>
        /// Ajout d'une commande d'un livre en BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreCmdOk_Click(object sender, EventArgs e)
        {
            if (!txbCmdLivreNumRecherche.Equals(""))
            {
                try
                {
                    string id = txbCmdLivreIdCmd.Text;
                    DateTime dateCommande = dtpCmdLivre.Value;
                    double montant = double.Parse(txbCmdLivreMontant.Text);
                    int nbExemplaire = int.Parse(txbCmdLivreNbExemplaires.Text);
                    string idSuivi = txbCmdLivreIdSuivi.Text;
                    string libelleSuivi = cbxCmdLivreSuivi.SelectedItem.ToString();
                    string idLivreDvd = txbCmdLivreIdLivreDvd.Text;
                    CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelleSuivi);
                    Console.WriteLine(commandeDocument);
                    if (controller.CreerCommandeDocument(commandeDocument))
                    {
                        MessageBox.Show("Commande " + id + "effectuée", "INFORMATION");
                        AfficherCmdLivreInfosCmd(commandeDocument);
                    }
                    else
                    {
                        MessageBox.Show("Commande déjà existante", "ERREUR");
                    }
                }
                catch
                {
                    MessageBox.Show("Vérifier que tous les champs sont correctement renseignés", "INFORMATION");
                }
            }
            else
            {
                MessageBox.Show("Aucun livre sélectionné");
            }
            RestaureConfigCmdLivres();
            TabCommandesLivres_Enter(sender, e);
        }

        /// <summary>
        /// Annulation de l'action en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreAnnuler_Click(object sender, EventArgs e)
        {
            RestaureConfigCmdLivres();
            TabCommandesLivres_Enter(sender, e);
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'une commande livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreModifier_Click(object sender, EventArgs e)
        {
            // Désactive le datagrid
            DefaultCmdLivre = false;

            // Activer l'accès à la gestion de la commande
            AccesInfosCmdLivreGrpBox(true);
            ActiverChampsInfosCmdLivre();

            // Active la modification statut commande + récupère item
            cbxCmdLivreSuivi.Enabled = true;
            recupSuivi = cbxCmdLivreSuivi.SelectedItem.ToString();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdLivreModifierOk, btnCmdLivreAnnuler };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à cacher
            Control[] hide = { btnCmdLivreCmd, btnCmdLivreModifier, btnCmdLivreSupprimer };
            foreach (Control control in hide) { control.Hide(); }

        }

        /// <summary>
        /// Valide ou non la modification d'une commande livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        private void BtnCmdLivreModifierOk_Click(object sender, EventArgs e)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            if (!txbCmdLivreIdCmd.Equals(""))
            {
                try
                {
                    string id = txbCmdLivreIdCmd.Text;
                    DateTime dateCommande = dtpCmdLivre.Value;
                    double montant = double.Parse(txbCmdLivreMontant.Text);
                    int nbExemplaire = int.Parse(txbCmdLivreNbExemplaires.Text);
                    string idSuivi = txbCmdLivreIdSuivi.Text;
                    string libelleSuivi = cbxCmdLivreSuivi.SelectedItem.ToString();
                    string idLivreDvd = txbCmdLivreIdLivreDvd.Text;
                    CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelleSuivi);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de cette commande ?", "INFORMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (recupSuivi.Equals("réglée") || recupSuivi.Equals("livrée"))
                        {
                            if (libelleSuivi.Equals("en cours") || libelleSuivi.Equals("relancée"))
                            {
                                MessageBox.Show("Impossible de rétrograder une commande déjà " + recupSuivi);
                            }
                            else
                            {
                                controller.ModifierCommandeDocument(commandeDocument);
                                MessageBox.Show("Modification de la commande effectuée");
                            }
                        }
                        else if (recupSuivi.Equals("en cours") || recupSuivi.Equals("relancée"))
                        {
                            if (libelleSuivi.Equals("réglée"))
                            {
                                MessageBox.Show("Une commande ne peut être réglée avant sa livraison");
                            }
                            else
                            {
                                controller.ModifierCommandeDocument(commandeDocument);
                                MessageBox.Show("Modification de la commande effectuée");
                            }
                        }
                        else
                        {
                            controller.ModifierCommandeDocument(commandeDocument);
                            MessageBox.Show("Modification de la commande effectuée");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification de cette commande");
                    }
                }
                catch
                {
                    MessageBox.Show("Veuillez sélectionner une commande valide", "Erreur");
                }

            }
            else
            {
                MessageBox.Show("Merci de renseigner un numéro de commande valide", "Erreur");
            }
            RestaureConfigCmdLivres();
            TabCommandesLivres_Enter(sender, e);
        }

        /// <summary>
        /// Supprimer une commande livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreSupprimer_Click(object sender, EventArgs e)
        {
            CommandeDocument commandeLivre = (CommandeDocument)bdgCommandesLivres.List[bdgCommandesLivres.Position];
            if (commandeLivre != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de cette commande ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (commandeLivre.IdSuivi != "00004")
                        {
                            controller.SupprimerCommandeDocument(commandeLivre);
                            MessageBox.Show("Suppression effectuée");

                        }
                        else
                        {
                            MessageBox.Show("Impossible de supprimer une commande déjà livrée", "ERREUR");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression annulée");
                    }

                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de cette commande, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucune commande sélectionnée");
            }
            RestaureConfigCmdLivres();
            TabCommandesLivres_Enter(sender, e);
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdLivreListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvCmdLivreListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Date commande":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.Montant).ToList();
                    break;
                case "Nb exemplaires":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }
            RemplirCmdLivreListe(sortedList);
        }
        #endregion

        #region Onglet Commandes DVD

        /// <summary>
        /// Ouverte de l'onglet Commandes DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandesDvd_Enter(object sender, EventArgs e)
        {
            if (DefaultCmdDvd)
            {
                lesDvd = controller.GetAllDvd();
                RemplirComboSuivi(controller.GetAllSuivis(), bdgSuivis, cbxCmdDvdSuivi);
                dgvCmdDvdListe.DataSource = null;
                ViderCmdDvdInfosTotal();
            }
        }

        /// <summary>
        /// Remplit le datagrid commande DVD avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesDvd">liste des DVD commandés</param>
        private void RemplirCmdDvdListe(List<CommandeDocument> lesCommandesDvd)
        {
            bdgCommandesDvd.DataSource = lesCommandesDvd;
            dgvCmdDvdListe.DataSource = bdgCommandesDvd;
            dgvCmdDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCmdDvdListe.Columns["IdLivreDvd"].Visible = false;
            dgvCmdDvdListe.Columns["IdSuivi"].Visible = false;
            dgvCmdDvdListe.Columns["DateCommande"].DisplayIndex = 0;
            dgvCmdDvdListe.Columns["DateCommande"].HeaderText = "Date commande";
            dgvCmdDvdListe.Columns["Montant"].DisplayIndex = 1;
            dgvCmdDvdListe.Columns["NbExemplaire"].DisplayIndex = 2;
            dgvCmdDvdListe.Columns["NbExemplaire"].HeaderText = "Nb exemplaires";
            dgvCmdDvdListe.Columns["LibelleSuivi"].HeaderText = "Suivi";
        }

        /// <summary>
        /// Recherche et affiche les infos du livre renseigné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdRechercheNum_Click(object sender, EventArgs e)
        {
            if (DefaultCmdDvd && !txbCmdDvdRechercheNum.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbCmdDvdRechercheNum.Text.ToString()));
                if (dvd != null)
                {
                    AfficherCmdDvdInfos(dvd);
                    RemplirCmdDvdListe(lesCommandesDocuments);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer un numéro de livre valide");
                TabCommandesDvd_Enter(sender, e);
            }
        }

        /// <summary>
        /// Affiches les informations du DVD sélectionné
        /// </summary>
        /// <param name="dvd">le DVD</param>
        private void AfficherCmdDvdInfos(Dvd dvd)
        {
            txbCmdDvdIdLivreDvd.Text = dvd.Id;
            txbCmdDvdDuree.Text = dvd.Duree.ToString();
            txbCmdDvdTitre.Text = dvd.Titre;
            txbCmdDvdReal.Text = dvd.Realisateur;
            txbCmdDvdSynopsis.Text = dvd.Synopsis;
            txbCmdDvdGenre.Text = dvd.Genre;
            txbCmdDvdPublic.Text = dvd.Public;
            txbCmdDvdRayon.Text = dvd.Rayon;
            string image = dvd.Image;
            try
            {
                pcbCmdDvd.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbCmdDvd.Image = null;
            }
            AfficherCmdDvdInfosCmdGrid();
        }

        /// <summary>
        /// Récupère et affiche les infos de commande du DVD dans le datagrid
        /// </summary>
        public void AfficherCmdDvdInfosCmdGrid()
        {
            string idDocument = txbCmdDvdRechercheNum.Text;
            lesCommandesDocuments = controller.GetCommandesDocument(idDocument);
            RemplirCmdDvdListe(lesCommandesDocuments);
        }

        /// <summary>
        /// Affiche les informations liées à la commande d'un DVD
        /// </summary>
        /// <param name="commandeDocument"></param>
        private void AfficherCmdDvdInfosCmd(CommandeDocument commandeDocument)
        {
            txbCmdDvdIdCmd.Text = commandeDocument.Id;
            cbxCmdDvdSuivi.Text = commandeDocument.LibelleSuivi;
            txbCmdDvdNbExemplaires.Text = commandeDocument.NbExemplaire.ToString();
            txbCmdDvdMontant.Text = commandeDocument.Montant.ToString();
            dtpCmdDvd.Value = commandeDocument.DateCommande;
        }

        /// <summary>
        /// Affiche le détail de la commande DVD en fonction de sa position datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (DefaultCmdDvd && dgvCmdDvdListe.CurrentCell != null)
            {
                try
                {
                    CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesDvd.List[bdgCommandesDvd.Position];
                    AfficherCmdDvdInfosCmd(commandeDocument);
                }
                catch
                {
                    ViderCmdDvdInfosTotal();
                }
            }
            else
            {
                ViderCmdDvdInfosTotal();
            }
        }

        /// <summary>
        /// Vide les zones d'affiches des infos du DVD
        /// </summary>
        private void ViderCmdDvdInfos()
        {
            txbCmdDvdIdLivreDvd.Text = "";
            txbCmdDvdDuree.Text = "";
            txbCmdDvdTitre.Text = "";
            txbCmdDvdReal.Text = "";
            txbCmdDvdSynopsis.Text = "";
            txbCmdDvdGenre.Text = "";
            txbCmdDvdPublic.Text = "";
            txbCmdDvdRayon.Text = "";
        }

        /// <summary>
        /// Vide les zones d'affichage des infos de commande du DVD 
        /// </summary>
        private void ViderCmdDvdInfosCmd()
        {
            txbCmdDvdNbExemplaires.Text = "";
            txbCmdDvdMontant.Text = "";
            cbxCmdDvdSuivi.Text = "";
            txbCmdDvdIdCmd.Text = "";
            dtpCmdDvd.Value = DateTime.Now;
        }

        /// <summary>
        /// Vide la totalité des informations commande et DVD
        /// </summary>
        private void ViderCmdDvdInfosTotal()
        {
            ViderCmdDvdInfos();
            ViderCmdDvdInfosCmd();
            DesactiverChampsInfosCmdDvd();
            txbCmdDvdRechercheNum.Text = "";
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion des commandes DVD
        /// </summary>
        /// <param name="access"></param>
        private void AccesInfosCmdDvdGrpBox(bool access)
        {
            grpCmdDvdCmdInfos.Enabled = access;
        }

        /// <summary>
        /// Change l'état des champs informations détaillées commande d'un DVD
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void SetReadOnlyCmdDvd(bool isReadOnly)
        {
            txbCmdDvdMontant.ReadOnly = isReadOnly;
            txbCmdDvdNbExemplaires.ReadOnly = isReadOnly;
            txbCmdDvdIdSuivi.ReadOnly = isReadOnly;
            txbCmdDvdIdCmd.ReadOnly = isReadOnly;
        }
        /// <summary>
        /// Active les champs commande DVD
        /// </summary>
        private void ActiverChampsInfosCmdDvd()
        {
            SetReadOnlyCmdDvd(false);
            dtpCmdDvd.Enabled = true;
        }

        /// <summary>
        /// Désactive les champs commande DVD
        /// </summary>
        private void DesactiverChampsInfosCmdDvd()
        {
            SetReadOnlyCmdDvd(true);
            dtpCmdDvd.Enabled = false;
        }

        /// <summary>
        /// Récupère l'id du suivi en fonction de la sélection du cbx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxCmdDvdSuivi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdDvdSuivi.SelectedIndex >= 0)
            {
                Suivi suivi = (Suivi)cbxCmdDvdSuivi.SelectedItem;
                txbCmdDvdIdSuivi.Text = suivi.Id;
            }
        }

        /// <summary>
        /// Restaure les paramètres Commandes DVD à son état initial
        /// </summary>
        public void RestaureConfigCmdDvd()
        {
            // Activer le lien avec le grid
            DefaultCmdDvd = true;
            dgvCmdDvdListe.Enabled = true;

            // Désactive l'accès à la gestion commande et le vide
            DesactiverChampsInfosCmdDvd();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdDvdCommander, btnCmdDvdModifier, btnCmdDvdSupprimer };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide = { btnCmdDvdCmdOk, btnCmdDvdModifierOk, btnCmdDvdAnnuler };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Active les champs nécessaires à la création d'une commande DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdCmd_Click(object sender, EventArgs e)
        {
            // Désactive le datagrid
            DefaultCmdDvd = false;
            dgvCmdDvdListe.Enabled = false;

            // Active l'accès à la gestion commande
            AccesInfosCmdDvdGrpBox(true);
            ActiverChampsInfosCmdDvd();
            ViderCmdDvdInfosCmd();
            cbxCmdDvdSuivi.SelectedIndex = 0;

            // Liste des contrôles à masquer 
            Control[] hide = { btnCmdDvdCommander, btnCmdDvdModifier, btnCmdDvdSupprimer };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = { btnCmdDvdCmdOk, btnCmdDvdAnnuler };
            foreach (Control control in show) { control.Show(); }

        }

        /// <summary>
        /// Ajout d'une commande d'un DVD en BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdCmdOK_Click(object sender, EventArgs e)
        {
            if (!txbCmdDvdRechercheNum.Text.Equals(""))
            {
                try
                {
                    string id = txbCmdDvdIdCmd.Text;
                    DateTime dateCommande = dtpCmdDvd.Value;
                    double montant = double.Parse(txbCmdDvdMontant.Text);
                    int nbExemplaires = int.Parse(txbCmdDvdNbExemplaires.Text);
                    string idSuivi = txbCmdDvdIdSuivi.Text;
                    string libelleSuivi = cbxCmdDvdSuivi.SelectedItem.ToString();
                    string idLivreDvd = txbCmdDvdIdLivreDvd.Text;
                    CommandeDocument commandeDvd = new CommandeDocument(id, dateCommande, montant, nbExemplaires, idLivreDvd, idSuivi, libelleSuivi);
                    if (controller.CreerCommandeDocument(commandeDvd))
                    {
                        MessageBox.Show("Commande effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Commande déjà existante", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("Vérifier que tous les champs sont correctement renseignés", "Information");
                }
            }
            else
            {
                MessageBox.Show("Aucun DVD sélectionné");
            }
            RestaureConfigCmdDvd();
            TabCommandesDvd_Enter(sender, e);
        }

        /// <summary>
        /// Annulation de l'action en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdAnnuler_Click(object sender, EventArgs e)
        {
            RestaureConfigCmdDvd();
            TabCommandesDvd_Enter(sender, e);
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'une commande DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdModifierDvd_Click(object sender, EventArgs e)
        {
            // Désactive le datagrid
            DefaultCmdDvd = false;
            // Active l'accès à la gestion commande
            AccesInfosCmdDvdGrpBox(true);
            ActiverChampsInfosCmdDvd();
            // Active la possibilité de modifier le statut de la commande
            cbxCmdDvdSuivi.Enabled = true;
            // Liste des contrôles à masquer 
            Control[] hide = { btnCmdDvdCommander, btnCmdDvdModifier, btnCmdDvdSupprimer };
            foreach (Control control in hide) { control.Hide(); }
            // Liste des contrôles à afficher
            Control[] show = { btnCmdDvdModifierOk, btnCmdDvdAnnuler };
            foreach (Control control in show) { control.Show(); }
        }

        /// <summary>
        /// Valide ou non la modification d'une commande DVD dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdModifierOk_Click(object sender, EventArgs e)
        {
            if (!txbCmdDvdIdCmd.Equals(""))
            {
                try
                {
                    string id = txbCmdDvdIdCmd.Text;
                    DateTime dateCommande = dtpCmdDvd.Value;
                    double montant = double.Parse(txbCmdDvdMontant.Text);
                    int nbExemplaire = int.Parse(txbCmdDvdNbExemplaires.Text);
                    string idSuivi = txbCmdDvdIdSuivi.Text;
                    string libelleSuivi = cbxCmdDvdSuivi.SelectedItem.ToString();
                    string idLivreDvd = txbCmdDvdIdLivreDvd.Text;
                    CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelleSuivi);
                    if (MessageBox.Show(this, "Confirmez-vous la modification cette commande ?", "INFORMATION",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierCommandeDocument(commandeDocument);
                        MessageBox.Show("Modification de la commande effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification de cette commande");
                    }
                }
                catch
                {
                    MessageBox.Show("Veuillez sélectionner une commande valide", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Merci de renseigner un numéro de commande valide", "Erreur");
            }
            RestaureConfigCmdDvd();
            TabCommandesDvd_Enter(sender, e);
        }

        /// <summary>
        /// Supprimer une commande DVD dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdSupprimer_Click(object sender, EventArgs e)
        {
            Commande commandeDocument = (Commande)bdgCommandesDvd.List[bdgCommandesDvd.Position];
            if (commandeDocument != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de cette commande ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerCommandeDocument(commandeDocument);
                        MessageBox.Show("Suppression effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression annulée");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de cette commande, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Aucune commande sélectionnée");
            }
            RestaureConfigCmdDvd();
            TabCommandesDvd_Enter(sender, e);
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCmdDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Date commande":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.Montant).ToList();
                    break;
                case "Nb exemplaires":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }
            RemplirCmdDvdListe(sortedList);
        }
        #endregion

        #region Onglet Commandes Revues

        /// <summary>
        /// Ouverture de l'onglet Commandes Revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandesRevues_Enter(object sender, EventArgs e)
        {
            if (DefaultCmdRevue)
            {
                lesRevues = controller.GetAllRevues();
                dgvCmdRevueListe.DataSource = null;
                ViderCmdRevueInfosTotal();
            }
        }

        /// <summary>
        /// Remplit le datagrid commande revue avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesAbosRevues"></param>
        private void RemplirCmdRevueListe(List<Abonnement> lesAbosRevues)
        {
            bdgCommandesRevues.DataSource = lesAbosRevues;
            dgvCmdRevueListe.DataSource = bdgCommandesRevues;
            dgvCmdRevueListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCmdRevueListe.Columns["DateCommande"].HeaderText = "Date de la commande";
            dgvCmdRevueListe.Columns["DateCommande"].DisplayIndex = 0;
            dgvCmdRevueListe.Columns["Montant"].DisplayIndex = 1;
            dgvCmdRevueListe.Columns["DateFinAbonnement"].HeaderText = "Date de fin d'abonnement";
            dgvCmdRevueListe.Columns["Id"].Visible = false;
            dgvCmdRevueListe.Columns["IdRevue"].Visible = false;
        }

        /// <summary>
        /// Recherche et affiche les infos de la revue renseignée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueNumRecherche_Click(object sender, EventArgs e)
        {
            if (DefaultCmdRevue && !txbCmdRevueNumRecherche.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbCmdRevueNumRecherche.Text.ToString()));
                if (revue != null)
                {
                    AfficherCmdRevueInfos(revue);
                    RemplirCmdRevueListe(lesAbonnementsRevues);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer un numéro de revue valide");
                TabCommandesRevues_Enter(sender, e);
            }
        }

        /// <summary>
        /// Affiche les infos de la revue sélectionnée
        /// </summary>
        /// <param name="revue"></param>
        private void AfficherCmdRevueInfos(Revue revue)
        {
            txbCmdRevueIdRevue.Text = revue.Id;
            txbCmdRevueTitre.Text = revue.Titre;
            txbCmdRevuePeriode.Text = revue.Periodicite;
            txbCmdRevueMaD.Text = revue.DelaiMiseADispo.ToString();
            txbCmdRevueGenre.Text = revue.Genre;
            txbCmdRevuePublic.Text = revue.Public;
            txbCmdRevueRayon.Text = revue.Rayon;
            string image = revue.Image;
            try
            {
                pcbCmdRevue.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbCmdRevue.Image = null;
            }
            AfficherCmdRevueInfosCmdGrid();
        }

        /// <summary>
        /// Récupère et affiche les infos de commande de la revue dans le datagrid
        /// </summary>
        private void AfficherCmdRevueInfosCmdGrid()
        {
            string idRevue = txbCmdRevueNumRecherche.Text;
            lesAbonnementsRevues = controller.GetAbonnementsRevue(idRevue);
            RemplirCmdRevueListe(lesAbonnementsRevues);
        }

        /// <summary>
        /// Récupère les informations de commande d'une revue
        /// Et initialise les éléments correspondants
        /// </summary>
        /// <param name="abonnement"></param>

        private void AfficherCmdRevueInfosCmd(Abonnement abonnement)
        {
            txbCmdRevueIdCmd.Text = abonnement.Id;
            txbCmdRevueMontant.Text = abonnement.Montant.ToString();
            dtpCmdRevueDateCmd.Value = abonnement.DateCommande;
            dtpCmdRevueFinAbo.Value = abonnement.DateFinAbonnement;
        }

        /// <summary>
        /// Affiche le détail de la commande revue en fonction de sa position datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdRevueListe_SelectionChanged(object sender, EventArgs e)
        {
            if (DefaultCmdRevue && dgvCmdRevueListe.CurrentCell != null)
            {
                try
                {
                    Abonnement abonnementRevue = (Abonnement)bdgCommandesRevues.List[bdgCommandesRevues.Position];
                    AfficherCmdRevueInfosCmd(abonnementRevue);
                }
                catch
                {
                    ViderCmdRevueInfosTotal();
                }
            }
            else
            {
                ViderCmdRevueInfosTotal();
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations revue
        /// </summary>
        private void ViderCmdRevueInfos()
        {
            txbCmdRevueIdRevue.Text = "";
            txbCmdRevueTitre.Text = "";
            txbCmdRevuePeriode.Text = "";
            txbCmdRevueMaD.Text = "";
            txbCmdRevueGenre.Text = "";
            txbCmdRevuePublic.Text = "";
            txbCmdRevueRayon.Text = "";
            txbCmdRevueNumRecherche.Text = "";
            pcbCmdRevue.Image = null;
        }

        /// <summary>
        /// Vide les zones d'affichages des infos de commande de la revue sélectionnée
        /// </summary>
        private void ViderCmdRevueInfosCmd()
        {
            txbCmdRevueIdCmd.Text = "";
            txbCmdRevueMontant.Text = "";
            txbCmdRevueNumRecherche.Text = "";
            dtpCmdRevueDateCmd.Value = DateTime.Now;
            dtpCmdRevueFinAbo.Value = DateTime.Now;
        }

        /// <summary>
        /// Vide la totalité des infos commande et revue
        /// </summary>
        private void ViderCmdRevueInfosTotal()
        {
            ViderCmdRevueInfos();
            ViderCmdRevueInfosCmd();
            DesactiverChampsInfosCmdRevue();
            txbCmdRevueNumRecherche.Text = "";
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion des commandes revues
        /// </summary>
        /// <param name="access"></param>
        private void AccesInfosCmdRevueGrpBox(bool access)
        {
            grpCmdRevueInfos.Enabled = access;
        }

        /// <summary>
        /// Gère l'accès à la gestion commande revues
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void SetReadOnlyCmdRevue(bool isReadOnly)
        {
            txbCmdRevueMontant.ReadOnly = isReadOnly;
            txbCmdRevueIdCmd.ReadOnly = isReadOnly;
        }

        /// <summary>
        /// Active les champs commande revue
        /// </summary>
        private void ActiverChampsInfosCmdRevue()
        {
            SetReadOnlyCmdRevue(false);
            dtpCmdRevueDateCmd.Enabled = true;
            dtpCmdRevueFinAbo.Enabled = true;
        }

        /// <summary>
        /// Désactive les champs commande revue
        /// </summary>
        private void DesactiverChampsInfosCmdRevue()
        {
            SetReadOnlyCmdRevue(true);
            dtpCmdRevueDateCmd.Enabled = false;
            dtpCmdRevueFinAbo.Enabled = false;
        }

        /// <summary>
        /// Restaure les paramètres Commandes Revues à son état initial
        /// </summary>
        public void RestaureConfigCmdRevues()
        {
            // Activer le lien avec le grid
            DefaultCmdRevue = true;
            dgvCmdRevueListe.Enabled = true;

            // Désactive l'accès à la gestion commande et le vide
            DesactiverChampsInfosCmdRevue();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdRevueCmd, btnCmdRevueModifier, btnCmdRevueSupprimer };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide = { btnCmdRevueCmdOk, btnCmdRevueModifierOk, btnCmdRevueAnnuler };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Activer les champs nécessaires à la création d'une commande revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueCmd_Click(object sender, EventArgs e)
        {
            if (!txbCmdRevueNumRecherche.Equals(""))
            {
                // Désactive le lien datagrid
                DefaultCmdRevue = false;
                dgvCmdRevueListe.Enabled = false;

                // Active l'accès à la gestion de la commande
                AccesInfosCmdRevueGrpBox(true);
                ActiverChampsInfosCmdRevue();
                ViderCmdRevueInfosCmd();

                // Liste des contrôles à masquer
                Control[] hide = { btnCmdRevueCmd, btnCmdRevueModifier, btnCmdRevueSupprimer };
                foreach (Control control in hide) { control.Hide(); }

                // Liste des contrôles à afficher
                Control[] show = { btnCmdRevueCmdOk, btnCmdRevueAnnuler };
                foreach (Control control in show) { control.Show(); }
            }
            else
            {
                MessageBox.Show("Renseigner un numéro de revue valide", "Erreur");
            }
        }

        /// <summary>
        /// Ajout d'une commande (= abonnement) de revue en BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueCmdOk_Click(object sender, EventArgs e)
        {
            if (!txbCmdRevueNumRecherche.Equals(""))
            {
                try
                {
                    string id = txbCmdRevueIdCmd.Text;
                    DateTime dateCommande = dtpCmdRevueDateCmd.Value;
                    DateTime dateFinAbonnement = dtpCmdRevueFinAbo.Value;
                    double montant = double.Parse(txbCmdRevueMontant.Text);
                    string idRevue = txbCmdRevueIdRevue.Text;
                    Abonnement abonnementRevue = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
                    if (controller.CreerAbonnement(abonnementRevue))
                    {
                        MessageBox.Show("Nouvel abonnement de la revue " + idRevue + " effectué.");
                    }
                    else
                    {
                        MessageBox.Show("Cet abonnement " + id + " existe déjà", "ERREUR");
                    }
                }
                catch
                {
                    MessageBox.Show("Vérifiez que tous les champs soient correctement renseignés", "INFORMATION");
                }
            }
            else
            {
                MessageBox.Show("Aucune revue sélectionnée");
            }
            RestaureConfigCmdRevues();
            TabCommandesRevues_Enter(sender, e);
        }

        /// <summary>
        /// Annulation de l'action en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueAnnuler_Click(object sender, EventArgs e)
        {
            RestaureConfigCmdRevues();
            TabCommandesRevues_Enter(sender, e);
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'une commande revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueModifier_Click(object sender, EventArgs e)
        {
            // Désactive le datagrid
            DefaultCmdRevue = false;

            // Activer l'accès à la gestion de la commande
            AccesInfosCmdRevueGrpBox(true);
            ActiverChampsInfosCmdRevue();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdRevueModifierOk, btnCmdRevueAnnuler };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide = { btnCmdRevueCmd, btnCmdRevueModifier, btnCmdRevueSupprimer };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Modification d'une commande de type revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueModifierOk_Click(object sender, EventArgs e)
        {
            if (!txbCmdRevueIdCmd.Equals(""))
            {
                try
                {
                    string id = txbCmdRevueIdCmd.Text;
                    DateTime dateCommande = dtpCmdRevueDateCmd.Value;
                    double montant = double.Parse(txbCmdRevueMontant.Text);
                    DateTime dateFinAbonnement = dtpCmdRevueFinAbo.Value;
                    string idRevue = txbCmdRevueIdRevue.Text;
                    Abonnement abonnementRevue = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de cette commande ?", "INFORMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.ModifierCommandeRevue(abonnementRevue);
                        MessageBox.Show("Modification de la commande effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification de cette commande");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur lors de la modification de cette commande");
                }
            }
            else
            {
                MessageBox.Show("Merci de renseigner un numéro de commande valide", "Erreur");
            }
            RestaureConfigCmdRevues();
            TabCommandesRevues_Enter(sender, e);
        }

        // <summary>
        /// Vérifie si la date de parution est comprise entre date commande et date fin abonnement
        /// <param name="dateCommande">date de prise de commande</param>
        /// <param name="dateFinAbonnement">date de fin d'abonnement</param>
        /// <param name="dateParution">date de comparaison entre les deux précédentes</param>
        /// <returns>true si la date est comprise entre ces deux dates</returns>
        public bool ParutionAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return (DateTime.Compare(dateCommande, dateParution) < 0 && DateTime.Compare(dateParution, dateFinAbonnement) < 0);
        }

        /// <summary>
        /// Vérifie qu'aucun exemplaire ne soit rattaché à un abonnement
        /// </summary>
        /// <param name="abonnement">l'abonnement cible</param>
        /// <returns>return true si aucun exemplaire rattaché</returns>
        public bool VerifierLienAbonnementExemplaire(Abonnement abonnement)
        {
            List<Exemplaire> lesExemplairesLienAbo = controller.GetExemplairesDocument(abonnement.IdRevue);
            bool supprimer = false;
            foreach (Exemplaire exemplaire in lesExemplairesLienAbo.Where(exemplaires => ParutionAbonnement(abonnement.DateCommande, abonnement.DateFinAbonnement, exemplaires.DateAchat)))
            {
                supprimer = true;
            }
            return !supprimer;
        }

        /// <summary>
        /// Supprimer une commande revue dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueSupprimer_Click(object sender, EventArgs e)
        {
            Abonnement abonnement = (Abonnement)bdgCommandesRevues.List[bdgCommandesRevues.Position];
            if (VerifierLienAbonnementExemplaire(abonnement))
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de l'abonnement " + abonnement.IdRevue + "?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.SupprimerAbonnement(abonnement))
                        {
                            MessageBox.Show("Suppression effectuée", "INFORMATION");
                            AfficherCmdRevueInfosCmdGrid();
                        }
                        else
                        {
                            MessageBox.Show("Erreur sur la suppression de cette commande, veuillez recommencer", "ERREUR");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression annulée", "INFORMATION");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur sur la suppression de cette commande, veuillez recommencer", "ERREUR");
                }
            }
            else
            {
                MessageBox.Show("Impossible de supprimer un abonnement contenant un ou plusieurs exemplaires", "INFORMATION");
            }
            RestaureConfigCmdRevues();
            TabCommandesRevues_Enter(sender, e);
        }
        #endregion
    }
}
