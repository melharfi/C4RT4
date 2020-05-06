using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Resources;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Xml;

namespace C4RT4
{
    public partial class zriwita : Form
    {
/////////////////////////////////////  Declaration des variable  ////////////////////////////
        private Label[] nom = new Label[4];
        private Label[] level = new Label[4];
        private Label lb_all_card = new Label();            // label de tas
        private Label winner_lb = new Label();
        private PictureBox backimg = new PictureBox();
        private PictureBox milieu = new PictureBox();
        private PictureBox[] avatarPB = new PictureBox[4];     // avatar des players dans le panneau
        private PictureBox[] all_card = new PictureBox[40];    // le tas
        internal PictureBox[,] p_card = new PictureBox[4, 15];  // les cartes des players
        private PictureBox[] choix = new PictureBox[4];
        private PictureBox[] avatar = new PictureBox[4];     // avatar des players dans le jeux
        private PictureBox winner_pic = new PictureBox();    // image de victoire
        private PictureBox[] winThumbs = new PictureBox[100];
        private Panel ngpan = null;
        private Panel[] player_pan = new Panel[4];          // panneau des players
        private TabControl ptab = new TabControl();
        private TabPage[] tab = new TabPage[4];
        private TextBox[] pnameT = new TextBox[4];          // nom des players dans la panneau
        private ComboBox[] avatarLB = new ComboBox[4];      //nom de l'avatar dans le panneau
        private ComboBox[] backsndLB = new ComboBox[4];     //profil sonor dans le panneau
        private ComboBox[] levelLB = new ComboBox[4];       // lvl dans le panneau
        private _player[] p = new _player[4];               // classe player
        private Profile load_profile = new Profile();
        private int nbr_player;
        private int[,] pos = new int[4, 2];
        private int block_pcard = 0;                 // affichage animé lors du survole de la sourie
        private int v_la_main;                       // la main
        private int nbr_cards;                       // = 40;
        private int[] p_pos = new int[] { 0, 1, 2, 3 };  //ordre chronologique
        private int[,] p_pos2 = new int[,] { { 1, 0, -1, -1, -1, -1 }, { 2, 0, 1, -1, -1, -1 }, { 2, 3, 1, 0, -1, -1 } };    //ordre des players
        private int[] extra = new[] { 7, 6, 10, 11 };    // cartes extra
        private int max_cards = 15;
        private string v_dos_card = "dos_1";
        private string v_front_card = "tamisé";
        private string[] matrice = new string[40];
        private string[] type_card = new string[4] { "-z", "-s", "-f", "-t" };
        private string v_milieu;
        private string v_milieu2;
        private bool launched = false;
        private bool winner_var = false;
        private bool ngpan_control = false;
        private bool block_me = false;
        private bool sound = true;
        private bool load_pro = false;          // true si la le chargement du profile a été fait.
        private bool debug_mode = false;
        private bool block = false;              // block when form is closed to stop process
        ///// cheats codes
        private bool cc_v_cards = false;      //afficher tout les cartes des joueurs
        ///// sound
        [DllImport("WinMM.dll")]
        private static extern bool PlaySound(string fname, int Mod, int flag);
        private System.Media.SoundPlayer backsound = new System.Media.SoundPlayer();
        //////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////  common line  ////////////////////////////////////
    private void toolStripButton1_Click(object sender, EventArgs e)
    {
        nouvellePartieToolStripMenuItem_Click(null, null);
    }

    /*[System.Runtime.InteropServices.DllImport("user32.dll")] 
    private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);


    private void FlashWindow()
    {
        if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
    }*/


    private void nouvellePartieToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (ngpan_control == false)
            new_game_menu();
        else if (ngpan_control == true)
        {
            if (ngpan.Visible == true)
                ngpan.Hide();

            winner_pic.Visible = false;
            ngpan.Show();
            ngpan.BringToFront();
            pnameT[0].Focus();
        }
    }

    private void tapi_motif1_Click(object sender, EventArgs e)
    {
        backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\1.jpg");
        tapi_motif1.Checked = true;
        tapi_motif2.Checked = false;
        tapi_motif3.Checked = false;
        tapi_motif4.Checked = false;
        tapi_motif5.Checked = false;
        if (load_pro == true)
        {
            load_profile.tapis = 1;
            profile_save();
        }
    }

    private void tapi_motif2_Click(object sender, EventArgs e)
    {
        backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\2.jpg");
        tapi_motif2.Checked = true;
        tapi_motif1.Checked = false;
        tapi_motif3.Checked = false;
        tapi_motif4.Checked = false;
        tapi_motif5.Checked = false;
        if (load_pro == true)
        {
            load_profile.tapis = 2;
            profile_save();
        }
    }

    private void tapi_motif3_Click(object sender, EventArgs e)
    {
        backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\3.jpg");
        tapi_motif3.Checked = true;
        tapi_motif1.Checked = false;
        tapi_motif2.Checked = false;
        tapi_motif4.Checked = false;
        tapi_motif5.Checked = false;
        if (load_pro == true)
        {
            load_profile.tapis = 3;
            profile_save();
        }
    }

    private void tapi_motif4_Click(object sender, EventArgs e)
    {
        backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\4.jpg");
        tapi_motif4.Checked = true;
        tapi_motif1.Checked = false;
        tapi_motif2.Checked = false;
        tapi_motif3.Checked = false;
        tapi_motif5.Checked = false;
        if (load_pro == true)
        {
            load_profile.tapis = 4;
            profile_save();
        }
    }

    private void tapi_motif5_Click(object sender, EventArgs e)
    {
        backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\5.jpg");
        tapi_motif5.Checked = true;
        tapi_motif1.Checked = false;
        tapi_motif2.Checked = false;
        tapi_motif3.Checked = false;
        tapi_motif4.Checked = false;
        if (load_pro == true)
        {
            load_profile.tapis = 5;
            profile_save();
        }
    }

    private void dos_card_m1_Click(object sender, EventArgs e)
    {
        dos_card_m1.Checked = true;
        dos_card_m2.Checked = false;
        dos_card_m3.Checked = false;
        v_dos_card = "dos_1";
        if (load_pro == true)
        {
            load_profile.dos = 1;
            profile_save();
        }
        change_back_card();
    }

    private void dos_card_m2_Click(object sender, EventArgs e)
    {
        dos_card_m1.Checked = false;
        dos_card_m2.Checked = true;
        dos_card_m3.Checked = false;
        v_dos_card = "dos_2";
        if (load_pro == true)
        {
            load_profile.dos = 2;
            profile_save();
        }
        change_back_card();
    }

    private void dos_card_m3_Click(object sender, EventArgs e)
    {
        dos_card_m1.Checked = false;
        dos_card_m2.Checked = false;
        dos_card_m3.Checked = true;
        v_dos_card = "dos_3";
        if (load_pro == true)
        {
            load_profile.dos = 3;
            profile_save();
        }
        change_back_card();
    }

    private void eclaire_card_Click(object sender, EventArgs e)
    {
        v_front_card = "tamisé";
        éclairéToolStripMenuItem.Checked = true;
        tamiséToolStripMenuItem.Checked = false;
        change_front_card("tamisé");
    }


    private void tamise_card_Click(object sender, EventArgs e)
    {
        v_front_card = "éclairé";
        tamiséToolStripMenuItem.Checked = true;
        éclairéToolStripMenuItem.Checked = false;
        change_front_card("éclairé");
    }
////////////////////////////////////////////////////////////////////////////////////




























/////////////////////////////////////  CODE START  /////////////////////////////////
        //FORM CONSTRUCTOR
        public zriwita()
        {
            InitializeComponent();
        }

        //FORM DESTRUCTOR
        ~zriwita()
        {
            destructor();
        }

        //2nd DESTRUCTOR
        private void destructor()
        {
            for (int cnt = 0; cnt < nbr_player; cnt++)
            {
                for (int cnt2 = 0; cnt2 < max_cards; cnt2++)
                    p_card[cnt, cnt2].Dispose();

                avatar[cnt].Dispose();
                nom[cnt].Dispose();

                if(cnt > 0)
                    level[cnt].Dispose();

                player_pan[cnt].Dispose();
            }

            nbr_player = 0;
            block_pcard = 0;
            v_la_main = 0;
            nbr_cards = 0;
            block_me = false;
            v_milieu = null;
            v_milieu2 = null;

            if(winner_var==true)
            {
                winner_pic.Dispose();
                winner_lb_timer.Enabled = false;
                winner_lb.Visible = false;
                winner_pic.Visible = false;
                winner_var = false;
            }

            for (int cnt2 = 0; cnt2 < all_card.Length; cnt2++)
                if (launched == true)
                    all_card[cnt2].Dispose();

            GC.Collect();
        }

/////////////////FORM LOAD
        private void zriwita_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(zriwita_FormClosing);
            backimg.Size = this.Size;
            //backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\1.jpg");
            backimg.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(backimg);
            zConsol.Text = "C4RT4 V" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\t(2010) Free Edition, HAVE FUN ;-)\n";
            backimg.Controls.Add(this.cheater_pic);
            toolStripProgressBar1.Value = 100;
            toolStripLabelProg.Text = "100 %";
            toolStripIncBtn.Enabled = false;
            profile_load();
        }

