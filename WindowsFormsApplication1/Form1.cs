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
        private JObject streams;

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

        private void Form1_Load(object sender, EventArgs e)
        {

            if (ExistsOnPath("livestreamer.exe"))
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


            string HTML = this.getHTML("https://api.twitch.tv/kraken/streams");

            this.streams = JObject.Parse(HTML);
            foreach(JToken stream in this.streams["streams"])
            {
                string st = (stream["channel"]["display_name"] + " playing " + stream["game"] + " for " + stream["viewers"] + " viewers");
                this.listBox1.Items.Add(st);
            }
        }

            private string getHTML(string URL)
            {
                System.Net.WebClient objWC = new System.Net.WebClient();
                return new System.Text.UTF8Encoding().GetString(objWC.DownloadData(URL));
            }

            private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
            {
                Console.WriteLine("Selected " + this.streams["streams"][this.listBox1.SelectedIndex]["channel"]["display_name"]);

                string channelName = this.streams["streams"][this.listBox1.SelectedIndex]["channel"]["name"].ToString();

                Process process = new Process();
                // Configure the process using the StartInfo properties.
                process.StartInfo.FileName = "livestreamer";
                process.StartInfo.Arguments = "twitch.tv/" + channelName + " source";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.Start();
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
                    Console.WriteLine(fullPath);
                    if (File.Exists(fullPath))
                        return fullPath;
                }
                return null;
            }
    }
}