using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace WOFFRandomizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string version = "0.2.1";
            this.Text = "World of Final Fantasy Maxima Randomizer v" + version;
            textBox1.ReadOnly = true;
            richTextBox1.ReadOnly = true;
            richTextBoxPercent.ReadOnly = true;
            this.MaximizeBox = false;

            // Toggle buttons based on input WOFF executable
            buttonRandomize.Enabled = false;
            buttonUninstall.Enabled = false;

            textBox2.PlaceholderText = "Seed value (can be blank)";

            // Enable boxes to a sort of basic preset
            checkBoxTreasures.Checked = true;
            checkBoxQuOrArenaPrizes.Checked = true;
            //checkBoxDialogue.Checked = true;
            checkBoxRandEnc.Checked = true;
            checkBoxBosses.Checked = true;
            //checkBoxBattleSpeed.Checked = true;
            //checkBoxDoubleExp.Checked = true;
            checkBoxMirageboard.Checked = true;
            //checkBoxSizes.Checked = true;
            //checkBoxStats.Checked = true;
            //checkBoxMovement.Checked = true;


            //// Disable checkboxes when dependent checkboxes are inactive
            //checkBoxLibra.Enabled = false;
            //checkBoxDataSeeds.Enabled = false;
            //checkBoxDataJewels.Enabled = false;


            // Check if WOFF.exe settings option is blank or not
            string currDir = Directory.GetCurrentDirectory();
            if (new FileInfo(Path.GetFullPath(currDir + "/settings.json")).Length != 0)
            {
                buttonRandomize.Enabled = true;
                buttonUninstall.Enabled = true;
                string jsonString = File.ReadAllText(Path.GetFullPath(currDir + "/settings.json"));
                RandoSettings deseJsonString = JsonSerializer.Deserialize<RandoSettings>(jsonString);
                textBox1.Text = $"{deseJsonString.exeFilePath}";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog v1 = new OpenFileDialog();

            v1.Filter = "EXE files (*.exe)|*.exe";

            if (v1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = v1.FileName;
                if (!textBox1.Text.EndsWith("WOFF.exe"))
                {
                    MessageBox.Show("This is not WOFF.exe. Please try again.");
                    textBox1.Text = "";
                    buttonRandomize.Enabled = false;
                    buttonUninstall.Enabled = false;
                }
                else
                {
                    buttonRandomize.Enabled = true;
                    buttonUninstall.Enabled = true;
                    string currDir = Directory.GetCurrentDirectory();
                    if (new FileInfo(Path.GetFullPath(currDir + "/settings.json")).Length == 0)
                    {
                        string pathText = textBox1.Text;
                        pathText = pathText.Replace("\\", "/");
                        string jsonText = "{" + Environment.NewLine +
                            "\t\"exeFilePath\": \"" + pathText + "\"" + Environment.NewLine +
                            "}";
                        System.IO.File.WriteAllText(currDir + "/settings.json", jsonText);

                    }
                }
            }

        }

        // Uninstall button
        private void button3_Click(object sender, EventArgs e)
        {
            string basepath = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf("WOFF.exe"));
            Uninstall.Run(basepath, richTextBox1, button1, buttonRandomize, buttonUninstall);
        }

        // Randomize button
        private void button2_Click(object sender, EventArgs e)
        {
            string basepath = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf("WOFF.exe"));
            bool mbActive = checkBoxMirageboard.Checked;
            bool enemActive = checkBoxRandEnc.Checked;
            bool bossActive = checkBoxBosses.Checked;
            bool itemActive = checkBoxTreasures.Checked;
            bool libraActive = checkBoxLibra.Checked;
            bool dataseedsActive = checkBoxDataSeeds.Checked;
            bool datajewelsActive = checkBoxDataJewels.Checked;
            bool readeritemsActive = checkBoxReaderItems.Checked;
            bool rareActive = checkBoxRareMon.Checked;
            bool sizesActive = checkBoxSizes.Checked;
            bool quPrizesActive = checkBoxQuOrArenaPrizes.Checked;
            bool doubleExpActive = checkBoxDoubleExp.Checked;
            bool murkActive = checkBoxMurkrift.Checked;
            bool statActive = checkBoxStats.Checked;
            bool battleSpeedActive = checkBoxBattleSpeed.Checked;
            bool movementActive = checkBoxMovement.Checked;
            bool dialogueActive = checkBoxDialogue.Checked;
            bool t2AttackItemsActive = checkBoxT2AttackItems.Checked;
            bool transfigActive = checkBoxTransfig.Checked;

            int totalChecks = 
                (mbActive ? 1 : 0) + 
                (enemActive ? 1 : 0) +
                (bossActive ? 1 : 0) +
                (itemActive ? 1 : 0) +
                (libraActive ? 1 : 0) +
                (dataseedsActive ? 1 : 0) +
                (datajewelsActive ? 1 : 0) +
                (readeritemsActive ? 1 : 0) +
                (rareActive ? 1 : 0) +
                (sizesActive ? 1 : 0) +
                (quPrizesActive ? 1 : 0) +
                (doubleExpActive ? 1 : 0) +
                (murkActive ? 1 : 0) +
                (statActive ? 1 : 0) +
                (battleSpeedActive ? 1 : 0) +
                (movementActive ? 1 : 0) +
                (dialogueActive ? 1 : 0) +
                (t2AttackItemsActive ? 1 : 0) +
                (transfigActive ? 1 : 0);

            Install.Run(basepath, textBox2.Text, "0.2.1", richTextBox1, mbActive, enemActive, bossActive, itemActive, libraActive,
                dataseedsActive, datajewelsActive, readeritemsActive, rareActive, sizesActive,
                quPrizesActive, murkActive, statActive, transfigActive, doubleExpActive, battleSpeedActive, movementActive, dialogueActive,
                t2AttackItemsActive, button1, buttonRandomize, buttonUninstall, totalChecks, richTextBoxPercent);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string dirName = Directory.GetCurrentDirectory();
            string readmePath = Path.GetFullPath(dirName + "/readme.md");
            Process.Start("notepad.exe", readmePath);


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // scroll to end whenever new logs are obtained
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void checkBoxTreasures_MouseHover(object sender, EventArgs e)
        {
            toolTipTreasures.Show("This shuffles most of the treasure chest contents in the game up to ending.\n Counts of items and some specialty items not included.", checkBoxTreasures);
        }

        private void checkBoxRandEnc_MouseHover(object sender, EventArgs e)
        {
            toolTipRandEnc.Show("This shuffles the random encounters around that appear up to ending.\nThis includes some postgame encounters.", checkBoxRandEnc);
        }

        private void checkBoxRareMon_MouseHover(object sender, EventArgs e)
        {
            toolTipRareMon.Show("This shuffles most of the rare monster battles.", checkBoxRareMon);
        }

        private void checkBoxBosses_MouseHover(object sender, EventArgs e)
        {
            toolTipBosses.Show("This shuffles bosses that appear during the main story, starting from Watchplains.\n" +
                "This doesn't include some bosses such as Exnine fights.", checkBoxBosses);
        }

        private void checkBoxMirageboard_MouseHover(object sender, EventArgs e)
        {
            toolTipMirageboard.Show("This shuffles most of the nodes between mirageboards for mirages.\nSome nodes are excluded that are " +
                "either not functional or cause the game to softlock.\nSome nodes may also repeat or be entirely useless, but that's how shuffling works for now.\n" +
                "Mirage-specific ability animations have interesting effects, but should not cause crashes.", checkBoxMirageboard);
        }

        private void checkBoxSizes_MouseHover(object sender, EventArgs e)
        {
            toolTipSizes.Show("This shuffles the sizes around that mirages can be.\nThis does not include XL. This may cause some interesting behaviors with stacks.\n" +
                "Stack ability animations are disabled to prevent crashes.", checkBoxSizes);
        }

        private void checkBoxQuOrArenaPrizes_MouseHover(object sender, EventArgs e)
        {
            toolTipQuOrArenaPrizes.Show("This shuffles the prizes that can be obtained between the arena and intervention quests.\n NPC quests are not included. " +
                "Repeat attempts are not included.\nThis also includes the Tama quest. ??? mementos are now displayed.", checkBoxQuOrArenaPrizes);
        }

        private void checkBoxDoubleExp_MouseHover(object sender, EventArgs e)
        {
            toolTipDoubleExp.Show("This doubles experience and gil earned in all battles.", checkBoxDoubleExp);
        }

        private void checkBoxMurkrift_MouseHover(object sender, EventArgs e)
        {
            toolTipMurkrift.Show("This shuffles the murkrift encounters with each other.\nFirst Behemoth encounter is excluded, " +
                "as is the Nightmare rare monster fight\nand most of the airship murkrifts (level 60 ones).", checkBoxMurkrift);
        }

        private void checkBoxStats_MouseHover(object sender, EventArgs e)
        {
            toolTipStats.Show("This randomizes the stats and growths that each mirage has.\nThis may affect enemies as well as mirage allies.\n" +
                "This only randomizes HP, Str, Def, Mag, MDef, and Agi.", checkBoxStats);
        }

        private void checkBoxBattleSpeed_MouseHover(object sender, EventArgs e)
        {
            toolTipBattleSpeed.Show("This multiplies the battle speed at all speed.\nWait setting recommended, maybe.", checkBoxBattleSpeed);
        }

        private void checkBoxLibra_MouseHover(object sender, EventArgs e)
        {
            toolTipLibra.Show("This includes the Libra Mirajewel treasure chest in Nether Nebula in the shuffle.", checkBoxLibra);
        }

        private void checkBoxTreasures_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTreasures.Checked == false)
            {
                checkBoxLibra.Enabled = false;
                checkBoxLibra.Checked = false;
                checkBoxDataSeeds.Enabled = false;
                checkBoxDataSeeds.Checked = false;
                checkBoxDataJewels.Enabled = false;
                checkBoxDataJewels.Checked = false;
                checkBoxReaderItems.Enabled = false;
                checkBoxReaderItems.Checked = false;
            }
            else
            {
                checkBoxLibra.Enabled = true;
                checkBoxDataSeeds.Enabled = true;
                checkBoxDataJewels.Enabled = true;
                checkBoxReaderItems.Enabled = true;
            }
        }

        private void checkBoxMovement_MouseHover(object sender, EventArgs e)
        {
            toolTipMovement.Show("This doubles movement speed, which also cuts encounter rate in half.\nWARNING: Use at your own risk, as it can put you out of " +
                "bounds if not careful\ndue to speed excess in some areas.", checkBoxMovement);
        }

        private void checkBoxDialogue_MouseHover(object sender, EventArgs e)
        {
            toolTipDialogue.Show("This speeds up the dialogue in the field and battles.\nFor field dialogue, " +
                "voiced dialogue in config must be set to Off.\nNOTE: This only works in English.", checkBoxDialogue);
        }

        private void checkBoxDataSeeds_MouseHover(object sender, EventArgs e)
        {
            toolTipDataSeeds.Show("This replaces the standard set of ability seeds in treasure chests\nwith random seeds from in the game data.\n", checkBoxDataSeeds);
        }

        private void checkBoxDataJewels_MouseHover(object sender, EventArgs e)
        {
            toolTipDataJewels.Show("This replaces the standard set of Mirajewels in treasure chests\nwith random Mirajewels from in the game data.\n", checkBoxDataJewels);
        }

        private void checkBoxT2AttackItems_MouseHover(object sender, EventArgs e)
        {
            toolTipT2AttackItems.Show("This removes the Tier 2 attack items in Chocolatte's shop (anti-QoL).", checkBoxT2AttackItems);
        }

        private void checkBoxReaderItems_MouseHover(object sender, EventArgs e)
        {
            toolTipReaderItems.Show("This shuffles reading items, such as Girl's Diary,\ninto the treasure randomization.", checkBoxReaderItems);
        }

        private void checkBoxTransfig_MouseHover(object sender, EventArgs e)
        {
            toolTipTransfig.Show("This shuffles what mirages can transfigure into along with mirageboard unlocks.\nSome mirageboard unlock nodes are removed to prevent major issues." +
                "\nNOTE: Not recommended to apply this or remove this mid-playthrough.", checkBoxTransfig);
        }
    }
}
