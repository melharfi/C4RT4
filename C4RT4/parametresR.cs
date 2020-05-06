using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.IO;

namespace C4RT4
{
    public partial class parametresR : Form
    {
        public main _main;
        public parametresR(main _form)
        {
            InitializeComponent();
            _main = _form;
        }

        private void syndefault_MouseLeave(object sender, EventArgs e)
        {
            descriptionOption.Visible = false;
        }

        private void syneditable_MouseLeave(object sender, EventArgs e)
        {
            descriptionOption.Visible = false;
        }

        private void synauto_MouseLeave(object sender, EventArgs e)
        {
            descriptionOption.Visible = false;
        }

        private void syndefault_MouseMove(object sender, MouseEventArgs e)
        {
            descriptionOption.Text = "La synchronisation de l'application va se faire tout les 4 seconds.";
            descriptionOption.Visible = true;
        }

        private void synauto_MouseMove(object sender, MouseEventArgs e)
        {
            descriptionOption.Text = "Le délai de synchronisation est calculé automatiquement.";
            descriptionOption.Visible = true;
        }

        private void syneditable_MouseMove(object sender, MouseEventArgs e)
        {
            descriptionOption.Text = "Forcer le délai de synchronisation. (Attention, consulter le FAQ).";
            descriptionOption.Visible = true;
        }

        private void syneditable_CheckedChanged(object sender, EventArgs e)
        {
            if (syneditable.Checked == true)
            {
                graphpanel.Visible = true;
                descriptionOption.Location = new Point(descriptionOption.Location.X, 176);
                appliquerBTN.Location = new Point(appliquerBTN.Location.X, 196);
                tabControl1.Height = 223;
                this.Height = 260;
                appliquerBTN.Enabled = true;
            }
            else
            {
                graphpanel.Visible = false;
                descriptionOption.Location = new Point(descriptionOption.Location.X, 102);
                appliquerBTN.Location = new Point(appliquerBTN.Location.X, 156);
                tabControl1.Height = 149;
                this.Height = 205;

            }
        }

        private void global_load()
        {
            if (File.Exists("save\\global.xml"))
            {
                XmlTextReader xml_global = new XmlTextReader("save\\global.xml");
                xml_global.WhitespaceHandling = WhitespaceHandling.None;
                int tmp_data;
                while (xml_global.Read())
                {
                    if (xml_global.LocalName == "option")
                    {
                        tmp_data = Int16.Parse(xml_global.ReadString());
                        if (tmp_data == 1)
                            syndefault.Checked = true;
                        else if (tmp_data == 2)
                            synauto.Checked = true;
                        else if (tmp_data == 3)
                            syneditable.Checked=true;
                        xml_global.Read();
                    }

                    if (xml_global.LocalName == "speed")
                    {
                        tmp_data = Int16.Parse(xml_global.ReadString());
                        if (tmp_data > 1 && tmp_data < 7)
                            syntrackBar.Value = tmp_data;
                        else if (tmp_data < 2)
                            syntrackBar.Value = 2;
                        else if (tmp_data > 6)
                            syntrackBar.Value = 6;
                        xml_global.Read();
                    }

                    if (xml_global.LocalName == "url")
                        url_server.Text = xml_global.ReadString();

                    if (xml_global.LocalName == "dns")
                    {
                        dns_server.Text = xml_global.ReadString();
                        aboutUrl.Text = dns_server.Text;
                    }
                }
                xml_global.Close();
            }
            else
            {
                MessageBox.Show(this, "Le fichier de sauveguard est introuvable,\nmerci de réinstaller l'application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void parametresR_FormClosing(object sender, FormClosingEventArgs e)
        {
            _main.Enabled = true;
        }

        private void parametresR_Load(object sender, EventArgs e)
        {
            global_load();
            versionLb.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\t(2010) Free Edition";
        }


        private void global_save()
        {
            if (File.Exists("save\\global.xml"))
            {
                XmlTextWriter xml_profile = new XmlTextWriter("save\\global.xml", null);
                xml_profile.WriteStartDocument();
                xml_profile.WriteStartElement("params");
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteStartElement("option");
                if (syndefault.Checked == true)
                    xml_profile.WriteString("1");
                else if (synauto.Checked == true)
                    xml_profile.WriteString("2");
                else if (syneditable.Checked == true)
                    xml_profile.WriteString("3");

                xml_profile.WriteEndElement();
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteStartElement("speed");
                xml_profile.WriteString(syntrackBar.Value.ToString());

                xml_profile.WriteEndElement();
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteStartElement("url");
                xml_profile.WriteString(url_server.Text);

                xml_profile.WriteEndElement();
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteStartElement("dns");
                xml_profile.WriteString(dns_server.Text);

                xml_profile.WriteEndElement();
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteStartElement("version");
                xml_profile.WriteString(Assembly.GetExecutingAssembly().GetName().Version.ToString());

                xml_profile.WriteEndElement();
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteEndDocument();
                xml_profile.Close();
            }
            else
            {
                MessageBox.Show(this, "Le fichier de sauveguard est introuvable,\nmerci de réinstaller l'application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
        }

        private void appliquerBTN_Click(object sender, EventArgs e)
        {
            global_save();
            MessageBox.Show("Relancer l'application pour appliquer les modifications", "Important", MessageBoxButtons.OK, MessageBoxIcon.Information);
            appliquerBTN.Enabled = false;
        }

        private void syndefault_CheckedChanged(object sender, EventArgs e)
        {
            if (syndefault.Checked == true)
                appliquerBTN.Enabled = true;
        }

        private void synauto_CheckedChanged(object sender, EventArgs e)
        {
            if (synauto.Checked == true)
                appliquerBTN.Enabled = true;
        }
    }
}
