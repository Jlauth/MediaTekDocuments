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
            this.dgvEcheancesAbosListe = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEcheancesAbosListe)).BeginInit();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Abonnement(s) expirant dans moins de 30 jours";
            // 
            // dgvEcheancesAbosListe
            // 
            this.dgvEcheancesAbosListe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEcheancesAbosListe.Location = new System.Drawing.Point(71, 75);
            this.dgvEcheancesAbosListe.Name = "dgvEcheancesAbosListe";
            this.dgvEcheancesAbosListe.Size = new System.Drawing.Size(421, 153);
            this.dgvEcheancesAbosListe.TabIndex = 1;
            // 
            // FrmEcheancesAbos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 285);
            this.Controls.Add(this.dgvEcheancesAbosListe);
            this.Controls.Add(this.label1);
            this.Name = "FrmEcheancesAbos";
            this.Text = "Alerte échéance abonnement";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEcheancesAbosListe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvEcheancesAbosListe;
    }
}