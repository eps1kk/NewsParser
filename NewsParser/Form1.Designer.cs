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
            this.exception_label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.actualNewsListBox = new System.Windows.Forms.ListBox();
            this.poolNewsListBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.actualSymbolListBox = new System.Windows.Forms.ListBox();
            this.poolSymbolListBox = new System.Windows.Forms.ListBox();
            this.reverse_checkBox = new System.Windows.Forms.CheckBox();
            this.volatileGroup = new System.Windows.Forms.GroupBox();
            this.lowRadioButton = new System.Windows.Forms.RadioButton();
            this.midRadioButton = new System.Windows.Forms.RadioButton();
            this.highRadioButton = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.terminal_path = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.volatileGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // exception_label
            // 
            this.exception_label.AutoSize = true;
            this.exception_label.Location = new System.Drawing.Point(17, 256);
            this.exception_label.Name = "exception_label";
            this.exception_label.Size = new System.Drawing.Size(127, 13);
            this.exception_label.TabIndex = 1;
            this.exception_label.Text = "Директория терминала";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 11);
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
            // actualNewsListBox
            // 
            this.actualNewsListBox.AllowDrop = true;
            this.actualNewsListBox.FormattingEnabled = true;
            this.actualNewsListBox.HorizontalScrollbar = true;
            this.actualNewsListBox.Location = new System.Drawing.Point(173, 27);
            this.actualNewsListBox.Name = "actualNewsListBox";
            this.actualNewsListBox.Size = new System.Drawing.Size(423, 95);
            this.actualNewsListBox.TabIndex = 6;
            // 
            // poolNewsListBox
            // 
            this.poolNewsListBox.FormattingEnabled = true;
            this.poolNewsListBox.HorizontalScrollbar = true;
            this.poolNewsListBox.Location = new System.Drawing.Point(173, 153);
            this.poolNewsListBox.Name = "poolNewsListBox";
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
            this.label5.Location = new System.Drawing.Point(170, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Пул новостей";
            // 
            // actualSymbolListBox
            // 
            this.actualSymbolListBox.AllowDrop = true;
            this.actualSymbolListBox.FormattingEnabled = true;
            this.actualSymbolListBox.Location = new System.Drawing.Point(12, 27);
            this.actualSymbolListBox.Name = "actualSymbolListBox";
            this.actualSymbolListBox.Size = new System.Drawing.Size(132, 95);
            this.actualSymbolListBox.TabIndex = 11;
            // 
            // poolSymbolListBox
            // 
            this.poolSymbolListBox.FormattingEnabled = true;
            this.poolSymbolListBox.Location = new System.Drawing.Point(12, 153);
            this.poolSymbolListBox.Name = "poolSymbolListBox";
            this.poolSymbolListBox.Size = new System.Drawing.Size(132, 95);
            this.poolSymbolListBox.TabIndex = 12;
            // 
            // reverse_checkBox
            // 
            this.reverse_checkBox.AutoSize = true;
            this.reverse_checkBox.Location = new System.Drawing.Point(315, 5);
            this.reverse_checkBox.Name = "reverse_checkBox";
            this.reverse_checkBox.Size = new System.Drawing.Size(61, 17);
            this.reverse_checkBox.TabIndex = 13;
            this.reverse_checkBox.Text = "reverse";
            this.reverse_checkBox.UseVisualStyleBackColor = true;
            // 
            // volatileGroup
            // 
            this.volatileGroup.Controls.Add(this.lowRadioButton);
            this.volatileGroup.Controls.Add(this.midRadioButton);
            this.volatileGroup.Controls.Add(this.highRadioButton);
            this.volatileGroup.Location = new System.Drawing.Point(375, -6);
            this.volatileGroup.Name = "volatileGroup";
            this.volatileGroup.Size = new System.Drawing.Size(167, 33);
            this.volatileGroup.TabIndex = 17;
            this.volatileGroup.TabStop = false;
            this.volatileGroup.Enter += new System.EventHandler(this.volatileGroup_Enter);
            // 
            // lowRadioButton
            // 
            this.lowRadioButton.AutoSize = true;
            this.lowRadioButton.Location = new System.Drawing.Point(109, 10);
            this.lowRadioButton.Name = "lowRadioButton";
            this.lowRadioButton.Size = new System.Drawing.Size(45, 17);
            this.lowRadioButton.TabIndex = 2;
            this.lowRadioButton.TabStop = true;
            this.lowRadioButton.Text = "Low";
            this.lowRadioButton.UseVisualStyleBackColor = true;
            // 
            // midRadioButton
            // 
            this.midRadioButton.AutoSize = true;
            this.midRadioButton.Location = new System.Drawing.Point(60, 10);
            this.midRadioButton.Name = "midRadioButton";
            this.midRadioButton.Size = new System.Drawing.Size(42, 17);
            this.midRadioButton.TabIndex = 1;
            this.midRadioButton.TabStop = true;
            this.midRadioButton.Text = "Mid";
            this.midRadioButton.UseVisualStyleBackColor = true;
            // 
            // highRadioButton
            // 
            this.highRadioButton.AutoSize = true;
            this.highRadioButton.Location = new System.Drawing.Point(7, 10);
            this.highRadioButton.Name = "highRadioButton";
            this.highRadioButton.Size = new System.Drawing.Size(47, 17);
            this.highRadioButton.TabIndex = 0;
            this.highRadioButton.TabStop = true;
            this.highRadioButton.Text = "High";
            this.highRadioButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(535, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // terminal_path
            // 
            this.terminal_path.Location = new System.Drawing.Point(173, 253);
            this.terminal_path.Name = "terminal_path";
            this.terminal_path.Size = new System.Drawing.Size(396, 20);
            this.terminal_path.TabIndex = 19;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(575, 251);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(28, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "ok";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 275);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.terminal_path);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.reverse_checkBox);
            this.Controls.Add(this.poolSymbolListBox);
            this.Controls.Add(this.actualSymbolListBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.poolNewsListBox);
            this.Controls.Add(this.actualNewsListBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.exception_label);
            this.Controls.Add(this.volatileGroup);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.volatileGroup.ResumeLayout(false);
            this.volatileGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label exception_label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox actualNewsListBox;
        private System.Windows.Forms.ListBox poolNewsListBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox actualSymbolListBox;
        private System.Windows.Forms.ListBox poolSymbolListBox;
        private System.Windows.Forms.CheckBox reverse_checkBox;
        private System.Windows.Forms.GroupBox volatileGroup;
        private System.Windows.Forms.RadioButton lowRadioButton;
        private System.Windows.Forms.RadioButton midRadioButton;
        private System.Windows.Forms.RadioButton highRadioButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox terminal_path;
        private System.Windows.Forms.Button button2;
    }
}

