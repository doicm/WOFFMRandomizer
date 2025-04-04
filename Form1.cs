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
            this.Text = "World of Final Fantasy Randomizer v0.1.2";
            textBox1.ReadOnly = true;
            richTextBox1.ReadOnly = true;

            // Toggle buttons based on input WOFF executable
            button2.Enabled = false;
            button3.Enabled = false;


            textBox2.PlaceholderText = "Seed value (can be blank)";

            // Check if WOFF.exe settings option is blank or not
            string currDir = Directory.GetCurrentDirectory();
            if (new FileInfo(Path.GetFullPath(currDir + "/settings.json")).Length != 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
                string jsonString = File.ReadAllText(Path.GetFullPath(currDir + "/settings.json"));
                RandoSettings deseJsonString = JsonSerializer.Deserialize<RandoSettings>(jsonString);
                textBox1.Text = $"{deseJsonString.exeFilePath}";
            }
        }



        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

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
                    button2.Enabled = false;
                    button3.Enabled = false;
                }
                else
                {
                    button2.Enabled = true;
                    button3.Enabled = true;
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

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string basepath = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf("WOFF.exe"));
            Uninstall.Run(basepath, richTextBox1, button1, button2, button3);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string basepath = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf("WOFF.exe"));
            bool mbActive = checkBox1.Checked;
            bool enemActive = checkBox2.Checked;
            bool bossActive = checkBox3.Checked;
            bool itemActive = checkBox4.Checked;
            bool rareActive = checkBox5.Checked;

            Install.Run(basepath, textBox2.Text, richTextBox1, mbActive, enemActive, bossActive, itemActive, rareActive, button1, button2, button3);


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox2.Checked) checkBox3.Enabled = true;
            //else
            //{
            //    checkBox3.Enabled = false;
            //    checkBox3.Checked = false;
            //}

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

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
