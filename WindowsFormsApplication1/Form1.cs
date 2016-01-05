using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Twitch.Net.Interfaces;
using Twitch.Net.Model;
using Twitch.Net.Clients;
using System.IO;
using System.Net;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        private String quality = "source";
        private JObject streams;
        private JObject games;
        private String selectedGame = null;
        private ImageList imageList;

        public Form1()
        {
            InitializeComponent();
        }

        private void installLivestreamer()
        {
            string currentPath = Path.GetDirectoryName(Application.ExecutablePath);
            var process = Process.Start(currentPath + "\\livestreamer-installer.exe");
            process.WaitForExit();
        }


        private void getGameList()
        {
            string HTML = this.getHTML("https://api.twitch.tv/kraken/games/top?limit=100");

            this.games = JObject.Parse(HTML);
            foreach (JToken game in this.games["top"])
            {           
                string gameName = (game["game"]["name"].ToString());
                this.comboBox1.Items.Add(gameName);           
            }
        
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.getGameList();
            this.MainMenuStrip = this.menuStrip1;

            this.imageList = new ImageList();
            this.imageList.ImageSize = new Size(256, 140);
            this.imageList.ColorDepth = ColorDepth.Depth32Bit;

            listView1.View = View.Details;
            listView1.LargeImageList = this.imageList;
            listView1.SmallImageList = this.imageList;
            listView1.TileSize = new Size(320,400);

            if (!ExistsOnPath("livestreamer.exe"))
            {
                DialogResult res = MessageBox.Show("livestreamer doesn't seem to be installed. We need livestreamer. Do you want to install that now?", "Stop!", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    Console.WriteLine("Installing..");
                    using (WebClient Client = new WebClient())
                    {
                        string url = "https://github.com/chrippa/livestreamer/releases/download/v1.12.1/livestreamer-v1.12.1-win32-setup.exe";
                        Uri ur = new Uri(url);
                        Client.DownloadFileCompleted += (senderr, ee) => this.installLivestreamer();
                        Client.DownloadFileAsync(ur, "livestreamer-installer.exe");
                    }
                }
                else if (res == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else {
                this.loadStreams();
            }
        }

        private void loadStreams()
        {
            this.listView1.Items.Clear();
            String URL = "https://api.twitch.tv/kraken/streams";
           
            if(this.selectedGame != null)
            {
                URL = URL + "?game=" + Uri.EscapeDataString(this.selectedGame);
                Cursor.Current = Cursors.WaitCursor;               
                this.imageList.Images.Clear();
            }

            string HTML = this.getHTML(URL);

            this.streams = JObject.Parse(HTML);
            int i = 0;
            foreach (JToken stream in this.streams["streams"])
            {
                //retrieve all image files
                string imagePath = stream["preview"]["medium"].ToString();

                //Add images to Imagelist
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(imagePath);
                MemoryStream ms = new MemoryStream(bytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                this.imageList.Images.Add(img);

                string st = (stream["channel"]["display_name"] + " playing " + stream["game"] + " for " + stream["viewers"] + " viewers (" + stream["channel"]["status"] + ")");
                this.listView1.Items.Add(new ListViewItem { ImageIndex = i, Text = st });
                i++;
            }
            this.listView1.Click += new System.EventHandler(this.listView1_Click);

            this.listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.None);
            this.listView1.Columns[0].Width = Width - 4 - SystemInformation.VerticalScrollBarWidth;
            this.listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                       
            Cursor.Current = Cursors.Default;
        }

            private string getHTML(string URL)
            {
                System.Net.WebClient objWC = new System.Net.WebClient();
                return new System.Text.UTF8Encoding().GetString(objWC.DownloadData(URL));
            }

        private void listView1_Click(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("livestreamer");
            if (pname.Length != 0)
            {
                return;
            }

            if (listView1.SelectedItems.Count > 0) { 
                int i = listView1.SelectedIndices[0];
                Console.WriteLine("Selected " + this.streams["streams"][i]["channel"]["display_name"]);

                string channelName = this.streams["streams"][i]["channel"]["name"].ToString();

                Process process = new Process();
                // Configure the process using the StartInfo properties.
                process.StartInfo.FileName = "livestreamer";
                process.StartInfo.Arguments = "twitch.tv/" + channelName + " " + this.quality;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                Cursor.Current = Cursors.WaitCursor;
                System.Threading.Thread.Sleep(5000);
                Cursor.Current = Cursors.Default;
            }
           }

        public static bool ExistsOnPath(string fileName)
            {
                if (GetFullPath(fileName) != null)
                    return true;
                return false;
            }

            public static string GetFullPath(string fileName)
            {
                if (File.Exists(fileName))
                    return Path.GetFullPath(fileName);

                var values = Environment.GetEnvironmentVariable("PATH");
                foreach (var path in values.Split(';'))
                {
                    var fullPath = Path.Combine(path, fileName);
                    if (File.Exists(fullPath))
                        return fullPath;
                }
                return null;
            }

        private void sourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sourceToolStripMenuItem.Checked = true;
            this.highToolStripMenuItem1.Checked = false;
            this.middleToolStripMenuItem1.Checked = false;
            this.lowToolStripMenuItem1.Checked = false;
            this.quality = "source";
        }

        private void highToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.sourceToolStripMenuItem.Checked = false;
            this.highToolStripMenuItem1.Checked = true;
            this.middleToolStripMenuItem1.Checked = false;
            this.lowToolStripMenuItem1.Checked = false;
            this.quality = "high";
        }

        private void middleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.sourceToolStripMenuItem.Checked = false;
            this.highToolStripMenuItem1.Checked = false;
            this.middleToolStripMenuItem1.Checked = true;
            this.lowToolStripMenuItem1.Checked = false;
            this.quality = "middle";
        }

        private void lowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.sourceToolStripMenuItem.Checked = false;
            this.highToolStripMenuItem1.Checked = false;
            this.middleToolStripMenuItem1.Checked = false;
            this.lowToolStripMenuItem1.Checked = true;
            this.quality = "low";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String game = comboBox1.Text;
            Console.WriteLine("Selected " + game);
            this.selectedGame = game;
            this.loadStreams();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.loadStreams();
        }
    }
}