using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe représentant l'interface principale de l'application MediaTekDocuments. 
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        /// <summary>
        /// Contrôleur lié à ce formulaire.
        /// </summary>
        private readonly FrmMediatekController controller;

        /// <summary>
        /// BindingSources contenant les informations des listes correspondantes.
        /// </summary>
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
        private readonly BindingSource bdgEtats = new BindingSource();

        /// <summary>
        /// Liste d'objets.
        /// </summary>
        private List<Livre> lesLivres = new List<Livre>();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Revue> lesRevues = new List<Revue>();
        private List<CommandeDocument> lesCommandesDocuments = new List<CommandeDocument>();
        private List<Abonnement> lesAbonnementsRevues = new List<Abonnement>();
        private List<ExemplaireDetail> lesDetailsExemplairesDocument = new List<ExemplaireDetail>();

        /// <summary>
        /// Booléens d'ouverture de frame.
        /// </summary>
        private bool DefaultLivre = true;
        private bool DefaultDvd = true;
        private bool DefaultRevue = true;
        private bool DefaultParution = true;
        private bool DefaultCmdLivre = true;
        private bool DefaultCmdDvd = true;
        private bool DefaultCmdRevue = true;

        /// <summary>
        /// Chaînes de caractères contenant le suivi des commandes DVD et livres.
        /// </summary>
        private string recupSuiviCmdDvd;
        private string recupSuiviCmdLivre;

        const string ETATNEUF = "00001"; // initialisation de l'état d'un exemplaire à "neuf"

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire.
        /// </summary>
        public FrmMediatek(string idService)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            // Vérification de l'ID du service
            if (idService != null)
            {
                idService = "50001";
                if (idService == "50001" || idService == "50000")
                {
                    // Affichage de la fenêtre de suivi des échéances des abonnements
                    FrmEcheancesAbos frmEcheancesAbos = new FrmEcheancesAbos();
                    frmEcheancesAbos.ShowDialog();
                }
                else if (idService == "50002")
                {
                    // Affichage des informations du service de prêts
                    AfficherInfosServicePrets();
                }
            }
        }

        /// <summary>
        /// Constructeur dédié à l'utilisation avec SpecFlow pour les tests BDD.
        /// </summary>
        public FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Affiche uniquement les onglets des pages "Livres", "DVD" et "Revues" si le service de prêts est sélectionné.
        /// Supprime les autres onglets de la TabControl.
        /// </summary>
        private void AfficherInfosServicePrets()
        {
            tabOngletsApplication.TabPages.Remove(tabReceptionRevue);
            tabOngletsApplication.TabPages.Remove(tabCmdLivres);
            tabOngletsApplication.TabPages.Remove(tabCmdDvd);
            tabOngletsApplication.TabPages.Remove(tabCmdRevues);
        }

        /// <summary>
        /// Remplit un ComboBox avec une liste d'objets Categorie (Genre, Public ou Rayon), 
        /// en utilisant un BindingSource pour lier la liste au ComboBox.
        /// </summary>
        /// <param name="lesCategories">La liste d'objets Categorie à utiliser pour remplir le ComboBox.</param>
        /// <param name="bdg">Le BindingSource utilisé pour lier la liste au ComboBox.</param>
        /// <param name="cbx">Le ComboBox à remplir.</param>
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
        /// Remplit un ComboBox avec une liste d'objets Suivi, en utilisant un BindingSource pour lier la liste au ComboBox.
        /// </summary>
        /// <param name="lesSuivis">La liste des objets Suivi à utiliser pour remplir le ComboBox.</param>
        /// <param name="bdg">Le BindingSource utilisé pour lier la liste au ComboBox.</param>
        /// <param name="cbx">Le ComboBox à remplir.</param>
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
        /// Remplit un ComboBox avec une liste d'objets Etat, en utilisant un BindingSource pour lier la liste au ComboBox.
        /// </summary>
        /// <param name="lesEtats">La liste d'objets Etat à utiliser pour remplir le ComboBox.</param>
        /// <param name="bdg">Le BindingSource utilisé pour lier la liste au ComboBox.</param>
        /// <param name="cbx">Le ComboBox à remplir.</param>
        public void RemplirComboEtat(List<Etat> lesEtats, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesEtats;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Vérifie si la touche pressée est un chiffre ou non, et empêche la saisie de caractères non numériques
        /// Si la touche pressée n'est pas un chiffre, affiche un MessageBox avec le message spécifié.
        /// </summary>
        /// <param name="e">L'événement KeyPress à gérer.</param>
        /// <param name="message">Le message à afficher dans le MessageBox en cas de saisie de caractères non numériques.</param>
        private void TextBoxOnlyNumbers_KeyPress(KeyPressEventArgs e, string message)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Onglet Livres

        /// <summary>
        /// Ouverture de l'onglet Livres.
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
                CacherIdentifiantsCategorieLivres();
                ErrorProviderLivreClear();
                grpLivresExemplaires.Enabled = false;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="livres">Liste des livres.</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            if (DefaultLivre && livres != null)
            {
                bdgLivresListe.DataSource = livres;
                dgvLivresListe.DataSource = bdgLivresListe;
                dgvLivresListe.Columns["Isbn"].Visible = false;
                dgvLivresListe.Columns["IdRayon"].Visible = false;
                dgvLivresListe.Columns["IdGenre"].Visible = false;
                dgvLivresListe.Columns["IdPublic"].Visible = false;
                dgvLivresListe.Columns["Image"].Visible = false;
                dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvLivresListe.Columns["Id"].DisplayIndex = 0;
                dgvLivresListe.Columns["Titre"].DisplayIndex = 1;
            }
            else
            {
                dgvLivresListe.DataSource = null;
            }
        }

        /// <summary>
        /// Remplir le datagrid avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="lesDetailsExemplaires">Liste des exemplaires livre</param>
        private void RemplirExemplairesLivreListe(List<ExemplaireDetail> lesDetailsExemplaires)
        {
            if (DefaultLivre && lesDetailsExemplaires != null)
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
        /// Affiche les informations du livre sélectionné dans les champs de texte correspondants.
        /// Charge l'image du livre dans le PictureBox pcbLivresImage.
        /// </summary>
        /// <param name="livre">Le livre sélectionné.</param>
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
        /// Récupère les infos des exemplaires du livre sélectionné via son numéro de recherche
        /// et initialise avec ces données le datagrid "dgvExemplairesLivreListe".
        /// </summary>
        private void AfficherExemplairesLivre()
        {
            string idDocument = txbLivresNumRecherche.Text;
            lesDetailsExemplairesDocument = controller.GetExemplaireDetailsDocument(idDocument);
            RemplirExemplairesLivreListe(lesDetailsExemplairesDocument);
        }

        /// <summary>
        /// Remplit le datagrid avec la liste complète des livres.
        /// Annulation de toutes les recherches et filtres.
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
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
                    MessageBox.Show("Numéro introuvable.", "Erreur");
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
        /// Filtre sur le combobox genre pour Livres.
        /// Affiche tous les livres correspondant au genre sélectionné.
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
        /// Récupère l'idGenre en fonction du combobox genre sélectionné
        /// lors de l'ajout, suppression ou modification d'un livre.
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
        /// Filtre sur le combobox public pour Livres.
        /// Affiche tous les livres correspondant au public sélectionné.
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
        /// Récupère l'idPublic en fonction du combobox public sélectionné
        /// lors de l'ajout, suppression ou modification d'un livre.
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
        /// Filtre sur le combobox rayon pour Livres.
        /// Affiche tous les livres correspondant au rayon sélectionné.
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
        /// Récupère l'idRayon en fonction du combobox rayon sélectionné
        /// lors de l'ajout, suppression ou modification d'un livre.
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
        /// Récupère l'idEtat en fonction de l'exemplaire Livres sélectionné.
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
        /// Sur la sélection d'une ligne ou cellule dans le datagrid.
        /// Affichage des informations du livre.
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
                    grpLivresExemplaires.Enabled = true;
                }
                catch
                {
                    VideLivresZones();
                    grpLivresExemplaires.Enabled = false;
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le datagrid Exemplaires Livres.
        /// Affiche l'état de l'exemplaire.
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
        /// Tri sur les colonnes du datagrid Livres.
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
        /// Tri sur les colonnes datagrid Exemplaires livre.
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
        /// Restaure l'onglet Livres à son état initial.
        /// </summary>
        private void RestaurationConfigLivres()
        {
            // Active le lien avec le datagrid
            DefaultLivre = true;
            dgvLivresListe.Enabled = true;

            // Liste des contrôles à masquer
            Control[] hide = {
                cbxLivresGenreEdit, cbxLivresPublicEdit, cbxLivresRayonEdit,
                btnAnnulerLivre, btnValiderAjouterLivre, btnValiderModifierLivre,
                btnAnnulerExemplairesLivre, btnModifierExemplairesLivreOk };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterLivre, btnModifierLivre, btnSupprimerLivre,
                btnModifierExemplairesLivre, btnSupprimerExemplairesLivre,
                cbxLivresGenres, cbxLivresPublics, cbxLivresRayons };
            foreach (Control control in show) { control.Show(); }

            DesactiverChampsInfosLivre();
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à l'ajout d'un livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouterLivre_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
            DefaultLivre = false;

            // Vide tous les champs et active les champs informations détaillées
            VideLivresInfosTotal();
            ActiverChampsInfosLivre();
            AccesInfosLivresGrpBox(true);

            // Liste des contrôles à masquer
            Control[] hide = {
                btnAjouterLivre, btnModifierLivre, btnSupprimerLivre,
                cbxLivresGenres, cbxLivresPublics, cbxLivresRayons };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAnnulerLivre, btnValiderAjouterLivre,
                cbxLivresGenreEdit, cbxLivresPublicEdit, cbxLivresRayonEdit
            };
            foreach (Control control in show) { control.Show(); }

            // Remplir les combobox en mode gestion livres
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayonEdit);

            // Focus sur le champ txbLivresNumero et vide les champs des ID catégories
            txbLivresNumero.Focus();
            txbLivresIdGenre.Clear();
            txbLivresIdRayon.Clear();
            txbLivresIdPublic.Clear();
        }

        /// <summary>
        /// Ajout d'un livre en BDD si les conditions sont remplies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderAjoutLivre_Click(object sender, EventArgs e)
        {
            // on vérifie que les champs nécessaires soient bien renseignés
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
                    // on vérifie si ce nouvel id existe en base de données
                    var checkIdLivre = controller.GetDocuments(id);
                    if (checkIdLivre.Count != 0)
                    {
                        MessageBox.Show("Le livre numéro " + id + " existe déjà.", "Erreur");
                        txbLivresNumero.Focus();
                    }
                    else
                    {
                        if (controller.CreerLivre(livre))
                        {
                            MessageBox.Show("Le livre " + titre + " vient d'être ajouté.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                ErrorProviderLivre();
            }
        }

        /// <summary>
        /// Vérifie si les champs requis sont bien renseignés.
        /// Affiche un indicateur le cas contraire.
        /// </summary>
        public void ErrorProviderLivre()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txbLivresNumero.Text))
            {
                ePLivreNumDocEdit.SetError(txbLivresNumero, "Numéro du document requis");
                isValid = false;
            }
            else
            {
                ePLivreNumDocEdit.SetError(txbLivresNumero, "");
            }

            if (cbxLivresGenreEdit.SelectedIndex < 0)
            {
                ePLivreGenreEdit.SetError(cbxLivresGenreEdit, "Sélectionner un genre");
                isValid = false;
            }
            else
            {
                ePLivreGenreEdit.SetError(cbxLivresGenreEdit, "");
            }

            if (cbxLivresPublicEdit.SelectedIndex < 0)
            {
                ePLivrePublicEdit.SetError(cbxLivresPublicEdit, "Sélectionner un public");
                isValid = false;
            }
            else
            {
                ePLivrePublicEdit.SetError(cbxLivresPublicEdit, "");

            }

            if (cbxLivresRayonEdit.SelectedIndex < 0)
            {
                ePLivreRayonEdit.SetError(cbxLivresRayonEdit, "Sélectionner un rayon");
                isValid = false;
            }
            else
            {
                ePLivreRayonEdit.SetError(cbxLivresRayonEdit, "");

            }

            if (isValid)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Efface tous les messages d'erreur affichés.
        /// </summary>
        public void ErrorProviderLivreClear()
        {
            ePLivreNumDocEdit.Clear();
            ePLivreGenreEdit.Clear();
            ePLivrePublicEdit.Clear();
            ePLivreRayonEdit.Clear();
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbLivresNumRecherche".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresNumRecherche_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbLivresNumero".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresNumero_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbLivresIsbn".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresIsbn_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à la modification d'un livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierLivre_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
            DefaultLivre = false;
            dgvLivresListe.Enabled = false;

            // Active les champs informations détaillées + focus sur ISBN + numéro non modifiable
            ActiverChampsInfosLivre();
            txbLivresIsbn.Focus();
            txbLivresNumero.ReadOnly = true;

            // Liste des contrôles à afficher
            Control[] show = {
            btnValiderModifierLivre, btnAnnulerLivre,
            cbxLivresPublicEdit, cbxLivresGenreEdit, cbxLivresRayonEdit };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à maquer
            Control[] hide = {
                btnModifierLivre, btnSupprimerLivre, btnAjouterLivre,
                cbxLivresGenres, cbxLivresPublics, cbxLivresRayons };
            foreach (Control control in hide) { control.Hide(); }

            // Remplit les combobox en mode gestion Livres
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayonEdit);
            RemplirLivresComboboxEdit();
        }

        /// <summary>
        /// Remplit les ComboBox de Gestion des genres, publics et rayons 
        /// avec les valeurs de la base de données Livres.
        /// </summary>
        private void RemplirLivresComboboxEdit()
        {
            cbxLivresGenreEdit.Text = txbLivresGenre.Text;
            cbxLivresPublicEdit.Text = txbLivresPublic.Text;
            cbxLivresRayonEdit.Text = txbLivresRayon.Text;
        }

        /// <summary>
        /// Valide ou non la modification d'un livre dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierLivreOk_Click(object sender, EventArgs e)
        {
            // on vérifie que les champs nécessaires soient bien renseignés
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

                    if (MessageBox.Show(this, "Confirmez-vous la modification du livre " + livre.Titre + " ?", "Information",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.ModifierLivre(livre))
                        {
                            MessageBox.Show("Modification du livre " + livre.Titre + " effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification du livre " + livre.Titre + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                txbLivresIsbn.Focus();
            }
        }

        /// <summary>
        /// Supprimer un livre dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerLivre_Click(object sender, EventArgs e)
        {
            // on récupère la position du livre sélectionné
            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            if (livre != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression du livre " + livre.Titre + " ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var checkExemplaireLivre = controller.GetExemplairesDocument(livre.Id);
                        var checkCommandeLivre = controller.GetCommandesDocument(livre.Id);
                        if (checkExemplaireLivre.Count != 0)
                        {
                            MessageBox.Show("Vous ne pouvez supprimer un livre ayant un ou plusieurs exemplaires rattachés.", "Erreur");
                        }
                        else if (checkCommandeLivre.Count != 0)
                        {
                            MessageBox.Show("Vous ne pouvez supprimer un livre ayant une ou plusieurs commandes en cours.", "Erreur");

                        }
                        else
                        {
                            if (controller.SupprimerLivre(livre))
                            {
                                MessageBox.Show("Suppression du livre " + livre.Titre + " effectuée.", "Information");
                            }
                            else
                            {
                                MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression du livre " + livre.Titre + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Aucun livre sélectionné.", "Information");
            }
        }

        /// <summary>
        /// Active les champs nécessaires à la modification de l'état d'un exemplaire livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierExemplairesLivre_Click(object sender, EventArgs e)
        {
            // Liste des contrôles à afficher
            Control[] show = { btnModifierExemplairesLivreOk, btnAnnulerExemplairesLivre };
            foreach (Control control in show) { control.Show(); }
            // Liste des contrôles à masquer
            Control[] hide = { btnSupprimerExemplairesLivre, btnModifierExemplairesLivre };
            foreach (Control control in hide) { control.Hide(); }
            // Active les champs nécessaires à la modification de l'état exemplaire
            cbxExemplairesLivreEtat.Enabled = true;
        }

        /// <summary>
        /// Valide ou non la modification de l'état d'un exemplaire livre dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierExemplairesLivreOk_Click(object sender, EventArgs e)
        {
            // on vérifie que les champs nécessaires soient bien renseignés
            if (!txbLivresNumero.Text.Equals("") && cbxExemplairesLivreEtat.SelectedIndex != -1)
            {
                try
                {
                    // on récupère la position du livre sélectionné
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
                        if (controller.ModifierExemplaire(exemplaire))
                        {
                            MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                cbxExemplairesLivreEtat.Focus();
            }
        }

        /// <summary>
        /// Supprime un exemplaire livre dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerExemplairesLivre_Click(object sender, EventArgs e)
        {
            // on récupère la position du livre sélectionné
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
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de l'exemplaire n°" + exemplaire.Numero + " ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.SupprimerExemplaire(exemplaire))
                        {
                            MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Aucun exemplaire sélectionné.", "Information");
            }
        }

        /// <summary>
        /// Annule l'ajout ou la modification d'un livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Confirmez-vous cette annulation?", "Information",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
        }

        /// <summary>
        /// Annule la modification de l'état d'un exemplaire livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerExemplairesLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RestaurationConfigLivres();
                TabLivres_Enter(sender, e);
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Change l'état des champs informations détaillées.
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
        /// Désactive l'accès aux champs infos Livres.
        /// </summary>
        private void DesactiverChampsInfosLivre()
        {
            SetReadOnlyLivre(true);
        }

        /// <summary>
        /// Active l'accès aux champs infos Livres.
        /// </summary>
        private void ActiverChampsInfosLivre()
        {
            SetReadOnlyLivre(false);
        }

        /// <summary>
        /// Permet ou interdit l'accès global à la gestion côté Livres.
        /// </summary>
        /// <param name="acces">True ou False.</param>
        private void AccesInfosLivresGrpBox(bool acces)
        {
            grpLivresInfos.Enabled = acces;
        }

        /// <summary>
        /// Vide les zones d'affichage des infos Livres.
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
        /// Vide les zones d'affichage des infos exemplaires livre.
        /// </summary>
        private void VideExemplairesLivreInfos()
        {
            bdgExemplairesLivreListe.DataSource = null;
            cbxExemplairesLivreEtat.Enabled = false;
            cbxExemplairesLivreEtat.SelectedIndex = -1;
            txbLibelleEtatLivre.Text = "";
        }

        /// <summary>
        /// Vide les zones de recherche et de filtre.
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
        /// Vide la totalité des infos Livres.
        /// </summary>
        private void VideLivresInfosTotal()
        {
            VideLivresInfos();
            VideLivresZones();
            dgvLivresListe.Rows.Clear();
        }

        /// <summary>
        /// Cette méthode cache les identifiants des catégories Livres utilisés dans le développement.
        /// </summary>
        private void CacherIdentifiantsCategorieLivres()
        {
            Control[] hide = { lblLivresIdGenre,lblLivresIdPublic, lblLivresIdRayon,
            txbLivresIdGenre, txbLivresIdPublic, txbLivresIdRayon };
            foreach (Control control in hide) { control.Hide(); }
        }

        #endregion

        #region Onglet DVD
        /// <summary>
        /// Ouverture de l'onglet DVD.
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
                CacherIdentifiantsCategorieDvd();
                ErrorProviderDvdClear();
                grpDvdExemplaires.Enabled = false;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="dvds">liste de DVD</param>
        private void RemplirDvdListe(List<Dvd> dvds)
        {
            if (DefaultDvd && dvds != null)
            {
                bdgDvdListe.DataSource = dvds;
                dgvDvdListe.DataSource = bdgDvdListe;
                dgvDvdListe.Columns["IdRayon"].Visible = false;
                dgvDvdListe.Columns["IdGenre"].Visible = false;
                dgvDvdListe.Columns["IdPublic"].Visible = false;
                dgvDvdListe.Columns["Image"].Visible = false;
                dgvDvdListe.Columns["Synopsis"].Visible = false;
                dgvDvdListe.Columns["Duree"].HeaderText = "Durée";
                dgvDvdListe.Columns["Realisateur"].HeaderText = "Réalisateur";
                dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDvdListe.Columns["Id"].DisplayIndex = 0;
                dgvDvdListe.Columns["Titre"].DisplayIndex = 1;
            }
            else
            {
                dgvDvdListe.DataSource = null;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="lesDetailsExemplaires">Liste des exemplaires DVD.</param>
        private void RemplirExemplairesDvdListe(List<ExemplaireDetail> lesDetailsExemplaires)
        {
            if (DefaultDvd && lesDetailsExemplaires != null)
            {
                bdgExemplairesDvdListe.DataSource = lesDetailsExemplaires;
                dgvExemplairesDvdListe.DataSource = bdgExemplairesDvdListe;
                dgvExemplairesDvdListe.Columns["Photo"].Visible = false;
                dgvExemplairesDvdListe.Columns["Id"].Visible = false;
                dgvExemplairesDvdListe.Columns["IdEtat"].Visible = false;
                dgvExemplairesDvdListe.Columns["Numero"].HeaderText = "Numéro";
                dgvExemplairesDvdListe.Columns["LibelleEtat"].HeaderText = "État";
                dgvExemplairesDvdListe.Columns["DateAchat"].HeaderText = "Date d'achat";
                dgvExemplairesDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvExemplairesDvdListe.Columns["DateAchat"].DisplayIndex = 0;
                dgvExemplairesDvdListe.Columns["LibelleEtat"].DisplayIndex = 1;
            }
            else
            {
                dgvExemplairesDvdListe.DataSource = null;
            }
        }

        /// <summary>
        /// Affiche les informations du DVD sélectionné dans les champs de texte correspondants.
        /// Charge l'image du DVD dans le PictureBox pcbDvdImage.
        /// </summary>
        /// <param name="dvd">Le DVD sélectionné.</param>
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
        /// Affiche les exemplaires du DVD sélectionné via son numéro de recherche.
        /// </summary>
        private void AfficherExemplairesDvd()
        {
            string idDocument = txbDvdNumRecherche.Text;
            lesDetailsExemplairesDocument = controller.GetExemplaireDetailsDocument(idDocument);
            RemplirExemplairesDvdListe(lesDetailsExemplairesDocument);
        }

        /// <summary>
        /// Remplit le datagrid avec la liste complète des DVD.
        /// Annulation de toutes les recherches et filtres.
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// Recherche et affichage du DVD dont on a saisi le numéro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (DefaultDvd && !txbDvdNumRecherche.Text.Equals(""))
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
                    grpDvdExemplaires.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Numéro introuvable", "Erreur");
                    RemplirDvdListeComplete();
                    grpDvdExemplaires.Enabled = false;
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des DVD dont le titreRevue matche avec la saisie
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (DefaultDvd && !txbDvdTitreRecherche.Text.Equals(""))
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
        /// Filtre sur le combobox genre pour Dvd.
        /// Affiche tous les dvd correspondant au genre sélectionné.
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
        /// Récupère l'idGenre en fonction du combobox genre sélectionné
        /// lors de l'ajout, suppression ou modification d'un dvd.
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
        /// Filtre sur le combobox public pour Dvd.
        /// Affiche tous les dvd correspondant au public sélectionné.
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
        /// Récupère l'idPublic en fonction du combobox public sélectionné
        /// lors de l'ajout, suppression ou modification d'un dvd.
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
        /// Filtre sur le combobox rayon pour Dvd.
        /// Affiche tous les dvd correspondant au rayon sélectionné.
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
        /// Récupère l'idRayon en fonction du combobox rayon sélectionné
        /// lors de l'ajout, suppression ou modification d'un dvd.
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
        /// Récupère l'idEtat en fonction de l'exemplaire DVD sélectionné.
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
        /// Sur la sélection d'une ligne ou cellule dans le datagrid.
        /// Affichage des informations du DVD.
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
        /// Sur la sélection d'une ligne ou cellule dans le datagrid Exemplaires DVD.
        /// Affiche l'état de l'exemplaire.
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
        /// Tri sur les colonnes du datagrid DVD.
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
        /// Tri sur les colonnes datagrid Exemplaires DVD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvExemplairesDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvExemplairesDvdListe.Columns[e.ColumnIndex].HeaderText;
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

        /// <summary>
        /// Restaure l'onglet DVD à son état initial.
        /// </summary>
        private void RestaurationConfigDvd()
        {
            // Active le lien avec le grid
            DefaultDvd = true;
            dgvDvdListe.Enabled = true;

            // Liste des contrôles à masquer
            Control[] hide = {
                cbxDvdGenreEdit, cbxDvdPublicEdit, cbxDvdRayonEdit,
                btnValiderAjouterDvd, btnValiderModifierDvd,  btnAnnulerDvd,
                btnAnnulerExemplairesDvd, btnModifierExemplairesDvdOk };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterDvd, btnModifierDvd, btnSupprimerDvd, btnModifierExemplairesDvd, btnSupprimerExemplairesDvd,
                cbxDvdGenres, cbxDvdPublics, cbxDvdRayons };
            foreach (Control control in show) { control.Show(); }

            DesactiverChampsInfosDvd();
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à l'ajout d'un DVD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouterDvd_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
            DefaultDvd = false;

            // Vide tous les champs et active les champs informations détaillées
            VideDvdInfosTotal();
            ActiverChampsInfosDvd();
            AccesInformationsDvdGroupBox(true);

            // Liste des contrôles à masquer
            Control[] hide = {
            btnAjouterDvd, btnModifierDvd, btnSupprimerDvd,
            cbxDvdGenres, cbxDvdPublics, cbxDvdRayons };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAnnulerDvd, btnValiderAjouterDvd,
                cbxDvdGenreEdit, cbxDvdPublicEdit, cbxDvdRayonEdit };
            foreach (Control control in show) { control.Show(); }

            // Rempli les combobox en mode gestion dvd
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayonEdit);

            // Focus sur le champs txbDvdNumero et vide les champs des ID catégorie
            txbDvdNumero.Focus();
            txbDvdIdGenre.Clear();
            txbDvdIdRayon.Clear();
            txbDvdIdPublic.Clear();
        }

        /// <summary>
        /// Ajout d'un DVD en BDD si les conditions sont remplies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderAjoutDvd_Click(object sender, EventArgs e)
        {
            // on vérifie que les champs nécessaires soient bien renseignés
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
                    // on vérifie si ce nouvel id existe en base de données
                    var checkIdDvd = controller.GetDocuments(id);
                    if (checkIdDvd.Count != 0)
                    {
                        MessageBox.Show("Le DVD numéro " + id + " existe déjà.", "Erreur");
                        txbDvdNumero.Focus();
                    }
                    else
                    {
                        if (controller.CreerDvd(dvd))
                        {
                            MessageBox.Show("Le DVD " + titre + " vient d'être ajouté.", "Information");

                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigDvd();
                TabDvd_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                ErrorProdiverDvd();
            }
        }

        /// <summary>
        /// Vérifie si les champs requis sont bien renseignés.
        /// Affiche un indicateur le cas contraire.
        /// </summary>
        public void ErrorProdiverDvd()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txbDvdNumero.Text))
            {
                ePDvdNumDocEdit.SetError(txbDvdNumero, "Numéro du document requis");
                isValid = false;
            }
            else
            {
                ePDvdNumDocEdit.SetError(txbDvdNumero, "");
            }

            if (string.IsNullOrEmpty(txbDvdDuree.Text))
            {
                ePDvdDureeEdit.SetError(txbDvdDuree, "Durée requise");
                isValid = false;
            }
            else
            {
                ePDvdDureeEdit.SetError(txbDvdDuree, "");
            }

            if (cbxDvdGenreEdit.SelectedIndex < 0)
            {
                ePDvdGenreEdit.SetError(cbxDvdGenreEdit, "Sélectionner un genre");
                isValid = false;
            }
            else
            {

                ePDvdGenreEdit.SetError(cbxDvdGenreEdit, "");
            }

            if (cbxDvdPublicEdit.SelectedIndex < 0)
            {
                ePDvdPublicEdit.SetError(cbxDvdPublicEdit, "Sélectionner un public");
                isValid = false;
            }
            else
            {
                ePDvdPublicEdit.SetError(cbxDvdPublicEdit, "");
            }

            if (cbxDvdRayonEdit.SelectedIndex < 0)
            {
                ePDvdRayonEdit.SetError(cbxDvdRayonEdit, "Sélectionner un rayon");
                isValid = false;
            }
            else
            {
                ePDvdRayonEdit.SetError(cbxDvdRayonEdit, "");
            }

            if (isValid)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Efface tous les messages d'erreur affichés.
        /// </summary>
        public void ErrorProviderDvdClear()
        {
            ePDvdNumDocEdit.Clear();
            ePDvdDureeEdit.Clear();
            ePDvdGenreEdit.Clear();
            ePDvdPublicEdit.Clear();
            ePDvdRayonEdit.Clear();
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbDvdNumRecherche". 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbDvdNumRecherche_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbDvdNumero". 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbDvdNumero_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbDvdDuree". 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbDvdDuree_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à la modification d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierDvd_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
            DefaultDvd = false;
            dgvDvdListe.Enabled = false;

            // Active les champs informations détaillées + focus sur durée + numéro non modifiable
            ActiverChampsInfosDvd();
            txbDvdDuree.Focus();
            txbDvdNumero.ReadOnly = true;

            // Liste des contrôles à masquer
            Control[] show = {
            btnValiderModifierDvd, btnAnnulerDvd,
            cbxDvdPublicEdit, cbxDvdGenreEdit, cbxDvdRayonEdit };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à afficher
            Control[] hide = {
                btnValiderAjouterDvd, btnModifierDvd, btnSupprimerDvd, btnAjouterDvd,
                cbxDvdGenres, cbxDvdPublics, cbxDvdRayons };
            foreach (Control control in hide) { control.Hide(); }

            // Remplit les combobox en mode gestion DVD
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayonEdit);
            RemplirDvdComboboxEdit();
        }

        /// <summary>
        /// Remplit les ComboBox de Gestion des genres, publics et rayons 
        /// avec les valeurs de la base de données DVD.
        /// </summary>
        private void RemplirDvdComboboxEdit()
        {
            cbxDvdGenreEdit.Text = txbDvdGenre.Text;
            cbxDvdPublicEdit.Text = txbDvdPublic.Text;
            cbxDvdRayonEdit.Text = txbDvdRayon.Text;
        }

        /// <summary>
        /// Modification d'un dvd en BDD si les conditions sont remplies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierDvdOk_Click(object sender, EventArgs e)
        {
            // on vérifie que les champs nécessaires soient bien renseignés
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

                    if (MessageBox.Show(this, "Confirmez-vous la modification du DVD " + dvd.Titre + " ?", "Information",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.ModifierDvd(dvd))
                        {
                            MessageBox.Show("Modification du DVD " + dvd.Titre + " effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification du DVD " + dvd.Titre + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigDvd();
                TabDvd_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                txbDvdDuree.Focus();
            }
        }

        /// <summary>
        /// Supprimer un DVD dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerDvd_Click(object sender, EventArgs e)
        {
            // on récupère la position du dvd sélectionné
            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
            if (dvd != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression du DVD " + dvd.Titre + " ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var checkExemplaireDvd = controller.GetExemplairesDocument(dvd.Id);
                        var checkCommandeDvd = controller.GetCommandesDocument(dvd.Id);
                        if (checkExemplaireDvd.Count != 0)
                        {
                            MessageBox.Show("Vous ne pouvez supprimer un DVD ayant un ou plusieurs exemplaires rattachés.", "Erreur");

                        }
                        else if (checkCommandeDvd.Count != 0)
                        {
                            MessageBox.Show("Vous ne pouvez supprimer un DVD ayant une ou plusieurs commandes en cours.", "Erreur");
                        }
                        else
                        {
                            if (controller.SupprimerDvd(dvd))
                            {
                                MessageBox.Show("Suppression du DVD " + dvd.Titre + " effectuée.", "Information");
                            }
                            else
                            {
                                MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression du DVD " + dvd.Titre + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigDvd();
                TabDvd_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Aucun DVD sélectionné.", "Information");
            }
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
            foreach (Control control in show) { control.Show(); }
            // Liste des contrôles à masquer
            Control[] hide = { btnModifierExemplairesDvd, btnSupprimerExemplairesDvd };
            foreach (Control control in hide) { control.Hide(); }
            // Active les champs nécessaires à la modification de l'état exemplaire
            cbxExemplairesDvdEtat.Enabled = true;
        }

        /// <summary>
        /// Valide ou non la modification de l'état d'un exemplaire revue dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExemplairesDvdModifierOk_Click(object sender, EventArgs e)
        {
            // on vérifie que les champs nécessaires soient bien renseignés
            if (!txbDvdNumero.Text.Equals("") && cbxExemplairesDvdEtat.SelectedIndex != -1)
            {
                try
                {
                    // on récupère la position du dvd sélectionné
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
                        if (controller.ModifierExemplaire(exemplaire))
                        {
                            MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification de l'état de l'exemplaire n°" + exemplaire.Numero + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigDvd();
                TabDvd_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                cbxExemplairesDvdEtat.Focus();
            }
        }

        /// <summary>
        /// Supprimer un exemplaire DVD dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerExemplairesDvd_Click(object sender, EventArgs e)
        {
            // on récupère la position du dvd sélectionné
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
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de l'exemplaire n°" + exemplaire.Numero + " ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.SupprimerExemplaire(exemplaire))
                        {
                            MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression de l'exemplaire n°" + exemplaire.Numero + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigDvd();
                TabDvd_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Aucun exemplaire sélectionné.", "Information");
            }

        }

        /// <summary>
        /// Annule l'ajout ou modification d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Confirmez-vous cette annulation?", "Information",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
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
                RestaurationConfigDvd();
                TabDvd_Enter(sender, e);
            }
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
        /// Désactive l'accès aux champs infos DVD
        /// </summary>
        private void DesactiverChampsInfosDvd()
        {
            SetReadOnlyDvd(true);
        }

        /// <summary>
        /// Active l'accès aux champs infos Dvd
        /// </summary>
        private void ActiverChampsInfosDvd()
        {
            SetReadOnlyDvd(false);
        }

        /// <summary>
        /// Permet ou interdit l'accès global à la gestion côté DVD
        /// </summary>
        /// <param name="acces"></param>
        private void AccesInformationsDvdGroupBox(bool acces)
        {
            grpDvdInfos.Enabled = acces;
        }

        /// <summary>
        /// Vide les zones d'affichage des infos DVD
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
        /// Vide les zones d'affichage des infos exemplaires DVD
        /// </summary>
        private void VideExemplairesDvdInfos()
        {
            bdgExemplairesDvdListe.DataSource = null;
            cbxExemplairesDvdEtat.Enabled = false;
            cbxExemplairesDvdEtat.SelectedIndex = -1;
            txbLibelleEtatDvd.Text = "";
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
        /// Vide la totalité des infos DVD
        /// </summary>
        private void VideDvdInfosTotal()
        {
            VideDvdInfos();
            VideDvdZones();
            dgvDvdListe.Rows.Clear();
        }

        /// <summary>
        /// Cacher les id des catégories DVD
        /// Seulement développement
        /// </summary>
        private void CacherIdentifiantsCategorieDvd()
        {
            Control[] hide = { lblDvdIdGenre, lblDvdIdPublic, lblDvdIdRayon,
            txbDvdIdGenre, txbDvdIdPublic, txbDvdIdRayon };
            foreach (Control control in hide) { control.Hide(); }
        }

        #endregion

        #region Onglet Revues

        /// <summary>
        /// Ouverture de l'onglet Revues.
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
                RemplirRevuesListeComplete();
                CacherIdentifiantsCategorieRevues();
                ErrorProviderRevuesClear();
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            if (DefaultRevue && revues != null)
            {
                bdgRevuesListe.DataSource = revues;
                dgvRevuesListe.DataSource = bdgRevuesListe;
                dgvRevuesListe.Columns["IdRayon"].Visible = false;
                dgvRevuesListe.Columns["IdGenre"].Visible = false;
                dgvRevuesListe.Columns["IdPublic"].Visible = false;
                dgvRevuesListe.Columns["Image"].Visible = false;
                dgvRevuesListe.Columns["Periodicite"].HeaderText = "Périodicité";
                dgvRevuesListe.Columns["DelaiMiseADispo"].HeaderText = "Délai mise à dispo";
                dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvRevuesListe.Columns["Id"].DisplayIndex = 0;
                dgvRevuesListe.Columns["Titre"].DisplayIndex = 1;
            }
            else
            {
                dgvRevuesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Remplit les ComboBox de Gestion des genres, publics et rayons 
        /// avec les valeurs de la base de données Revues.
        /// </summary>
        private void RemplirRevuesComboboxEdit()
        {
            cbxRevuesGenreEdit.Text = txbRevuesGenre.Text;
            cbxRevuesPublicEdit.Text = txbRevuesPublic.Text;
            cbxRevuesRayonEdit.Text = txbRevuesRayon.Text;
        }

        /// <summary>
        /// Affiche les informations de la revue sélectionnée dans les champs de texte correspondants.
        /// Charge l'image de la revue dans le PictureBox pcbRevuesImage.
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
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
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
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
                }
                else
                {
                    MessageBox.Show("Numéro introuvable", "Erreur");
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
            if (DefaultRevue && !txbRevuesTitreRecherche.Text.Equals(""))
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
        /// Filtre sur le combobox genre pour Revues.
        /// Affiche toutes les revues correspondant au genre sélectionné.
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
        /// Récupère l'idGenre en fonction du combobox genre sélectionné
        /// lors de l'ajout, suppression ou modification d'une revue.
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
        /// Filtre sur le combobox public pour Revues.
        /// Affiche toutes les revues correspondant au public sélectionné.
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
        /// Récupère l'idPublic en fonction du combobox public sélectionné
        /// lors de l'ajout, suppression ou modification d'une revue.
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
        /// Filtre sur le combobox rayon pour Revues.
        /// Affiche toutes les revues correspondant au rayon sélectionné.
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
        /// Récupère l'idRayon en fonction du combobox rayon sélectionné
        /// lors de l'ajout, suppression ou modification d'une revue.
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
        /// Sur la sélection d'une ligne ou cellule dans le datagrid Revues.
        /// Affichage des informations de la revue.
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
        /// Tri sur les colonnes du datagrid Revues.
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
                case "Délai mise à dispo":
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

        /// <summary>
        /// Restaure l'onglet Revues à son état initial.
        /// </summary>
        private void RestaurationConfigRevues()
        {
            // Active le lien avec le grid
            DefaultRevue = true;
            dgvRevuesListe.Enabled = true;

            // Liste des contrôles à masquer
            Control[] hide = {
                cbxRevuesGenreEdit, cbxRevuesPublicEdit, cbxRevuesRayonEdit,
                btnAnnulerRevue, btnValiderAjouterRevue, btnValiderModifierRevue };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterRevue, btnModifierRevue, btnSupprimerRevue,
                cbxRevuesGenres, cbxRevuesPublics, cbxRevuesRayons };
            foreach (Control control in show) { control.Show(); }

            DesactiverChampsInfosRevues();
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à l'ajout d'une revue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouterRevue_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
            DefaultRevue = false;

            // Vide tous les champs et active les champs informations détaillées
            VideRevuesInfosTotal();
            ActiverChampsInfosRevues();
            AccesInformationsRevuesGroupBox(true);


            // Liste des contrôles à masquer
            Control[] hide = {
                btnAjouterRevue, btnModifierRevue, btnSupprimerRevue,
                cbxRevuesGenres, cbxRevuesPublics, cbxRevuesRayons };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAnnulerRevue, btnValiderAjouterRevue,
                cbxRevuesGenreEdit, cbxRevuesPublicEdit, cbxRevuesRayonEdit };
            foreach (Control control in show) { control.Show(); }

            // Remplir les combobox en mode gestion revues
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayonEdit);

            // Focus sur le champs txbRevuesNumero et vide les champs des ID catégories
            txbRevuesNumero.Focus();
            txbRevuesIdGenre.Clear();
            txbRevuesIdRayon.Clear();
            txbRevuesIdPublic.Clear();
        }

        /// <summary>
        /// Ajout d'une revue en BDD si les conditions sont remplies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderAjoutRevue_Click(object sender, EventArgs e)
        {
            // on vérifie que les champs nécessaires soient bien renseignés
            if (!txbRevuesNumero.Text.Equals("") && !txbRevuesDelaiMiseADispo.Text.Equals("") && cbxRevuesGenreEdit.SelectedIndex != -1 && cbxRevuesPublicEdit.SelectedIndex != -1 && cbxRevuesRayonEdit.SelectedIndex != -1)
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
                    int delaiMiseADispo = int.Parse(txbRevuesDelaiMiseADispo.Text);
                    Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);
                    // on vérifie si ce nouvel id existe en base de données
                    var checkIdRevue = controller.GetDocuments(id);
                    if (checkIdRevue.Count != 0)
                    {
                        MessageBox.Show("La revue " + titre + " existe déjà.", "Erreur");
                        txbRevuesNumero.Focus();
                    }
                    else
                    {
                        if (controller.CreerRevue(revue))
                        {
                            MessageBox.Show("La revue " + titre + " vient d'être ajoutée.", "Information");

                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigRevues();
                TabRevues_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                ErrorProviderRevue();
            }
        }

        /// <summary>
        /// Vérifie si les champs requis sont bien renseignés.
        /// Affiche un indicateur le cas contraire.
        /// </summary>
        public void ErrorProviderRevue()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txbRevuesNumero.Text))
            {
                ePRevueNumDocEdit.SetError(txbRevuesNumero, "Numéro du document requis");
                isValid = false;
            }
            else
            {
                ePRevueNumDocEdit.SetError(txbRevuesNumero, "");
            }

            if (string.IsNullOrEmpty(txbRevuesDelaiMiseADispo.Text))
            {
                ePRevueMaD.SetError(txbRevuesDelaiMiseADispo, "Renseigner un délai de mise à disposition.");
                isValid = false;
            }
            else
            {
                ePRevueMaD.SetError(txbRevuesDelaiMiseADispo, "");
            }

            if (cbxRevuesGenreEdit.SelectedIndex < 0)
            {
                ePRevueGenreEdit.SetError(cbxRevuesGenreEdit, "Sélectionner un genre");
                isValid = false;
            }
            else
            {
                ePRevueGenreEdit.SetError(cbxRevuesGenreEdit, "");
            }

            if (cbxRevuesPublicEdit.SelectedIndex < 0)
            {
                ePRevuePublicEdit.SetError(cbxRevuesPublicEdit, "Sélectionner un public");
                isValid = false;
            }
            else
            {
                ePRevuePublicEdit.SetError(cbxRevuesPublicEdit, "");

            }

            if (cbxRevuesRayonEdit.SelectedIndex < 0)
            {
                ePRevueRayonEdit.SetError(cbxRevuesRayonEdit, "Sélectionner un rayon");
                isValid = false;
            }
            else
            {
                ePRevueRayonEdit.SetError(cbxRevuesRayonEdit, "");
            }

            if (isValid)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Efface tous les messages d'erreur affichés.
        /// </summary>
        public void ErrorProviderRevuesClear()
        {
            ePRevueNumDocEdit.Clear();
            ePRevueMaD.Clear();
            ePRevueGenreEdit.Clear();
            ePRevuePublicEdit.Clear();
            ePRevueRayonEdit.Clear();
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbRevuesNumRecherche".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbRevuesNumRecherche_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbRevuesNumero".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbRevuesNumero_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbRevuesDelaiMiseADispo".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbRevuesDelai_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à la modification d'une revue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierRevue_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
            DefaultRevue = false;

            // Active les champs nécessaires
            ActiverChampsInfosRevues();
            txbRevuesTitre.Focus();
            txbRevuesNumero.ReadOnly = true;
            dgvRevuesListe.Enabled = false;

            // Liste des contrôles à afficher
            Control[] show = {
                btnValiderModifierRevue, btnAnnulerRevue,
                cbxRevuesPublicEdit, cbxRevuesGenreEdit, cbxRevuesRayonEdit };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide = {
                btnModifierRevue, btnSupprimerRevue, btnAjouterRevue,
                cbxRevuesGenres, cbxRevuesPublics, cbxRevuesRayons};
            foreach (Control control in hide) { control.Hide(); }

            // Remplir les combobox en mode gestion revues
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayonEdit);
            RemplirRevuesComboboxEdit();
        }

        /// <summary>
        /// Modification d'une revue en BDD si les conditions sont remplies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierRevueOk_Click(object sender, EventArgs e)
        {
            // on vérifie que les champs nécessaires soient bien renseignés
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
                    int delaiMiseADispo = int.Parse(txbRevuesDelaiMiseADispo.Text);
                    Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);

                    if (MessageBox.Show(this, "Confirmez-vous la modification de la revue " + revue.Titre + " ?", "Information",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.ModifierRevue(revue))
                        {
                            MessageBox.Show("Modification de la revue " + revue.Titre + " effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification de la revue " + revue.Titre + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigRevues();
                TabRevues_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                txbRevuesTitre.Focus();
            }
        }

        /// <summary>
        /// Supprimer une revue dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerRevue_Click(object sender, EventArgs e)
        {
            // on récupère la position de la revue sélectionnée
            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
            if (revue != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de la revue " + revue.Titre + " ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var checkExemplaireRevue = controller.GetExemplairesDocument(revue.Id);
                        var checkCommandeRevue = controller.GetAbonnementsRevue(revue.Id);
                        if (checkExemplaireRevue.Count != 0)
                        {
                            MessageBox.Show("Vous ne pouvez supprimer une revue ayant un ou plusieurs exemplaires rattachés.", "Erreur");

                        }
                        else if (checkCommandeRevue.Count != 0)
                        {
                            MessageBox.Show("Vous ne pouvez supprimer une revue ayant une ou plusieurs commandes en cours.", "Erreur");

                        }
                        else
                        {
                            if (controller.SupprimerRevue(revue))
                            {
                                MessageBox.Show("Suppression de la revue " + revue.Titre + " effectuée.", "Information");
                            }
                            else
                            {
                                MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression de la revue " + revue.Titre + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigRevues();
                TabRevues_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Aucune revue sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Annule l'ajout ou modification d'une revue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Confirmez-vous cette annulation?", "Information",
               MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RestaurationConfigRevues();
                TabRevues_Enter(sender, e);
            }
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
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Change l'état des champs informations détaillées.
        /// </summary>
        private void SetReadOnlyRevues(bool isReadOnly)
        {
            txbRevuesNumero.ReadOnly = isReadOnly;
            txbRevuesTitre.ReadOnly = isReadOnly;
            txbRevuesGenre.ReadOnly = isReadOnly;
            txbRevuesPublic.ReadOnly = isReadOnly;
            txbRevuesRayon.ReadOnly = isReadOnly;
            txbRevuesImage.ReadOnly = isReadOnly;
            txbRevuesDelaiMiseADispo.ReadOnly = isReadOnly;
            txbRevuesPeriodicite.ReadOnly = isReadOnly;
        }

        /// <summary>
        /// Désactive l'accès aux champs infos Revues.
        /// </summary>
        private void DesactiverChampsInfosRevues()
        {
            SetReadOnlyRevues(true);
        }

        /// <summary>
        /// Active l'accès aux champs infos Revues.
        /// </summary>
        private void ActiverChampsInfosRevues()
        {
            SetReadOnlyRevues(false);
        }

        /// <summary>
        /// Permet ou interdit l'accès global à la gestion côté revues.
        /// </summary>
        /// <param name="acces">True ou False</param>
        private void AccesInformationsRevuesGroupBox(bool acces)
        {
            grpRevuesInfos.Enabled = acces;
        }

        /// <summary>
        /// Vide les zones d'affichage des infos Revues
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDelaiMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Vide les zones de recherche et de filtre.
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
        /// Vide la totalité des informations revues.
        /// </summary>
        private void VideRevuesInfosTotal()
        {
            VideRevuesInfos();
            VideRevuesZones();
            dgvRevuesListe.Rows.Clear();
        }

        /// <summary>
        ///  Cette méthode cache les identifiants des catégories Revues utilisés dans le développement.
        /// Seulement développement
        /// </summary>
        private void CacherIdentifiantsCategorieRevues()
        {
            Control[] hide = { lblRevueIdGenre, lblRevueIdPublic, lblRevueIdRayon,
            txbRevuesIdGenre, txbRevuesIdPublic, txbRevuesIdRayon };
            foreach (Control control in hide) { control.Hide(); }
        }

        #endregion

        #region Onglet Parutions

        /// <summary>
        /// Ouverture de l'onglet : récupère les revues et vide tous les champs.
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
                grpGestionEtatParutions.Enabled = false;
                grpReceptionExemplaire.Enabled = false;
                cbxParutionsEtat.SelectedIndex = -1;
                txbReceptionRevueNumero.Text = "";
                txbLibelleEtatParution.Text = "";
                ErrorProviderParutionsClear();
            }
        }

        /// <summary>
        /// Remplit le datagrid des exemplaires avec la liste reçue en paramètre.
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
                dgvReceptionExemplairesListe.Columns["Numero"].HeaderText = "Numéro";
                dgvReceptionExemplairesListe.Columns["DateAchat"].HeaderText = "Date d'achat";
                dgvReceptionExemplairesListe.Columns["LibelleEtat"].HeaderText = "État";
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvReceptionExemplairesListe.Columns["Numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["DateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
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
                    grpGestionEtatParutions.Enabled = true;
                    grpReceptionExemplaire.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    grpGestionEtatParutions.Enabled = false;
                    grpReceptionExemplaire.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue sont aussi effacées.
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
        /// Affichage des informations de la revue sélectionnée et les exemplaires.
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
        /// Affiche les exemplaires de la revue sélectionnée via son numéro de recherche.
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
        /// et vide les objets graphiques.
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
        /// Recherche d'image sur disque (pour l'exemplaire à insérer).
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
        /// Enregistrement du nouvel exemplaire lors du clic sur le bouton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            // on vérifie que le champ nécessaire soit bien renseigné
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
                    // on vérifie si ce nouvel id existe en base de données
                    var checkNumeroParution = controller.GetNumeroParution(numero.ToString());
                    Console.WriteLine(numero.ToString());
                    if (checkNumeroParution.Count != 0)
                    {
                        MessageBox.Show("Numéro de publication déjà existant", "Erreur");
                        txbReceptionExemplaireNumero.Focus();
                    }
                    else
                    {
                        if (controller.CreerExemplaire(exemplaire))
                        {
                            MessageBox.Show("L'exemplaire n° " + exemplaire.Numero + " vient d'être ajouté.", "Information");
                            AfficheReceptionExemplairesRevue();
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
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
                MessageBox.Show("Numéro de parution obligatoire", "Information");
                ErrorProviderParutionsRevues();
            }
        }

        /// <summary>
        /// Vérifie si le champ requis est bien renseigné.
        /// Affiche un indicateur le cas contraire.
        /// </summary>
        public void ErrorProviderParutionsRevues()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txbReceptionExemplaireNumero.Text))
            {
                ePReceptionExemplaireRevue.SetError(txbReceptionExemplaireNumero, "Numéro de parution obligatoire");
                isValid = false;
            }
            else
            {
                ePReceptionExemplaireRevue.SetError(txbReceptionExemplaireNumero, "");
            }

            if (isValid)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Efface tous les messages d'erreur affichés.
        /// </summary>
        public void ErrorProviderParutionsClear()
        {
            ePReceptionExemplaireRevue.Clear();
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbReceptionExemplaireNumero".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbReceptionExemplaireNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Tri sur une colonne du datagrid Réception Exemplaires.
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
        /// Active les champs nécessaire à la modification de l'état d'une parution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierParutions_Click(object sender, EventArgs e)
        {
            // Liste des contrôles à afficher
            Control[] show = { btnModifierParutionOk, btnAnnulerParution };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide = { btnModifierParution, btnSupprimerParution };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Valide ou non la modification de l'état d'une parution dans la BDD.
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
                        if (controller.ModifierExemplaire(exemplaire))
                        {
                            MessageBox.Show("Modification de l'état de la parution n°" + exemplaire.Numero + " effectuée", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification de l'état de la parution n°" + exemplaire.Numero + " annulée", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
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
        /// Annule la modification d'une parution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerParution_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler cette modification?", "Information",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RestaurationConfigParutions();
                TabReceptionRevue_Enter(sender, e);
            }
        }

        /// <summary>
        /// Supprimer une parution dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimerParution_Click(object sender, EventArgs e)
        {
            // on récupère la position de la parution sélectionnée
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
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de la parution n°" + exemplaire.Numero + " ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.SupprimerExemplaire(exemplaire))
                        {
                            MessageBox.Show("Suppression de la parution n°" + exemplaire.Numero + " effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression de la parution n°" + exemplaire.Numero + " annulée", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
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
        /// Restaurer l'onglet Parutions des revues à son état initial.
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

        /// <summary>
        /// Ouverture de l'onglet Commandes Livres.
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
                ErrorProviderCmdLivreClear();
                grpCmdLivreInfosCmd.Enabled = false;
                grpCmdLivresGestion.Enabled = false;
            }
        }

        /// <summary>
        /// Remplit le datagrid commande livre avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="lesCommandesLivres ">liste des livres commandés</param>
        private void RemplirCmdLivreListe(List<CommandeDocument> lesCommandesLivres)
        {
            if (DefaultCmdLivre && lesCommandesLivres != null)
            {
                bdgCommandesLivres.DataSource = lesCommandesLivres;
                dgvCmdLivreListe.DataSource = bdgCommandesLivres;
                dgvCmdLivreListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvCmdLivreListe.Columns["IdLivreDvd"].Visible = false;
                dgvCmdLivreListe.Columns["IdSuivi"].Visible = false;
                //dgvCmdLivreListe.Columns["Id"].Visible = false;
                dgvCmdLivreListe.Columns["DateCommande"].DisplayIndex = 0;
                dgvCmdLivreListe.Columns["DateCommande"].HeaderText = "Date commande";
                dgvCmdLivreListe.Columns["Montant"].DisplayIndex = 1;
                dgvCmdLivreListe.Columns["NbExemplaire"].DisplayIndex = 2;
                dgvCmdLivreListe.Columns["NbExemplaire"].HeaderText = "Nb exemplaires";
                dgvCmdLivreListe.Columns["LibelleSuivi"].HeaderText = "Suivi";
            }
            else
            {
                dgvCmdLivreListe.DataSource = null;
            }
        }

        /// <summary>
        /// Affiche les informations du livre sélectionné dans les champs de texte correspondants.
        /// Affiche également les informations de commande du livre.
        /// Charge l'image du livre dans le PictureBox pcbCmdLivre.
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
        /// Récupère les infos de commande du livre via son numéro de recherche
        /// et initialise avec ces données le datagrid "dgvCmdLivreListe".
        /// </summary>
        public void AfficherCmdLivreInfoCmdGrid()
        {
            string idDocument = txbCmdLivreNumRecherche.Text;
            lesCommandesDocuments = controller.GetCommandesDocument(idDocument);
            RemplirCmdLivreListe(lesCommandesDocuments);
        }

        /// <summary>
        /// Récupère les informations de commande du livre.
        /// </summary>
        /// <param name="commandeLivre"></param>
        private void RecupererCmdLivreInfosCmd(CommandeDocument commandeLivre)
        {
            txbCmdLivreIdCmd.Text = commandeLivre.Id;
            cbxCmdLivreSuivi.Text = commandeLivre.LibelleSuivi;
            txbCmdLivreNbExemplaires.Text = commandeLivre.NbExemplaire.ToString();
            txbCmdLivreMontant.Text = commandeLivre.Montant.ToString();
            dtpCmdLivre.Value = commandeLivre.DateCommande;
        }

        /// <summary>
        /// Recherche et affiche les infos du livre dont on a saisi le numéro.
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
                    grpCmdLivreInfosCmd.Enabled = true;
                    grpCmdLivresGestion.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Numéro introuvable.", "Information");
                    txbCmdLivreNumRecherche.Text = "";
                    grpCmdLivreInfosCmd.Enabled = false;
                    grpCmdLivresGestion.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer un numéro de livre valide.", "Erreur");
            }
        }

        /// <summary>
        /// Récupère l'idSuivi en fonction du combobox Suivi sélectionné
        /// lors de l'ajout, suppression ou modification d'une commande livre.
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
        /// Sur la sélection d'une ligne ou cellule dans le datagrid
        /// affichage des informations de commande livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdLivreListe_SelectionChanged(object sender, EventArgs e)
        {
            if (DefaultCmdLivre && dgvCmdLivreListe.CurrentCell != null)
            {
                try
                {
                    CommandeDocument commandeLivre = (CommandeDocument)bdgCommandesLivres.List[bdgCommandesLivres.Position];
                    RecupererCmdLivreInfosCmd(commandeLivre);
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
        /// Tri sur les colonnes du datagrid Commandes Livres.
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
                case "Id":
                    sortedList = lesCommandesDocuments.OrderBy(o => o.Id).ToList();
                    break;
            }
            RemplirCmdLivreListe(sortedList);
        }

        /// <summary>
        /// Restaure l'onglet Commandes Livres à son état initial.
        /// </summary>
        public void RestaurationConfigCmdLivres()
        {
            // Active le lien avec le grid
            DefaultCmdLivre = true;
            dgvCmdLivreListe.Enabled = true;

            // Désactive l'accès à la gestion commande livre et le vide
            DesactiverChampsInfosCmdLivre();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdLivreCmd, btnCmdLivreModifier, btnCmdLivreSupprimer };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide = { btnCmdLivreCmdOk, btnCmdLivreModifierOk, btnCmdLivreAnnuler };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à l'ajout d'une commande livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreCmd_Click(object sender, EventArgs e)
        {
            if (!txbCmdLivreNumRecherche.Equals(""))
            {
                // Désactive le lien entre le datagrid et l'édition
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
            else
            {
                MessageBox.Show("Aucun livre sélectionné.", "Erreur");
            }
        }

        /// <summary>
        /// Ajout d'une commande d'un livre en BDD si les conditions sont remplies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreCmdOk_Click(object sender, EventArgs e)
        {
            // vérifie que les champs nécessaires soient bien renseignés
            if (!txbCmdLivreIdCmd.Text.Equals("") && !txbCmdLivreMontant.Text.Equals("") && !txbCmdLivreNbExemplaires.Equals(""))
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
                    CommandeDocument commandeLivre = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelleSuivi);
                    // vérifie si le numéro de commande existe en base de données
                    var checkIdCmdLivre = controller.GetCommandeId(id);
                    if (checkIdCmdLivre.Count != 0)
                    {
                        MessageBox.Show("La commande n° " + id + " existe déjà.", "Erreur");
                        txbCmdLivreIdCmd.Focus();
                    }
                    else
                    {
                        if (controller.CreerCommandeDocument(commandeLivre))
                        {
                            MessageBox.Show("Commande du livre n° " + idLivreDvd + " effectuée", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                            TabCommandesLivres_Enter(sender, e);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                    TabCommandesLivres_Enter(sender, e);
                }
                RestaurationConfigCmdLivres();
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                ErrorProviderCmdLivre();
            }
        }

        /// <summary>
        /// Vérifie si les champs requis sont bien renseignés.
        /// Affiche un indicateur le cas contraire.
        /// </summary>
        public void ErrorProviderCmdLivre()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txbCmdLivreIdCmd.Text))
            {
                epCmdLivreNumCmd.SetError(txbCmdLivreIdCmd, "Numéro de commande requis");
                isValid = false;
            }
            else
            {
                epCmdLivreNumCmd.SetError(txbCmdLivreIdCmd, "");
            }

            if (string.IsNullOrEmpty(txbCmdLivreNbExemplaires.Text))
            {
                ePCmdLivreNbExemplaire.SetError(txbCmdLivreNbExemplaires, "Nombre d'exemplaires requis");
                isValid = false;
            }
            else
            {
                ePCmdLivreNbExemplaire.SetError(txbCmdLivreNbExemplaires, "");
            }

            if (string.IsNullOrEmpty(txbCmdLivreMontant.Text))
            {
                ePCmdLivreMontant.SetError(txbCmdLivreMontant, "Montant requis");
                isValid = false;
            }
            else
            {
                ePCmdLivreMontant.SetError(txbCmdLivreMontant, "");
            }

            if (isValid)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Efface tous les messages d'erreur affichés.
        /// </summary>
        public void ErrorProviderCmdLivreClear()
        {
            epCmdLivreNumCmd.Clear();
            ePCmdLivreNbExemplaire.Clear();
            ePCmdLivreMontant.Clear();
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbCmdLivreNbExemplaires".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbCmdLivreNbExemplaires_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à la modification d'une commande livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreModifier_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
            DefaultCmdLivre = false;

            // Activer l'accès à la gestion de la commande
            AccesInfosCmdLivreGrpBox(true);
            ActiverChampsInfosCmdLivre();

            // Active la modification statut commande + récupère item suivi
            cbxCmdLivreSuivi.Enabled = true;
            recupSuiviCmdLivre = cbxCmdLivreSuivi.SelectedItem.ToString();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdLivreModifierOk, btnCmdLivreAnnuler };
            foreach (Control control in show) { control.Show(); }
            // Liste des contrôles à cacher

            Control[] hide = { btnCmdLivreCmd, btnCmdLivreModifier, btnCmdLivreSupprimer };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Valide ou non la modification d'une commande livre dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreModifierOk_Click(object sender, EventArgs e)
        {
            // vérifie que les champs nécessaires soient bien renseignés
            if (!txbCmdLivreIdCmd.Text.Equals("") && !txbCmdLivreMontant.Text.Equals("") && !txbCmdLivreNbExemplaires.Equals(""))
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
                    CommandeDocument commandeLivre = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelleSuivi);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de cette commande ?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (recupSuiviCmdLivre.Equals("réglée") || recupSuiviCmdLivre.Equals("livrée"))
                        {
                            if (libelleSuivi.Equals("en cours") || libelleSuivi.Equals("relancée"))
                            {
                                MessageBox.Show("Impossible de rétrograder une commande déjà " + recupSuiviCmdLivre, "Erreur");
                            }
                            else
                            {
                                if (controller.ModifierCommandeDocument(commandeLivre))
                                {
                                    MessageBox.Show("Modification de la commande n° " + id + " effectuée.", "Information");
                                }
                                else
                                {
                                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                                }
                            }
                        }
                        else if (!recupSuiviCmdLivre.Equals("livrée"))
                        {
                            if (libelleSuivi.Equals("réglée"))
                            {
                                MessageBox.Show("Une commande ne peut être réglée avant sa livraison", "Erreur");
                            }
                            else
                            {
                                if (controller.ModifierCommandeDocument(commandeLivre))
                                {
                                    MessageBox.Show("Modification de la commande n° " + id + " effectuée.", "Information");
                                }
                                else
                                {
                                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                                }
                            }
                        }
                        else
                        {
                            if (controller.ModifierCommandeDocument(commandeLivre))
                            {
                                MessageBox.Show("Modification de la commande n° " + id + " effectuée.", "Information");
                            }
                            else
                            {
                                MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification de la commande n° " + id + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                TabCommandesLivres_Enter(sender, e);
                RestaurationConfigCmdLivres();
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                ErrorProviderCmdLivre();
            }
        }

        /// <summary>
        /// Supprimer une commande livre dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreSupprimer_Click(object sender, EventArgs e)
        {
            // on récupère la position de la commande livre sélectionnée
            CommandeDocument commandeLivre = (CommandeDocument)bdgCommandesLivres.List[bdgCommandesLivres.Position];
            if (commandeLivre != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de cette commande ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (commandeLivre.IdSuivi != "00004")
                        {
                            if (controller.SupprimerCommandeDocument(commandeLivre))
                            {
                                MessageBox.Show("Suppression de la commande n° " + commandeLivre.Id + " effectuée.", "Information");
                            }
                            else
                            {
                                MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Impossible de supprimer une commande ayant été livrée.", "Information");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression de la commande n° " + commandeLivre.Id + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                TabCommandesLivres_Enter(sender, e);
                RestaurationConfigCmdLivres();
            }
            else
            {
                MessageBox.Show("Aucune commande sélectionnée.", "Erreur");
            }
        }

        /// <summary>
        /// Annulation de l'action en cours.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreAnnuler_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler votre demande?", "Information",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RestaurationConfigCmdLivres();
                TabCommandesLivres_Enter(sender, e);
            }
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion des commandes livres.   
        /// </summary>
        /// <param name="access">true ou false</param>
        private void AccesInfosCmdLivreGrpBox(bool access)
        {
            grpCmdLivreInfosCmd.Enabled = access;
        }

        /// <summary>
        /// Change l'état des champs informations détaillées commande d'un livre.
        /// </summary>
        private void SetReadOnlyCmdLivre(bool isReadOnly)
        {
            txbCmdLivreMontant.ReadOnly = isReadOnly;
            txbCmdLivreNbExemplaires.ReadOnly = isReadOnly;
            txbCmdLivreIdSuivi.ReadOnly = isReadOnly;
            txbCmdLivreIdCmd.ReadOnly = isReadOnly;
        }

        /// <summary>
        /// Activer les champs commande livre.
        /// </summary>
        private void ActiverChampsInfosCmdLivre()
        {
            SetReadOnlyCmdLivre(false);
            dtpCmdLivre.Enabled = true;
        }

        /// <summary>
        /// Désactive les champs commande livre.
        /// </summary>
        private void DesactiverChampsInfosCmdLivre()
        {
            SetReadOnlyCmdLivre(true);
            dtpCmdLivre.Enabled = false;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre sélectionné.
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
        /// Vide les zones d'affichage des infos de commande du livre sélectionné.
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
        /// Vide la totalité des infos livre et commande livre.
        /// </summary>
        private void ViderCmdLivreInfosTotal()
        {
            ViderCmdLivreInfos();
            ViderCmdLivreInfosCmd();
            DesactiverChampsInfosCmdLivre();
            txbCmdLivreNumRecherche.Text = "";
            cbxCmdLivreSuivi.Enabled = false;
        }
        #endregion

        #region Onglet Commandes DVD

        /// <summary>
        /// Ouverte de l'onglet Commandes DVD.
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
                ErrorProviderCmdDvdClear();
                grpCmdDvdInfosCmd.Enabled = false;
                grpCmdDvdGestion.Enabled = false;
            }
        }

        /// <summary>
        /// Remplit le datagrid commande DVD avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="lesCommandesDvd">liste des DVD commandés</param>
        private void RemplirCmdDvdListe(List<CommandeDocument> lesCommandesDvd)
        {
            if (DefaultCmdDvd && lesCommandesDvd != null)
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
            else
            {
                dgvCmdDvdListe.DataSource = null;
            }
        }

        /// <summary>
        /// Affiche les informations du DVD sélectionné dans les champs de texte correspondants.
        /// Affiche également les informations de commande du DVD.
        /// Charge l'image du livre dans le PictureBox pcbCmdDvd.
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
        /// Récupère les infos de commande du dvd via son numéro de recherche
        /// et initialise avec ces données le datagrid "dgvCmdDvdListe".
        /// </summary>
        public void AfficherCmdDvdInfosCmdGrid()
        {
            string idDocument = txbCmdDvdRechercheNum.Text;
            lesCommandesDocuments = controller.GetCommandesDocument(idDocument);
            RemplirCmdDvdListe(lesCommandesDocuments);
        }

        /// <summary>
        /// Récupère les infos de commande du dvd renseigné.
        /// Initialise les éléments correspondants.
        /// </summary>
        /// <param name="commandeDvd"></param>
        private void AfficherCmdDvdInfosCmd(CommandeDocument commandeDvd)
        {
            txbCmdDvdIdCmd.Text = commandeDvd.Id;
            cbxCmdDvdSuivi.Text = commandeDvd.LibelleSuivi;
            txbCmdDvdNbExemplaires.Text = commandeDvd.NbExemplaire.ToString();
            txbCmdDvdMontant.Text = commandeDvd.Montant.ToString();
            dtpCmdDvd.Value = commandeDvd.DateCommande;
        }

        /// <summary>
        /// Recherche et affiche les infos du dvd dont on a saisi le numéro.
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
                    grpCmdDvdInfosCmd.Enabled = true;
                    grpCmdDvdGestion.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Numéro introuvable.", "Information");
                    txbCmdDvdRechercheNum.Text = "";
                    grpCmdDvdInfosCmd.Enabled = false;
                    grpCmdDvdGestion.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer un numéro de DVD valide.", "Erreur");
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le datagrid,
        /// affichage des informations de commande DVD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (DefaultCmdDvd && dgvCmdDvdListe.CurrentCell != null)
            {
                try
                {
                    CommandeDocument commandeDvd = (CommandeDocument)bdgCommandesDvd.List[bdgCommandesDvd.Position];
                    AfficherCmdDvdInfosCmd(commandeDvd);
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
        /// Récupère l'idSuivi en fonction du combobox Suivi sélectionné
        /// lors de l'ajout, suppression ou modification d'une commande DVD.
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
        /// Tri sur les colonnes du datagrid Commandes Dvd.
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

        /// <summary>
        /// Restaure l'onglet Commandes Dvd à son état initial.
        /// </summary>
        public void RestaurationConfigCmdDvd()
        {
            // Active le lien avec le grid
            DefaultCmdDvd = true;
            dgvCmdDvdListe.Enabled = true;

            // Désactive l'accès à la gestion commande dvd et le vide
            DesactiverChampsInfosCmdDvd();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdDvdCommander, btnCmdDvdModifier, btnCmdDvdSupprimer };
            foreach (Control control in show) { control.Show(); }
            // Liste des contrôles à masquer
            Control[] hide = { btnCmdDvdCmdOk, btnCmdDvdModifierOk, btnCmdDvdAnnuler };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à l'ajout d'une commande DVD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdCmd_Click(object sender, EventArgs e)
        {
            if (!txbCmdDvdRechercheNum.Equals(""))
            {
                // Désactive le lien entre le datagrid et l'édition
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
            else
            {
                MessageBox.Show("Aucun DVD sélectionné.", "Erreur");
            }
        }

        /// <summary>
        /// Ajout d'une commande d'un DVD en BDD si les conditions sont remplies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdCmdOK_Click(object sender, EventArgs e)
        {
            // vérifie que les champs nécessaires soient bien renseignés
            if (!txbCmdDvdIdCmd.Text.Equals("") && !txbCmdDvdMontant.Text.Equals("") && !txbCmdDvdNbExemplaires.Equals(""))
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
                    // vérifie si le numéro de commande dvd existe en base de données
                    var checkIdCmdDvd = controller.GetCommandeId(id);
                    if (checkIdCmdDvd.Count != 0)
                    {
                        MessageBox.Show("La commande n° " + id + " existe déjà.", "Erreur");
                    }
                    else
                    {
                        if (controller.CreerCommandeDocument(commandeDvd))
                        {
                            MessageBox.Show("Commande du DVD " + idLivreDvd + " effectuée", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigCmdDvd();
                TabCommandesDvd_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires", "Information");
                ErrorProviderCmdDvd();
            }
        }

        /// <summary>
        /// Vérifie si les champs requis sont bien renseignés.
        /// Affiche un indicateur le cas contraire.
        /// </summary>
        public void ErrorProviderCmdDvd()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txbCmdDvdIdCmd.Text))
            {
                ePCmdDvdNumCmd.SetError(txbCmdDvdIdCmd, "Numéro de commande requis");
                isValid = false;
            }
            else
            {
                ePCmdDvdNumCmd.SetError(txbCmdDvdIdCmd, "");
            }

            if (string.IsNullOrEmpty(txbCmdDvdNbExemplaires.Text))
            {
                ePCmdDvdNbExemplaire.SetError(txbCmdDvdNbExemplaires, "Nombre d'exemplaires requis");
                isValid = false;
            }
            else
            {
                ePCmdDvdNbExemplaire.SetError(txbCmdDvdNbExemplaires, "");
            }

            if (string.IsNullOrEmpty(txbCmdDvdMontant.Text))
            {
                ePCmdDvdMontant.SetError(txbCmdDvdMontant, "Montant requis");
                isValid = false;
            }
            else
            {
                ePCmdDvdMontant.SetError(txbCmdDvdMontant, "");
            }

            if (isValid)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Efface tous les messages d'erreur affichés.
        /// </summary>
        public void ErrorProviderCmdDvdClear()
        {
            ePCmdDvdNumCmd.Clear();
            ePCmdDvdNbExemplaire.Clear();
            ePCmdDvdMontant.Clear();
        }

        /// <summary>
        /// Événement KeyPress pour le TextBox "txbCmdDvdNbExemplaires".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbCmdDvdNbExemplaires_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxOnlyNumbers_KeyPress(e, "Veuillez saisir des chiffres uniquement.");
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à la modification d'une commande DVD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdModifierDvd_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
            DefaultCmdDvd = false;

            // Active l'accès à la gestion commande
            AccesInfosCmdDvdGrpBox(true);
            ActiverChampsInfosCmdDvd();

            // Active la possibilité de modifier le statut de la commande + récupère item suivi
            cbxCmdDvdSuivi.Enabled = true;
            recupSuiviCmdDvd = cbxCmdDvdSuivi.SelectedItem.ToString();

            // Liste des contrôles à masquer 
            Control[] hide = { btnCmdDvdCommander, btnCmdDvdModifier, btnCmdDvdSupprimer };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = { btnCmdDvdModifierOk, btnCmdDvdAnnuler };
            foreach (Control control in show) { control.Show(); }
        }

        /// <summary>
        /// Valide ou non la modification d'une commande DVD dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdModifierOk_Click(object sender, EventArgs e)
        {
            // vérifie que les champs nécessaires soient bien renseignés
            if (!txbCmdDvdIdCmd.Text.Equals("") && !txbCmdDvdMontant.Text.Equals("") && !txbCmdDvdNbExemplaires.Equals(""))
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
                    CommandeDocument commandeDvd = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelleSuivi);
                    if (MessageBox.Show(this, "Confirmez-vous la modification cette commande ?", "Information",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (recupSuiviCmdDvd.Equals("réglée") || recupSuiviCmdDvd.Equals("livrée"))
                        {
                            if (libelleSuivi.Equals("en cours") || libelleSuivi.Equals("relancée"))
                            {
                                MessageBox.Show("Impossible de rétrograder une commande déjà " + recupSuiviCmdDvd, "Erreur");
                            }
                            else
                            {
                                if (controller.ModifierCommandeDocument(commandeDvd))
                                {
                                    MessageBox.Show("Modification de la commande n° " + id + "effectuée.", "Information");
                                }
                                else
                                {
                                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                                }
                            }
                        }
                        else if (recupSuiviCmdDvd.Equals("en cours") || recupSuiviCmdDvd.Equals("relancée"))
                        {
                            if (libelleSuivi.Equals("réglée"))
                            {
                                MessageBox.Show("Une commande ne peut être réglée avant sa livraison", "Erreur");
                            }
                            else
                            {
                                if (controller.ModifierCommandeDocument(commandeDvd))
                                {
                                    MessageBox.Show("Modification de la commande n° " + id + "effectuée.", "Information");
                                }
                                else
                                {
                                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                                }
                            }
                        }
                        else
                        {
                            if (controller.ModifierCommandeDocument(commandeDvd))
                            {
                                MessageBox.Show("Modification de la commande n° " + id + "effectuée.", "Information");
                            }
                            else
                            {
                                MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification de la commande n° " + id + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigCmdDvd();
                TabCommandesDvd_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                ErrorProviderCmdDvd();
            }
        }

        /// <summary>
        /// Supprimer une commande DVD dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdSupprimer_Click(object sender, EventArgs e)
        {
            // on récupère la position de la commande dvd sélectionnée
            CommandeDocument commandeDvd = (CommandeDocument)bdgCommandesDvd.List[bdgCommandesDvd.Position];
            if (commandeDvd != null)
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de cette commande ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (commandeDvd.IdSuivi != "00004")
                        {
                            if (controller.SupprimerCommandeDocument(commandeDvd))
                            {
                                MessageBox.Show("Suppression de la commande n° " + commandeDvd.Id + " effectuée.", "Information");
                            }
                            else
                            {
                                MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Impossible de supprimer une commande ayant été livrée.", "Information");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression de la commande n° " + commandeDvd.Id + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigCmdDvd();
                TabCommandesDvd_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Aucune commande sélectionnée.", "Erreur");
            }
        }

        /// <summary>
        /// Annulation de l'action en cours.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdAnnuler_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler votre demande?", "Information",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RestaurationConfigCmdDvd();
                TabCommandesDvd_Enter(sender, e);
            }
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion des commandes DVD.
        /// </summary>
        /// <param name="access"></param>
        private void AccesInfosCmdDvdGrpBox(bool access)
        {
            grpCmdDvdInfosCmd.Enabled = access;
        }

        /// <summary>
        /// Change l'état des champs informations détaillées commande DVD.
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
        /// Active les champs commande DVD.
        /// </summary>
        private void ActiverChampsInfosCmdDvd()
        {
            SetReadOnlyCmdDvd(false);
            dtpCmdDvd.Enabled = true;
        }

        /// <summary>
        /// Désactive les champs commande DVD.
        /// </summary>
        private void DesactiverChampsInfosCmdDvd()
        {
            SetReadOnlyCmdDvd(true);
            dtpCmdDvd.Enabled = false;
        }

        /// <summary>
        /// Vide les zones d'affiches des infos du DVD.
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
            pcbCmdDvd.Image = null;
        }

        /// <summary>
        /// Vide les zones d'affichage des infos de commande du DVD. 
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
        /// Vide la totalité des informations commande et DVD.
        /// </summary>
        private void ViderCmdDvdInfosTotal()
        {
            ViderCmdDvdInfos();
            ViderCmdDvdInfosCmd();
            DesactiverChampsInfosCmdDvd();
            txbCmdDvdRechercheNum.Text = "";
            cbxCmdDvdSuivi.Enabled = false;
        }
        #endregion

        #region Onglet Commandes Revues

        /// <summary>
        /// Ouverture de l'onglet Commandes Revues.
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
                ErrorProviderCmdRevueClear();
                grpCmdRevueInfosCmd.Enabled = false;
                grpCmdRevuesGestion.Enabled = false;
            }
        }

        /// <summary>
        /// Remplit le datagrid commande revue avec la liste reçue en paramètre.
        /// </summary>
        /// <param name="lesAbosRevues"></param>
        private void RemplirCmdRevueListe(List<Abonnement> lesAbosRevues)
        {
            if (DefaultCmdRevue && lesAbosRevues != null)
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
            else
            {
                dgvCmdRevueListe.DataSource = null;
            }
        }

        /// <summary>
        /// Affiche les informations de la revue sélectionnée dans les champs de texte correspondants.
        /// Affiche également les informations de commande d'une revue.
        /// Charge l'image du livre dans le PictureBox pcbCmdRevue.
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
        /// Récupère les infos de commande de la revue via son numéro de recherche
        /// et initialise avec ces données le datagrid "dgvCmdRevueListe".
        /// </summary>
        private void AfficherCmdRevueInfosCmdGrid()
        {
            string idRevue = txbCmdRevueNumRecherche.Text;
            lesAbonnementsRevues = controller.GetAbonnementsRevue(idRevue);
            RemplirCmdRevueListe(lesAbonnementsRevues);
        }

        /// <summary>
        /// Récupère les informations d'abonnement de la commande revue.
        /// </summary>
        /// <param name="abonnement"></param>
        private void RecupererCmdRevueInfosCmd(Abonnement abonnement)
        {
            txbCmdRevueIdCmd.Text = abonnement.Id;
            txbCmdRevueMontant.Text = abonnement.Montant.ToString();
            dtpCmdRevueDateCmd.Value = abonnement.DateCommande;
            dtpCmdRevueFinAbo.Value = abonnement.DateFinAbonnement;
        }

        /// <summary>
        /// Recherche et affiche les infos de la revue dont on a saisi le numéro.
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
                    grpCmdRevueInfosCmd.Enabled = true;
                    grpCmdRevuesGestion.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Numéro introuvable.", "Information");
                    txbCmdRevueNumRecherche.Text = "";
                    grpCmdRevueInfosCmd.Enabled = false;
                    grpCmdRevuesGestion.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer un numéro de revue valide.", "Erreur");
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le datagrid
        /// affichage des informations abonnement revue.
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
                    RecupererCmdRevueInfosCmd(abonnementRevue);
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
        /// Tri sur les colonnes du datagrid Commandes Dvd.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdRevueListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCmdDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Date commande":
                    sortedList = lesAbonnementsRevues.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesAbonnementsRevues.OrderBy(o => o.Montant).ToList();
                    break;
                case "Date de fin d'abonnement":
                    sortedList = lesAbonnementsRevues.OrderBy(o => o.Montant).ToList();
                    break;
            }
            RemplirCmdRevueListe(sortedList);
        }

        /// <summary>
        /// Restaure l'onglet Commandes Revues à son état initial.
        /// </summary>
        public void RestaurationConfigCmdRevues()
        {
            // Activer le lien avec le grid
            DefaultCmdRevue = true;
            dgvCmdRevueListe.Enabled = true;

            // Désactive l'accès à la gestion commande revues et le vide
            DesactiverChampsInfosCmdRevue();

            // Liste des contrôles à afficher
            Control[] show = { btnCmdRevueCmd, btnCmdRevueModifier, btnCmdRevueSupprimer };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à masquer
            Control[] hide = { btnCmdRevueCmdOk, btnCmdRevueModifierOk, btnCmdRevueAnnuler };
            foreach (Control control in hide) { control.Hide(); }
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à l'ajout d'une commande revue.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// </summary>
        private void BtnCmdRevueCmd_Click(object sender, EventArgs e)
        {
            if (!txbCmdRevueNumRecherche.Equals(""))
            {
                // Désactive le lien entre le datagrid et l'édition
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
                MessageBox.Show("Aucune revue sélectionnée.", "Erreur");
            }
        }

        /// <summary>
        /// Ajout d'une commande revue en BDD si les conditions sont remplies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueCmdOk_Click(object sender, EventArgs e)
        {
            // vérifie que les champs nécessaires soient bien renseignés
            if (!txbCmdRevueIdCmd.Equals("") && !txbCmdRevueMontant.Equals(""))
            {
                try
                {
                    string id = txbCmdRevueIdCmd.Text;
                    DateTime dateCommande = dtpCmdRevueDateCmd.Value;
                    DateTime dateFinAbonnement = dtpCmdRevueFinAbo.Value;
                    double montant = double.Parse(txbCmdRevueMontant.Text);
                    string idRevue = txbCmdRevueIdRevue.Text;
                    Abonnement abonnementRevue = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
                    // vérifie si le numéro d'abonnement existe en base de données
                    var checkIdCmdRevue = controller.GetAbonnementsRevue(id);
                    if (checkIdCmdRevue.Count != 0)
                    {
                        MessageBox.Show("L'abonnement n° " + id + " existe déjà.", "Erreur");
                        txbCmdRevueIdCmd.Focus();
                    }
                    else
                    {
                        if (controller.CreerAbonnement(abonnementRevue))
                        {
                            MessageBox.Show("Abonnement de la revue n° " + idRevue + " effectué.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");

                }
                RestaurationConfigCmdRevues();
                TabCommandesRevues_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                ErrorProviderCmdRevue();
            }
        }

        /// <summary>
        /// Vérifie si les champs requis sont bien renseignés.
        /// Affiche un indicateur le cas contraire.
        /// </summary>
        public void ErrorProviderCmdRevue()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txbCmdRevueIdCmd.Text))
            {
                ePCmdRevueNumCmd.SetError(txbCmdRevueIdCmd, "Numéro de commande requis");
                isValid = false;
            }
            else
            {
                ePCmdRevueNumCmd.SetError(txbCmdRevueIdCmd, "");
            }

            if (string.IsNullOrEmpty(txbCmdRevueMontant.Text))
            {
                ePCmdRevueMontant.SetError(txbCmdRevueMontant, "Montant requis");
                isValid = false;
            }
            else
            {
                ePCmdRevueMontant.SetError(txbCmdRevueMontant, "");
            }

            if (isValid)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Efface tous les messages d'erreur affichés.
        /// </summary>
        public void ErrorProviderCmdRevueClear()
        {
            ePCmdRevueNumCmd.Clear();
            ePCmdRevueMontant.Clear();
        }

        /// <summary>
        /// Initialise les champs nécessaires ou non à la modification d'une commande revue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueModifier_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre le datagrid et l'édition
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
        /// Modification d'une commande de type revue dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueModifierOk_Click(object sender, EventArgs e)
        {
            // vérifie que les champs nécessaires soient bien renseignés
            if (!txbCmdRevueIdCmd.Equals("") && !txbCmdRevueMontant.Equals(""))
            {
                try
                {
                    string id = txbCmdRevueIdCmd.Text;
                    DateTime dateCommande = dtpCmdRevueDateCmd.Value;
                    double montant = double.Parse(txbCmdRevueMontant.Text);
                    DateTime dateFinAbonnement = dtpCmdRevueFinAbo.Value;
                    string idRevue = txbCmdRevueIdRevue.Text;
                    Abonnement abonnementRevue = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
                    if (MessageBox.Show(this, "Confirmez-vous la modification de cette commande ?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.ModifierCommandeRevue(abonnementRevue))
                        {
                            MessageBox.Show("Modification de la commande n° " + id + "effectuée.", "Information");
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Modification de la commande n° " + id + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigCmdRevues();
                TabCommandesRevues_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires.", "Information");
                ErrorProviderCmdRevue();
            }
        }

        /// <summary>
        /// Supprimer une commande revue dans la BDD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueSupprimer_Click(object sender, EventArgs e)
        {
            // on récupère la position de l'abonnement revue sélectionné
            Abonnement abonnement = (Abonnement)bdgCommandesRevues.List[bdgCommandesRevues.Position];
            if (VerifierLienAbonnementExemplaire(abonnement))
            {
                try
                {
                    if (MessageBox.Show(this, "Confirmez-vous la suppression de l'abonnement " + abonnement.IdRevue + "?", "Attention",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (controller.SupprimerAbonnement(abonnement))
                        {
                            MessageBox.Show("Suppression de l'abonnement " + abonnement.Id + " effectuée.", "Information");
                            AfficherCmdRevueInfosCmdGrid();
                        }
                        else
                        {
                            MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Suppression de l'abonnement n° " + abonnement.Id + " annulée.", "Information");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur, veuillez recommencer ou contacter un administrateur.", "Erreur");
                }
                RestaurationConfigCmdRevues();
                TabCommandesRevues_Enter(sender, e);
            }
            else
            {
                MessageBox.Show("Impossible de supprimer un abonnement contenant un ou plusieurs exemplaires.", "Erreur");
            }
        }

        /// <summary>
        /// Annulation de l'action en cours.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueAnnuler_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Voulez-vous annuler votre demande?", "Information",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RestaurationConfigCmdRevues();
                TabCommandesRevues_Enter(sender, e);
            }
        }

        // <summary>
        /// Vérifie si la date de parution est comprise entre date commande et date fin abonnement.
        /// <param name="dateCommande">Date de prise de commande</param>
        /// <param name="dateFinAbonnement">Date de fin d'abonnement</param>
        /// <param name="dateParution">Date de comparaison entre les deux précédentes</param>
        /// <returns>true si la date est comprise entre ces deux dates</returns>
        public bool ParutionAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return (DateTime.Compare(dateCommande, dateParution) < 0 && DateTime.Compare(dateParution, dateFinAbonnement) < 0);
        }

        /// <summary>
        /// Vérifie qu'aucun exemplaire ne soit rattaché à un abonnement.
        /// </summary>
        /// <param name="abonnement">l'abonnement cible</param>
        /// <returns>return true si aucun exemplaire rattaché</returns>
        public bool VerifierLienAbonnementExemplaire(Abonnement abonnement)
        {
            List<Exemplaire> lesExemplairesLienAbo = controller.GetExemplairesDocument(abonnement.IdRevue);
            bool supprimer = false;
            foreach (Exemplaire exemplaire in lesExemplairesLienAbo.Where(exemplaires => ParutionAbonnement
            (abonnement.DateCommande, abonnement.DateFinAbonnement, exemplaires.DateAchat)))
            {
                supprimer = true;
            }
            return !supprimer;
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion des commandes revues.
        /// </summary>
        /// <param name="access"></param>
        private void AccesInfosCmdRevueGrpBox(bool access)
        {
            grpCmdRevueInfos.Enabled = access;
        }

        /// <summary>
        /// Gère l'accès à la gestion commande revues.
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void SetReadOnlyCmdRevue(bool isReadOnly)
        {
            txbCmdRevueMontant.ReadOnly = isReadOnly;
            txbCmdRevueIdCmd.ReadOnly = isReadOnly;
        }

        /// <summary>
        /// Active les champs commande revues.
        /// </summary>
        private void ActiverChampsInfosCmdRevue()
        {
            SetReadOnlyCmdRevue(false);
            dtpCmdRevueDateCmd.Enabled = true;
            dtpCmdRevueFinAbo.Enabled = true;
        }

        /// <summary>
        /// Désactive les champs commande revues.
        /// </summary>
        private void DesactiverChampsInfosCmdRevue()
        {
            SetReadOnlyCmdRevue(true);
            dtpCmdRevueDateCmd.Enabled = false;
            dtpCmdRevueFinAbo.Enabled = false;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations revues.
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
        /// Vide les zones d'affichages des infos de commande de la revue sélectionnée.
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
        /// Vide la totalité des infos revues et commandes revues.
        /// </summary>
        private void ViderCmdRevueInfosTotal()
        {
            ViderCmdRevueInfos();
            ViderCmdRevueInfosCmd();
            DesactiverChampsInfosCmdRevue();
            txbCmdRevueNumRecherche.Text = "";
        }
        #endregion
    }
}
