namespace MediaTekDocuments.view
{
    partial class FrmAuthentification
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbAuthentificationLogin = new System.Windows.Forms.TextBox();
            this.txbAuthentificationPwd = new System.Windows.Forms.TextBox();
            this.grpBoxAuth = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAuthConnexion = new System.Windows.Forms.Button();
            this.btnQuitter = new System.Windows.Forms.Button();
            this.grpBoxAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(84, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Identifiant : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(84, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mot de passe : ";
            // 
            // txbAuthentificationLogin
            // 
            this.txbAuthentificationLogin.Location = new System.Drawing.Point(237, 65);
            this.txbAuthentificationLogin.MaxLength = 16;
            this.txbAuthentificationLogin.Name = "txbAuthentificationLogin";
            this.txbAuthentificationLogin.Size = new System.Drawing.Size(188, 20);
            this.txbAuthentificationLogin.TabIndex = 2;
            // 
            // txbAuthentificationPwd
            // 
            this.txbAuthentificationPwd.Location = new System.Drawing.Point(237, 120);
            this.txbAuthentificationPwd.MaxLength = 8;
            this.txbAuthentificationPwd.Name = "txbAuthentificationPwd";
            this.txbAuthentificationPwd.PasswordChar = '*';
            this.txbAuthentificationPwd.Size = new System.Drawing.Size(188, 20);
            this.txbAuthentificationPwd.TabIndex = 3;
            // 
            // grpBoxAuth
            // 
            this.grpBoxAuth.Controls.Add(this.txbAuthentificationPwd);
            this.grpBoxAuth.Controls.Add(this.label3);
            this.grpBoxAuth.Controls.Add(this.txbAuthentificationLogin);
            this.grpBoxAuth.Controls.Add(this.label2);
            this.grpBoxAuth.Controls.Add(this.label1);
            this.grpBoxAuth.Location = new System.Drawing.Point(58, 47);
            this.grpBoxAuth.Name = "grpBoxAuth";
            this.grpBoxAuth.Size = new System.Drawing.Size(544, 187);
            this.grpBoxAuth.TabIndex = 4;
            this.grpBoxAuth.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(213, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Veuillez renseigner vos identifiants";
            // 
            // btnAuthConnexion
            // 
            this.btnAuthConnexion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuthConnexion.Location = new System.Drawing.Point(452, 262);
            this.btnAuthConnexion.Name = "btnAuthConnexion";
            this.btnAuthConnexion.Size = new System.Drawing.Size(150, 33);
            this.btnAuthConnexion.TabIndex = 4;
            this.btnAuthConnexion.Text = "Connexion";
            this.btnAuthConnexion.UseVisualStyleBackColor = true;
            this.btnAuthConnexion.Click += new System.EventHandler(this.BtnConnexion_Click);
            // 
            // btnQuitter
            // 
            this.btnQuitter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuitter.ForeColor = System.Drawing.Color.Red;
            this.btnQuitter.Location = new System.Drawing.Point(58, 262);
            this.btnQuitter.Name = "btnQuitter";
            this.btnQuitter.Size = new System.Drawing.Size(150, 33);
            this.btnQuitter.TabIndex = 5;
            this.btnQuitter.Text = "Quitter";
            this.btnQuitter.UseVisualStyleBackColor = true;
            this.btnQuitter.Click += new System.EventHandler(this.BtnQuitter_Click);
            // 
            // FrmAuthentification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 321);
            this.Controls.Add(this.btnQuitter);
            this.Controls.Add(this.btnAuthConnexion);
            this.Controls.Add(this.grpBoxAuth);
            this.Name = "FrmAuthentification";
            this.Text = "Page d\'authentification";
            this.grpBoxAuth.ResumeLayout(false);
            this.grpBoxAuth.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbAuthentificationLogin;
        private System.Windows.Forms.TextBox txbAuthentificationPwd;
        private System.Windows.Forms.GroupBox grpBoxAuth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAuthConnexion;
        private System.Windows.Forms.Button btnQuitter;
    }
}