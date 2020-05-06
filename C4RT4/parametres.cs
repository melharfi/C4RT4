using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C4RT4
{
    public partial class parametres : Form
    {
        public Profile _profile = null;
        public zriwita _zriwita = null;

        public parametres(zriwita _form1,Profile _class1)
        {
            InitializeComponent();
            _zriwita = _form1;
            _profile = _class1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_profile.extra[0] > 9)
                extra1.SelectedIndex = _profile.extra[0] - 3;
            else
                extra1.SelectedIndex = _profile.extra[0]-1;
            
            if (_profile.extra[1] > 9)
                extra2.SelectedIndex = _profile.extra[1] - 3;
            else
                extra2.SelectedIndex = _profile.extra[1]-1;

            if (_profile.extra[2] > 9)
                extra3.SelectedIndex = _profile.extra[2] - 3;
            else
                extra3.SelectedIndex = _profile.extra[2]-1;

            if (_profile.extra[3] > 9)
                extra4.SelectedIndex = _profile.extra[3] - 3;
            else
                extra4.SelectedIndex = _profile.extra[3]-1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (extra1.SelectedIndex != extra2.SelectedIndex && extra1.SelectedIndex != extra3.SelectedIndex && extra1.SelectedIndex != extra4.SelectedIndex)
            {
                if (extra2.SelectedIndex != extra3.SelectedIndex && extra2.SelectedIndex != extra4.SelectedIndex)
                {
                    if (extra3.SelectedIndex != extra4.SelectedIndex)
                    {
                        _profile.extra[0] = Int16.Parse(extra1.SelectedItem.ToString());
                        Properties.Settings.Default.Set_extra1 = _profile.extra[0];

                        _profile.extra[1] = Int16.Parse(extra2.SelectedItem.ToString());
                        Properties.Settings.Default.Set_extra2 = _profile.extra[1];

                        _profile.extra[2] = Int16.Parse(extra3.SelectedItem.ToString());
                        Properties.Settings.Default.Set_extra3 = _profile.extra[2];

                        _profile.extra[3] = Int16.Parse(extra4.SelectedItem.ToString());
                        Properties.Settings.Default.Set_extra4 = _profile.extra[3];

                        Properties.Settings.Default.Save();
                        _zriwita.profile_save();

                        button1.Enabled = false;
                    }
                    else
                        MessageBox.Show(this, "Valeur EXTRA 3 est déja choisie, choisissez une autre", "Ouups !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show(this, "Valeur EXTRA 2 est déja choisie, choisissez une autre", "Ouups !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show(this, "Valeur EXTRA 1 est déja choisie, choisissez une autre", "Ouups !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            extra1.SelectedIndex = 6;
            extra2.SelectedIndex = 5;
            extra3.SelectedIndex = 7;
            extra4.SelectedIndex = 8;

            button1.Enabled = true;
            //button1_Click(null, null);
        }

        private void parametres_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            GC.Collect();
        }

        private void changed_extra1(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void changed_extra2(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void changed_extra3(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void changed_extra4(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