//////////////// LOAD PROFILE
        private void profile_load()
        {
            if (File.Exists("save\\zriwita.xml"))
            {
                XmlTextReader xml_profile = new XmlTextReader("save\\zriwita.xml");
                xml_profile.WhitespaceHandling = WhitespaceHandling.None;

                while (xml_profile.Read())
                {
                    if (xml_profile.LocalName == "dos")
                    {
                        load_profile.dos = Int16.Parse(xml_profile.ReadString());
                        if (load_profile.dos == 1)
                            dos_card_m1_Click(null, null);
                        else if (load_profile.dos == 2)
                            dos_card_m2_Click(null, null);
                        else if (load_profile.dos == 3)
                            dos_card_m3_Click(null, null);
                        xml_profile.Read();
                    }

                    load_profile.tapis = Properties.Settings.Default.tapis;

                    if (load_profile.tapis == 1)
                        tapi_motif1_Click(null, null);
                    else if (load_profile.tapis == 2)
                        tapi_motif2_Click(null, null);
                    else if (load_profile.tapis == 3)
                        tapi_motif3_Click(null, null);
                    else if (load_profile.tapis == 4)
                        tapi_motif4_Click(null, null);
                    else if (load_profile.tapis == 5)
                        tapi_motif5_Click(null, null);

                    if (xml_profile.LocalName == "son")
                    {
                        load_profile.son = Int16.Parse(xml_profile.ReadString());
                        if (load_profile.son == 0)
                            toolStripButton1_Click_1(null, null);
                        xml_profile.Read();
                    }

                    load_profile.extra[0] = Properties.Settings.Default.Set_extra1;
                    load_profile.extra[1] = Properties.Settings.Default.Set_extra2;
                    load_profile.extra[2] = Properties.Settings.Default.Set_extra3;
                    load_profile.extra[3] = Properties.Settings.Default.Set_extra4;

                }
                xml_profile.Close();
                load_pro = true;
            }
            else
            {
                MessageBox.Show(this,"Le fichier de sauveguard est introuvable,\nmerci de ré-installer l'application.", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
        }

        internal void profile_save()
        {
            if (File.Exists("save\\zriwita.xml"))
            {
                XmlTextWriter xml_profile = new XmlTextWriter("save\\zriwita.xml", null);
                xml_profile.WriteStartDocument();
                xml_profile.WriteStartElement("params");
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteStartElement("dos");
                xml_profile.WriteString(load_profile.dos.ToString());
                xml_profile.WriteEndElement();
                xml_profile.WriteWhitespace("\n");

                Properties.Settings.Default.tapis = load_profile.tapis;
                Properties.Settings.Default.Save();

                xml_profile.WriteStartElement("son");
                xml_profile.WriteString(load_profile.son.ToString());
                xml_profile.WriteEndElement();
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteEndDocument();
                xml_profile.Close();
            }
            else
            {
                MessageBox.Show(this, "Le fichier de sauveguard est introuvable,\nmerci de réinstaller l'application.", "Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                this.Close();
            }
        }


//////////////////FROM CLOSE
        void zriwita_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            main._main.Show();

            if (main._main.bs_cb.Checked == true)
                main._main.playBackSound();
        }

////////// NEW GAME MENU
        private void new_game_menu()
        {
            this.SuspendLayout();
            ngpan_control = true;
            // add a new panel
            ngpan = new Panel();
            ngpan.Size = new Size(250, 300);
            ngpan.Location = new Point((this.Width - ngpan.Width) >> 1, (this.Height - ngpan.Height) >> 1);
            ngpan.BackColor = Color.CornflowerBlue;
            ngpan.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(ngpan);
            ngpan.BringToFront();

            // add a new label "Pérsonaliser sa partie"
            Label ngpanL1 = new Label();
            ngpanL1.Text = "Pérsonaliser sa partie";
            ngpanL1.ForeColor = Color.DarkRed;
            ngpanL1.Font = new Font(ngpanL1.Font.Name, 9, FontStyle.Bold);
            ngpanL1.AutoSize = true;
            ngpan.Controls.Add(ngpanL1);
            ngpanL1.Location = new Point((ngpan.Width - ngpanL1.Width) >> 1, 2);

            // add a new label "Nombre d'adversaire"
            Label nplab = new Label();
            nplab.Text = "Nombre d'adversaire : ";
            nplab.ForeColor = Color.WhiteSmoke;
            nplab.AutoSize = true;
            ngpan.Controls.Add(nplab);
            nplab.Location = new Point(6, 46);

            // add radio buttons for players
            RadioButton p2rad = new RadioButton();
            p2rad.Text = "2P";
            p2rad.AutoSize = true;
            p2rad.Checked = true;
            ngpan.Controls.Add(p2rad);
            p2rad.Location = new Point(nplab.Width + 6, 44);
            p2rad.Click += new EventHandler(p2rad_Click);

            RadioButton p3rad = new RadioButton();
            p3rad.Text = "3P";
            p3rad.AutoSize = true;
            ngpan.Controls.Add(p3rad);
            p3rad.Location = new Point(nplab.Width + 46, 44);
            p3rad.Click += new EventHandler(p3rad_Click);

            RadioButton p4rad = new RadioButton();
            p4rad.Text = "4P";
            p4rad.AutoSize = true;
            ngpan.Controls.Add(p4rad);
            p4rad.Location = new Point(nplab.Width + 86, 44);
            p4rad.Click += new EventHandler(p4rad_Click);

            // add a new Tabcontrol
            ptab.Height = 200;
            ptab.Location = new Point((ngpan.Width - ptab.Width) >> 1, nplab.Location.Y + 20);
            ngpan.Controls.Add(ptab);
            tab_add(2);

            // add a new button
            Button launchBTN = new Button();
            launchBTN.Text = "Commencer la partie";
            launchBTN.AutoSize = true;
            launchBTN.UseVisualStyleBackColor = true;
            launchBTN.BackColor = Color.Gainsboro;
            launchBTN.Location = new Point(((ngpan.Width - launchBTN.Width) >> 2) - 20, ptab.Location.Y + ptab.Height + 4);
            ngpan.Controls.Add(launchBTN);
            launchBTN.Click += new EventHandler(launchBTN_Click);
            pnameT[0].Focus();

            // add cancel button
            Button cancelBTN = new Button();
            cancelBTN.Text = "Annuler";
            cancelBTN.AutoSize = true;
            cancelBTN.UseVisualStyleBackColor = true;
            cancelBTN.BackColor = Color.Gainsboro;
            cancelBTN.Location = new Point(launchBTN.Location.X + launchBTN.Width + 14, launchBTN.Location.Y);
            ngpan.Controls.Add(cancelBTN);
            cancelBTN.Click += new EventHandler(cancelBTN_Click);
            this.ResumeLayout();
        }

        // btn fermer du panneau
        void cancelBTN_Click(object sender, EventArgs e)
        {
            ngpan.Hide();
        }


////////// add tabs
        private int tab_add(int nbr_tab)
        {
            ptab.TabPages.Clear();

            for (int cnt = 0; cnt < nbr_tab; cnt++)
            {
                tab[cnt] = new TabPage();
                ptab.Controls.Add(tab[cnt]);

                if (cnt == 0)
                    tab[cnt].Text = " Vous";
                else
                    tab[cnt].Text = "Player " + (cnt + 1);

                // label nom du player
                Label[] pnameL = new Label[4];
                pnameL[cnt] = new Label();
                pnameL[cnt].Text = "Pseudo :";
                pnameL[cnt].AutoSize = true;
                pnameL[cnt].Location = new Point(16, 10);
                tab[cnt].Controls.Add(pnameL[cnt]);

                // textbox nom du player
                pnameT[cnt] = new TextBox();
                pnameT[cnt].Location = new Point(pnameL[cnt].Width + 16, 8);
                pnameT[cnt].MaxLength = 10;
                pnameT[cnt].KeyDown += new KeyEventHandler(pnameT_KeyDown);

                if(cnt==0)
                    pnameT[cnt].Text = "Player_" + cnt;
                else
                    pnameT[cnt].Text = "BOT_" + cnt;

                tab[cnt].Controls.Add(pnameT[cnt]);

                Random rnd = new Random();
                DirectoryInfo di_files = new DirectoryInfo(@"img\zriwita\avatars");
                FileInfo[] fi_files = di_files.GetFiles();
                int rnd2 = rnd.Next(fi_files.Length);
                bool valid_dir = false;
                
                for (int cnt_files = 0; cnt_files < fi_files.Length; cnt_files++)
                    if (fi_files[cnt_files].Extension==".jpg")
                    {
                        valid_dir = true;
                        break;
                    }

                if (valid_dir == true)
                    while (fi_files[rnd2].Extension != ".jpg")
                        rnd2 = rnd.Next(fi_files.Length);
                else
                {
                    MessageBox.Show(this,"Vous n'avez aucun avatar installé sur votre bibliotheque\n\t\t ré-installer l'application svp","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    this.Close();
                    break;
                }

                //image d'avatar
                avatarPB[cnt] = new PictureBox();
                avatarPB[cnt].SizeMode = PictureBoxSizeMode.StretchImage;
                avatarPB[cnt].Location = new Point(16, 40);
                avatarPB[cnt].Size = new Size(80, 70);
                
                if(valid_dir==true)
                    avatarPB[cnt].Image = (Bitmap)Image.FromFile(@"img\zriwita\avatars\"+fi_files[rnd2]);
                
                tab[cnt].Controls.Add(avatarPB[cnt]);
                
                //combobox d'avatar
                avatarLB[cnt] = new ComboBox();
                avatarLB[cnt].DropDownStyle = ComboBoxStyle.DropDownList;

                for (int cnt_avatar = 0; cnt_avatar < fi_files.Length; cnt_avatar++)
                    if (fi_files[cnt_avatar].Extension == ".jpg")
                        avatarLB[cnt].Items.Add(fi_files[cnt_avatar].Name.Substring(0, fi_files[cnt_avatar].Name.IndexOf(".")));
                rnd2 = rnd.Next(avatarLB[cnt].Items.Count);
                
                //PictureBox pour Avatar
                avatarLB[cnt].SelectedIndex = rnd2;
                avatarLB[cnt].Font = new Font(avatarLB[cnt].Font.Name, 9, FontStyle.Bold);
                avatarLB[cnt].Size = new Size(86, 25);
                avatarLB[cnt].Location = new Point((avatarPB[cnt].Width) + 18, avatarPB[cnt].Location.Y + (avatarPB[cnt].Width - avatarLB[cnt].Height) - 10);
                avatarLB[cnt].SelectedIndexChanged += new EventHandler(zriwita_SelectedIndexChanged);
                tab[cnt].Controls.Add(avatarLB[cnt]);

                //Label pour profil sonore
                Label[] backsndL = new Label[4];
                backsndL[cnt] = new Label();
                backsndL[cnt].Text = "Profil sonore :";
                backsndL[cnt].AutoSize = true;
                backsndL[cnt].Location = new Point(10, avatarLB[cnt].Location.Y + avatarLB[cnt].Height + 10);
                tab[cnt].Controls.Add(backsndL[cnt]);

                //Combobox pour profil sonor
                backsndLB[cnt] = new ComboBox();
                backsndLB[cnt].DropDownStyle = ComboBoxStyle.DropDownList;

                backsndLB[cnt].Items.Add("Activé");
                backsndLB[cnt].Items.Add("Désactivé");

                backsndLB[cnt].Font = new Font(backsndLB[cnt].Font.Name, 9, FontStyle.Bold);
                backsndLB[cnt].Size = new Size(104, 30);
                backsndLB[cnt].Location = new Point(backsndL[cnt].Location.X + backsndL[cnt].Width, backsndL[cnt].Location.Y - 2);
                tab[cnt].Controls.Add(backsndLB[cnt]);
                backsndLB[cnt].SelectedIndex = 0;

                //Label pour level
                if (cnt > 0)
                {
                    // tab level
                    Label[] levelL = new Label[4];
                    levelL[cnt] = new Label();
                    levelL[cnt].Text = "Dificulté :";
                    levelL[cnt].AutoSize = true;
                    levelL[cnt].Location = new Point(10, backsndLB[cnt].Location.Y + backsndLB[cnt].Height + 6);
                    tab[cnt].Controls.Add(levelL[cnt]);

                    // Combobox level
                    levelLB[cnt] = new ComboBox();
                    levelLB[cnt].DropDownStyle = ComboBoxStyle.DropDownList;
                    levelLB[cnt].Items.Add("Facile");
                    levelLB[cnt].Items.Add("Normal");
                    levelLB[cnt].Items.Add("Dificile");
                    levelLB[cnt].Items.Add("Aléatoire");
                    levelLB[cnt].Font = new Font(levelLB[cnt].Font.Name, 9, FontStyle.Bold);
                    levelLB[cnt].Size = new Size(104, 30);
                    levelLB[cnt].Location = new Point(levelL[cnt].Location.X + levelL[cnt].Width + 20, levelL[cnt].Location.Y);
                    levelLB[cnt].SelectedIndex = 0;
                    tab[cnt].Controls.Add(levelLB[cnt]);
                }
            }
            return 0;
        }

        void pnameT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                launchBTN_Click("Commencer la partie", null);
                zriwita.ActiveForm.Focus();
            }
            else if (e.KeyValue == 27)
            {
                ngpan.Hide();
                zriwita.ActiveForm.Focus();
            }
        }

        //AVATAR CHANGE
        void zriwita_SelectedIndexChanged(object sender, EventArgs e)
        {
            avatarPB[ptab.SelectedIndex].Image = (Bitmap)Image.FromFile(@"img\zriwita\avatars\" + avatarLB[ptab.SelectedIndex].Text + ".jpg");
        }

        // LAUNCH GAME
        void launchBTN_Click(object sender, EventArgs e)
        {
            bool error = false;

            for (int cnt = 0; cnt < ptab.TabCount; cnt++)
                if (pnameT[cnt].Text == "")
                {
                    MessageBox.Show(this,"Veillez donner un nom au joueur N°" + (cnt + 1),"Attention",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                    ptab.SelectTab(cnt);
                    pnameT[cnt].Focus();
                    error = true;
                    break;
                }

            if (error == false)
            {
                if (launched== true)
                {
                    for(int cnt=0;cnt<nbr_player;cnt++)
                        p[cnt].destructor();
                    destructor();
                }
                
                Random rnd = new Random();
                int i;

                for (int cnt = 0; cnt < ptab.TabCount; cnt++)
                {
                    p[cnt] = new _player(this);
                    p[cnt].nom = pnameT[cnt].Text;
                    p[cnt].avatar = avatarLB[cnt].Text;
                    p[cnt].position = cnt;

                    if (backsndLB[cnt].Text == "Activé")
                        p[cnt].sound = true;
                    else
                        p[cnt].sound = false;

                    if (cnt > 0)
                        if (levelLB[cnt].Text == "Aléatoire")
                        {
                            i = rnd.Next(3);
                            p[cnt].type = "CPU";

                            if (i == 0) p[cnt].level = "Facile";
                            else if (i == 1) p[cnt].level = "Normal";
                            else if (i == 2) p[cnt].level = "Dificile";
                        }
                        else
                        {
                            p[cnt].level = levelLB[cnt].Text;
                            p[cnt].type = "CPU";
                        }
                    else
                        p[cnt].type = "PLAYER";
                }
                var_ini();
                nbr_player = ptab.TabCount;
                ngpan.Hide();
                set_po(ptab.TabCount);
                nbr_cards = 40;
                start();
                zConsol.Text += "SYSTEM > Game started successfuly\n";
                préférenceTS.Enabled = false;
            }
        }

        // initialisation
        private void var_ini()
        {
            winner_lb.Visible = false;
            block_pcard = 0;
            block_me = false;
            v_la_main=0;

            if (launched == true)
            {
                for (int cnt = 0; cnt < 4; cnt++)
                    choix[cnt].Visible = false;

                for (int cnt = 0; cnt < 100; cnt++)
                    winThumbs[cnt].Visible = false;
            }
        }

        // initialisation da la matrice
        private void start()
        {
            int inx = 0;

            foreach (string type in type_card)
            {
                for (int cnt = inx; cnt < (10 + inx); cnt++)
                    if (cnt == (7 + inx)) matrice[cnt] = ((cnt - inx) + 3) + type;
                    else if (cnt == (8 + inx)) matrice[cnt] = ((cnt - inx) + 3) + type;
                    else if (cnt == (9 + inx)) matrice[cnt] = ((cnt - inx) + 3) + type;
                    else matrice[cnt] = ((cnt - inx) + 1) + type;
                inx += 10;
            }

            //création des vecteurs des players
            Random rnd1 = new Random();
            int rnd2 = rnd1.Next(39);
            
            for (int cnt = 0; cnt < nbr_player; cnt++)
                for (int cnt2 = 0; cnt2 < 5; cnt2++)
                {
                    while (matrice[rnd2] == null)
                        rnd2 = rnd1.Next(39);

                    p[cnt].cards[cnt2] = matrice[rnd2];
                    matrice[rnd2] = null;
                }

            // initialisation de la valeur de la carte du milieu
            rnd2 = rnd1.Next(39);
            bool tmp_var = true;
            while (tmp_var)
            {
                if (matrice[rnd2] != null)
                {
                    if (matrice[rnd2].Split('-')[0] == load_profile.extra[0].ToString() || matrice[rnd2].Split('-')[0] == load_profile.extra[1].ToString() || matrice[rnd2].Split('-')[0] == load_profile.extra[2].ToString() || matrice[rnd2].Split('-')[0] == load_profile.extra[3].ToString())
                        rnd2 = rnd1.Next(39);
                    else
                        break;
                }
                else
                    rnd2 = rnd1.Next(39);
            }

            v_milieu = matrice[rnd2];
            matrice[rnd2] = null;

            // position des cartes
            int sp = 0;
            int[,] pos = { { 300, this.Height - 202 }, { 300, shortcuts.Location.Y + shortcuts.Height }, { this.Width - 180, shortcuts.Location.Y + shortcuts.Height + 230 }, { 0, shortcuts.Location.Y + shortcuts.Height + 230 } };
            
            // création des cartes a tirer
            for (int cnt = 0; cnt < 40; cnt++)
            {
                all_card[cnt] = new PictureBox();
                all_card[cnt].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                all_card[cnt].SizeMode = PictureBoxSizeMode.AutoSize;
                all_card[cnt].Location = new Point(200 + sp, ((this.Height - all_card[cnt].Height) >> 1) - 20 + sp);
                backimg.Controls.Add(all_card[cnt]);
                all_card[cnt].Name = cnt.ToString();
                all_card[cnt].BringToFront();
                sp += 2;
                all_card[cnt].Click += new EventHandler(all_card_Click);
                if (cnt > (39 - (5 * nbr_player)))
                    all_card[cnt].Visible = false;
            }

            // label du comptage des cartes
            lb_all_card.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lb_all_card.ForeColor = Color.DarkOrange;
            nbr_cards = nbr_cards - 1 - (5 * nbr_player);
            lb_all_card.Text = "Il reste " + nbr_cards + " cartes";
            lb_all_card.AutoSize = true;
            lb_all_card.Location = new Point(all_card[0].Location.X, all_card[0].Location.Y - 16);
            lb_all_card.Visible = true;
            lb_all_card.BackColor = Color.Transparent;
            lb_all_card.Parent = backimg;
            backimg.Controls.Add(lb_all_card);
            lb_all_card.BringToFront();

            // création du carte du milieu
            milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");
            milieu.SizeMode = PictureBoxSizeMode.StretchImage;
            milieu.Size = new Size(140, 240);
            milieu.Location = new Point((this.Width >> 1) - (milieu.Width >> 1), (this.Height >> 1) - (milieu.Height >> 1));
            this.Controls.Add(milieu);
            milieu.BringToFront();
            v_la_main = 0;
            milieu.Visible = true;
            for (int cnt = 0; cnt < nbr_player; cnt++)
                add_cards_once(pos[cnt, 0], pos[cnt, 1], cnt, v_front_card, v_dos_card, cnt);
            t_tour.Enabled = true;

            //creation des cartes de CHOIX
            for (int cnt = 0; cnt < 4; cnt++)
            {
                choix[cnt]=new PictureBox();
                choix[cnt].Image=(Bitmap)Image.FromFile(@"img\cartes\"+ v_front_card + @"\1" + type_card[cnt] + ".jpg");
                choix[cnt].SizeMode = PictureBoxSizeMode.StretchImage;
                choix[cnt].Size = new Size(70, 100);
                choix[cnt].Location = new Point(milieu.Location.X - 120 + (cnt * 100), milieu.Location.Y + 200);
                this.Controls.Add(choix[cnt]);
                choix[cnt].BringToFront();
                choix[cnt].Name = cnt.ToString();
                choix[cnt].MouseEnter += new EventHandler(choix_MouseEnter);
                choix[cnt].MouseLeave += new EventHandler(choix_MouseLeave);
                choix[cnt].Click += new EventHandler(choix_Click);
                choix[cnt].Visible = false;
            }
        }

        void choix_MouseLeave(object sender, EventArgs e)
        {
            PictureBox tmp = (PictureBox)sender;
            tmp.Size = new Size(70, 100);
        }

        void choix_MouseEnter(object sender, EventArgs e)
        {
            PictureBox tmp = (PictureBox)sender;
            tmp.Size = new Size(80, 120);
        }

        // initialisation des position des players
        private int set_po(int nbr)
        {   
            int[,] pos = { { 200, this.Height - 140 }, { 200, shortcuts.Location.Y + shortcuts.Height }, { this.Width - 200, shortcuts.Location.Y + shortcuts.Height + 100 }, { 0, shortcuts.Location.Y + shortcuts.Height + 100} };

            for (int cnt = 0; cnt < nbr; cnt++)
            {
                // affichage du paneau du player nbr
                player_pan[cnt] = new Panel();
                player_pan[cnt].Size = new Size(92, 120);
                player_pan[cnt].Location = new Point(pos[cnt, 0], pos[cnt, 1]);
                player_pan[cnt].BackColor = Color.LightGray;
                this.Controls.Add(player_pan[cnt]);
                player_pan[cnt].BringToFront();

                // affichage de l'avatar
                avatar[cnt] = new PictureBox();
                avatar[cnt].Image = (Bitmap)Image.FromFile(@"img\zriwita\avatars\" + p[cnt].avatar + ".jpg");
                avatar[cnt].SizeMode = PictureBoxSizeMode.StretchImage;
                avatar[cnt].Size = new Size(80, 80);
                avatar[cnt].Location = new Point(6, 4);
                player_pan[cnt].Controls.Add(avatar[cnt]);
                
                // affichage du nom
                nom[cnt] = new Label();
                nom[cnt].AutoSize = true;
                nom[cnt].Text = p[cnt].nom;
                nom[cnt].Font = new Font(nom[cnt].Font.Name, 9, FontStyle.Bold);
                nom[cnt].ForeColor = Color.Red;
                nom[cnt].BackColor = Color.Transparent;
                nom[cnt].Location = new Point(4, avatar[cnt].Height + 4);
                player_pan[cnt].Controls.Add(nom[cnt]);

                //affichage du niveau
                if (cnt > 0)
                {
                    level[cnt] = new Label();
                    level[cnt].AutoSize = true;
                    level[cnt].Text = "Niveau : " + p[cnt].level;
                    level[cnt].Font = new Font(level[cnt].Font.Name, 8);
                    if (p[cnt].level == "Facile") level[cnt].ForeColor = Color.Green;
                    else if (p[cnt].level == "Normal") level[cnt].ForeColor = Color.Blue;
                    else level[cnt].ForeColor = Color.Red;
                    level[cnt].BackColor = Color.Transparent;
                    level[cnt].Location = new Point(4, nom[cnt].Location.Y + nom[cnt].Height + 4);
                    player_pan[cnt].Controls.Add(level[cnt]);
                }
            }
            return 0;
        }

///////// DYSPLAY OF 15 CARDS * NUMBER OF PLAYERS
        private void add_cards_once(int x, int y, int z, string v_front_card, string v_dos_card, int player)
        {
            int first_pos;
            int second_pos;

            if (z >= 2)
            {
                first_pos = 0;
                second_pos = 20;
            }
            else
            {
                first_pos = 30;
                second_pos = 0;
            }

            for (int cnt2 = 0; cnt2 < max_cards; cnt2++)
            {
                p_card[player,cnt2] = new PictureBox();

                if (z == 0)
                {
                    if (cnt2 < 5)
                        p_card[player, cnt2].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt2] + ".jpg");
                    else
                        p_card[player, cnt2].Visible = false;

                    p_card[player, cnt2].Size = new Size(100, 170);
                }
                else
                {
                    if (cnt2 < 5 && cc_v_cards == true)
                        p_card[player, cnt2].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt2] + ".jpg");
                    else
                        p_card[player, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));

                    if (cnt2 > 4)
                        p_card[player, cnt2].Visible = false;

                    if (z == 1)
                        p_card[player, cnt2].Size = new Size(100, 170);
                    else if (z == 2)
                    {
                        p_card[player, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                        p_card[player, cnt2].Size = new Size(170, 100);
                    }
                    else if (z == 3)
                    {
                        p_card[player, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        p_card[player, cnt2].Size = new Size(170, 100);
                    }
                }

                p_card[player, cnt2].SizeMode = PictureBoxSizeMode.StretchImage;
                p_card[player, cnt2].Location = new Point(x + (cnt2 * first_pos), y + (cnt2 * second_pos));
                p_card[player, cnt2].Name = cnt2.ToString();
                this.Controls.Add(p_card[player, cnt2]);
                p_card[player, cnt2].BringToFront();

                if (z == 0)
                {
                    p_card[player, cnt2].MouseLeave += new EventHandler(p_card_MouseLeave);
                    p_card[player, cnt2].MouseEnter += new EventHandler(p_card_MouseEnter);
                    p_card[0, cnt2].Click += new EventHandler(p_card_Click);
                    p_card[player, cnt2].Cursor = Cursors.Hand;
                }
            }
            launched = true;
            avantToolStripMenuItem.Enabled = true;
            éclairéToolStripMenuItem.Enabled = true;
            tamiséToolStripMenuItem.Enabled = true;
            RGtoolStripButton.Enabled = true;
            rejouerToolStripMenuItem.Enabled = true;
        }

///////// ADD CARDS
        private void add_cards(int player, int nbr)
        {
            Random rnd1 = new Random();
            int rnd2;

            if (nbr_cards > 0)
                for (int cnt = 0; cnt < nbr; cnt++)
                {
                    for (int cnt2 = 0; cnt2 < max_cards; cnt2++)
                        if (p[player].cards[cnt2] == null)
                        {
                            rnd2 = rnd1.Next(39);

                            while (matrice[rnd2] == null && matrice[rnd2] != v_milieu)
                                rnd2 = rnd1.Next(39);

                            p[player].cards[cnt2] = matrice[rnd2];
                            matrice[rnd2] = null;

                            if (player == 0)
                                p_card[player, cnt2].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt2] + ".jpg");
                            else if (player == 1)
                                if (cc_v_cards == false)
                                    p_card[player, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                                else
                                    p_card[player, cnt2].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt2] + ".jpg");
                            else if (player == 2)
                            {
                                if (cc_v_cards == false)
                                    p_card[player, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                                else
                                    p_card[player, cnt2].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt2] + ".jpg");

                                p_card[player, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                            }
                            else
                            {
                                if (cc_v_cards == false)
                                    p_card[player, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                                else
                                    p_card[player, cnt2].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt2] + ".jpg");

                                p_card[player, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                            }
                            p_card[player, cnt2].Visible = true;
                            p_card[player, cnt2].BringToFront();
                            break;
                        }
                    all_card[nbr_cards].Visible = false;
                    nbr_cards--;
                    lb_all_card.Text = "il reste " + nbr_cards + " cartes";
                }
            else
            {
                // regeneration function
                MessageBox.Show(this,"Aucune carte trouvé !!!\n reporter le bug SVP (Erreur 31)","Erreur interne",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

///////// evenement MOUSEOUT
        private void p_card_MouseLeave(object sender, EventArgs e)
        {
            block_pcard = 0;
            PictureBox tmp = (PictureBox)sender;
            tmp.Location = new Point(tmp.Location.X, this.Height - 202);
            tmp.Size = new Size(100, 170);
        }

///////// evenement MOUSEOVER
        private void p_card_MouseEnter(object sender, EventArgs e)
        {
            if (v_la_main == 0 && block_me==false)
                if (block_pcard < 30)
                {
                    PictureBox tmp = (PictureBox)sender;
                    tmp.Location = new Point(tmp.Location.X, tmp.Location.Y - 45);
                    tmp.Size = new Size(130, 220);
                    block_pcard++;
                }
        }

//////// organisation des cartes
        private void organiser(int player)
        {
            for (int cnt = 0; cnt < max_cards; cnt++)
                if (p[player].cards[cnt] == null)
                {
                    if (cnt < max_cards - 1)
                    {
                        if (p[player].cards[cnt + 1] != null)
                        {
                            p[player].cards[cnt] = p[player].cards[cnt + 1];
                            p[player].cards[cnt + 1] = null;

                            if (player == 0)
                                p_card[player, cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt] + ".jpg");
                            else if (player == 1)
                                if (cc_v_cards == false)
                                    p_card[player, cnt].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                                else
                                    p_card[player, cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt] + ".jpg");
                            else if (player == 2)
                            {
                                if (cc_v_cards == false)
                                    p_card[player, cnt].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                                else
                                    p_card[player, cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt] + ".jpg");

                                p_card[player, cnt].Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                            }
                            else
                            {
                                if (cc_v_cards == false)
                                    p_card[player, cnt].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                                else
                                    p_card[player, cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt] + ".jpg");

                                p_card[player, cnt].Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                            }
                            p_card[player, cnt].Visible = true;
                            p_card[player, cnt].BringToFront();
                        }
                        else
                        {
                            p_card[player, cnt].Visible = false;
                            break;
                        }
                    }
                    else
                    {
                        p[player].cards[max_cards - 1] = null;
                        p_card[player, max_cards - 1].Visible = false;
                        MessageBox.Show(this,"Joueur " + player + " n'a plus de cartes !!! \n Reporter le bug SVP (Erreur 32)","Erreur interne",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
        }

///////// TOUR
        private void tour(int pos)
        {
            if (p[pos].level == "Facile")
                mind_1(pos);
            else if (p[pos].level == "Normal")
                mind_2(pos);
            else if (p[pos].level == "Dificile")
                mind_3(pos);
        }

////////// click sur le tas des cartes
        void all_card_Click(object sender, EventArgs e)
        {
            if (v_la_main == 0 && block_me == false)
            {
                PictureBox tmp = (PictureBox)sender;
                int count_p_cards = 0;

                for (int cnt = 0; cnt < max_cards; cnt++)
                    if (p[0].cards[cnt] == null)
                        break;
                    else
                        count_p_cards++;

                if (count_p_cards < max_cards)
                    add_cards(0, 1);
                else
                {
                    // lancement du son si le player a 15 cartes

                }
                v_la_main = p_pos2[nbr_player - 2, v_la_main];
                t_tp.Enabled = true;

                if(debug_mode==true)
                    check_bugs();
            }
        }

/////////// click event on tab ///////////////////////////
        void p2rad_Click(object sender, EventArgs e)
        {
            tab_add(2);
        }

        void p3rad_Click(object sender, EventArgs e)
        {
            tab_add(3);
        }

        void p4rad_Click(object sender, EventArgs e)
        {
            tab_add(4);
        }

        /////////////////////////  changement du dos des cartes  ////////////////////
        private void change_back_card()
        {
            for (int cnt = 1; cnt < nbr_player; cnt++)
                for (int cnt2 = 0; cnt2 < max_cards; cnt2++)
                {
                    p_card[cnt, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                    if (cnt == 2)
                        p_card[cnt, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                    else if (cnt == 3)
                        p_card[cnt, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                }
        }

        /////////////////////////  changement du l'avant des cartes  ////////////////////
        private void change_front_card(string type)
        {
            if (type == "tamisé")
            {
                milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");
                for (int cnt = 0; p[0].cards[cnt]!=null ; cnt++)
                    p_card[0, cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[0].cards[cnt] + ".jpg");
            }
            else
            {
                milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");
                for (int cnt = 0; p[0].cards[cnt] != null; cnt++)
                    p_card[0, cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[0].cards[cnt] + ".jpg");
            }
        }

        private void t_tour_Tick(object sender, EventArgs e)
        {
            for (int c_player = 0; c_player < nbr_player; c_player++)
            {
                if (c_player == v_la_main)
                {
                    if (player_pan[v_la_main].BackColor == Color.DarkRed)
                        player_pan[v_la_main].BackColor = Color.CadetBlue;
                    else
                        player_pan[v_la_main].BackColor = Color.DarkRed;
                }
                else
                    player_pan[c_player].BackColor = Color.LightGray;
            }
        }

        private void t_tp_Tick(object sender, EventArgs e)
        {
            t_tp.Enabled = false;
            t_tp.Interval = 2000;
            next_player2();
        }

        private void next_player2()
        {
            if (v_la_main == 0)
            {
                block_me = false;
                if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }
            else
                block_me = true;
            
            tour(v_la_main);
        }


////////////////////// evenement CLICK Player 0
        private void p_card_Click(object sender, EventArgs e)
        {
            if (v_la_main == 0 && block_me == false)
            {
                PictureBox pic = (PictureBox)sender;
                int pic_number = Int16.Parse(pic.Name);

                for (int cnt = 0; cnt < max_cards; cnt++)
                {
                    p_card[0, cnt].Size = new Size(100, 170);
                    p_card[0, cnt].Location = new Point(p_card[0, cnt].Location.X, this.Height - 202);
                }

                // Si la condition est correcte
                if (p[0].cards[pic_number].Split('-')[0] == v_milieu.Split('-')[0] || p[0].cards[pic_number].Split('-')[1] == v_milieu.Split('-')[1])
                {
                    // passage de la carte au tirage
                    for (int cnt = 0; cnt < 40; cnt++)
                        if (matrice[cnt] == null)
                        {
                            if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                                matrice[cnt] = v_milieu2;
                            else
                                matrice[cnt] = v_milieu;
                            break;
                        }

                    v_milieu = p[0].cards[pic_number];
                    milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");

                    p[0].cards[pic_number] = null;
                    organiser(0);

                    nbr_cards++;
                    all_card[nbr_cards].Visible = true;
                    lb_all_card.Text = "il reste " + nbr_cards + " cartes";

                    if (v_milieu.Split('-')[0] == load_profile.extra[0].ToString())
                    {
                        sound_launcher(v_la_main, "extra1");
                        add_cards(p_pos2[nbr_player - 2, v_la_main], 2);
                    }
                    else if (v_milieu.Split('-')[0] == load_profile.extra[1].ToString())
                    {
                        sound_launcher(v_la_main, "extra2");
                        add_cards(p_pos2[nbr_player - 2, v_la_main], 1);
                    }
                    else if (v_milieu.Split('-')[0] == load_profile.extra[2].ToString())
                        sound_launcher(v_la_main, "extra3");
                    else if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                        sound_launcher(v_la_main, "extra4");

                    if (check_winner(v_la_main) != 0)
                    {
                        if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                        {
                            block_me = true;
                            for (int cnt = 0; cnt < 4; cnt++)
                                choix[cnt].Visible = true;

                        }
                        else
                        {
                            if (v_milieu.Split('-')[0] == load_profile.extra[0].ToString() || v_milieu.Split('-')[0] == load_profile.extra[1].ToString() || v_milieu.Split('-')[0] == load_profile.extra[2].ToString())
                                v_la_main = p_pos2[nbr_player - 2, p_pos2[nbr_player - 2, v_la_main]];
                            else
                                v_la_main = p_pos2[nbr_player - 2, v_la_main];

                            t_tp.Enabled = true;
                        }
                    }
                    else
                    {
                        // some code if you win
                    }
                    if(debug_mode==true)
                        check_bugs();
                }
            }
        }

        ///////////////////// check bugs
        void check_bugs()
        {
            int cnt_bugs = 0;

            for (int cnt = 0; cnt < 40; cnt++)
                if (matrice[cnt] != null)
                    cnt_bugs++;

            if (cnt_bugs != nbr_cards)
                MessageBox.Show("Erreur, le nombre de carte mémorisé n'est pas le même sur la matrice\nMatrice =" + cnt_bugs + "/Carte mémorisé =" + nbr_cards);
            
            bool found = false;
            string tmp_card;

            for (int c_type = 0; c_type < 4; c_type++)
            {
                for (int c_card = 1; c_card < 13; c_card++)
                {
                    found = false;
                    if (c_card == 8)
                        c_card += 2;

                    tmp_card = c_card + type_card[c_type];

                    // recherche dans la matrice
                    for (int c_matrice = 0; c_matrice < 40; c_matrice++)
                        if (matrice[c_matrice] == c_card + type_card[c_type])
                        {
                            found = true;
                            break;
                        }

                    // recherche dans la carte du milieu
                    if (found == false)
                    {
                        if (c_card + type_card[c_type] == v_milieu)
                            found = true;
                        else if (c_card + type_card[c_type] == v_milieu2)
                            found = true;
                    }

                    // recherche dans les cartes des joueurs
                    if (found == false)
                        for (int c_player = 0; c_player < nbr_player; c_player++)
                        {
                            for (int c_c_player = 0; c_c_player < max_cards; c_c_player++)
                            {
                                if (p[c_player].cards[c_c_player] != null)
                                {
                                    if (p[c_player].cards[c_c_player] == c_card + type_card[c_type])
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                else
                                    break;
                            }
                            if (found == true)
                                break;
                        }

                    if (found == false)
                        MessageBox.Show(this, "La carte " + c_card + type_card[c_type] + " est manquante","Erreur",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
        ///////////////////////////// Choix CLICK event  ////////////////////////////
        void choix_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            int pic_number = Int16.Parse(pic.Name);
            v_milieu2 = v_milieu;

            if (pic_number == 0)
                v_milieu = load_profile.extra[3] + "-z";
            else if (pic_number == 1)
                v_milieu = load_profile.extra[3] + "-s";
            else if (pic_number == 2)
                v_milieu = load_profile.extra[3] + "-f";
            else
                v_milieu = load_profile.extra[3] + "-t";
            
            milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");

            for (int cnt = 0; cnt < 4; cnt++)
                choix[cnt].Visible = false;
            
            sound_launcher(v_la_main, "extra4" + v_milieu.Split('-')[1]);
            int result = check_winner(v_la_main);

            if (result != 0)
            {
                block_me = true;
                v_la_main = p_pos2[nbr_player - 2, v_la_main];
                t_tp.Enabled = true;
            }
            if(debug_mode==true)
                check_bugs();
        }



///////////////////////////////////  mind  1   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        private void mind_1(int player)
        {
            bool found = false;

            for (int cnt = 0; p[player].cards[cnt] != null; cnt++)
            {
                string[] data_carte = p[player].cards[cnt].Split('-');
                string[] data_milieu = v_milieu.Split('-');
                if (data_milieu[0] == load_profile.extra[3].ToString())
                    data_milieu[1] = v_milieu.Split('-')[1];

                // recherche d'une similarité
                if (data_carte[0] == data_milieu[0] || data_carte[1] == data_milieu[1])
                {
                    // passage de la carte au tirage
                    for (int cnt2 = 0; cnt2 < 40; cnt2++)
                        if (matrice[cnt2] == null)
                        {
                            matrice[cnt2] = v_milieu;
                            nbr_cards++;
                            all_card[nbr_cards].Visible = true;

                            lb_all_card.Text = "il reste " + nbr_cards + " cartes";
                            break;
                        }

                    // update de la carte de milieu
                    v_milieu = p[player].cards[cnt];
                    if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                        v_milieu2 = v_milieu;
                    p[player].cards[cnt] = null;
                    milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");
                    found = true;
                    organiser(player);
                    break;
                }
            }

            if (found == true)
            {
                if (v_milieu.Split('-')[0] == load_profile.extra[0].ToString())
                {
                    sound_launcher(player, "extra1");
                    add_cards(p_pos2[nbr_player - 2, v_la_main], 2);
                }
                else if (v_milieu.Split('-')[0] == load_profile.extra[1].ToString())
                {
                    sound_launcher(player, "extra2");
                    add_cards(p_pos2[nbr_player - 2, v_la_main], 1);
                }
                else if (v_milieu.Split('-')[0] == load_profile.extra[2].ToString())
                {
                    sound_launcher(player, "extra3");
                }
                else if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                {
                    sound_launcher(player, "extra4");
                    sound_launcher(player, "extra4" + v_milieu2.Split('-')[1]);
                }

                int result = check_winner(v_la_main);

                if (result != 0)
                    if (nbr_player > 2)
                    {
                        if (v_milieu.Split('-')[0] != load_profile.extra[0].ToString() && v_milieu.Split('-')[0] != load_profile.extra[1].ToString() && v_milieu.Split('-')[0] != load_profile.extra[2].ToString())
                            v_la_main = p_pos2[nbr_player - 2, v_la_main];
                        else
                            v_la_main = p_pos2[nbr_player - 2, p_pos2[nbr_player - 2, v_la_main]];

                        if (v_la_main == 0)
                            t_tp.Interval = 10;

                        t_tp.Enabled = true;
                    }
                    else
                        if (v_milieu.Split('-')[0] == load_profile.extra[0].ToString() || v_milieu.Split('-')[0] == load_profile.extra[1].ToString() || v_milieu.Split('-')[0] == load_profile.extra[2].ToString())
                            t_tp.Enabled = true;
                        else
                        {
                            v_la_main = p_pos2[nbr_player - 2, v_la_main];
                            t_tp.Interval = 10;
                            t_tp.Enabled = true;
                        }
            }
            else
            {
                add_cards(player, 1);
                v_la_main = p_pos2[nbr_player - 2, v_la_main];

                if (v_la_main == 0)
                    t_tp.Interval = 10;
                
                t_tp.Enabled = true;
            }
            if(debug_mode==true)
                check_bugs();
             
        }

/////////////////////////////////////  mind 2   \\\\\\\\\\\\\\\\\\\\\\\\\\\\
        private void mind_2(int player)
        {   
            bool found = false;

            for (int c_extra = 0; c_extra < 3; c_extra++)
            {
                for (int c_card = 0; c_card < max_cards; c_card++)
                {
                    if (p[player].cards[c_card] != null && (p[player].cards[c_card].Split('-')[0] == v_milieu.Split('-')[0] || p[player].cards[c_card].Split('-')[1] == v_milieu.Split('-')[1]))
                    {
                        if (p[player].cards[c_card].Split('-')[0] == load_profile.extra[c_extra].ToString())
                        {
                            for (int c_matrix = 0; c_matrix < 40; c_matrix++)
                                if (matrice[c_matrix] == null)
                                {
                                    if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                                        matrice[c_matrix] = v_milieu2;
                                    else
                                        matrice[c_matrix] = v_milieu;
                                    break;
                                }

                            v_milieu = p[player].cards[c_card];
                            milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");

                            p[player].cards[c_card] = null;
                            organiser(player);

                            nbr_cards++;
                            all_card[nbr_cards].Visible = true;
                            lb_all_card.Text = "il reste " + nbr_cards + " cartes";

                            found = true;
                            break;
                        }
                    }
                }

                if (found == true)
                    break;
            }

            if (found == true)
            {
                if (v_milieu.Split('-')[0] == load_profile.extra[0].ToString())
                {
                    sound_launcher(player, "extra1");
                    add_cards(p_pos2[nbr_player - 2, v_la_main], 2);
                }
                else if (v_milieu.Split('-')[0] == load_profile.extra[1].ToString())
                {
                    sound_launcher(player, "extra2");
                    add_cards(p_pos2[nbr_player - 2, v_la_main], 1);
                }
                else if (v_milieu.Split('-')[0] == load_profile.extra[2].ToString())
                    sound_launcher(player, "extra3");

                if (check_winner(v_la_main) != 0)
                {
                    if (v_milieu.Split('-')[0] != load_profile.extra[0].ToString() && v_milieu.Split('-')[0] != load_profile.extra[1].ToString() && v_milieu.Split('-')[0] != load_profile.extra[2].ToString())
                        v_la_main = p_pos2[nbr_player - 2, v_la_main];
                    else
                        v_la_main = p_pos2[nbr_player - 2, p_pos2[nbr_player - 2, v_la_main]];

                    if (v_la_main == 0)
                        t_tp.Interval = 10;

                    t_tp.Enabled = true;
                }
            }
            else
            {
                for (int c_card = 0; c_card < max_cards; c_card++)
                {
                    if (p[player].cards[c_card] != null && (p[player].cards[c_card].Split('-')[0] == v_milieu.Split('-')[0] || p[player].cards[c_card].Split('-')[1] == v_milieu.Split('-')[1]))
                    {
                        for (int c_matrix = 0; c_matrix < 40; c_matrix++)
                            if (matrice[c_matrix] == null)
                            {
                                if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                                    matrice[c_matrix] = v_milieu2;
                                else
                                    matrice[c_matrix] = v_milieu;
                                break;
                            }

                        v_milieu = p[player].cards[c_card];
                        milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");

                        p[player].cards[c_card] = null;
                        organiser(player);

                        nbr_cards++;
                        all_card[nbr_cards].Visible = true;
                        lb_all_card.Text = "il reste " + nbr_cards + " cartes";

                        found = true;
                        break;
                    }
                }

                if (found == true)
                {
                    if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                    {
                        sound_launcher(player, "extra4");
                        v_milieu2 = v_milieu;
                        v_milieu = load_profile.extra[3] + "-" + p[player].cards[0].Split('-')[1];
                        sound_launcher(player, "extra4" + v_milieu.Split('-')[1]);
                        
                    }
                    milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");

                    if (check_winner(v_la_main) != 0)
                    {
                        v_la_main = p_pos2[nbr_player - 2, v_la_main];
                        if (v_la_main == 0)
                            t_tp.Interval = 10;
                        t_tp.Enabled = true;
                    }
                }
                else
                {
                    add_cards(player, 1);
                    v_la_main = p_pos2[nbr_player - 2, v_la_main];

                    if (v_la_main == 0)
                        t_tp.Interval = 10;

                    t_tp.Enabled = true;
                }
            }

            if(debug_mode==true)
                check_bugs();
        }

        /// /////////////////////////   mind 3  \\\\\\\\\\\\\\\\\\\\\\\\\\
        private void mind_3(int player)
        {
            bool found = false;

            for (int c_extra = 0; c_extra < 3; c_extra++)
            {
                for (int c_card = 0; c_card < max_cards; c_card++)
                {
                    if (p[player].cards[c_card] != null && (p[player].cards[c_card].Split('-')[0] == v_milieu.Split('-')[0] || p[player].cards[c_card].Split('-')[1] == v_milieu.Split('-')[1]))
                    {
                        if (p[player].cards[c_card].Split('-')[0] == load_profile.extra[c_extra].ToString())
                        {
                            for (int c_matrix = 0; c_matrix < 40; c_matrix++)
                                if (matrice[c_matrix] == null)
                                {
                                    if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                                        matrice[c_matrix] = v_milieu2;
                                    else
                                        matrice[c_matrix] = v_milieu;
                                    break;
                                }
                            
                            v_milieu = p[player].cards[c_card];
                            milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");

                            p[player].cards[c_card] = null;
                            organiser(player);

                            nbr_cards++;
                            all_card[nbr_cards].Visible = true;
                            lb_all_card.Text = "il reste " + nbr_cards + " cartes";

                            found = true;
                            break;
                        }
                    }
                }

                if (found == true)
                    break;
            }
            
            if (found == true)
            {
                if (v_milieu.Split('-')[0] == load_profile.extra[0].ToString())
                {
                    sound_launcher(player, "extra1");
                    add_cards(p_pos2[nbr_player - 2, v_la_main], 2);
                }
                else if (v_milieu.Split('-')[0] == load_profile.extra[1].ToString())
                {
                    sound_launcher(player, "extra2");
                    add_cards(p_pos2[nbr_player - 2, v_la_main], 1);
                }
                else if (v_milieu.Split('-')[0] == load_profile.extra[2].ToString())
                    sound_launcher(player, "extra3");

                if (check_winner(v_la_main) != 0)
                {
                    if (v_milieu.Split('-')[0] != load_profile.extra[0].ToString() && v_milieu.Split('-')[0] != load_profile.extra[1].ToString() && v_milieu.Split('-')[0] != load_profile.extra[2].ToString())
                        v_la_main = p_pos2[nbr_player - 2, v_la_main];
                    else
                        v_la_main = p_pos2[nbr_player - 2, p_pos2[nbr_player - 2, v_la_main]];

                    if (v_la_main == 0)
                        t_tp.Interval = 10;

                    t_tp.Enabled = true;
                }
            }
            else
            {
                for (int c_card = 0; c_card < max_cards; c_card++)
                {
                    if (p[player].cards[c_card] != null && (p[player].cards[c_card].Split('-')[0] == v_milieu.Split('-')[0] || p[player].cards[c_card].Split('-')[1] == v_milieu.Split('-')[1]))
                    {
                        for (int c_matrix = 0; c_matrix < 40; c_matrix++)
                            if (matrice[c_matrix] == null)
                            {
                                if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                                    matrice[c_matrix] = v_milieu2;
                                else
                                    matrice[c_matrix] = v_milieu;
                                break;
                            }

                        v_milieu = p[player].cards[c_card];
                        milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");

                        p[player].cards[c_card] = null;
                        organiser(player);

                        nbr_cards++;
                        all_card[nbr_cards].Visible = true;
                        lb_all_card.Text = "il reste " + nbr_cards + " cartes";

                        found = true;
                        break;
                    }
                }

                if (found == true)
                {
                    if (v_milieu.Split('-')[0] == load_profile.extra[3].ToString())
                    {
                        int pos_extra = -1;
                        sound_launcher(player, "extra4");
                        v_milieu2 = v_milieu;
                        ///////////////////////////   recherche d'une extra    ////////////////////

                        for (int c_extra = 0; c_extra < 3; c_extra++)
                        {
                            for (int c_card = 0; c_card < max_cards; c_card++)
                            {
                                if (p[player].cards[c_card] != null && p[player].cards[c_card].Split('-')[0] == load_profile.extra[c_extra].ToString())
                                {
                                    pos_extra = c_card;
                                    break;
                                }
                            }
                            if (pos_extra != -1)
                                break;
                        }

                        if(pos_extra!=-1)
                            v_milieu = load_profile.extra[3] + "-" + p[player].cards[pos_extra].Split('-')[1];
                        else
                            v_milieu = load_profile.extra[3] + "-" + p[player].cards[0].Split('-')[1];
                        ///////////////////////////////////////////////
                        sound_launcher(player, "extra4" + v_milieu.Split('-')[1]);

                    }
                    milieu.Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");

                    if (check_winner(v_la_main) != 0)
                    {
                        v_la_main = p_pos2[nbr_player - 2, v_la_main];
                        if (v_la_main == 0)
                            t_tp.Interval = 10;
                        t_tp.Enabled = true;
                    }
                }
                else
                {
                    add_cards(player, 1);
                    v_la_main = p_pos2[nbr_player - 2, v_la_main];

                    if (v_la_main == 0)
                        t_tp.Interval = 10;

                    t_tp.Enabled = true;
                }
            }
            if(debug_mode==true)
                check_bugs();
        }

        //////////////////////  check the winner  //////////////////
        private int check_winner(int player)
        {
            int cnt_cards = 0;
            for (int cnt = 0; cnt < p[player].cards.Length; cnt++)
                if (p[player].cards[cnt] == null)
                    break;
                else
                    cnt_cards++;

            if (cnt_cards == 0)
            {
            /////// nétoyage de l'écrant \\\\\
                for (int win_cnt_p = 0; win_cnt_p < nbr_player; win_cnt_p++)
                    for (int win_cnt_card = 0; win_cnt_card< p[win_cnt_p].cards.Length; win_cnt_card++)
                        p_card[win_cnt_p, win_cnt_card].Visible = false;

                winner_pic = new PictureBox();

                if (player == 0)
                {
                    winner_pic.Image = (Bitmap)Image.FromFile(@"img\trophe.gif");
                    winner_lb.Text = "Vous avez gagné la partie";
                    winner_lb.Location = new Point((this.Width / 2) - 280, this.Height - 200);
                    if (sound == true)
                    {
                        backsound.SoundLocation = @"son\applaud.wav";
                        backsound.Play();
                    }
                    Random rand_width = new Random(unchecked((int)DateTime.Now.Ticks));
                    Random rand_height = new Random();
                    for (int cnt = 0; cnt < 100; cnt++)
                    {
                        winThumbs[cnt] = new PictureBox();
                        winThumbs[cnt].Image = (Bitmap)Image.FromFile(@"img\winThumbs.gif");
                        winThumbs[cnt].SizeMode = PictureBoxSizeMode.AutoSize;
                        winThumbs[cnt].BringToFront();
                        backimg.Controls.Add(winThumbs[cnt]);
                        int rndW = rand_width.Next(20, this.Width);
                        int rndH = rand_height.Next(20, this.Height - 40);
                        winThumbs[cnt].Location = new Point(rndW, rndH);
                        winThumbs[cnt].Show();
                    }
                }
                else
                {
                    winner_pic.Image = (Bitmap)Image.FromFile(@"img\GameOver.gif");
                    winner_lb.Text = p[player].nom.ToUpper() + " a gagné la partie";
                    winner_lb.Location = new Point((this.Width / 2) - 250, this.Height - 200);
                    if (sound == true)
                    {
                        backsound.SoundLocation = @"son\perdre.wav";
                        backsound.Play();
                    }
                }

                //////// param winner_pic commun \\\\
                winner_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                winner_pic.BackColor = Color.Transparent;
                backimg.Controls.Add(winner_pic);
                winner_pic.Location = new Point((this.Width / 2) - (winner_pic.Width / 2), (this.Height / 2) - (winner_pic.Height / 2) - 100);
                winner_pic.BringToFront();
                milieu.Visible = false;
                winner_pic.Show();
                winner_var = true;

                /////// param du label winner commun \\\\\
                winner_lb.Font = new Font("Verdana", 30F, System.Drawing.FontStyle.Bold);
                winner_lb.ForeColor = Color.Red;
                winner_lb.BackColor = Color.Transparent;
                winner_lb.AutoSize = true;
                winner_lb.BringToFront();
                backimg.Controls.Add(winner_lb);
                winner_lb.Show();
                winner_lb_timer.Enabled = true;

                préférenceTS.Enabled = true;
                block_me = true;
            }
            return cnt_cards;
        }
        
        ////////////////////  sound launcher  ////////////////////////
        private void sound_launcher(int pl, string extraS)
        {
            if (block == false)
                if (sound == true)
                    if (p[pl].sound)
                    {
                        Random rnd = new Random();
                        DirectoryInfo di_files = new DirectoryInfo(@"profiles\zriwita\" + p[pl].avatar + @"\" + extraS + @"\");
                        FileInfo[] fi_files = di_files.GetFiles();
                        bool valid_dir = false;

                        for (int cnt = 0; cnt < fi_files.Length; cnt++)
                            if (fi_files[cnt].Extension == ".wav" || fi_files[cnt].Extension == ".mp3" || fi_files[cnt].Extension == ".au")
                            {
                                valid_dir = true;
                                break;
                            }

                        if (valid_dir == true)
                            PlaySound(@"profiles\zriwita\" + p[pl].avatar + @"\" + extraS + @"\" + fi_files[rnd.Next(fi_files.Length)].Name, 0, 0x00020000);
                        else
                        {
                            MessageBox.Show("Aucun média n'a été trouvé sur la bibliotheque\n\tRéinstaller l'application svp");
                            this.Close();
                        }
                    }
        }

        private void préférenceTS_Click(object sender, EventArgs e)
        {
            parametres param_form = new parametres(this,load_profile);
            param_form.Show();
        }

        private void RGtoolStripButton_Click(object sender, EventArgs e)
        {
            for (int cnt = 0; cnt < nbr_player; cnt++)
            {
                p[cnt].destructor2();
                player_pan[cnt].Dispose();
            }

            for (int cnt = 0; cnt < all_card.Length; cnt++)
                if (launched == true)
                    all_card[cnt].Dispose();

            GC.Collect();

            var_ini();
            set_po(ptab.TabCount);
            nbr_cards = 40;
            winner_pic.Visible = false;
            start();
        }

        void zConsoltb_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue == 13 && zConsoltb.Text!="")
                console();
            else if (e.KeyValue == 27)
                ZConsolpan.Visible = false;
        }

        private void console()
        {
            string[] c_message ={"CHEAT > Game not launched yet idiot\n",
                                  "CHEAT > Done cheater lol\n",
                                  "CMD > Check parameters\n",
                                  "CHEAT > Missed parameters\n"};
            bool tmp_close = false;

            if (zConsoltb.Text.ToLower() == "version")
                zConsol.Text +="CMD > Built 2.5.100.0\n";
            else if (zConsoltb.Text.ToLower() == "cls")
                zConsol.Text = "C4RT4 V" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\t(2010) Free Edition, HAVE FUN ;-)\n";
            else if (zConsoltb.Text.ToLower() == "hide")
            {
                if (launched == true)
                {
                    cc_v_cards = false;
                    for (int cnt = 1; cnt < nbr_player; cnt++)
                        for (int cnt2 = 0; p[cnt].cards[cnt2] != null; cnt2++)
                        {
                            p_card[cnt, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));

                            if (cnt == 2)
                            {
                                p_card[cnt, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                                p_card[cnt, cnt2].Size = new Size(170, 100);
                            }
                            else if (cnt == 3)
                            {
                                p_card[cnt, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                                p_card[cnt, cnt2].Size = new Size(170, 100);
                            }
                        }
                    zConsol.Text += c_message[1];
                }
                else
                    zConsol.Text += c_message[0];
                
            }
            else if (zConsoltb.Text.ToLower() == "show")
            {
                if (launched == true)
                {
                    cc_v_cards = true;
                    for (int cnt = 1; cnt < nbr_player; cnt++)
                        for (int cnt2 = 0; p[cnt].cards[cnt2] != null; cnt2++)
                        {
                            p_card[cnt, cnt2].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[cnt].cards[cnt2] + ".jpg");

                            if (cnt == 2)
                            {
                                p_card[cnt, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                                p_card[cnt, cnt2].Size = new Size(170, 100);
                            }
                            else if (cnt == 3)
                            {
                                p_card[cnt, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                                p_card[cnt, cnt2].Size = new Size(170, 100);
                            }
                        }
                    cheater_pic.Location = new Point(-cheater_pic.Width, this.Height - cheater_pic.Height - 30);
                    cheater_pic.Visible = true;
                    zConsol.Text += c_message[1];
                    backsound.SoundLocation = @"son\cheater.wav";
                    cheater_t1.Enabled = true;
                    backsound.Play();
                }
                else
                    zConsol.Text += c_message[0];
            }
            else if (zConsoltb.Text.Length > 2 && zConsoltb.Text.ToLower().Substring(0, 3) == "add")
            {
                if (launched == true)
                {
                    string zConsoldata = zConsoltb.Text.Replace(" ","");
                    if (zConsoldata.IndexOf("-") != -1 && zConsoldata.Split('-').Length==3)
                    {
                        string[] zcd_split = zConsoldata.Split('-');
                        try
                        {
                            if (Int16.Parse(zcd_split[1]) < nbr_player && Int16.Parse(zcd_split[2]) < 4)
                            {
                                add_cards(Int16.Parse(zcd_split[1]), Int16.Parse(zcd_split[2]));
                                cheater_pic.Location = new Point(-cheater_pic.Width, this.Height - cheater_pic.Height - 30);
                                cheater_pic.Visible = true;
                                zConsol.Text += c_message[1];
                                backsound.SoundLocation = @"son\cheater.wav";
                                cheater_t1.Enabled = true;
                                backsound.Play();
                            }
                            else
                                zConsol.Text += c_message[2];
                        }
                        catch
                        {
                            zConsol.Text += c_message[3];
                        }
                    }
                    else
                        zConsol.Text += c_message[3];
                }
                else
                    zConsol.Text += c_message[0];
            }
            else if (zConsoltb.Text.ToLower() == "exit")
                ZConsolpan.Visible = false;
            else if (zConsoltb.Text.Length > 4 && zConsoltb.Text.ToLower().Substring(0, 5) == "close")
            {
                tmp_close = true;
                string zConsoldata = zConsoltb.Text.Replace(" ", "");
                if (zConsoldata.IndexOf("-") != -1 && zConsoldata.Split('-')[1] == "a")
                {
                    this.Close();
                    main._main.Close();
                }
                else
                    this.Close();
            }
            else if (zConsoltb.Text.Length > 5 && zConsoltb.Text.ToLower().Substring(0, 6) == "matrix")
            {
                if (launched == true)
                {
                    string matrix_tbl = "";
                    for (int cnt = 0; cnt < 40; cnt++)
                        matrix_tbl += matrice[cnt] + " , ";
                    string zConsoldata = zConsoltb.Text.Replace(" ", "");
                    if (zConsoldata.IndexOf("-") != -1 && zConsoldata.Split('-')[1] == "s")
                        MessageBox.Show(matrix_tbl);
                    else
                        zConsol.Text += "CMD > " + matrix_tbl + "\n";
                }
                else
                    zConsol.Text += c_message[0];
            }
            else if (zConsoltb.Text.ToLower() == "v_middle")
            {
                if (launched == true)
                    zConsol.Text += "CMD > " + v_milieu + "\n";
                else
                    zConsol.Text += c_message[0];
            }
            else if (zConsoltb.Text.Length > 5 && zConsoltb.Text.ToLower().Substring(0, 5) == "stats")
            {
                if (launched == true)
                {
                    string zConsoldata = zConsoltb.Text.Replace(" ", "");
                    if (zConsoldata.IndexOf("-") != -1 && zConsoldata.Split('-').Length == 2)
                    {
                        string[] zcd_split = zConsoldata.Split('-');
                        try
                        {
                            if (Int16.Parse(zcd_split[1]) < nbr_player)
                            {
                                string stats = "";
                                for (int cnt = 0; cnt<max_cards && p[Int16.Parse(zcd_split[1])].cards[cnt] != null; cnt++)
                                    stats += p[Int16.Parse(zcd_split[1])].cards[cnt] + ",";
                                zConsol.Text += "CHEAT > Player " + zcd_split[1] + "'s stats\n\t" + stats + "\n";
                                cheater_pic.Location = new Point(-cheater_pic.Width, this.Height - cheater_pic.Height - 30);
                                cheater_pic.Visible = true;
                                zConsol.Text += c_message[1];
                                backsound.SoundLocation = @"son\cheater.wav";
                                cheater_t1.Enabled = true;
                                backsound.Play();
                            }
                            else
                                zConsol.Text += c_message[2];
                        }
                        catch
                        {
                            zConsol.Text += c_message[3];
                        }
                    }
                    else
                        zConsol.Text += c_message[3];
                }
                else
                    zConsol.Text += c_message[0];
            }
            else if (zConsoltb.Text.ToLower() == "restart")
            {
                RGtoolStripButton_Click(null, null);
                ZConsolpan.BringToFront();
                zConsol.Text += "CMD > Game restarted\n";
            }
            else if (zConsoltb.Text.ToLower() == "extra")
            {
                zConsol.Text += "CMD >Extra : " + load_profile.extra[0] + "," + load_profile.extra[1] + "," + load_profile.extra[2] + "," + load_profile.extra[3] + "\n";
            }
            else if (zConsoltb.Text.ToLower() == "debug on")
            {
                zConsol.Text += "CMD >DEBUG : Mode Débug ON\n";
                debug_mode = true;
            }
            else if (zConsoltb.Text.ToLower() == "debug off")
            {
                zConsol.Text += "CMD >DEBUG : Mode Débug OFF\n";
                debug_mode =false;
            }
            else if (zConsoltb.Text.Length == 4 && zConsoltb.Text.Substring(0, 4).ToLower() == "help")
            {
                zConsol.Text += "CMD > HELP Command line.\n";
                zConsol.Text += "[version]\t Version de C4RT4.\n";
                zConsol.Text += "[cls]\t Efface le texte.\n";
                zConsol.Text += "[hide]\t Cartes caché (dos affiché) (CODE TRICHE !!).\n";
                zConsol.Text += "[show]\t Cartes visibles (face affiché) (CODE TRICHE !!).\n";
                zConsol.Text += "[add] -[X] -[Y] Ajoute des cartes au joueur X,\n";
                zConsol.Text += "\t(X étant le numero de classement du joueur en commancant par 0,1,2,3)\n";
                zConsol.Text += "\t(Y, est le nombre de cartes a ajouté au joueur séléctioné),\n";
                zConsol.Text += "\tAttention : maximum 3 cartes a ajouté par commande (CODE TRICHE !!).\n";
                zConsol.Text += "[exit]\t Cache la console.\n";
                zConsol.Text += "[close]\t Ferme la fenetre enfant\n";
                zConsol.Text += "\t [close] -[a] ferme l'application entiere.\n";
                zConsol.Text += "[matrix]\t Affiche la table matrix, (cartes non utilisé encore).\n";
                zConsol.Text += "[v_middle]\t Affiche la valeur de la carte milieux (pour DEBUGAGE).\n";
                zConsol.Text += "[stats] -[X]\t Affiche la table du joueur X\n";
                zConsol.Text += "\t Attention : X peux être 0,1,2,3 (CODE TRICHE).\n";
                zConsol.Text += "[restart]\t Re-jouer a nouveau.\n";
                zConsol.Text += "[extra]\t Affiche les valeurs de Extra 1,2,3,4\n";
                zConsol.Text += "[debug] [on/off]\t Active/Désactive le mode débugage\n";
            }
            else
                zConsol.Text += ">" + zConsoltb.Text + "\n";

            if (tmp_close == false)
            {
                zConsol.SelectionStart = zConsol.Text.Length;
                zConsol.ScrollToCaret();
                zConsol.Refresh();
                zConsoltb.Text = "";
            }
        }


        private void cheater_t1_Tick(object sender, EventArgs e)
        {
            if (cheater_t2.Enabled == false)
            {
                if (cheater_pic.Location.X < 0)
                    cheater_pic.Location = new Point(cheater_pic.Location.X + 25, cheater_pic.Location.Y);
                else
                {
                    cheater_t1.Enabled = false;
                    cheater_t2.Interval = 1500;
                    cheater_t2.Enabled = true;
                }
            }
            else
            {
                cheater_pic.Location = new Point(-110, cheater_pic.Location.Y);
                cheater_t2.Enabled = false;
            }
        }

        private void cheater_t2_Tick(object sender, EventArgs e)
        {
            cheater_t2.Interval = 50;
            if (cheater_pic.Location.X > -100)
                cheater_pic.Location = new Point(cheater_pic.Location.X - 25, cheater_pic.Location.Y);
            else
                cheater_t2.Enabled = false;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ZConsolpan.Visible == true)
                ZConsolpan.Visible = false;
            else
            {
                ZConsolpan.Location = new Point((this.Width - ZConsolpan.Width) >> 1, (this.Height - ZConsolpan.Height) >> 1);
                ZConsolpan.BringToFront();
                ZConsolpan.Visible = true;
                zConsoltb.Focus();
            }
        }

        private void ConsolTS_Click(object sender, EventArgs e)
        {
            if (ZConsolpan.Visible == true)
                ZConsolpan.Visible = false;
            else
            {
                ZConsolpan.Location = new Point((this.Width - ZConsolpan.Width) >> 1, (this.Height - ZConsolpan.Height) >> 1);
                ZConsolpan.BringToFront();
                ZConsolpan.Visible = true;
                zConsoltb.Focus();
            }
        }

        private void aProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutbox = new AboutBox1();
            aboutbox.Show();
        }

        private void winner_lb_timer_Tick(object sender, EventArgs e)
        {
            if (winner_lb.ForeColor == Color.Red)
                winner_lb.ForeColor = Color.SkyBlue;
            else
                winner_lb.ForeColor = Color.Red;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (sound == true)
            {
                sound = false;
                toolStripButton1.Image = Properties.Resources.volumeKo;
                
                if (load_pro == true)
                {
                    load_profile.son = 0;
                    profile_save();
                }
            }
            else
            {
                sound = true;
                toolStripButton1.Image = Properties.Resources.volumeOk;

                if (load_pro == true)
                {
                    load_profile.son = 1;
                    profile_save();
                }
            }
        }

        private void param_allow_Click(object sender, EventArgs e)
        {
            if (préférenceTS.Enabled == true)
                préférenceTS.Enabled = false;
            else
                préférenceTS.Enabled = true;
        }

        private void toolStripDecBtn_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Increment(-10);
            toolStripLabelProg.Text = toolStripProgressBar1.Value + " %";
            this.Opacity = double.Parse(toolStripProgressBar1.Value.ToString()) / 100;
            toolStripIncBtn.Enabled = true;
            if (toolStripProgressBar1.Value == 30)
                toolStripDecBtn.Enabled = false;
        }

        private void toolStripIncBtn_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Increment(10);
            toolStripLabelProg.Text = toolStripProgressBar1.Value + " %";
            this.Opacity = double.Parse(toolStripProgressBar1.Value.ToString()) / 100;
            toolStripDecBtn.Enabled = true;
            if (toolStripProgressBar1.Value == 100)
                toolStripIncBtn.Enabled = false;
        }

        private void zriwita_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            block = true;
            this.Dispose();
            GC.Collect();
        }

        private void rejouerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RGtoolStripButton_Click(null, null);
        }
    }
}