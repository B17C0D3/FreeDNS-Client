using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDNS_Client
{
    class Settings : EventArgs
    {
        public event EventHandler changedIntervall;
        public event EventHandler statusChanged;
        public event EventHandler domainsChanged;
        private string cfgFile = "FreeDNS.ini";
        private CfgFile config;

        public int UpdateInterval
        {
            get
            {
                return Convert.ToInt32(config.getValue("Settings", "UpdateIntervall", false));
            }
            set
            {
                config.setValue("Settings", "UpdateIntervall", value.ToString(), false);
                if (changedIntervall != null)
                    changedIntervall(this, new EventArgs());
            }
        }

        public bool LastStatus
        {
            get
            {
                return Convert.ToBoolean(config.getValue("Settings", "LastStatus", false));
            }
            set
            {
                config.setValue("Settings", "LastStatus", value.ToString(), false);
                if (statusChanged != null)
                    statusChanged(this, new EventArgs());
            }
        }
        public bool UseIPv4
        {
            get
            {
                return Convert.ToBoolean(config.getValue("Settings", "UseIPv4", false));
            }
            set
            {
                config.setValue("Settings", "UseIPv4", value.ToString(), false);
            }
        }
        public bool UseIPv6
        {
            get
            {
                return Convert.ToBoolean(config.getValue("Settings", "UseIPv6", false));
            }
            set
            {
                config.setValue("Settings", "UseIPv6", value.ToString(), false);
            }
        }
        public string LastIPv4
        {
            get
            {
                return config.getValue("Settings", "IPv4", false);
            }
            set
            {
                config.setValue("Settings", "IPv4", value, false);
            }
        }
        public string LastIPv6
        {
            get
            {
                return config.getValue("Settings", "IPv6", false);
            }
            set
            {
                config.setValue("Settings", "IPv6", value, false);
            }
        }

        public string[] DomainNames { get; private set; } = new string[] { };
        private List<Domain> domains = new List<Domain>();
        public Settings()
        {
            try
            {
                config = new CfgFile(cfgFile);
                if (config.Count() > 4)
                {
                    LoadDomainNames();
                    LoadDomains();
                }
                else
                    throw new Exception();
            }
            catch
            {
                //erstellle standart config
                config = new CfgFile();
                config.FileName = cfgFile;
                UpdateInterval = 30;
                LastStatus = false;
                UseIPv4 = false;
                UseIPv6 = false;

                Save();
            }
        }

        public Domain[] getDomains() => domains.ToArray();
        private void LoadDomains()
        {
            if (DomainNames.Length > -1)
            {
                domains.Clear();
                foreach (string s in DomainNames)
                {
                    SortedList<string, string> d = config.getCaption(s);
                    domains.Add(new Domain(config, s, d["key"], d["ip"], d["lastip"], d["lastupdate"], Convert.ToBoolean(d["activ"])));
                }
            }
        }

        public void AddDomain(bool activ, string name, string key, string ip)
        {
            domains.Add(new Domain(this.config, name, key, ip, activ));
            string names = config.getValue("Settings", "Domains", false);
            if (names == String.Empty)
                config.setValue("Settings", "Domains", name, false);
            else
                config.setValue("Settings", "Domains", $"{names};{name}", false);
            LoadDomainNames();

            if (domainsChanged != null)
                domainsChanged(this, new EventArgs());
        }

        private void LoadDomainNames()
        {
            string s = config.getValue("Settings", "Domains", false);
            if (s != String.Empty)
                this.DomainNames = s.Split(';');
        }

        public bool Save() => config.Save();
    }

    public class Domain
    {
        private CfgFile config;

        public bool Activ
        {
            get
            {
                return Convert.ToBoolean(config.getValue(Name, "activ", false));
            }
            set
            {
                config.setValue(Name, "activ", value.ToString(), false);
            }
        }
        public string Name { get; private set; }
        public string Key
        {
            get
            {
                return config.getValue(Name, "key", true);
            }
            private set
            {
                config.setValue(Name, "key", value, true);
            }
        }
        public string Ip
        {
            get
            {
                return config.getValue(Name, "ip", false);
            }
            set
            {
                config.setValue(Name, "ip", value, false);
            }
        }

        public string LastIP
        {
            get
            {
                return config.getValue(Name, "LastIP", false);
            }
            set
            {
                config.setValue(Name, "LastIP", value, false);
            }
        }
        public string LastUpdate
        {
            get
            {
                return config.getValue(Name, "lastUpdate", false);
            }
            set
            {
                config.setValue(Name, "lastUpdate", value, false);
            }
        }

        public Domain(Object c, string name, string key)
        {
            this.config = (CfgFile)c;
            this.Name = name;
            this.Key = key;
            this.Ip = "IPv4";
            this.LastIP = "0";
            this.LastUpdate = "Never";
            this.Activ = false;
        }
        public Domain(Object c, string name, string key, string ip, bool activ)
        {
            this.config = (CfgFile)c;
            this.Name = name;
            this.Key = key;
            this.Ip = ip;
            this.LastIP = "0";
            this.LastUpdate = "Never";
            this.Activ = activ;
        }

        public Domain(Object c, string name, string key, string ip, string lastIP, string lastUpdate, bool activ)
        {
            this.config = (CfgFile)c;
            this.Name = name;
            this.Key = key;
            this.Ip = ip;
            this.LastIP = lastIP;
            this.LastUpdate = lastUpdate;
            this.Activ = activ;
        }

        public void Update(string response)
        {
            LastIP = "0";
            LastUpdate = DateTime.Now.ToString();
        }

        public void deleteMe()
        {

        }
    }
}
