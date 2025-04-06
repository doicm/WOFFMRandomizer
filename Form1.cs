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
            this.Text = "World of Final Fantasy Randomizer v0.1.5";
            textBox1.ReadOnly = true;
            richTextBox1.ReadOnly = true;

            // Toggle buttons based on input WOFF executable
            buttonRandomize.Enabled = false;
            buttonUninstall.Enabled = false;

            textBox2.PlaceholderText = "Seed value (can be blank)";

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

        private void button3_Click(object sender, EventArgs e)
        {
            string basepath = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf("WOFF.exe"));
            Uninstall.Run(basepath, richTextBox1, button1, buttonRandomize, buttonUninstall);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string basepath = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf("WOFF.exe"));
            bool mbActive = checkBoxMirageboard.Checked;
            bool enemActive = checkBoxRandEnc.Checked;
            bool bossActive = checkBoxBosses.Checked;
            bool itemActive = checkBoxTreasures.Checked;
            bool rareActive = checkBoxRareMon.Checked;
            bool sizesActive = checkBoxSizes.Checked;
            bool quPrizesActive = checkBoxQuOrArenaPrizes.Checked;
            bool doubleExpBool = checkBoxDoubleExp.Checked;

            Install.Run(basepath, textBox2.Text, richTextBox1, mbActive, enemActive, bossActive, itemActive, rareActive, sizesActive,
                quPrizesActive, doubleExpBool, button1, buttonRandomize, buttonUninstall);

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
            toolTipRandEnc.Show("This shuffles the random encounters around that appear up to ending.\nThis doesn't include postgame encounters.", checkBoxRandEnc);
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
                "Stack ability animations are disabled to prevent crashes.\n" +
                "WARNING: If randomizing in the middle of a playthrough, please remove mirages\nfrom all stacks and save before shuffling.", checkBoxSizes);
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
    }
}
