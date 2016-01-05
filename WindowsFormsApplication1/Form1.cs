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
using Microsoft.VisualBasic;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
                
        private JObject streams;
        private JObject followedStreams;
        private JObject games;
        private ImageList imageList;
        private ImageList followedImageList;

        public string SelectedGame
        {
            get
            {
                return TwitchStreamer.Properties.Settings.Default.selectedGame;
            }

            set
            {
                TwitchStreamer.Properties.Settings.Default.selectedGame = value;
                TwitchStreamer.Properties.Settings.Default.Save();
            }
        }

        public string twitchUsername
        {
            get
            {
                return TwitchStreamer.Properties.Settings.Default.twitchUsername;
            }

            set
            {
                TwitchStreamer.Properties.Settings.Default.twitchUsername = value;
                TwitchStreamer.Properties.Settings.Default.Save();
            }
        }

        public string Quality
        {
            get
            {
                return TwitchStreamer.Properties.Settings.Default.quality;
            }

            set
            {
                TwitchStreamer.Properties.Settings.Default.quality = value;
                TwitchStreamer.Properties.Settings.Default.Save();
            }
        }

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
            this.comboBox1.Items.Add("All games");
            this.games = JObject.Parse(HTML);
            foreach (JToken game in this.games["top"])
            {           
                string gameName = (game["game"]["name"].ToString());
                this.comboBox1.Items.Add(gameName);           
            }
        
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.imageList = new ImageList();
            this.imageList.ImageSize = new Size(256, 140);
            this.imageList.ColorDepth = ColorDepth.Depth32Bit;

            this.followedImageList = new ImageList();
            this.followedImageList.ImageSize = new Size(80, 45);
            this.followedImageList.ColorDepth = ColorDepth.Depth32Bit;

            listView1.View = View.Details;
            listView1.LargeImageList = this.imageList;
            listView1.SmallImageList = this.imageList;
            listView1.TileSize = new Size(320, 400);

            listView2.View = View.Details;            
            listView2.TileSize = new Size(320, 400);

            if (this.twitchUsername.Length == 0)
            {
                listView2.LargeImageList = null;
                listView2.SmallImageList = null;
                listView2.Items.Add("Set your Twitch username to see your followed channels");
            } else
            {
                listView2.LargeImageList = this.followedImageList;
                listView2.SmallImageList = this.followedImageList;
                this.loadFollowedStreams();
            }

            this.getGameList();
            this.setupQualityAndGame();
            this.MainMenuStrip = this.menuStrip1;
        
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
            this.imageList.Images.Clear();
            String URL = "https://api.twitch.tv/kraken/streams";
            Cursor.Current = Cursors.WaitCursor;

            if (this.SelectedGame != null)
            {
                URL = URL + "?game=" + Uri.EscapeDataString(this.SelectedGame);                           
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
                process.StartInfo.Arguments = "twitch.tv/" + channelName + " " + this.Quality;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                Cursor.Current = Cursors.WaitCursor;
                System.Threading.Thread.Sleep(5000);
                Cursor.Current = Cursors.Default;
            }
           }

        private void listView2_Click(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("livestreamer");
            if (pname.Length != 0)
            {
                return;
            }

            if (listView2.SelectedItems.Count > 0)
            {
                int i = listView2.SelectedIndices[0];
                Console.WriteLine("Selected " + this.followedStreams["streams"][i]["channel"]["display_name"]);

                string channelName = this.followedStreams["streams"][i]["channel"]["name"].ToString();

                Process process = new Process();
                // Configure the process using the StartInfo properties.
                process.StartInfo.FileName = "livestreamer";
                process.StartInfo.Arguments = "twitch.tv/" + channelName + " " + this.Quality;
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
            this.Quality = "source";
        }

        private void highToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.sourceToolStripMenuItem.Checked = false;
            this.highToolStripMenuItem1.Checked = true;
            this.middleToolStripMenuItem1.Checked = false;
            this.lowToolStripMenuItem1.Checked = false;
            this.Quality = "high";
        }

        private void middleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.sourceToolStripMenuItem.Checked = false;
            this.highToolStripMenuItem1.Checked = false;
            this.middleToolStripMenuItem1.Checked = true;
            this.lowToolStripMenuItem1.Checked = false;
            this.Quality = "middle";
        }

        private void lowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.sourceToolStripMenuItem.Checked = false;
            this.highToolStripMenuItem1.Checked = false;
            this.middleToolStripMenuItem1.Checked = false;
            this.lowToolStripMenuItem1.Checked = true;
            this.Quality = "low";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String game = comboBox1.Text;
            Console.WriteLine("Selected " + game);
            if(game == "All games")
            {
                game = null;
            } 
            this.SelectedGame = game;
            this.loadStreams();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.loadStreams();
        }

        private void setupQualityAndGame()
        {
            if(this.Quality.Length > 0)
            {
                switch(this.Quality)
                {
                    case "source":
                        this.sourceToolStripMenuItem_Click(null, null);
                        break;
                    case "high":
                        this.highToolStripMenuItem1_Click(null, null);
                        break;
                    case "middle":
                        this.middleToolStripMenuItem1_Click(null, null);
                        break;
                    case "low":
                        this.lowToolStripMenuItem1_Click(null, null);
                        break;
                }
            }
            if(this.SelectedGame.Length > 0)
            {
                this.comboBox1.Text = this.SelectedGame;
            }
        }

        private void twitchUsernameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.twitchUsername = Microsoft.VisualBasic.Interaction.InputBox("Enter your Twitch username to see your followed channels", "Twitch username", "", -1, -1);

            this.followedImageList.ImageSize = new Size(80, 45);
            listView2.LargeImageList = this.followedImageList;
            listView2.SmallImageList = this.followedImageList;

            this.loadFollowedStreams();
        }

        private void loadFollowedStreams()
        {
            this.listView2.Items.Clear();
            this.followedImageList.Images.Clear();
            String URL = "https://api.twitch.tv/kraken/users/" + this.twitchUsername + "/follows/channels?limit=100";
            Cursor.Current = Cursors.WaitCursor;
                 
            string HTML = this.getHTML(URL);

            this.followedStreams = JObject.Parse(HTML);
            int i = 0;
            String queryString = "";
            foreach (JToken stream in this.followedStreams["follows"])
            {           
                if(i == 0)
                {
                    queryString = stream["channel"]["display_name"].ToString();
                } else
                {
                    queryString = queryString + "," + stream["channel"]["display_name"].ToString();
                }

                i++;                
            }          

            String queryURL = "https://api.twitch.tv/kraken/streams?channel=" + Uri.EscapeDataString(queryString);

            HTML = this.getHTML(queryURL);

            this.followedStreams = JObject.Parse(HTML);

            i = 0;
            foreach (JToken stream in this.followedStreams["streams"])
            {
                //retrieve all image files
                string imagePath = stream["preview"]["small"].ToString();

                //Add images to Imagelist
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(imagePath);
                MemoryStream ms = new MemoryStream(bytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                this.followedImageList.Images.Add(img);

                string st = (stream["channel"]["display_name"] + " playing " + stream["game"] + " for " + stream["viewers"] + " viewers (" + stream["channel"]["status"] + ")");
                this.listView2.Items.Add(new ListViewItem { ImageIndex = i, Text = st });

                i++;
            }
            this.listView2.Click += new System.EventHandler(this.listView2_Click);

            this.listView2.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.None);
            this.listView2.Columns[0].Width = Width - 4 - SystemInformation.VerticalScrollBarWidth;
            this.listView2.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

            Cursor.Current = Cursors.Default;

        }
    }
}