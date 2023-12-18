
namespace WinFormsPresentationslager
{
    partial class LoggaIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLoggaIn = new System.Windows.Forms.Button();
            this.txtAnvNamn = new System.Windows.Forms.TextBox();
            this.Lösenord = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonLoggaIn
            // 
            this.buttonLoggaIn.Location = new System.Drawing.Point(407, 239);
            this.buttonLoggaIn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonLoggaIn.Name = "buttonLoggaIn";
            this.buttonLoggaIn.Size = new System.Drawing.Size(82, 25);
            this.buttonLoggaIn.TabIndex = 0;
            this.buttonLoggaIn.Text = "Logga in";
            this.buttonLoggaIn.UseVisualStyleBackColor = true;
            this.buttonLoggaIn.Click += new System.EventHandler(this.buttonLoggaIn_Click);
            // 
            // txtAnvNamn
            // 
            this.txtAnvNamn.Location = new System.Drawing.Point(389, 143);
            this.txtAnvNamn.Name = "txtAnvNamn";
            this.txtAnvNamn.Size = new System.Drawing.Size(181, 23);
            this.txtAnvNamn.TabIndex = 1;
            // 
            // Lösenord
            // 
            this.Lösenord.Location = new System.Drawing.Point(389, 191);
            this.Lösenord.Name = "Lösenord";
            this.Lösenord.PasswordChar = '*';
            this.Lösenord.Size = new System.Drawing.Size(181, 23);
            this.Lösenord.TabIndex = 2;
            this.Lösenord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Lösenord_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(294, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Användarnamn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Lösenord";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bauhaus 93", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(327, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(290, 28);
            this.label3.TabIndex = 5;
            this.label3.Text = "Välkommen till SkiCenter!";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label4.Location = new System.Drawing.Point(621, 388);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(242, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "© Grupp 6, 21SU1C, Högskolan i Borås 2023-";
            // 
            // LoggaIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 412);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Lösenord);
            this.Controls.Add(this.txtAnvNamn);
            this.Controls.Add(this.buttonLoggaIn);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "LoggaIn";
            this.Text = "SkiCenter";
            this.Load += new System.EventHandler(this.LoggaIn_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLoggaIn;
        private System.Windows.Forms.TextBox txtAnvNamn;
        private System.Windows.Forms.TextBox Lösenord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

