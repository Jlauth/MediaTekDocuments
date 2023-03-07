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
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgSuivis = new BindingSource();
        private readonly BindingSource bdgCommandesLivres = new BindingSource();
        private readonly BindingSource bdgCommandesDvd = new BindingSource();
        private readonly BindingSource bdgCommandesRevues = new BindingSource();

        private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();
        private List<CommandeDocument> lesCommandesRevues = new List<CommandeDocument>();

        private bool DefaultLivre = true;
        private bool DefaultDvd = true;
        private bool DefaultRevue = true;
        private bool DefaultCommandeLivre = true;
        private bool DefaultCommandeDvd = true;
        private bool DefaultCommandeRevue = true;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
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
        /// <param name="bdg">binding source contenant les informations</param>
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

        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
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
                RemplirLivresListeComplete();
            }
        }


        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
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
        /// Recherche et affichage des livres dont le titre matche avec la saisie.
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
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
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
        /// Afin de pas pouvoir les éditer
        /// </summary>
        private void DesactiverChampsInfosLivre()
        {
            SetReadOnlyLivre(true);
        }

        private void ActiverChampsInfosLivre()
        {
            SetReadOnlyLivre(false);
        }

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
        /// Permet ou interdit l'accès à la gestion côté Livres
        /// </summary>
        /// <param name="acces">True ou False</param>
        private void AccesInformationsLivresGroupBox(bool acces)
        {
            grpLivresInfos.Enabled = acces;

        }

        /// <summary>
        /// Tri sur les colonnes
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
        /// Uniquement en mode edition
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
        /// Uniquement en mode edition
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
        /// En mode edition uniquement
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
        /// Actualiser l'UI pour ajouter un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouterLivre_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre grid et edition
            DefaultLivre = false;
            // Vider tous les champs et activer les champs informations détaillées
            VideLivresInfosTotal();
            ActiverChampsInfosLivre();
            AccesInformationsLivresGroupBox(true);
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
            // Focus sur le champ "ID livre" et vider les champs des ID categories
            txbLivresNumero.Focus();
            txbLivresIdGenre.Clear();
            txbLivresIdRayon.Clear();
            txbLivresIdPublic.Clear();

        }

        /// <summary>
        /// Ajout d'un livre si non présent en BDD
        /// Ou refuse le cas échéant
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
        /// Restauration onglet Livre initial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerLivre_Click(object sender, EventArgs e)
        {
            RestaurationConfigLivres();
            TabLivres_Enter(sender, e);
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'un Livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierLivre_Click(object sender, EventArgs e)
        {
            // Désactive le lien avec le grid en mode edition
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
        /// Valide ou non la modification d'un livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValiderModifierLivre_Click(object sender, EventArgs e)
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
                        MessageBox.Show("Modification du livre " + livre.Titre + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Modification du livre " + livre.Titre + " annulée");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un livre valide");
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
                    Console.WriteLine("Erreur sur la suppression de ce livre, veuillez recommencer");
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
        /// Restaure l'onglet Livres à son stade initial
        /// </summary>
        private void RestaurationConfigLivres()
        {
            // Activer le lien avec le grid
            DefaultLivre = true;
            // Liste des contrôles à masquer
            Control[] hide = { txbLivresIdGenre, txbLivresIdPublic, txbLivresIdRayon,
                lblLivresIdGenre, lblLivresIdPublic, lblLivresIdRayon,
                cbxLivresGenreEdit, cbxLivresPublicEdit, cbxLivresRayonEdit,
                btnAnnulerLivre, btnValiderAjouterLivre
            };
            foreach (Control control in hide)
            {
                control.Hide();
            }

            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterLivre, btnModifierLivre, btnSupprimerLivre,
                cbxLivresGenres, cbxLivresPublics, cbxLivresRayons
            };
            foreach (Control control in show)
            {
                control.Show();
            }
            DesactiverChampsInfosLivre();
        }



        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvds = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabDvd_Enter(object sender, EventArgs e)
        {
            if (DefaultDvd)
            {
                lesDvds = controller.GetAllDvd();
                RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
                RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
                RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> dvds)
        {
            bdgDvdListe.DataSource = dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du dvds dont on a saisi le numéro.
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
                Dvd dvd = lesDvds.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> dvds = new List<Dvd>() { dvd };
                    RemplirDvdListe(dvds);
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
        /// Recherche et affichage des dvds dont le titre matche acec la saisie.
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
                lesDvdsParTitre = lesDvds.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdsParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
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
        /// Vide les zones d'affichage des informations du dvd
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
                List<Dvd> Dvd = lesDvds.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère de l'idGenre en fonction du genre select cbx
        /// Uniquement en mode edition
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
                List<Dvd> Dvd = lesDvds.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Récupère l'idPublic en fonction du public select en cbx
        /// Uniquement en mode edition
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
                List<Dvd> Dvd = lesDvds.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// Récupère l'idRayon en fonction du rayon select en cbx
        /// En mode edition uniquement
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
        /// affichage des informations du dvd
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
        /// Actualiser l'UI pour ajouter un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouterDvd_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre grid et edition
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
            // Focus sur le champ "ID DVD" et vider les champs des ID categorie
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
        }

        /// <summary>
        /// Annule l'ajout d'un dvd
        /// Restauration onglet Dvd initial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerAjoutDvd_Click(object sender, EventArgs e)
        {
            RestaurationConfigDvd();
            TabDvd_Enter(sender, e);
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'un dvd
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
            // Activer le groupe de contrôles "Informations Dvd"
            ActiverChampsInfosDvd();
            // Focus sur le champ "Durée" et vider les champs des ID
            txbDvdNumero.ReadOnly = true;
            txbDvdDuree.Focus();
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
                        MessageBox.Show("Modification du dvd " + dvd.Titre + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Modification du dvd " + dvd.Titre + " annulée");
                    }
                }
                catch
                {
                    MessageBox.Show("Erreur interne, veuillez recommencer");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un dvd valide");
            }
            RestaurationConfigDvd();
            TabDvd_Enter(sender, e);
        }

        /// <summary>
        /// Supprimer un dvd dans la BDD
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
                    if (MessageBox.Show(this, "Confirmez-vous la suppression du dvd " + dvd.Titre + " ?", "ATTENTION",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        controller.SupprimerDvd(dvd);
                        MessageBox.Show("Suppression du dvd " + dvd.Titre + " effectuée");
                    }
                    else
                    {
                        MessageBox.Show("Suppression du dvd " + dvd.Titre + " annulée");
                    }

                }
                catch
                {
                    Console.WriteLine("Erreur sur la suppression de ce dvd, veuillez recommencer");
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
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des dvds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des dvds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des dvds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des dvds
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvds);
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
        /// Vide la totalité des informations dvd
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
                btnValiderAjouterDvd, btnValiderModifierDvd,  btnAnnulerAjouterDvd,  btnAnnulerModifierDvd
            };
            // Masquer tous les contrôles de la liste
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterDvd, btnModifierDvd, btnSupprimerDvd,
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
        /// Afin de pas pouvoir les éditer
        /// </summary>
        private void DesactiverChampsInfosDvd()
        {
            SetReadOnlyDvd(true);
        }

        private void ActiverChampsInfosDvd()
        {
            SetReadOnlyDvd(false);
        }

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
        /// Permet ou interdit l'accès à la gestion côté DVD
        /// </summary>
        /// <param name="acces"></param>
        private void AccesInformationsDvdGroupBox(bool acces)
        {
            grpDvdInfos.Enabled = acces;
        }

        /// <summary>
        /// Tri sur les colonnes
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
                    sortedList = lesDvds.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvds.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvds.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvds.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvds.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvds.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvds.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
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
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
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
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
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
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
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
        /// Vide les zones d'affichage des informations de la reuve
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
        /// Uniquement en mode edition
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
        /// Uniquement en mode edition
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
        /// En mode edition uniquement
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

        private void BtnAjouterRevue_Click(object sender, EventArgs e)
        {
            // Désactive le lien entre grid et edition
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
            // Masquer tous les contrôles de la liste
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Liste des contrôles à afficher
            Control[] show = {
                btnAnnulerRevue, btnValiderAjouterRevue,
                txbRevuesIdGenre, txbRevuesIdPublic, txbRevuesIdRayon,
                cbxRevuesGenreEdit, cbxRevuesPublicEdit, cbxRevuesRayonEdit,
                lblRevueIdGenre, lblRevueIdPublic, lblRevueIdRayon
            };
            // Afficher tous les contrôles de la liste
            foreach (Control control in show)
            {
                control.Show();
            }
            // Remplir les cbx ajoutés avec la BDD
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenreEdit);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublicEdit);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayonEdit);
            // Focus sur le champ "ID livre" et vider les champs des ID categories
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
        /// Restauration onglet Revue initial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAnnulerAjoutRevue_Click(object sender, EventArgs e)
        {
            RestaurationConfigRevues();
            TabRevues_Enter(sender, e);
        }

        /// <summary>
        /// Active les champs nécessaires à la modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifierRevue_Click(object sender, EventArgs e)
        {
            DefaultRevue = false;
            Control[] show = {
            btnValiderModifierRevue, btnAnnulerRevue,
            cbxRevuesPublicEdit, cbxRevuesGenreEdit, cbxRevuesRayonEdit,
            };
            foreach (Control control in show)
            {
                control.Show();
            }

            Control[] hide =
            {
                btnModifierRevue, btnSupprimerRevue, btnAjouterRevue,
                cbxRevuesGenres, cbxRevuesPublics, cbxRevuesRayons
            };
            foreach (Control control in hide)
            {
                control.Hide();
            }
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
                    Console.WriteLine("Erreur sur la suppression de cette revue, veuillez recommencer");
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
        /// Restaure l'UI revues initiale
        /// </summary>
        private void RestaurationConfigRevues()
        {
            DefaultRevue = true;
            // Liste des contrôles à masquer
            Control[] hide = { txbRevuesIdGenre, txbRevuesIdPublic, txbRevuesIdRayon,
                lblRevueIdGenre, lblRevueIdPublic, lblRevueIdRayon,
                cbxRevuesGenreEdit, cbxRevuesPublicEdit, cbxRevuesRayonEdit,
                btnAnnulerRevue, btnValiderAjouterRevue, btnValiderModifierRevue
            };
            // Masquer tous les contrôles de la liste
            foreach (Control control in hide)
            {
                control.Hide();
            }
            // Liste des contrôles à afficher
            Control[] show = {
                btnAjouterRevue, btnModifierRevue, btnSupprimerRevue,
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
        private void DesactiverChampsInfosRevues()
        {
            SetReadOnlyRevues(true);
        }

        private void ActiverChampsInfosRevues()
        {
            SetReadOnlyRevues(false);
        }

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
                case "Periodicite":
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
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le datagrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
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
                    MessageBox.Show("numéro introuvable");
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
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
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
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
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
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
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
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
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

        #endregion

        #region Onglet Commandes Livres

        /// <summary>
        /// Ouverture de l'onglet Commandes de Livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string recupSuivi;

        private void TabCommandesLivres_Enter(object sender, EventArgs e)
        {
            if (DefaultCommandeLivre)
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
        /// <param name="lesCommandesLivres">liste des livres commandés</param>
        private void RemplirCmdLivreListe(List<CommandeDocument> lesCommandesLivres)
        {
            bdgCommandesLivres.DataSource = lesCommandesLivres;
            dgvCmdLivreListe.DataSource = bdgCommandesLivres;
            dgvCmdLivreListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCmdLivreListe.Columns["IdLivreDvd"].HeaderText = "Numéro livre";
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
        private void BtnCmdLivreRechercheNum_Click(object sender, EventArgs e)
        {
            if (DefaultCommandeLivre && !txbCmdLivreRechercheNum.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbCmdLivreRechercheNum.Text.ToString()));
                if (livre != null)
                {
                    AfficherCmdLivreInfos(livre);
                    RemplirCmdLivreListe(lesCommandesLivres);
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
        /// ARécupère ett affiche les infos de commande du livre  dans le datagrid
        /// </summary>
        public void AfficherCmdLivreInfoCmdGrid()
        {
            string idDocument = txbCmdLivreRechercheNum.Text;
            lesCommandesLivres = controller.GetCommandeDocument(idDocument);
            RemplirCmdLivreListe(lesCommandesLivres);
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
            if (DefaultCommandeLivre && dgvCmdLivreListe.CurrentCell != null)
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
            txbCmdLivreRechercheNum.Text = "";
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
            txbCmdLivreRechercheNum.Text = "";
            dtpCmdLivre.Value = DateTime.Now;

        }

        /// <summary>
        /// Vide la totalité des infos commmande et livre
        /// </summary>
        private void ViderCmdLivreInfosTotal()
        {
            ViderCmdLivreInfos();
            ViderCmdLivreInfosCmd();
            DesactiverChampsInfosCmdLivre();
            txbCmdLivreRechercheNum.Text = "";
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

        private void ActiverChampsInfosCmdLivre()
        {
            SetReadOnlyCmdLivre(false);

        }

        private void DesactiverChampsInfosCmdLivre()
        {
            SetReadOnlyCmdLivre(true);
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
        /// Restaure les paramètres cmd Livres à son état initial
        /// </summary>
        public void RestaureConfigCmdLivres()
        {
            // Activer le lien avec le grid
            DefaultCommandeLivre = true;
            dgvCmdLivreListe.Enabled = true;

            // Désactive l'accès à la gestion commande et le vide
            DesactiverChampsInfosCmdLivre();
            //ViderCmdLivreInfosCmd();
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
            DefaultCommandeLivre = false;
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
            if (!txbCmdLivreRechercheNum.Equals(""))
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
                    if (controller.CreerCommandeDocument(commandeLivre))
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
            DefaultCommandeLivre = false;

            // Activer l'accès à la gestion de la commande
            AccesInfosCmdLivreGrpBox(true);
            ActiverChampsInfosCmdLivre();

            // Active la modification status commande + récupère item
            cbxCmdLivreSuivi.Enabled = true;
            recupSuivi = cbxCmdLivreSuivi.SelectedItem.ToString();
            Console.WriteLine(recupSuivi);

            // Liste des contrôles à afficher
            Control[] show = { btnCmdLivreModifierOk, btnCmdLivreAnnuler };
            foreach (Control control in show) { control.Show(); }

            // Liste des contrôles à cacher
            Control[] hide = { btnCmdLivreCmd, btnCmdLivreModifier, btnCmdLivreSupprimer };
            foreach (Control control in hide) { control.Hide(); }


        }


        /// Valide ou non la modification d'une commande livre dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdLivreModifierOk_Click(object sender, EventArgs e)
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
                    if (MessageBox.Show(this, "Confirmez-vous la modification cette commande ?", "INFORMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
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
            CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesLivres.List[bdgCommandesLivres.Position];
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
                    Console.WriteLine("Erreur sur la suppression de cette commande, veuillez recommencer");
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
                    sortedList = lesCommandesLivres.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Montant).ToList();
                    break;
                case "Nb exemplaires":
                    sortedList = lesCommandesLivres.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesLivres.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }
            RemplirCmdLivreListe(sortedList);
        }



        #endregion

        #region Onglet Commandes Dvd

        /// <summary>
        /// Ouverte de l'onglet Commandes DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCommandesDvd_Enter(object sender, EventArgs e)
        {
            if (DefaultCommandeDvd)
            {
                lesDvds = controller.GetAllDvd();
                RemplirComboSuivi(controller.GetAllSuivis(), bdgSuivis, cbxCmdDvdSuivi);
                dgvCmdDvdListe.DataSource = null;
                ViderCmdDvdInfosTotal();
            }
        }


        /// <summary>
        /// Remplit le datagrid commande dvd avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesDvd">liste des dvd commandés</param>
        private void RemplirCmdDvdListe(List<CommandeDocument> lesCommandesDvd)
        {
            bdgCommandesDvd.DataSource = lesCommandesDvd;
            dgvCmdDvdListe.DataSource = bdgCommandesDvd;
            dgvCmdDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCmdDvdListe.Columns["IdLivreDvd"].Visible = false;
            dgvCmdDvdListe.Columns["IdSuivi"].Visible = false;
            dgvCmdDvdListe.Columns["Id"].Visible = false;
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
            if (DefaultCommandeDvd && !txbCmdDvdRechercheNum.Text.Equals(""))
            {
                Dvd dvd = lesDvds.Find(x => x.Id.Equals(txbCmdDvdRechercheNum.Text.ToString()));
                if (dvd != null)
                {
                    AfficherCmdDvdInfos(dvd);
                    RemplirCmdDvdListe(lesCommandesDvd);
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
        /// Affiches les informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
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
        /// Récupère et affiche les infos de commande du dvd  dans le datagrid
        /// </summary>
        public void AfficherCmdDvdInfosCmdGrid()
        {
            string idDocument = txbCmdDvdRechercheNum.Text;
            lesCommandesDvd = controller.GetCommandeDocument(idDocument);
            RemplirCmdDvdListe(lesCommandesDvd);
        }

        private void AfficherCmdDvdInfosCmd(CommandeDocument commandeDocument)
        {
            txbCmdDvdIdCmd.Text = commandeDocument.Id;
            cbxCmdDvdSuivi.Text = commandeDocument.LibelleSuivi;
            txbCmdDvdNbExemplaires.Text = commandeDocument.NbExemplaire.ToString();
            txbCmdDvdMontant.Text = commandeDocument.Montant.ToString();
            dtpCmdDvd.Value = commandeDocument.DateCommande;
        }

        /// <summary>
        /// Affiche le détail de la commande dvd en fonction de sa position datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (DefaultCommandeDvd && dgvCmdDvdListe.CurrentCell != null)
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
        /// Vide les zones d'affiches des infos du dvd
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
        /// Vide les zones d'affichage des infos de commande du dvd 
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
        /// Vide la totalité des informations commande et dvd
        /// </summary>
        private void ViderCmdDvdInfosTotal()
        {
            ViderCmdDvdInfos();
            ViderCmdDvdInfosCmd();
            DesactiverChampsInfosCmdDvd();
            txbCmdDvdRechercheNum.Text = "";
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion des commandes dvd
        /// </summary>
        /// <param name="access"></param>
        private void AccesInfoCmdDvdGrpBox(bool access)
        {
            grpCmdDvdCmdInfos.Enabled = access;
        }

        /// <summary>
        /// Change l'état des champs informations détaillées commande d'un dvd
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void SetReadOnlyCmdDvd(bool isReadOnly)
        {
            txbCmdDvdMontant.ReadOnly = isReadOnly;
            txbCmdDvdNbExemplaires.ReadOnly = isReadOnly;
            txbCmdDvdIdCmd.ReadOnly = isReadOnly;
        }

        private void ActiverChampsInfosCmdDvd()
        {
            SetReadOnlyCmdDvd(false);
        }

        private void DesactiverChampsInfosCmdDvd()
        {
            SetReadOnlyCmdDvd(true);
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
        /// Restaure les paramètres cmd Dvd à son état initial
        /// </summary>
        public void RestaureConfigCmdDvd()
        {
            // Activer le lien avec le grid
            DefaultCommandeDvd = true;
            // Désactive l'accès à la gestion commande et le vide
            DesactiverChampsInfosCmdDvd();
            ViderCmdDvdInfosCmd();
            // Liste des contrôles à afficher
            Control[] show = { btnCmdDvdCommander, btnCmdDvdModifier, btnCmdDvdSupprimer };
            foreach (Control control in show) { control.Show(); }
            // Liste des contrôles à masquer
            Control[] hide = { btnCmdDvdCmdOk, btnCmdDvdModifierOk, btnCmdDvdAnnuler };
            foreach (Control control in hide) { control.Hide(); }
        }


        /// <summary>
        /// Active les champs nécessaires à la création d'une commande dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdCommander_Click(object sender, EventArgs e)
        {
            // Désactice le datagrid
            DefaultCommandeDvd = false;
            // Active l'accès à la gestion commande
            AccesInfoCmdDvdGrpBox(true);
            ActiverChampsInfosCmdDvd();
            // Set l'index du cbxSuivi à "en cours"
            cbxCmdDvdSuivi.SelectedIndex = 0;
            // Liste des contrôles à masquer 
            Control[] hide = { btnCmdDvdCommander, btnCmdDvdModifier, btnCmdDvdSupprimer };
            foreach (Control control in hide) { control.Hide(); }
            // Liste des contrôles à afficher
            Control[] show = { btnCmdDvdCmdOk, btnCmdDvdAnnuler };
            foreach (Control control in show) { control.Show(); }

        }

        /// <summary>
        /// Ajout d'une commande d'un dvd en BDD
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
                MessageBox.Show("Aucun dvd sélectionné");
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
        /// Active les champs nécessaires à la modification d'une commande dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdModifierDvd_Click(object sender, EventArgs e)
        {
            // Désactive le datagrid
            DefaultCommandeDvd = false;
            // Active l'accès à la gestion commande
            AccesInfoCmdDvdGrpBox(true);
            ActiverChampsInfosCmdDvd();
            // Active la possibilité de modifier le status de la commande
            cbxCmdDvdSuivi.Enabled = true;
            // Liste des contrôles à masquer 
            Control[] hide = { btnCmdDvdCommander, btnCmdDvdModifier, btnCmdDvdSupprimer };
            foreach (Control control in hide) { control.Hide(); }
            // Liste des contrôles à afficher
            Control[] show = { btnCmdDvdModifierOk, btnCmdDvdAnnuler };
            foreach (Control control in show) { control.Show(); }
        }

        /// <summary>
        /// Valide ou non la modification d'une commande dvd dans la BDD
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
        /// Supprimer une commande dvd dans la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCmdDvdSupprimer_Click(object sender, EventArgs e)
        {
            CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesDvd.List[bdgCommandesDvd.Position];
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
                    Console.WriteLine("Erreur sur la suppression de cette commande, veuillez recommencer");
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
                    sortedList = lesCommandesLivres.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Montant).ToList();
                    break;
                case "Nb exemplaires":
                    sortedList = lesCommandesLivres.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesLivres.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }
            RemplirCmdDvdListe(sortedList);
        }
        #endregion

        #region Onglet Commandes Revues

        private void TabCommandesRevues_Enter(object sender, EventArgs e)
        {
            if (DefaultCommandeRevue)
            {
                lesRevues = controller.GetAllRevues();
                RemplirComboSuivi(controller.GetAllSuivis(), bdgSuivis, cbxCmdRevueSuivi);
                dgvCmdRevueListe.DataSource = null;
                ViderCmdRevueInfosTotal();
            }
        }

        /// <summary>
        /// Remplit le datagrid commande revue avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesRevues"></param>
        private void RemplirCmdRevueListe(List<CommandeDocument> lesCommandesRevues)
        {
            bdgCommandesRevues.DataSource = lesCommandesRevues;
            dgvCmdRevueListe.DataSource = bdgCommandesRevues;
            dgvCmdRevueListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Recherche et affiche les infos de la revue renseignée
        /// </summary>
        /// <param name="sendr"></param>
        /// <param name="e"></param>
        private void BtnCmdRevueNumRecherche_Click(object sender, EventArgs e)
        {
            if (DefaultCommandeRevue && !txbCmdRevueNumRecherche.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbCmdRevueNumRecherche.Text.ToString()));
                if (revue != null)
                {
                    AfficherCmdRevueInfos(revue);
                    RemplirCmdRevueListe(lesCommandesRevues);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer un numéro de livre valide");
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
        }

        private void AfficherCmdRevueInfosCmdGrid()
        {
            string idDocument = txbCmdRevueNumRecherche.Text;
            lesRevues = controller.GetAllRevues();
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
            pcbCmdRevue.Image = null;
        }

        /// <summary>
        /// Vide les zones d'affichages des infos de commande de la revue sélectionnée
        /// </summary>
        private void ViderCmdRevueInfosCmd()
        {
            txbCmdRevueIdCmd.Text = "";
            txbCmdRevueNbExemplaires.Text = "";
            txbCmdRevueMontant.Text = "";
            cbxCmdRevueSuivi.SelectedIndex = -1;
            txbCmdRevueNumRecherche.Text = "";
            dtpCmdRevue.Value = DateTime.Now;
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

        private void SetReadOnlyCmdRevue(bool isReadOnly)
        {
            txbCmdRevueMontant.ReadOnly = isReadOnly;
            txbCmdRevueNbExemplaires.ReadOnly = isReadOnly;
            txbCmdRevueIdSuivi.ReadOnly = isReadOnly;
            txbCmdRevueIdCmd.ReadOnly = isReadOnly;
        }

        private void ActiverChampsInfosCmdRevue()
        {
            SetReadOnlyCmdRevue(false);
        }

        private void DesactiverChampsInfosCmdRevue()
        {
            SetReadOnlyCmdRevue(true);
        }

        /// <summary>
        /// Récupère l'id du suivi en fonction de la sélection du cbx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxCmdRevueLibelleSuivi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCmdRevueSuivi.SelectedIndex >= 0)
            {
                Suivi suivi = (Suivi)cbxCmdRevueSuivi.SelectedItem;
                txbCmdRevueIdSuivi.Text = suivi.Id;
            }
        }

        /// <summary>
        /// Restaure les paramètres cmd Revues à son état initial
        /// </summary>
        public void RestaureConfigCmdRevues()
        {
            // Activer le lien avec le grid
            DefaultCommandeRevue = true;
            dgvCmdRevueListe.Enabled = true;
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
            // Désactive le lien datagrid
            DefaultCommandeRevue = false;
            dgvCmdRevueListe.Enabled = false;

            // Active l'accès à la gestion de la commande
            AccesInfosCmdRevueGrpBox(true);
            ActiverChampsInfosCmdRevue();
            ViderCmdRevueInfosCmd();
            cbxCmdRevueSuivi.SelectedIndex = 0;

            // Liste des contrôles à masquer
            Control[] hide = { btnCmdRevueCmd, btnCmdRevueModifier, btnCmdRevueSupprimer };
            foreach (Control control in hide) { control.Hide(); }

            // Liste des contrôles à afficher
            Control[] show = { btnCmdRevueCmdOk, btnCmdRevueAnnuler };
            foreach (Control control in show) { control.Show(); }
        }

        private void BtnCmdRevueCmdOk_Click(object sender, EventArgs e)
        {
            if (!txbCmdLivreRechercheNum.Equals(""))
            {
                try
                {
                    string id = txbCmdRevueIdCmd.Text;
                    DateTime dateCommande = dtpCmdRevue.Value;
                    double montant = double.Parse(txbCmdRevueMontant.Text);
                    int nbExemplaires = int.Parse(txbCmdRevueNbExemplaires.Text);
                    string idLivreDvd = txbCmdRevueIdRevue.Text;
                    string idSuivi = txbCmdRevueIdSuivi.Text;
                    string libelleSuivi = cbxCmdRevueSuivi.SelectedItem.ToString();
                    CommandeDocument commandeRevue = new CommandeDocument(id, dateCommande, montant, nbExemplaires, idLivreDvd, idSuivi, libelleSuivi);
                    if (controller.CreerCommandeDocument(commandeRevue))
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
                MessageBox.Show("Aucune revue sélectionnée");
            }
            RestaureConfigCmdRevues();
            TabCommandesRevues_Enter(sender, e);
        }

        #endregion
     
    }
}
