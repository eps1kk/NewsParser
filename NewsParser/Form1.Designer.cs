namespace NewsParser
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.newsListBox = new System.Windows.Forms.ListBox();
            this.poolNewsListBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.symbolListBox = new System.Windows.Forms.ListBox();
            this.poolSymbolListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 253);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Exception";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(337, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Новости используемые";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Символы используемые";
            // 
            // newsListBox
            // 
            this.newsListBox.AllowDrop = true;
            this.newsListBox.FormattingEnabled = true;
            this.newsListBox.Location = new System.Drawing.Point(173, 27);
            this.newsListBox.Name = "newsListBox";
            this.newsListBox.Size = new System.Drawing.Size(423, 95);
            this.newsListBox.TabIndex = 6;
            // 
            // poolNewsListBox
            // 
            this.poolNewsListBox.FormattingEnabled = true;
            this.poolNewsListBox.HorizontalScrollbar = true;
            this.poolNewsListBox.Location = new System.Drawing.Point(173, 153);
            this.poolNewsListBox.Name = "poolNewsListBox";
            this.poolNewsListBox.ScrollAlwaysVisible = true;
            this.poolNewsListBox.Size = new System.Drawing.Size(423, 95);
            this.poolNewsListBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Пул символов";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(287, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Пул новостей";
            // 
            // symbolListBox
            // 
            this.symbolListBox.AllowDrop = true;
            this.symbolListBox.FormattingEnabled = true;
            this.symbolListBox.Location = new System.Drawing.Point(12, 27);
            this.symbolListBox.Name = "symbolListBox";
            this.symbolListBox.Size = new System.Drawing.Size(132, 95);
            this.symbolListBox.TabIndex = 11;
            // 
            // poolSymbolListBox
            // 
            this.poolSymbolListBox.FormattingEnabled = true;
            this.poolSymbolListBox.Location = new System.Drawing.Point(12, 155);
            this.poolSymbolListBox.Name = "poolSymbolListBox";
            this.poolSymbolListBox.Size = new System.Drawing.Size(132, 95);
            this.poolSymbolListBox.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 276);
            this.Controls.Add(this.poolSymbolListBox);
            this.Controls.Add(this.symbolListBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.poolNewsListBox);
            this.Controls.Add(this.newsListBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox newsListBox;
        private System.Windows.Forms.ListBox poolNewsListBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox symbolListBox;
        private System.Windows.Forms.ListBox poolSymbolListBox;
    }
}

