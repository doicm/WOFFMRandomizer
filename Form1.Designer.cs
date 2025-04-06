namespace WOFFRandomizer
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            textBox1 = new TextBox();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            checkBoxMirageboard = new CheckBox();
            label3 = new Label();
            label4 = new Label();
            checkBoxRandEnc = new CheckBox();
            checkBoxBosses = new CheckBox();
            buttonRandomize = new Button();
            buttonUninstall = new Button();
            checkBoxTreasures = new CheckBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            richTextBox1 = new RichTextBox();
            buttonReadme = new Button();
            textBox2 = new TextBox();
            checkBoxRareMon = new CheckBox();
            checkBoxSizes = new CheckBox();
            toolTipTreasures = new ToolTip(components);
            toolTipRandEnc = new ToolTip(components);
            toolTipRareMon = new ToolTip(components);
            toolTipBosses = new ToolTip(components);
            toolTipMirageboard = new ToolTip(components);
            toolTipSizes = new ToolTip(components);
            toolTipQuOrArenaPrizes = new ToolTip(components);
            checkBoxQuOrArenaPrizes = new CheckBox();
            checkBoxDoubleExp = new CheckBox();
            toolTipDoubleExp = new ToolTip(components);
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(233, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(462, 23);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(701, 12);
            button1.Name = "button1";
            button1.Size = new Size(150, 23);
            button1.TabIndex = 1;
            button1.Text = "Find WOFF.exe";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(704, 68);
            label1.Name = "label1";
            label1.Size = new Size(109, 21);
            label1.TabIndex = 2;
            label1.Text = "Mirageboard";
            // 
            // label2
            // 
            label2.BackColor = SystemColors.AppWorkspace;
            label2.Location = new Point(646, 68);
            label2.Name = "label2";
            label2.Size = new Size(1, 300);
            label2.TabIndex = 3;
            // 
            // checkBoxMirageboard
            // 
            checkBoxMirageboard.AutoSize = true;
            checkBoxMirageboard.Location = new Point(684, 114);
            checkBoxMirageboard.Name = "checkBoxMirageboard";
            checkBoxMirageboard.Size = new Size(100, 19);
            checkBoxMirageboard.TabIndex = 4;
            checkBoxMirageboard.Text = "Shuffle Nodes";
            checkBoxMirageboard.UseVisualStyleBackColor = true;
            checkBoxMirageboard.MouseHover += checkBoxMirageboard_MouseHover;
            // 
            // label3
            // 
            label3.BackColor = SystemColors.AppWorkspace;
            label3.Location = new Point(431, 68);
            label3.Name = "label3";
            label3.Size = new Size(1, 300);
            label3.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(504, 68);
            label4.Name = "label4";
            label4.Size = new Size(74, 21);
            label4.TabIndex = 5;
            label4.Text = "Enemies";
            // 
            // checkBoxRandEnc
            // 
            checkBoxRandEnc.AutoSize = true;
            checkBoxRandEnc.Location = new Point(471, 114);
            checkBoxRandEnc.Name = "checkBoxRandEnc";
            checkBoxRandEnc.Size = new Size(138, 19);
            checkBoxRandEnc.TabIndex = 7;
            checkBoxRandEnc.Text = "Shuffle Random Encs";
            checkBoxRandEnc.UseVisualStyleBackColor = true;
            checkBoxRandEnc.MouseHover += checkBoxRandEnc_MouseHover;
            // 
            // checkBoxBosses
            // 
            checkBoxBosses.AutoSize = true;
            checkBoxBosses.Location = new Point(471, 164);
            checkBoxBosses.Name = "checkBoxBosses";
            checkBoxBosses.Size = new Size(101, 19);
            checkBoxBosses.TabIndex = 8;
            checkBoxBosses.Text = "Shuffle Bosses";
            checkBoxBosses.UseVisualStyleBackColor = true;
            checkBoxBosses.MouseHover += checkBoxBosses_MouseHover;
            // 
            // buttonRandomize
            // 
            buttonRandomize.Font = new Font("Segoe UI", 12F);
            buttonRandomize.Location = new Point(51, 256);
            buttonRandomize.Name = "buttonRandomize";
            buttonRandomize.Size = new Size(119, 54);
            buttonRandomize.TabIndex = 9;
            buttonRandomize.Text = "Randomize";
            buttonRandomize.UseVisualStyleBackColor = true;
            buttonRandomize.Click += button2_Click;
            // 
            // buttonUninstall
            // 
            buttonUninstall.Location = new Point(51, 316);
            buttonUninstall.Name = "buttonUninstall";
            buttonUninstall.Size = new Size(119, 23);
            buttonUninstall.TabIndex = 10;
            buttonUninstall.Text = "Uninstall";
            buttonUninstall.UseVisualStyleBackColor = true;
            buttonUninstall.Click += button3_Click;
            // 
            // checkBoxTreasures
            // 
            checkBoxTreasures.AutoSize = true;
            checkBoxTreasures.Location = new Point(260, 114);
            checkBoxTreasures.Name = "checkBoxTreasures";
            checkBoxTreasures.Size = new Size(148, 19);
            checkBoxTreasures.TabIndex = 12;
            checkBoxTreasures.Text = "Shuffle Treasure Chests";
            checkBoxTreasures.UseVisualStyleBackColor = true;
            checkBoxTreasures.MouseHover += checkBoxTreasures_MouseHover;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(308, 68);
            label5.Name = "label5";
            label5.Size = new Size(52, 21);
            label5.TabIndex = 11;
            label5.Text = "Items";
            // 
            // label6
            // 
            label6.BackColor = SystemColors.AppWorkspace;
            label6.Location = new Point(220, 68);
            label6.Name = "label6";
            label6.Size = new Size(1, 300);
            label6.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("DejaVu Sans", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(12, 21);
            label7.MaximumSize = new Size(200, 0);
            label7.Name = "label7";
            label7.Size = new Size(197, 112);
            label7.TabIndex = 14;
            label7.Text = "World of Final Fantasy Randomizer v0.1.5";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(56, 144);
            label8.Name = "label8";
            label8.Size = new Size(105, 15);
            label8.TabIndex = 15;
            label8.Text = "Created by: Doicm";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(5, 379);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            richTextBox1.Size = new Size(851, 67);
            richTextBox1.TabIndex = 16;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // buttonReadme
            // 
            buttonReadme.Location = new Point(51, 345);
            buttonReadme.Name = "buttonReadme";
            buttonReadme.Size = new Size(119, 23);
            buttonReadme.TabIndex = 17;
            buttonReadme.Text = "Readme";
            buttonReadme.UseVisualStyleBackColor = true;
            buttonReadme.Click += button4_Click;
            // 
            // textBox2
            // 
            textBox2.ForeColor = SystemColors.ActiveCaptionText;
            textBox2.Location = new Point(5, 220);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(204, 23);
            textBox2.TabIndex = 18;
            // 
            // checkBoxRareMon
            // 
            checkBoxRareMon.AutoSize = true;
            checkBoxRareMon.Location = new Point(471, 139);
            checkBoxRareMon.Name = "checkBoxRareMon";
            checkBoxRareMon.Size = new Size(134, 19);
            checkBoxRareMon.TabIndex = 19;
            checkBoxRareMon.Text = "Shuffle Rare Mirages";
            checkBoxRareMon.UseVisualStyleBackColor = true;
            checkBoxRareMon.MouseHover += checkBoxRareMon_MouseHover;
            // 
            // checkBoxSizes
            // 
            checkBoxSizes.AutoSize = true;
            checkBoxSizes.Location = new Point(684, 139);
            checkBoxSizes.Name = "checkBoxSizes";
            checkBoxSizes.Size = new Size(131, 19);
            checkBoxSizes.TabIndex = 20;
            checkBoxSizes.Text = "Shuffle Mirage Sizes";
            checkBoxSizes.UseVisualStyleBackColor = true;
            checkBoxSizes.MouseHover += checkBoxSizes_MouseHover;
            // 
            // checkBoxQuOrArenaPrizes
            // 
            checkBoxQuOrArenaPrizes.AutoSize = true;
            checkBoxQuOrArenaPrizes.Location = new Point(260, 139);
            checkBoxQuOrArenaPrizes.Name = "checkBoxQuOrArenaPrizes";
            checkBoxQuOrArenaPrizes.Size = new Size(135, 34);
            checkBoxQuOrArenaPrizes.TabIndex = 21;
            checkBoxQuOrArenaPrizes.Text = "Shuffle Intervention/\r\nColiseum Rewards";
            checkBoxQuOrArenaPrizes.UseVisualStyleBackColor = true;
            checkBoxQuOrArenaPrizes.MouseHover += checkBoxQuOrArenaPrizes_MouseHover;
            // 
            // checkBoxDoubleExp
            // 
            checkBoxDoubleExp.AutoSize = true;
            checkBoxDoubleExp.Location = new Point(471, 345);
            checkBoxDoubleExp.Name = "checkBoxDoubleExp";
            checkBoxDoubleExp.Size = new Size(164, 19);
            checkBoxDoubleExp.TabIndex = 22;
            checkBoxDoubleExp.Text = "Double Exp and Gil Earned";
            checkBoxDoubleExp.UseVisualStyleBackColor = true;
            checkBoxDoubleExp.MouseHover += checkBoxDoubleExp_MouseHover;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(863, 450);
            Controls.Add(checkBoxDoubleExp);
            Controls.Add(checkBoxQuOrArenaPrizes);
            Controls.Add(checkBoxSizes);
            Controls.Add(checkBoxRareMon);
            Controls.Add(textBox2);
            Controls.Add(buttonReadme);
            Controls.Add(richTextBox1);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(checkBoxTreasures);
            Controls.Add(label5);
            Controls.Add(buttonUninstall);
            Controls.Add(buttonRandomize);
            Controls.Add(checkBoxBosses);
            Controls.Add(checkBoxRandEnc);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(checkBoxMirageboard);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private Label label1;
        private Label label2;
        private CheckBox checkBoxMirageboard;
        private Label label3;
        private Label label4;
        private CheckBox checkBoxRandEnc;
        private CheckBox checkBoxBosses;
        private Button buttonRandomize;
        private Button buttonUninstall;
        private CheckBox checkBoxTreasures;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private RichTextBox richTextBox1;
        private Button buttonReadme;
        private TextBox textBox2;
        private CheckBox checkBoxRareMon;
        private CheckBox checkBoxSizes;
        private ToolTip toolTipTreasures;
        private ToolTip toolTipRandEnc;
        private ToolTip toolTipRareMon;
        private ToolTip toolTipBosses;
        private ToolTip toolTipMirageboard;
        private ToolTip toolTipSizes;
        private ToolTip toolTipQuOrArenaPrizes;
        private CheckBox checkBoxQuOrArenaPrizes;
        private CheckBox checkBoxDoubleExp;
        private ToolTip toolTipDoubleExp;
    }
}
