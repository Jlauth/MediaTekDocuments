using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    partial class FrmEcheancesAbos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEcheancesAbos));
            this.dgvEcheancesAbosListe = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOkEcheancesAbos = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEcheancesAbosListe)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvEcheancesAbosListe
            // 
            this.dgvEcheancesAbosListe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEcheancesAbosListe.Location = new System.Drawing.Point(71, 75);
            this.dgvEcheancesAbosListe.Name = "dgvEcheancesAbosListe";
            this.dgvEcheancesAbosListe.ReadOnly = true;
            this.dgvEcheancesAbosListe.Size = new System.Drawing.Size(529, 153);
            this.dgvEcheancesAbosListe.TabIndex = 1;
            this.dgvEcheancesAbosListe.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvEcheancesAbos_ColumnHeaderMouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Abonnement(s) arrivant à expiration dans 30 jours ou moins";
            // 
            // btnOkEcheancesAbos
            // 
            this.btnOkEcheancesAbos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOkEcheancesAbos.Location = new System.Drawing.Point(452, 250);
            this.btnOkEcheancesAbos.Name = "btnOkEcheancesAbos";
            this.btnOkEcheancesAbos.Size = new System.Drawing.Size(150, 45);
            this.btnOkEcheancesAbos.TabIndex = 2;
            this.btnOkEcheancesAbos.Text = "Continuer";
            this.btnOkEcheancesAbos.UseVisualStyleBackColor = true;
            this.btnOkEcheancesAbos.Click += new System.EventHandler(this.BtnOkEcheancesAbos_Click);
            // 
            // FrmEcheancesAbos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 321);
            this.Controls.Add(this.btnOkEcheancesAbos);
            this.Controls.Add(this.dgvEcheancesAbosListe);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmEcheancesAbos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alerte échéance abonnement";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEcheancesAbosListe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvEcheancesAbosListe;
        private System.Windows.Forms.Button btnOkEcheancesAbos;
    }
}