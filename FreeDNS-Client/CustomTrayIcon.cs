using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeDNS_Client
{
    public partial class CustomTrayIcon : ApplicationContext
    {
        private Settings settings;
        private System.Timers.Timer timer;
        private LogWriter logWriter;

        public CustomTrayIcon()
        {
            InitializeComponent();
            this.logWriter = new LogWriter(System.IO.Directory.GetCurrentDirectory());
            this.logWriter.weekly = true;
            this.settings = new Settings();
            this.timer = new System.Timers.Timer(settings.UpdateInterval * 60000);
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(Update);
            logWriter.WriteMessage("FreeDNS succsessfully started");
            if (settings.LastStatus)
            {
                this.timerToolStripMenuItem.Text = "Stop FreeDNS";
                Update();
                timer.Start();
                logWriter.WriteMessage($"update will start every {settings.UpdateInterval} minutes");
            }
            else
                logWriter.WriteMessage($"update timer is not running");
        }

        private void Update(object sender, System.Timers.ElapsedEventArgs e) => Update();
        private void Update(object sender, EventArgs e) => Update();
        private void StartStopTime(object sender, EventArgs e)
        {
            if (settings.LastStatus)
            {
                this.timerToolStripMenuItem.Text = "Resume FreeDNS";
                this.timer.Stop();
                this.settings.LastStatus = false;
                logWriter.WriteMessage("FreeDNS update interval stopped by user");
            }
            else
            {
                this.timerToolStripMenuItem.Text = "Stop FreeDNS";
                this.timer.Start();
                this.settings.LastStatus = true;
                logWriter.WriteMessage("FreeDNS update interval started by user");
                logWriter.WriteMessage($"update will start every {settings.UpdateInterval} minutes");
            }
            settings.Save();
        }
        private void Close(object sender, EventArgs e)
        {
            logWriter.WriteMessage("FreeDNS closed by user");
            logWriter.Dispose();
            settings.Save();
            notifyIcon.Visible = false;
            components.Dispose();
            ExitThread();
        }

        public void Update()
        {
            if (settings.UseIPv4)
            {
                string ipv4;
                do { ipv4 = CheckPublicIPv4(); } while (!IsIPv4(ipv4));
                if (settings.LastIPv4 != ipv4 && ipv4 != String.Empty)
                {
                    settings.LastIPv4 = ipv4;
                    logWriter.WriteMessage($"Neue IPv4={ipv4}");
                }
                foreach (Domain d in settings.getDomains())
                    if (d.Activ && d.Ip == "IPv4" && (d.LastIP != ipv4))
                    {
                        UpdateDNS(d.Key, ipv4);
                        d.LastIP = ipv4;
                        d.LastUpdate = DateTime.Now.ToString();
                    }
            }
            if (settings.UseIPv6)
            {
                string ipv6 = CheckPublicIPv6();
                if (settings.LastIPv6 != ipv6 && ipv6 != String.Empty)
                {
                    logWriter.WriteMessage($"Neue IPv6={ipv6}");
                    settings.LastIPv6 = ipv6;
                }
                foreach (Domain d in settings.getDomains())
                    if (d.Activ && d.Ip == "IPv6" && (d.LastIP != ipv6))
                    {
                        UpdateDNS(d.Key, ipv6);
                        d.LastIP = ipv6;
                        d.LastUpdate = DateTime.Now.ToString();
                    }
            }
            logWriter.WriteMessage("completet the update routine");
        }

        private string CheckPublicIPv4()
        {
            try
            {
                string url = "http://checkip.dyndns.org";
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd();
                return response.Split(':')[1].Split('<')[0].Trim(' ');
            }
            catch (Exception e)
            {
                logWriter.WriteMessage("Exception in function CheckPublicIPv4 :\n" + e.Message);
            }
            return String.Empty;
        }

        private string CheckPublicIPv6()
        {
            try
            {
                string url = "http://checkipv6.dyndns.org";
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd();
                return response.Split(' ')[5].Split('<')[0].Trim(' ');
            }
            catch (Exception e)
            {
                logWriter.WriteMessage("Exception in function CheckPublicIPv6 :\n" + e.Message);
            }
            return String.Empty;
        }

        private void UpdateDNS(string key, string ip)
        {
            try
            {
                string url = $@"http://freedns.afraid.org/dynamic/update.php?{key}&address={ip}";
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd();
                logWriter.WriteMessage(response.Trim());
            }
            catch (Exception e)
            {
                logWriter.WriteMessage("Exception in function UpdateDNS :\n" + e.Message);
            }
        }

        public static bool IsIPv4(string IP)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.IsMatch(IP, @"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$\b");
            }
            catch
            {
                return false;
            }
        }
    }
}