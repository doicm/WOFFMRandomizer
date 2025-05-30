﻿namespace WOFFRandomizer
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
            toolTipMurkrift = new ToolTip(components);
            checkBoxMurkrift = new CheckBox();
            label8 = new Label();
            checkBoxStats = new CheckBox();
            toolTipStats = new ToolTip(components);
            checkBoxBattleSpeed = new CheckBox();
            toolTipBattleSpeed = new ToolTip(components);
            checkBoxLibra = new CheckBox();
            toolTipLibra = new ToolTip(components);
            toolTipMovement = new ToolTip(components);
            checkBoxMovement = new CheckBox();
            checkBoxDialogue = new CheckBox();
            toolTipDialogue = new ToolTip(components);
            checkBoxDataSeeds = new CheckBox();
            checkBoxDataJewels = new CheckBox();
            toolTipDataSeeds = new ToolTip(components);
            toolTipDataJewels = new ToolTip(components);
            label9 = new Label();
            label2 = new Label();
            label10 = new Label();
            toolTipT2AttackItems = new ToolTip(components);
            checkBoxT2AttackItems = new CheckBox();
            toolTipReaderItems = new ToolTip(components);
            checkBoxReaderItems = new CheckBox();
            toolTipTransfig = new ToolTip(components);
            checkBoxTransfig = new CheckBox();
            richTextBoxPercent = new RichTextBox();
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
            label1.Location = new Point(716, 63);
            label1.Name = "label1";
            label1.Size = new Size(71, 21);
            label1.TabIndex = 2;
            label1.Text = "Mirages";
            // 
            // checkBoxMirageboard
            // 
            checkBoxMirageboard.AutoSize = true;
            checkBoxMirageboard.Location = new Point(668, 93);
            checkBoxMirageboard.Name = "checkBoxMirageboard";
            checkBoxMirageboard.Size = new Size(171, 19);
            checkBoxMirageboard.TabIndex = 4;
            checkBoxMirageboard.Text = "Shuffle Mirageboard Nodes";
            checkBoxMirageboard.UseVisualStyleBackColor = true;
            checkBoxMirageboard.MouseHover += checkBoxMirageboard_MouseHover;
            // 
            // label3
            // 
            label3.BackColor = SystemColors.AppWorkspace;
            label3.Location = new Point(432, 68);
            label3.Name = "label3";
            label3.Size = new Size(1, 208);
            label3.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(507, 63);
            label4.Name = "label4";
            label4.Size = new Size(74, 21);
            label4.TabIndex = 5;
            label4.Text = "Enemies";
            // 
            // checkBoxRandEnc
            // 
            checkBoxRandEnc.AutoSize = true;
            checkBoxRandEnc.Location = new Point(458, 93);
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
            checkBoxBosses.Location = new Point(458, 143);
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
            checkBoxTreasures.Location = new Point(252, 93);
            checkBoxTreasures.Name = "checkBoxTreasures";
            checkBoxTreasures.Size = new Size(148, 19);
            checkBoxTreasures.TabIndex = 12;
            checkBoxTreasures.Text = "Shuffle Treasure Chests";
            checkBoxTreasures.UseVisualStyleBackColor = true;
            checkBoxTreasures.CheckedChanged += checkBoxTreasures_CheckedChanged;
            checkBoxTreasures.MouseHover += checkBoxTreasures_MouseHover;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(311, 63);
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
            label7.Font = new Font("DejaVu Sans", 16F, FontStyle.Bold);
            label7.Location = new Point(5, 12);
            label7.MaximumSize = new Size(220, 0);
            label7.Name = "label7";
            label7.Size = new Size(215, 100);
            label7.TabIndex = 14;
            label7.Text = "World of Final Fantasy Maxima Randomizer v0.2.1";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(5, 379);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            richTextBox1.Size = new Size(851, 62);
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
            checkBoxRareMon.Location = new Point(458, 118);
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
            checkBoxSizes.Location = new Point(668, 118);
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
            checkBoxQuOrArenaPrizes.Location = new Point(252, 218);
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
            checkBoxDoubleExp.Location = new Point(458, 341);
            checkBoxDoubleExp.Name = "checkBoxDoubleExp";
            checkBoxDoubleExp.Size = new Size(164, 19);
            checkBoxDoubleExp.TabIndex = 22;
            checkBoxDoubleExp.Text = "Double Exp and Gil Earned";
            checkBoxDoubleExp.UseVisualStyleBackColor = true;
            checkBoxDoubleExp.MouseHover += checkBoxDoubleExp_MouseHover;
            // 
            // checkBoxMurkrift
            // 
            checkBoxMurkrift.AutoSize = true;
            checkBoxMurkrift.Location = new Point(458, 168);
            checkBoxMurkrift.Name = "checkBoxMurkrift";
            checkBoxMurkrift.Size = new Size(114, 19);
            checkBoxMurkrift.TabIndex = 23;
            checkBoxMurkrift.Text = "Shuffle Murkrifts";
            checkBoxMurkrift.UseVisualStyleBackColor = true;
            checkBoxMurkrift.MouseHover += checkBoxMurkrift_MouseHover;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(56, 118);
            label8.Name = "label8";
            label8.Size = new Size(105, 15);
            label8.TabIndex = 15;
            label8.Text = "Created by: Doicm";
            // 
            // checkBoxStats
            // 
            checkBoxStats.AutoSize = true;
            checkBoxStats.Location = new Point(668, 143);
            checkBoxStats.Name = "checkBoxStats";
            checkBoxStats.Size = new Size(153, 19);
            checkBoxStats.TabIndex = 24;
            checkBoxStats.Text = "Randomize Mirage Stats";
            checkBoxStats.UseVisualStyleBackColor = true;
            checkBoxStats.MouseHover += checkBoxStats_MouseHover;
            // 
            // checkBoxBattleSpeed
            // 
            checkBoxBattleSpeed.AutoSize = true;
            checkBoxBattleSpeed.Location = new Point(458, 316);
            checkBoxBattleSpeed.Name = "checkBoxBattleSpeed";
            checkBoxBattleSpeed.Size = new Size(137, 19);
            checkBoxBattleSpeed.TabIndex = 25;
            checkBoxBattleSpeed.Text = "Increase Battle Speed";
            checkBoxBattleSpeed.UseVisualStyleBackColor = true;
            checkBoxBattleSpeed.MouseHover += checkBoxBattleSpeed_MouseHover;
            // 
            // checkBoxLibra
            // 
            checkBoxLibra.AutoSize = true;
            checkBoxLibra.Location = new Point(270, 118);
            checkBoxLibra.Name = "checkBoxLibra";
            checkBoxLibra.Size = new Size(146, 19);
            checkBoxLibra.TabIndex = 26;
            checkBoxLibra.Text = "Shuffle Libra Mirajewel";
            checkBoxLibra.UseVisualStyleBackColor = true;
            checkBoxLibra.MouseHover += checkBoxLibra_MouseHover;
            // 
            // checkBoxMovement
            // 
            checkBoxMovement.AutoSize = true;
            checkBoxMovement.Location = new Point(668, 316);
            checkBoxMovement.Name = "checkBoxMovement";
            checkBoxMovement.Size = new Size(160, 19);
            checkBoxMovement.TabIndex = 27;
            checkBoxMovement.Text = "Double Movement Speed";
            checkBoxMovement.UseVisualStyleBackColor = true;
            checkBoxMovement.MouseHover += checkBoxMovement_MouseHover;
            // 
            // checkBoxDialogue
            // 
            checkBoxDialogue.AutoSize = true;
            checkBoxDialogue.Location = new Point(252, 316);
            checkBoxDialogue.Name = "checkBoxDialogue";
            checkBoxDialogue.Size = new Size(126, 19);
            checkBoxDialogue.TabIndex = 28;
            checkBoxDialogue.Text = "Speed Up Dialogue";
            checkBoxDialogue.UseVisualStyleBackColor = true;
            checkBoxDialogue.MouseHover += checkBoxDialogue_MouseHover;
            // 
            // checkBoxDataSeeds
            // 
            checkBoxDataSeeds.AutoSize = true;
            checkBoxDataSeeds.Location = new Point(270, 143);
            checkBoxDataSeeds.Name = "checkBoxDataSeeds";
            checkBoxDataSeeds.Size = new Size(156, 19);
            checkBoxDataSeeds.TabIndex = 29;
            checkBoxDataSeeds.Text = "Replace Set Ability Seeds";
            checkBoxDataSeeds.UseVisualStyleBackColor = true;
            checkBoxDataSeeds.MouseHover += checkBoxDataSeeds_MouseHover;
            // 
            // checkBoxDataJewels
            // 
            checkBoxDataJewels.AutoSize = true;
            checkBoxDataJewels.Location = new Point(270, 168);
            checkBoxDataJewels.Name = "checkBoxDataJewels";
            checkBoxDataJewels.Size = new Size(145, 19);
            checkBoxDataJewels.TabIndex = 30;
            checkBoxDataJewels.Text = "Replace Set Mirajewels";
            checkBoxDataJewels.UseVisualStyleBackColor = true;
            checkBoxDataJewels.MouseHover += checkBoxDataJewels_MouseHover;
            // 
            // label9
            // 
            label9.BackColor = SystemColors.AppWorkspace;
            label9.Location = new Point(641, 68);
            label9.Name = "label9";
            label9.Size = new Size(1, 208);
            label9.TabIndex = 31;
            // 
            // label2
            // 
            label2.BackColor = SystemColors.AppWorkspace;
            label2.Location = new Point(227, 285);
            label2.Name = "label2";
            label2.Size = new Size(625, 1);
            label2.TabIndex = 32;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(519, 289);
            label10.Name = "label10";
            label10.Size = new Size(40, 21);
            label10.TabIndex = 33;
            label10.Text = "QoL";
            // 
            // checkBoxT2AttackItems
            // 
            checkBoxT2AttackItems.Location = new Point(252, 339);
            checkBoxT2AttackItems.Name = "checkBoxT2AttackItems";
            checkBoxT2AttackItems.Size = new Size(155, 34);
            checkBoxT2AttackItems.TabIndex = 34;
            checkBoxT2AttackItems.Text = "Remove Powerful Attack Items from Shop";
            checkBoxT2AttackItems.UseVisualStyleBackColor = true;
            checkBoxT2AttackItems.MouseHover += checkBoxT2AttackItems_MouseHover;
            // 
            // checkBoxReaderItems
            // 
            checkBoxReaderItems.AutoSize = true;
            checkBoxReaderItems.Location = new Point(270, 193);
            checkBoxReaderItems.Name = "checkBoxReaderItems";
            checkBoxReaderItems.Size = new Size(141, 19);
            checkBoxReaderItems.TabIndex = 35;
            checkBoxReaderItems.Text = "Shuffle Reading Items";
            checkBoxReaderItems.UseVisualStyleBackColor = true;
            checkBoxReaderItems.MouseHover += checkBoxReaderItems_MouseHover;
            // 
            // checkBoxTransfig
            // 
            checkBoxTransfig.AutoSize = true;
            checkBoxTransfig.BackgroundImageLayout = ImageLayout.None;
            checkBoxTransfig.Location = new Point(668, 168);
            checkBoxTransfig.Name = "checkBoxTransfig";
            checkBoxTransfig.Size = new Size(151, 19);
            checkBoxTransfig.TabIndex = 36;
            checkBoxTransfig.Text = "Shuffle Transfigurations";
            checkBoxTransfig.UseVisualStyleBackColor = true;
            checkBoxTransfig.MouseHover += checkBoxTransfig_MouseHover;
            // 
            // richTextBoxPercent
            // 
            richTextBoxPercent.Location = new Point(5, 447);
            richTextBoxPercent.Multiline = false;
            richTextBoxPercent.Name = "richTextBoxPercent";
            richTextBoxPercent.ScrollBars = RichTextBoxScrollBars.None;
            richTextBoxPercent.Size = new Size(851, 21);
            richTextBoxPercent.TabIndex = 37;
            richTextBoxPercent.Text = "";
            richTextBoxPercent.WordWrap = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(863, 470);
            Controls.Add(richTextBoxPercent);
            Controls.Add(checkBoxTransfig);
            Controls.Add(checkBoxReaderItems);
            Controls.Add(checkBoxT2AttackItems);
            Controls.Add(label10);
            Controls.Add(label2);
            Controls.Add(label9);
            Controls.Add(checkBoxDataJewels);
            Controls.Add(checkBoxDataSeeds);
            Controls.Add(checkBoxDialogue);
            Controls.Add(checkBoxMovement);
            Controls.Add(checkBoxLibra);
            Controls.Add(checkBoxBattleSpeed);
            Controls.Add(checkBoxStats);
            Controls.Add(checkBoxMurkrift);
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
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private Label label1;
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
        private ToolTip toolTipMurkrift;
        private CheckBox checkBoxMurkrift;
        private Label label8;
        private CheckBox checkBoxStats;
        private ToolTip toolTipStats;
        private CheckBox checkBoxBattleSpeed;
        private ToolTip toolTipBattleSpeed;
        private CheckBox checkBoxLibra;
        private ToolTip toolTipLibra;
        private ToolTip toolTipMovement;
        private CheckBox checkBoxMovement;
        private CheckBox checkBoxDialogue;
        private ToolTip toolTipDialogue;
        private CheckBox checkBoxDataSeeds;
        private CheckBox checkBoxDataJewels;
        private ToolTip toolTipDataSeeds;
        private ToolTip toolTipDataJewels;
        private Label label9;
        private Label label2;
        private Label label10;
        private ToolTip toolTipT2AttackItems;
        private CheckBox checkBoxT2AttackItems;
        private ToolTip toolTipReaderItems;
        private CheckBox checkBoxReaderItems;
        private ToolTip toolTipTransfig;
        private CheckBox checkBoxTransfig;
        private RichTextBox richTextBoxPercent;
    }
}
