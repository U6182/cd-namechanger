namespace CD_NameChanger
{
    partial class CDNameChanger
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.folderOpenBtn = new System.Windows.Forms.Button();
            this.fileOpenBtn = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.changeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // folderOpenBtn
            // 
            this.folderOpenBtn.Location = new System.Drawing.Point(535, 44);
            this.folderOpenBtn.Name = "folderOpenBtn";
            this.folderOpenBtn.Size = new System.Drawing.Size(71, 19);
            this.folderOpenBtn.TabIndex = 1;
            this.folderOpenBtn.Text = "フォルダ";
            this.folderOpenBtn.UseVisualStyleBackColor = true;
            this.folderOpenBtn.Click += new System.EventHandler(this.folderOpenBtn_Click);
            // 
            // fileOpenBtn
            // 
            this.fileOpenBtn.Location = new System.Drawing.Point(458, 44);
            this.fileOpenBtn.Name = "fileOpenBtn";
            this.fileOpenBtn.Size = new System.Drawing.Size(71, 19);
            this.fileOpenBtn.TabIndex = 3;
            this.fileOpenBtn.Text = "ファイル";
            this.fileOpenBtn.UseVisualStyleBackColor = true;
            this.fileOpenBtn.Click += new System.EventHandler(this.fileOpenBtn_Click);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(46, 44);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ReadOnly = true;
            this.pathTextBox.Size = new System.Drawing.Size(406, 19);
            this.pathTextBox.TabIndex = 4;
            // 
            // changeBtn
            // 
            this.changeBtn.BackColor = System.Drawing.SystemColors.MenuText;
            this.changeBtn.BackgroundImage = global::CD_NameChanger.Properties.Resources.change2;
            this.changeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.changeBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.changeBtn.Location = new System.Drawing.Point(46, 93);
            this.changeBtn.Name = "changeBtn";
            this.changeBtn.Size = new System.Drawing.Size(560, 328);
            this.changeBtn.TabIndex = 2;
            this.changeBtn.UseVisualStyleBackColor = false;
            this.changeBtn.Click += new System.EventHandler(this.change_Click);
            // 
            // CDNameChanger
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.fileOpenBtn);
            this.Controls.Add(this.changeBtn);
            this.Controls.Add(this.folderOpenBtn);
            this.MaximumSize = new System.Drawing.Size(816, 489);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "CDNameChanger";
            this.Text = "CD-CHANGER";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button folderOpenBtn;
        private System.Windows.Forms.Button changeBtn;
        private System.Windows.Forms.Button fileOpenBtn;
        private System.Windows.Forms.TextBox pathTextBox;
    }
}

