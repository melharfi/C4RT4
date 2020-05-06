using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Resources;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Xml;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;

namespace C4RT4
{
    public partial class rondaR : Form
    {
        /////////////////////////////////////  Declaration des variable  ////////////////////////////
        private Label[] nom = new Label[4];
        private Label[] ville = new Label[4];
        private Label[] team = new Label[4];
        private Label lb_all_card = new Label();            // label de tas
        private Label winner_lb = new Label();
        private Label winner_pts = new Label();              // affichage des points de la fin de la partie
        private Label[] p_pts = new Label[4];
        private Label[] p_crd_won = new Label[4];
        private Label[] p_pts_won = new Label[4];
        private Label[] p_kh_won = new Label[4];
        private Label[] p_dfou3 = new Label[4];
        private Label[] p_bnt_won = new Label[4];
        private PictureBox[] p_double = new PictureBox[4];
        private PictureBox backimg = new PictureBox();
        private PictureBox[] milieu = new PictureBox[10];
        private PictureBox[] avatarPB = new PictureBox[4];     // avatar des players dans le panneau
        private PictureBox[] all_card = new PictureBox[40];    // le tas
        private PictureBox[,] p_card = new PictureBox[4, 4];  // les cartes des players
        private PictureBox[] avatar = new PictureBox[4];     // avatar des players dans le jeux
        private PictureBox[] winThumbs = new PictureBox[100];
        private PictureBox winner_pic = new PictureBox();    // image de victoire
        private Panel ngpan = null;
        private Panel[] player_pan = new Panel[4];          // panneau des players
        private Panel[] p_stats = new Panel[4];
        private TabControl ptab = new TabControl();
        private TabPage[] tab = new TabPage[4];
        private Label[] pnameT = new Label[4];          // nom des players dans la panneau
        private Label[] avatarLB = new Label[4];      //nom de l'avatar dans le panneau
        private ComboBox[] backsndLB = new ComboBox[4];     //profil sonor dans le panneau
        private _playerR[] p = new _playerR[4];               // classe player
        private session_wan session_w = new session_wan();
        private profileR load_profileR = new profileR();
        private int[,] pos = new int[4, 2];
        private int block_pcard = 0;                 // affichage animé lors du survole de la sourie
        private int v_la_main = 0;                       // la main
        private int nbr_cards;                       // = 40;
        private int[] p_pos = new int[] { 0, 1, 2, 3 };  //ordre chronologique
        private int[,] p_pos2 = new int[,] { { 1, 0, -1, -1, -1, -1 }, { 2, 0, 1, -1, -1, -1 }, { 2, 3, 1, 0, -1, -1 } };    //ordre des players
        private int[] convert_pos;
        private int max_cards = 4;
        private int chrono_cnt = 20;
        private int chrono_checker_cnt = 40;
        private string v_dos_card = "dos_1";
        private string v_front_card = "tamisé";
        private string[] matrice = new string[40];
        private string[] type_card = new string[4] { "-z", "-s", "-f", "-t" };
        private string[] v_milieu=new string[10];
        private string hosted_server;
        private bool launched = false;
        private bool block_me = false;
        private bool load_pro = false;          // true si la le chargement du profile a été fait.
        private bool block = false;              // block when form is closed to stop process
        private bool refrech_view_once = false;
        private bool refrech_view_once_2 = false;
        ///// sound
        [DllImport("WinMM.dll")]
        private static extern bool PlaySound(string fname, int Mod, int flag);
        private System.Media.SoundPlayer backsound = new System.Media.SoundPlayer();
        private WebClient client1 = new WebClient(), client2 = new WebClient(), client3 = new WebClient();
        private WebClient client_syn1 = new WebClient(), client_syn2 = new WebClient();
        private WebClient client_chat_checker1 = new WebClient(), client_chat_checker2 = new WebClient();
        /////////////////////////////////////////////////////////////////////////////////

        public rondaR(session_wan session_wan1, _playerR p1, _playerR p2, _playerR p3, _playerR p4)
        {
            InitializeComponent();

            session_w = session_wan1;
            this.Text = session_w.nom + " [RONDA]";
            /// masque
            /// 

            if (session_w.nbr_player == 2)
            {
                p_stats[0] = new Panel();
                p_stats[1] = new Panel();

                p_crd_won[0] = new Label();
                p_crd_won[1] = new Label();

                p_crd_won[0] = p1_crd_won;
                p_crd_won[1] = p2_crd_won;

                p_pts_won[0] = new Label();
                p_pts_won[1] = new Label();

                p_pts_won[0] = p1_pts;
                p_pts_won[1] = p2_pts;

                p_kh_won[0] = new Label();
                p_kh_won[1] = new Label();

                p_kh_won[0] = p1_kh_won;
                p_kh_won[1] = p2_kh_won;

                p_dfou3[0] = new Label();
                p_dfou3[1] = new Label();

                p_dfou3[0] = p1_dfou3;
                p_dfou3[1] = p2_dfou3;

                p_double[0] = new PictureBox();
                p_double[1] = new PictureBox();

                p_double[0] = p1_double;
                p_double[1] = p2_double;

                p_bnt_won[0] = new Label();
                p_bnt_won[1] = new Label();

                p_bnt_won[0] = p1_bnt_won;
                p_bnt_won[1] = p2_bnt_won;

                if (session_w.p_pos == 0)
                {
                    p[0] = p1;
                    p[1] = p2;

                    convert_pos = new int[] { 0, 1 };

                    p_stats[0] = P1stat;
                    p_stats[1] = P2stat;
                }
                else if (session_w.p_pos == 1)
                {
                    p[0] = p2;
                    p[1] = p1;

                    convert_pos = new int[] { 1, 0 };

                    p_stats[0] = P2stat;
                    p_stats[1] = P1stat;
                }
            }
            else if (session_w.nbr_player == 4)
            {
                p_stats[0] = new Panel();
                p_stats[1] = new Panel();
                p_stats[2] = new Panel();
                p_stats[3] = new Panel();

                p_crd_won[0] = new Label();
                p_crd_won[1] = new Label();
                p_crd_won[2] = new Label();
                p_crd_won[3] = new Label();

                p_crd_won[0] = p1_crd_won;
                p_crd_won[1] = p2_crd_won;
                p_crd_won[2] = p3_crd_won;
                p_crd_won[3] = p4_crd_won;

                p_pts_won[0] = new Label();
                p_pts_won[1] = new Label();
                p_pts_won[2] = new Label();
                p_pts_won[3] = new Label();

                p_pts_won[0] = p1_pts;
                p_pts_won[1] = p2_pts;
                p_pts_won[2] = p1_pts;
                p_pts_won[3] = p2_pts;

                p_kh_won[0] = new Label();
                p_kh_won[1] = new Label();
                p_kh_won[2] = new Label();
                p_kh_won[3] = new Label();

                p_kh_won[0] = p1_kh_won;
                p_kh_won[1] = p2_kh_won;
                p_kh_won[2] = p3_kh_won;
                p_kh_won[3] = p4_kh_won;

                p_dfou3[0] = new Label();
                p_dfou3[1] = new Label();
                p_dfou3[2] = new Label();
                p_dfou3[3] = new Label();

                p_dfou3[0] = p1_dfou3;
                p_dfou3[1] = p2_dfou3;
                p_dfou3[2] = p3_dfou3;
                p_dfou3[3] = p4_dfou3;

                p_double[0] = new PictureBox();
                p_double[1] = new PictureBox();
                p_double[2] = new PictureBox();
                p_double[3] = new PictureBox();

                p_double[0] = p1_double;
                p_double[1] = p2_double;
                p_double[2] = p3_double;
                p_double[3] = p4_double;

                p_bnt_won[0] = new Label();
                p_bnt_won[1] = new Label();
                p_bnt_won[2] = new Label();
                p_bnt_won[3] = new Label();

                p_bnt_won[0] = p1_bnt_won;
                p_bnt_won[1] = p2_bnt_won;
                p_bnt_won[2] = p3_bnt_won;
                p_bnt_won[3] = p4_bnt_won;

                if (session_w.p_pos == 0)
                {
                    p[0] = p1;
                    p[1] = p3;
                    p[2] = p2;
                    p[3] = p4;

                    convert_pos = new int[] { 0, 2, 1, 3 };

                    p_stats[0] = P1stat;
                    p_stats[1] = P3stat;
                    p_stats[2] = P2stat;
                    p_stats[3] = P4stat;
                }
                else if (session_w.p_pos == 1)
                {
                    p[0] = p2;
                    p[1] = p4;
                    p[2] = p3;
                    p[3] = p1;

                    convert_pos = new int[] { 3, 0, 2, 1 };

                    p_stats[0] = P2stat;
                    p_stats[1] = P4stat;
                    p_stats[2] = P3stat;
                    p_stats[3] = P1stat;
                }
                else if (session_w.p_pos == 2)
                {
                    p[0] = p3;
                    p[1] = p1;
                    p[2] = p4;
                    p[3] = p2;

                    convert_pos = new int[] { 1, 3, 0, 2 };

                    p_stats[0] = P3stat;
                    p_stats[1] = P1stat;
                    p_stats[2] = P4stat;
                    p_stats[3] = P2stat;
                }
                else if (session_w.p_pos == 3)
                {
                    p[0] = p4;
                    p[1] = p2;
                    p[2] = p1;
                    p[3] = p3;

                    convert_pos = new int[] { 2, 1, 3, 0 };

                    p_stats[0] = P4stat;
                    p_stats[1] = P2stat;
                    p_stats[2] = P1stat;
                    p_stats[3] = P3stat;
                }
            }

            if (main.option_loaded == 2)
            {
                chat_checker.Interval = main.speed_loaded;
                cmd_checker.Interval = main.speed_loaded;
            }
            else if (main.speed_loaded == 3)
                syn_network.Enabled = true;
        }

        //FORM DESTRUCTOR
        ~rondaR()
        {
            destructor();
        }

        //2nd DESTRUCTOR
        private void destructor()
        {
            this.Dispose();
            GC.Collect();
        }

        private void t_tour_Tick(object sender, EventArgs e)
        {
            for (int c_player = 0; c_player < session_w.nbr_player; c_player++)
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

        private void winner_lb_timer_Tick(object sender, EventArgs e)
        {
            if (winner_lb.ForeColor == Color.Red)
                winner_lb.ForeColor = Color.SkyBlue;
            else
                winner_lb.ForeColor = Color.Red;
        }

        private void syn_network_Tick(object sender, EventArgs e)
        {
            chat_checker.Interval = main.speed_loaded;
            cmd_checker.Interval = main.speed_loaded;
        }

        private void chat_checker_Tick(object sender, EventArgs e)
        {
            ////// timer qui cherche les nouveau membres ajoutés a la partie
            if (IsConnected())
            {
                header();
                if (client_chat_checker1.IsBusy == false)
                    client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                else if (client_chat_checker2.IsBusy == false)
                    client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                else
                {
                    evenement_area.Text = evenement_area.Text + "\n" + @"ERREUR\>Historique chat non recue";
                    //noWebClientfree();
                }
            }
            else
            {
                //destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmd_checker_Tick(object sender, EventArgs e)
        {
            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?get_cmd=1&cur_cmd=" + session_w.index_cmd + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                else if (client2.IsBusy == false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?get_cmd=1&cur_cmd=" + session_w.index_cmd + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?get_cmd=1&cur_cmd=" + session_w.index_cmd + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                else
                {
                    evenement_area.Text = evenement_area.Text + "\n" + @"ERREUR\>Impossible de récuperer les évenements system";
                    //noWebClientfree();
                }
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chrono_Tick(object sender, EventArgs e)
        {
            chrono_cnt--;
            if (chrono_cnt < 10 && chrono_cnt > 0)
            {
                chrono_lb.Text = chrono_cnt.ToString();
                chrono_lb.Visible = true;
            }
            else if (chrono_cnt == 0)
            {
                chrono_lb.Visible = false;
                chrono.Enabled = false;
                chrono_cnt = 20;
                selfplay();
            }
        }

        private void chrono_checker_Tick(object sender, EventArgs e)
        {
            chrono_checker_cnt--;
            if (chrono_checker_cnt == 0)
            {
                if (IsConnected())
                {
                    header();
                    if (client1.IsBusy == false)
                    {
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?kick=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        if (session_w.chrono == 1)
                        {
                            chrono_checker.Enabled = false;
                            chrono_checker_cnt = 30;
                        }

                        avatar[v_la_main].Image = (Bitmap)Image.FromFile(@"img\kicked.jpg");
                    }
                    else if (client2.IsBusy == false)
                    {
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?kick=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        if (session_w.chrono == 1)
                        {
                            chrono_checker.Enabled = false;
                            chrono_checker_cnt = 30;
                        }

                        avatar[v_la_main].Image = (Bitmap)Image.FromFile(@"img\kicked.jpg");
                    }
                    else if (client3.IsBusy == false)
                    {
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?kick=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        if (session_w.chrono == 1)
                        {
                            chrono_checker.Enabled = false;
                            chrono_checker_cnt = 30;
                        }

                        avatar[v_la_main].Image = (Bitmap)Image.FromFile(@"img\kicked.jpg");
                    }
                    else
                        evenement_area.Text = evenement_area.Text + "\n" + @"ERREUR\>Controle system échoué,E1";
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void rondaR_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(rondaR_FormClosing);
            client1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client1_DownloadStringCompleted);
            client2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client2_DownloadStringCompleted);
            client3.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client3_DownloadStringCompleted);
            client_syn1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_syn1_DownloadStringCompleted);
            client_syn2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_syn2_DownloadStringCompleted);
            client_chat_checker1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_chat_checker1_DownloadStringCompleted);
            client_chat_checker2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_chat_checker2_DownloadStringCompleted);
            backimg.Size = this.Size;
            backimg.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(backimg);
            toolStripProgressBar2.Value = 100;
            toolStripLabel1.Text = "100 %";
            toolStripButton4.Enabled = false;
            profile_load();
            global_load();
            loading_panel.Location = new Point((this.Width - loading_panel.Width) / 2, (this.Height - loading_panel.Height) / 2);
            loading_panel.Visible = true;
            hosted_server = main._main.hosted_server;
            new_game_menu();
            panel1.Width = this.Width;
            chat_area.Width = this.Width - 350;
            chat_msg.Width = this.Width - 350;
            evenement_area.Width = 350;
            evenement_area.Location = new Point(this.Width - 350);
            panel1.Visible = true;
            session_w.index_cmd = 0;
            chat_checker.Enabled = true;
            cmd_checker.Enabled = true;
            ngpan.Visible = false;
            chrono_lb.Location = new Point(this.Width - chrono_lb.Width, 0);
            chrono_lb.BringToFront();
            /////////// traitement des données importés
        }

        void rondaR_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            main._main.Show();

            if (main._main.bs_cb.Checked == true)
                main._main.playBackSound();
        }

        void client3_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

        }

        void client2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

        }

        void client1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
            {
                if (data[1] == "etat player")
                {
                    if (data[2] == "ready")
                        launch_partie.Visible = false;
                }
                else if (data[1] == "cmd")
                {
                    if (session_w.index_cmd < Int16.Parse(data[3]) || Int16.Parse(data[3]) == 1)
                    {
                        if (data[2] == "first initialisation" && refrech_view_once == false)
                        {
                            refrech_view_once = true;
                            session_w.index_cmd = Int16.Parse(data[3]);
                            v_la_main = convert_pos[Int16.Parse(data[4])];
                            nbr_cards = Int16.Parse(data[5]);
                            lb_all_card.Text = "Il reste " + nbr_cards + " cartes";
                            
                            for (int cnt = 0; cnt < session_w.nbr_player; cnt++)
                            {
                                string[] data_player = data[7].Split('|');
                                if (cnt == session_w.p_pos)
                                    for (int cnt2 = 0; cnt2 < 4; cnt2++)
                                    {
                                        p[0].cards[cnt2] = data_player[session_w.p_pos].Split(',')[cnt2];
                                    }
                                else
                                {
                                    for (int cnt2 = 0; cnt2 < 4; cnt2++)
                                    {
                                        if (cnt2 < 4)
                                            p[cnt].cards[cnt2] = "x";
                                    }
                                }
                            }
                            launchBTN_Click(null, null);

                            if (v_la_main == 0)
                            {
                                block_me = false;
                                if (session_w.chrono == 1)
                                {
                                    chrono_cnt = 20;
                                    chrono.Enabled = true;
                                    chrono_checker_cnt = 30;
                                    chrono_checker.Enabled = false;
                                }
                            }
                            else
                            {
                                block_me = true;
                                if (session_w.chrono == 1)
                                {
                                    chrono_checker_cnt = 30;
                                    chrono_checker.Enabled = true;
                                }
                            }

                            all_card_function();
                            t_tour.Enabled = true;

                            ///// repassage a la condition "update" éxcéptionellement pour la mise-à-jour des points

                            if (IsConnected())
                            {
                                header();
                                if (client1.IsBusy == false)
                                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?get_cmd=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                                else if (client2.IsBusy == false)
                                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?get_cmd=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                                else if (client3.IsBusy == false)
                                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?get_cmd=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                                else
                                {
                                    evenement_area.Text = evenement_area.Text + "\n" + @"ERREUR\>Impossible de récuperer les évenements system";
                                    //noWebClientfree();
                                }
                            }
                            else
                            {
                                destroySession();
                                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            /////////////////////////////////////////////
                        }
                        else if (data[2] == "update" || (session_w.index_cmd == 1 && refrech_view_once_2 == false))
                        {
                            refrech_view_once_2 = true;
                            session_w.index_cmd = Int16.Parse(data[3]);
                            evenement_area.Text = e.Result;
                            v_la_main = convert_pos[Int16.Parse(data[4])];
                            nbr_cards = Int16.Parse(data[5]);
                            lb_all_card.Text = "Il reste " + nbr_cards + " cartes";
                            
                            // affichage des cartes du milieu
                            for (int cnt = 0; cnt < 10; cnt++)
                            {
                                if (data[6]!="" && cnt < data[6].Split(',').Length)
                                {
                                    v_milieu[cnt] = data[6].Split(',')[cnt];
                                    milieu[cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu[cnt] + ".jpg");
                                    milieu[cnt].BringToFront();
                                    milieu[cnt].Visible = true;
                                }
                                else
                                {
                                    v_milieu[cnt] = "null";
                                    milieu[cnt].Visible = false;
                                }
                            }
                            
                            // affichage des cartes des joueurs
                            for (int cnt = 0; cnt < session_w.nbr_player; cnt++)
                            {
                                string[] data_player = data[7].Split('|');
                                if (cnt == session_w.p_pos)
                                    for (int cnt2 = 0; cnt2 < 4; cnt2++)
                                    {
                                        if (data_player[session_w.p_pos] != "0" && cnt2 < data_player[session_w.p_pos].Split(',').Length && data_player[session_w.p_pos] != "")
                                            p[0].cards[cnt2] = data_player[session_w.p_pos].Split(',')[cnt2];
                                        else
                                            p[0].cards[cnt2] = "null";
                                    }
                                else
                                {
                                    for (int cnt2 = 0; cnt2 < 4; cnt2++)
                                    {
                                        if (cnt2 < Int16.Parse(data_player[cnt]))
                                            p[convert_pos[cnt]].cards[cnt2] = "x";
                                        else
                                            p[convert_pos[cnt]].cards[cnt2] = "null";
                                    }
                                }
                                organiser(cnt);
                            }

                            // affectation des points
                            for (int cnt = 0; cnt < data[8].Split(',').Length; cnt++)
                            {
                                if (cnt == 0)
                                {
                                    if (convert_pos[0] == 0)
                                    {
                                        p_crd_won[convert_pos[0]].Text = data[8].Split(',')[0];

                                        if (Int16.Parse(data[9].Split(',')[0]) > 4)
                                        {
                                            p_kh_won[convert_pos[0]].Text = (Int16.Parse(data[9].Split(',')[0]) / 5).ToString();
                                            p_bnt_won[convert_pos[0]].Text = (Int16.Parse(data[9].Split(',')[0]) - (Int16.Parse(p_kh_won[convert_pos[0]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[0]].Text = data[9].Split(',')[0];
                                            p_kh_won[convert_pos[0]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[0]) == 0)
                                            p_dfou3[convert_pos[0]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[0]) == 1)
                                            p_dfou3[convert_pos[0]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[0]) == 2)
                                            p_dfou3[convert_pos[0]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[0]) == 0)
                                            p_double[convert_pos[0]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[0]) == 1)
                                        {
                                            p_double[convert_pos[0]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[0]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[0]) == 3)
                                        {
                                            p_double[convert_pos[0]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[0]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[1] == 0)
                                    {
                                        p_crd_won[convert_pos[1]].Text = data[8].Split(',')[0];
                                        if (Int16.Parse(data[9].Split(',')[0]) > 4)
                                        {
                                            p_kh_won[convert_pos[1]].Text = (Int16.Parse(data[9].Split(',')[0]) / 5).ToString();
                                            p_bnt_won[convert_pos[1]].Text = (Int16.Parse(data[9].Split(',')[0]) - (Int16.Parse(p_kh_won[convert_pos[1]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[1]].Text = data[9].Split(',')[0];
                                            p_kh_won[convert_pos[1]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[0]) == 0)
                                            p_dfou3[convert_pos[1]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[0]) == 1)
                                            p_dfou3[convert_pos[1]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[0]) == 2)
                                            p_dfou3[convert_pos[1]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[0]) == 0)
                                            p_double[convert_pos[1]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[0]) == 1)
                                        {
                                            p_double[convert_pos[1]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[1]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[0]) == 3)
                                        {
                                            p_double[convert_pos[1]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[1]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[2] == 0)
                                    {
                                        p_crd_won[convert_pos[2]].Text = data[8].Split(',')[0];
                                        if (Int16.Parse(data[9].Split(',')[0]) > 4)
                                        {
                                            p_kh_won[convert_pos[2]].Text = (Int16.Parse(data[9].Split(',')[0]) / 5).ToString();
                                            p_bnt_won[convert_pos[2]].Text = (Int16.Parse(data[9].Split(',')[0]) - (Int16.Parse(p_kh_won[convert_pos[2]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[2]].Text = data[9].Split(',')[0];
                                            p_kh_won[convert_pos[2]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[0]) == 0)
                                            p_dfou3[convert_pos[2]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[0]) == 1)
                                            p_dfou3[convert_pos[2]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[0]) == 2)
                                            p_dfou3[convert_pos[2]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[0]) == 0)
                                            p_double[convert_pos[2]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[0]) == 1)
                                        {
                                            p_double[convert_pos[2]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[2]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[0]) == 3)
                                        {
                                            p_double[convert_pos[2]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[2]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[3] == 0)
                                    {
                                        p_crd_won[convert_pos[3]].Text = data[8].Split(',')[0];
                                        if (Int16.Parse(data[9].Split(',')[0]) > 4)
                                        {
                                            p_kh_won[convert_pos[3]].Text = (Int16.Parse(data[9].Split(',')[0]) / 5).ToString();
                                            p_bnt_won[convert_pos[3]].Text = (Int16.Parse(data[9].Split(',')[0]) - (Int16.Parse(p_kh_won[convert_pos[3]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[3]].Text = data[9].Split(',')[0];
                                            p_kh_won[convert_pos[3]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[0]) == 0)
                                            p_dfou3[convert_pos[3]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[0]) == 1)
                                            p_dfou3[convert_pos[3]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[0]) == 2)
                                            p_dfou3[convert_pos[3]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[0]) == 0)
                                            p_double[convert_pos[3]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[0]) == 1)
                                        {
                                            p_double[convert_pos[3]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[3]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[0]) == 3)
                                        {
                                            p_double[convert_pos[3]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[3]].Visible = true;
                                        }
                                    }
                                }
                                else if (cnt == 1)
                                {
                                    if (convert_pos[0] == 1)
                                    {
                                        p_crd_won[convert_pos[0]].Text = data[8].Split(',')[1];
                                        if (Int16.Parse(data[9].Split(',')[1]) > 4)
                                        {
                                            p_kh_won[convert_pos[0]].Text = (Int16.Parse(data[9].Split(',')[1]) / 5).ToString();
                                            p_bnt_won[convert_pos[0]].Text = (Int16.Parse(data[9].Split(',')[1]) - (Int16.Parse(p_kh_won[convert_pos[0]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[0]].Text = data[9].Split(',')[1];
                                            p_kh_won[convert_pos[0]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[1]) == 0)
                                            p_dfou3[convert_pos[0]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[1]) == 1)
                                            p_dfou3[convert_pos[0]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[1]) == 2)
                                            p_dfou3[convert_pos[0]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[1]) == 0)
                                            p_double[convert_pos[0]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[1]) == 1)
                                        {
                                            p_double[convert_pos[0]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[0]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[1]) == 3)
                                        {
                                            p_double[convert_pos[0]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[0]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[1] == 1)
                                    {
                                        p_crd_won[convert_pos[1]].Text = data[8].Split(',')[1];
                                        if (Int16.Parse(data[9].Split(',')[1]) > 4)
                                        {
                                            p_kh_won[convert_pos[1]].Text = (Int16.Parse(data[9].Split(',')[1]) / 5).ToString();
                                            p_bnt_won[convert_pos[1]].Text = (Int16.Parse(data[9].Split(',')[1]) - (Int16.Parse(p_kh_won[convert_pos[1]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[1]].Text = data[9].Split(',')[1];
                                            p_kh_won[convert_pos[1]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[1]) == 0)
                                            p_dfou3[convert_pos[1]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[1]) == 1)
                                            p_dfou3[convert_pos[1]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[1]) == 2)
                                            p_dfou3[convert_pos[1]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[1]) == 0)
                                            p_double[convert_pos[1]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[1]) == 1)
                                        {
                                            p_double[convert_pos[1]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[1]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[1]) == 3)
                                        {
                                            p_double[convert_pos[1]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[1]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[2] == 1)
                                    {
                                        p_crd_won[convert_pos[2]].Text = data[8].Split(',')[1];
                                        if (Int16.Parse(data[9].Split(',')[1]) > 4)
                                        {
                                            p_kh_won[convert_pos[2]].Text = (Int16.Parse(data[9].Split(',')[1]) / 5).ToString();
                                            p_bnt_won[convert_pos[2]].Text = (Int16.Parse(data[9].Split(',')[1]) - (Int16.Parse(p_kh_won[convert_pos[2]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[2]].Text = data[9].Split(',')[1];
                                            p_kh_won[convert_pos[2]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[1]) == 0)
                                            p_dfou3[convert_pos[2]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[1]) == 1)
                                            p_dfou3[convert_pos[2]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[1]) == 2)
                                            p_dfou3[convert_pos[2]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[1]) == 0)
                                            p_double[convert_pos[2]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[1]) == 1)
                                        {
                                            p_double[convert_pos[2]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[2]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[1]) == 3)
                                        {
                                            p_double[convert_pos[2]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[2]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[3] == 1)
                                    {
                                        p_crd_won[convert_pos[3]].Text = data[8].Split(',')[1];
                                        if (Int16.Parse(data[9].Split(',')[1]) > 4)
                                        {
                                            p_kh_won[convert_pos[3]].Text = (Int16.Parse(data[9].Split(',')[1]) / 5).ToString();
                                            p_bnt_won[convert_pos[3]].Text = (Int16.Parse(data[9].Split(',')[1]) - (Int16.Parse(p_kh_won[convert_pos[3]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[3]].Text = data[9].Split(',')[1];
                                            p_kh_won[convert_pos[3]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[1]) == 0)
                                            p_dfou3[convert_pos[3]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[1]) == 1)
                                            p_dfou3[convert_pos[3]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[1]) == 2)
                                            p_dfou3[convert_pos[3]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[1]) == 0)
                                            p_double[convert_pos[3]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[1]) == 1)
                                        {
                                            p_double[convert_pos[3]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[3]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[1]) == 3)
                                        {
                                            p_double[convert_pos[3]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[3]].Visible = true;
                                        }
                                    }
                                }
                                else if (cnt == 2)
                                {
                                    if (convert_pos[0] == 2)
                                    {
                                        p_crd_won[convert_pos[0]].Text = data[8].Split(',')[2];
                                        if (Int16.Parse(data[9].Split(',')[2]) > 4)
                                        {
                                            p_kh_won[convert_pos[0]].Text = (Int16.Parse(data[9].Split(',')[2]) / 5).ToString();
                                            p_bnt_won[convert_pos[0]].Text = (Int16.Parse(data[9].Split(',')[2]) - (Int16.Parse(p_kh_won[convert_pos[0]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[0]].Text = data[9].Split(',')[2];
                                            p_kh_won[convert_pos[0]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[2]) == 0)
                                            p_dfou3[convert_pos[0]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[2]) == 1)
                                            p_dfou3[convert_pos[0]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[2]) == 2)
                                            p_dfou3[convert_pos[0]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[2]) == 0)
                                            p_double[convert_pos[0]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[2]) == 1)
                                        {
                                            p_double[convert_pos[0]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[0]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[2]) == 3)
                                        {
                                            p_double[convert_pos[0]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[0]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[1] == 2)
                                    {
                                        p_crd_won[convert_pos[1]].Text = data[8].Split(',')[2];
                                        if (Int16.Parse(data[9].Split(',')[2]) > 4)
                                        {
                                            p_kh_won[convert_pos[1]].Text = (Int16.Parse(data[9].Split(',')[2]) / 5).ToString();
                                            p_bnt_won[convert_pos[1]].Text = (Int16.Parse(data[9].Split(',')[2]) - (Int16.Parse(p_kh_won[convert_pos[1]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[1]].Text = data[9].Split(',')[2];
                                            p_kh_won[convert_pos[1]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[2]) == 0)
                                            p_dfou3[convert_pos[1]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[2]) == 1)
                                            p_dfou3[convert_pos[1]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[2]) == 2)
                                            p_dfou3[convert_pos[1]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[2]) == 0)
                                            p_double[convert_pos[1]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[2]) == 1)
                                        {
                                            p_double[convert_pos[1]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[1]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[2]) == 3)
                                        {
                                            p_double[convert_pos[1]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[1]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[2] == 2)
                                    {
                                        p_crd_won[convert_pos[2]].Text = data[8].Split(',')[2];
                                        if (Int16.Parse(data[9].Split(',')[2]) > 4)
                                        {
                                            p_kh_won[convert_pos[2]].Text = (Int16.Parse(data[9].Split(',')[2]) / 5).ToString();
                                            p_bnt_won[convert_pos[2]].Text = (Int16.Parse(data[9].Split(',')[2]) - (Int16.Parse(p_kh_won[convert_pos[2]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[2]].Text = data[9].Split(',')[2];
                                            p_kh_won[convert_pos[2]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[2]) == 0)
                                            p_dfou3[convert_pos[2]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[2]) == 1)
                                            p_dfou3[convert_pos[2]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[2]) == 2)
                                            p_dfou3[convert_pos[2]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[2]) == 0)
                                            p_double[convert_pos[2]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[2]) == 1)
                                        {
                                            p_double[convert_pos[2]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[2]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[2]) == 3)
                                        {
                                            p_double[convert_pos[2]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[2]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[3] == 2)
                                    {
                                        p_crd_won[convert_pos[3]].Text = data[8].Split(',')[2];
                                        if (Int16.Parse(data[9].Split(',')[2]) > 4)
                                        {
                                            p_kh_won[convert_pos[3]].Text = (Int16.Parse(data[9].Split(',')[2]) / 5).ToString();
                                            p_bnt_won[convert_pos[3]].Text = (Int16.Parse(data[9].Split(',')[2]) - (Int16.Parse(p_kh_won[convert_pos[3]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[3]].Text = data[9].Split(',')[2];
                                            p_kh_won[convert_pos[3]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[2]) == 0)
                                            p_dfou3[convert_pos[3]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[2]) == 1)
                                            p_dfou3[convert_pos[3]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[2]) == 2)
                                            p_dfou3[convert_pos[3]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[2]) == 0)
                                            p_double[convert_pos[3]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[2]) == 1)
                                        {
                                            p_double[convert_pos[3]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[3]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[2]) == 3)
                                        {
                                            p_double[convert_pos[3]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[3]].Visible = true;
                                        }
                                    }
                                }
                                else if (cnt == 3)
                                {
                                    if (convert_pos[0] == 3)
                                    {
                                        p_crd_won[convert_pos[0]].Text = data[8].Split(',')[3];
                                        if (Int16.Parse(data[9].Split(',')[3]) > 4)
                                        {
                                            p_kh_won[convert_pos[0]].Text = (Int16.Parse(data[9].Split(',')[3]) / 5).ToString();
                                            p_bnt_won[convert_pos[0]].Text = (Int16.Parse(data[9].Split(',')[3]) - (Int16.Parse(p_kh_won[convert_pos[0]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[0]].Text = data[9].Split(',')[3];
                                            p_kh_won[convert_pos[0]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[3]) == 0)
                                            p_dfou3[convert_pos[0]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[3]) == 1)
                                            p_dfou3[convert_pos[0]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[3]) == 2)
                                            p_dfou3[convert_pos[0]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[3]) == 0)
                                            p_double[convert_pos[0]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[3]) == 1)
                                        {
                                            p_double[convert_pos[0]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[0]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[3]) == 3)
                                        {
                                            p_double[convert_pos[0]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[0]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[1] == 3)
                                    {
                                        p_crd_won[convert_pos[1]].Text = data[8].Split(',')[3];
                                        if (Int16.Parse(data[9].Split(',')[3]) > 4)
                                        {
                                            p_kh_won[convert_pos[1]].Text = (Int16.Parse(data[9].Split(',')[3]) / 5).ToString();
                                            p_bnt_won[convert_pos[1]].Text = (Int16.Parse(data[9].Split(',')[3]) - (Int16.Parse(p_kh_won[convert_pos[1]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[1]].Text = data[9].Split(',')[3];
                                            p_kh_won[convert_pos[1]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[3]) == 0)
                                            p_dfou3[convert_pos[1]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[3]) == 1)
                                            p_dfou3[convert_pos[1]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[3]) == 2)
                                            p_dfou3[convert_pos[1]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[3]) == 0)
                                            p_double[convert_pos[1]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[3]) == 1)
                                        {
                                            p_double[convert_pos[1]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[1]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[3]) == 3)
                                        {
                                            p_double[convert_pos[1]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[1]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[2] == 3)
                                    {
                                        p_crd_won[convert_pos[2]].Text = data[8].Split(',')[3];
                                        if (Int16.Parse(data[9].Split(',')[3]) > 4)
                                        {
                                            p_kh_won[convert_pos[2]].Text = (Int16.Parse(data[9].Split(',')[3]) / 5).ToString();
                                            p_bnt_won[convert_pos[2]].Text = (Int16.Parse(data[9].Split(',')[3]) - (Int16.Parse(p_kh_won[convert_pos[2]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[2]].Text = data[9].Split(',')[3];
                                            p_kh_won[convert_pos[2]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[3]) == 0)
                                            p_dfou3[convert_pos[2]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[3]) == 1)
                                            p_dfou3[convert_pos[2]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[3]) == 2)
                                            p_dfou3[convert_pos[2]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[3]) == 0)
                                            p_double[convert_pos[2]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[3]) == 1)
                                        {
                                            p_double[convert_pos[2]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[2]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[3]) == 3)
                                        {
                                            p_double[convert_pos[2]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[2]].Visible = true;
                                        }
                                    }
                                    else if (convert_pos[3] == 3)
                                    {
                                        p_crd_won[convert_pos[3]].Text = data[8].Split(',')[3];
                                        if (Int16.Parse(data[9].Split(',')[3]) > 4)
                                        {
                                            p_kh_won[convert_pos[3]].Text = (Int16.Parse(data[9].Split(',')[3]) / 5).ToString();
                                            p_bnt_won[convert_pos[3]].Text = (Int16.Parse(data[9].Split(',')[3]) - (Int16.Parse(p_kh_won[convert_pos[3]].Text) * 5)).ToString();
                                        }
                                        else
                                        {
                                            p_bnt_won[convert_pos[3]].Text = data[9].Split(',')[3];
                                            p_kh_won[convert_pos[3]].Text = "0";
                                        }

                                        if (Int16.Parse(data[10].Split(',')[3]) == 0)
                                            p_dfou3[convert_pos[3]].Text = "";
                                        else if (Int16.Parse(data[10].Split(',')[3]) == 1)
                                            p_dfou3[convert_pos[3]].Text = "DS";
                                        else if (Int16.Parse(data[10].Split(',')[3]) == 2)
                                            p_dfou3[convert_pos[3]].Text = "DC";

                                        if (Int16.Parse(data[11].Split(',')[3]) == 0)
                                            p_double[convert_pos[3]].Visible = false;
                                        else if (Int16.Parse(data[11].Split(',')[3]) == 1)
                                        {
                                            p_double[convert_pos[3]].Image = Properties.Resources.etoile2;
                                            p_double[convert_pos[3]].Visible = true;
                                        }
                                        else if (Int16.Parse(data[11].Split(',')[3]) == 3)
                                        {
                                            p_double[convert_pos[3]].Image = Properties.Resources.etoile3;
                                            p_double[convert_pos[3]].Visible = true;
                                        }
                                    }
                                }
                            }
                            // p1_kh_won.Text




                            //////////////////////////////////////
                            all_card_function();
                            if (v_la_main == 0)
                            {
                                block_me = false;
                                if (session_w.chrono == 1)
                                {
                                    chrono_cnt = 20;
                                    chrono.Enabled = true;
                                    chrono_checker_cnt = 30;
                                    chrono_checker.Enabled = false;
                                }
                            }
                            else
                            {
                                block_me = true;
                                if (session_w.chrono == 1)
                                {
                                    chrono_checker_cnt = 30;
                                    chrono_checker.Enabled = true;
                                }
                            }
                        }
                        else if (data[2] == "winner")
                        {/*
                            cmd_checker.Enabled = false;
                            chrono_checker.Enabled = false;
                            chrono.Enabled = false;
                            session_w.index_cmd = Int16.Parse(data[3]);
                            v_la_main = convert_pos[Int16.Parse(data[4])];
                            nbr_cards = Int16.Parse(data[5]);
                            lb_all_card.Text = "Il reste " + nbr_cards + " cartes";
                            v_milieu = data[6];
                            milieu[0].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");
                            check_winner(v_la_main);
                            session_w.playing = false;*/
                        }
                        else if (data[2] == "null")
                        {
                            cmd_checker.Enabled = false;
                            chrono_checker.Enabled = false;
                            chrono.Enabled = false;
                            session_w.index_cmd = Int16.Parse(data[3]);
                            v_la_main = convert_pos[Int16.Parse(data[4])];
                            null_party(v_la_main);
                            session_w.playing = false;

                        }
                    }
                }
                else if (data[1] == "cmd2")
                {
                    if (data[2] == "extra3")
                    {
                        if (data[3] == "first_step")
                        {
                            block_me = true;
                            int tmp_cnt_cards = 0;
                            for (int cnt = 0; cnt < 4; cnt++)//it was 15 not 4
                                if (p[0].cards[cnt] == "null")
                                {
                                    tmp_cnt_cards = cnt;
                                    break;
                                }

                            if (tmp_cnt_cards > 1)
                            {
                                /*for (int cnt = 0; cnt < 4; cnt++)
                                {
                                    choix[cnt].BringToFront();
                                    choix[cnt].Visible = true;
                                }*/
                            }
                        }
                        else if (data[3] == "final_step")
                        {
                            /*for (int cnt = 0; cnt < 4; cnt++)
                                choix[cnt].Visible = false;*/
                        }
                    }
                }
                else if (data[1] == "take")
                {

                }
                else if (data[1] == "points")
                {
                    // affichage des points a la fin du match
                    winner_pts = new Label();
                    winner_pts.Font = new Font("Verdana", 30F, System.Drawing.FontStyle.Bold);
                    if (Int16.Parse(data[2]) < 1000)
                        winner_pts.ForeColor = Color.Red;
                    else if (Int16.Parse(data[2]) < 2000)
                        winner_pts.ForeColor = Color.Yellow;
                    else if (Int16.Parse(data[2]) < 3000)
                        winner_pts.ForeColor = Color.Blue;
                    else if (Int16.Parse(data[2]) < 4000)
                        winner_pts.ForeColor = Color.Green;

                    winner_pts.BackColor = Color.Transparent;
                    if (v_la_main == 0)
                        winner_pts.Text = "+" + (Int16.Parse(data[2]) - p[0].points) + " Pts   [Total:" + data[2] + " Pts]";
                    else
                        winner_pts.Text = "-" + (p[0].points - Int16.Parse(data[2])) + " Pts   [Total:" + data[2] + " Pts]";

                    winner_pts.AutoSize = true;
                    winner_pts.BringToFront();
                    backimg.Controls.Add(winner_pts);
                    winner_pts.Location = new Point((this.Width / 2) - (winner_pts.Width / 2), winner_lb.Location.Y + winner_lb.Height + 20);
                    winner_pts.Show();
                    main._main.updatePts(Int16.Parse(data[2]));
                }
            }
        }

        void client_chat_checker2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');
            if (data[0] == "C4RT4")
            {
                if (data[1] == "chat")
                {
                    if (data[2] == "msg")
                    {
                        string[] all_data = data[3].Split('/');
                        int index_chat = Int16.Parse(data[4]);

                        if (session_w.index_chat < index_chat)
                        {
                            for (int cnt = session_w.index_chat; cnt < all_data.Length; cnt++)
                            {
                                string[] all_msg = all_data[cnt].Split(':');

                                if (session_w.index_chat < Int16.Parse(all_msg[0]))
                                {
                                    session_w.index_chat = Int16.Parse(all_msg[0]);
                                    chat_msg.Text = "";

                                    if (chat_area.Text == "")
                                        chat_area.Text = all_msg[1] + ">dit: " + all_msg[2];
                                    else
                                        chat_area.Text = chat_area.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];
                                    chat_area.SelectionStart = chat_area.Text.Length;
                                    chat_area.ScrollToCaret();
                                    chat_area.Refresh();
                                }
                                else
                                    break;
                            }
                        }

                        if (data[5] == "Initialisation")
                        {
                            string[] data_client = data[6].Split('/');
                            p1_name.Text = data_client[0].Split(':')[0];
                            if (data_client[0].Split(':')[1] == "waiting")
                            {
                                p1_lb.Text = "En attente";
                                p1_lb.ForeColor = Color.Red;
                            }
                            else if (data_client[0].Split(':')[1] == "ready")
                            {
                                p1_lb.Text = "Prét";
                                p1_lb.ForeColor = Color.Green;
                            }
                            if (p1_name.Text == session_w.user && p1_lb.Text == "En attente")
                                launch_partie.Enabled = true;

                            p1_name.Visible = true;
                            p1_lb.Visible = true;
                            //////////////////////////////////////////////////
                            p2_name.Text = data_client[1].Split(':')[0];
                            if (data_client[1].Split(':')[1] == "waiting")
                            {
                                p2_lb.Text = "En attente";
                                p2_lb.ForeColor = Color.Red;
                            }
                            else if (data_client[1].Split(':')[1] == "ready")
                            {
                                p2_lb.Text = "Prét";
                                p2_lb.ForeColor = Color.Green;
                            }
                            if (p2_name.Text == session_w.user && p2_lb.Text == "En attente")
                                launch_partie.Enabled = true;

                            p2_name.Visible = true;
                            p2_lb.Visible = true;
                            /////////////////////////////////////////
                            if (session_w.nbr_player >= 3)
                            {
                                p3_name.Text = data_client[2].Split(':')[0];
                                if (data_client[2].Split(':')[1] == "waiting")
                                {
                                    p3_lb.Text = "En attente";
                                    p3_lb.ForeColor = Color.Red;
                                }
                                else if (data_client[2].Split(':')[1] == "ready")
                                {
                                    p3_lb.Text = "Prét";
                                    p3_lb.ForeColor = Color.Green;
                                }

                                if (p3_name.Text == session_w.user && p3_lb.Text == "En attente")
                                    launch_partie.Enabled = true;

                                p3_name.Visible = true;
                                p3_lb.Visible = true;
                            }

                            ///////////////////////////////////////////////
                            if (session_w.nbr_player >= 4)
                            {
                                p4_name.Text = data_client[3].Split(':')[0];
                                if (data_client[3].Split(':')[1] == "waiting")
                                {
                                    p4_lb.Text = "En attente";
                                    p4_lb.ForeColor = Color.Red;
                                }
                                else if (data_client[3].Split(':')[1] == "ready")
                                {
                                    p4_lb.Text = "Prét";
                                    p4_lb.ForeColor = Color.Green;
                                }

                                if (p4_name.Text == session_w.user && p4_lb.Text == "En attente")
                                    launch_partie.Enabled = true;

                                p4_name.Visible = true;
                                p4_lb.Visible = true;
                            }
                        }
                        else if (data[3] == "Occupe")
                        {
                            if (cmd_checker.Enabled == false)
                                cmd_checker.Enabled = true;
                        }
                    }
                    else if (data[2] == "nothing")
                    {
                        if (data[3] == "Initialisation")
                        {
                            string[] data_client = data[4].Split('/');
                            p1_name.Text = data_client[0].Split(':')[0];
                            if (data_client[0].Split(':')[1] == "waiting")
                            {
                                p1_lb.Text = "En attente";
                                p1_lb.ForeColor = Color.Red;
                            }
                            else if (data_client[0].Split(':')[1] == "ready")
                            {
                                p1_lb.Text = "Prét";
                                p1_lb.ForeColor = Color.Green;
                            }
                            if (p1_name.Text == session_w.user && p1_lb.Text == "En attente")
                                launch_partie.Enabled = true;

                            p1_name.Visible = true;
                            p1_lb.Visible = true;
                            //////////////////////////////////////////////////
                            p2_name.Text = data_client[1].Split(':')[0];
                            if (data_client[1].Split(':')[1] == "waiting")
                            {
                                p2_lb.Text = "En attente";
                                p2_lb.ForeColor = Color.Red;
                            }
                            else if (data_client[1].Split(':')[1] == "ready")
                            {
                                p2_lb.Text = "Prét";
                                p2_lb.ForeColor = Color.Green;
                            }
                            if (p2_name.Text == session_w.user && p2_lb.Text == "En attente")
                                launch_partie.Enabled = true;

                            p2_name.Visible = true;
                            p2_lb.Visible = true;
                            /////////////////////////////////////////
                            if (session_w.nbr_player >= 3)
                            {
                                p3_name.Text = data_client[2].Split(':')[0];
                                if (data_client[2].Split(':')[1] == "waiting")
                                {
                                    p3_lb.Text = "En attente";
                                    p3_lb.ForeColor = Color.Red;
                                }
                                else if (data_client[2].Split(':')[1] == "ready")
                                {
                                    p3_lb.Text = "Prét";
                                    p3_lb.ForeColor = Color.Green;
                                }

                                if (p3_name.Text == session_w.user && p3_lb.Text == "En attente")
                                    launch_partie.Enabled = true;

                                p3_name.Visible = true;
                                p3_lb.Visible = true;
                            }

                            ///////////////////////////////////////////////
                            if (session_w.nbr_player >= 4)
                            {
                                p4_name.Text = data_client[3].Split(':')[0];
                                if (data_client[3].Split(':')[1] == "waiting")
                                {
                                    p4_lb.Text = "En attente";
                                    p4_lb.ForeColor = Color.Red;
                                }
                                else if (data_client[3].Split(':')[1] == "ready")
                                {
                                    p4_lb.Text = "Prét";
                                    p4_lb.ForeColor = Color.Green;
                                }

                                if (p4_name.Text == session_w.user && p4_lb.Text == "En attente")
                                    launch_partie.Enabled = true;

                                p4_name.Visible = true;
                                p4_lb.Visible = true;
                            }
                        }
                        else if (data[3] == "Occupe")
                        {
                            if (cmd_checker.Enabled == false)
                                cmd_checker.Enabled = true;
                        }
                    }
                }
                else if (data[1] == "droped")
                {
                    destroySession();
                    MessageBox.Show(this, "Vous avez été exclus de la partie.\nmerci de choisir une autre table.", "Erreur de session", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void client_chat_checker1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');
            if (data[0] == "C4RT4")
            {
                if (data[1] == "chat")
                {
                    if (data[2] == "msg")
                    {
                        string[] all_data = data[3].Split('/');
                        int index_chat = Int16.Parse(data[4]);

                        if (session_w.index_chat < index_chat)
                        {
                            for (int cnt = session_w.index_chat; cnt < all_data.Length; cnt++)
                            {
                                string[] all_msg = all_data[cnt].Split(':');

                                if (session_w.index_chat < Int16.Parse(all_msg[0]))
                                {
                                    session_w.index_chat = Int16.Parse(all_msg[0]);
                                    chat_msg.Text = "";

                                    if (chat_area.Text == "")
                                        chat_area.Text = all_msg[1] + ">dit: " + all_msg[2];
                                    else
                                        chat_area.Text = chat_area.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];
                                    chat_area.SelectionStart = chat_area.Text.Length;
                                    chat_area.ScrollToCaret();
                                    chat_area.Refresh();
                                }
                                else
                                    break;
                            }
                        }

                        if (data[5] == "Initialisation")
                        {
                            string[] data_client = data[6].Split('/');
                            p1_name.Text = data_client[0].Split(':')[0];
                            if (data_client[0].Split(':')[1] == "waiting")
                            {
                                p1_lb.Text = "En attente";
                                p1_lb.ForeColor = Color.Red;
                            }
                            else if (data_client[0].Split(':')[1] == "ready")
                            {
                                p1_lb.Text = "Prét";
                                p1_lb.ForeColor = Color.Green;
                            }
                            if (p1_name.Text == session_w.user && p1_lb.Text == "En attente")
                                launch_partie.Enabled = true;

                            p1_name.Visible = true;
                            p1_lb.Visible = true;
                            //////////////////////////////////////////////////
                            p2_name.Text = data_client[1].Split(':')[0];
                            if (data_client[1].Split(':')[1] == "waiting")
                            {
                                p2_lb.Text = "En attente";
                                p2_lb.ForeColor = Color.Red;
                            }
                            else if (data_client[1].Split(':')[1] == "ready")
                            {
                                p2_lb.Text = "Prét";
                                p2_lb.ForeColor = Color.Green;
                            }
                            if (p2_name.Text == session_w.user && p2_lb.Text == "En attente")
                                launch_partie.Enabled = true;

                            p2_name.Visible = true;
                            p2_lb.Visible = true;
                            /////////////////////////////////////////
                            if (session_w.nbr_player >= 3)
                            {
                                p3_name.Text = data_client[2].Split(':')[0];
                                if (data_client[2].Split(':')[1] == "waiting")
                                {
                                    p3_lb.Text = "En attente";
                                    p3_lb.ForeColor = Color.Red;
                                }
                                else if (data_client[2].Split(':')[1] == "ready")
                                {
                                    p3_lb.Text = "Prét";
                                    p3_lb.ForeColor = Color.Green;
                                }

                                if (p3_name.Text == session_w.user && p3_lb.Text == "En attente")
                                    launch_partie.Enabled = true;

                                p3_name.Visible = true;
                                p3_lb.Visible = true;
                            }

                            ///////////////////////////////////////////////
                            if (session_w.nbr_player >= 4)
                            {
                                p4_name.Text = data_client[3].Split(':')[0];
                                if (data_client[3].Split(':')[1] == "waiting")
                                {
                                    p4_lb.Text = "En attente";
                                    p4_lb.ForeColor = Color.Red;
                                }
                                else if (data_client[3].Split(':')[1] == "ready")
                                {
                                    p4_lb.Text = "Prét";
                                    p4_lb.ForeColor = Color.Green;
                                }

                                if (p4_name.Text == session_w.user && p4_lb.Text == "En attente")
                                    launch_partie.Enabled = true;

                                p4_name.Visible = true;
                                p4_lb.Visible = true;
                            }
                        }
                        else if (data[3] == "Occupe")
                        {
                            if (cmd_checker.Enabled == false)
                                cmd_checker.Enabled = true;
                        }
                    }
                    else if (data[2] == "nothing")
                    {
                        if (data[3] == "Initialisation")
                        {
                            string[] data_client = data[4].Split('/');
                            p1_name.Text = data_client[0].Split(':')[0];
                            if (data_client[0].Split(':')[1] == "waiting")
                            {
                                p1_lb.Text = "En attente";
                                p1_lb.ForeColor = Color.Red;
                            }
                            else if (data_client[0].Split(':')[1] == "ready")
                            {
                                p1_lb.Text = "Prét";
                                p1_lb.ForeColor = Color.Green;
                            }
                            if (p1_name.Text == session_w.user && p1_lb.Text == "En attente")
                                launch_partie.Enabled = true;

                            p1_name.Visible = true;
                            p1_lb.Visible = true;
                            //////////////////////////////////////////////////
                            p2_name.Text = data_client[1].Split(':')[0];
                            if (data_client[1].Split(':')[1] == "waiting")
                            {
                                p2_lb.Text = "En attente";
                                p2_lb.ForeColor = Color.Red;
                            }
                            else if (data_client[1].Split(':')[1] == "ready")
                            {
                                p2_lb.Text = "Prét";
                                p2_lb.ForeColor = Color.Green;
                            }
                            if (p2_name.Text == session_w.user && p2_lb.Text == "En attente")
                                launch_partie.Enabled = true;

                            p2_name.Visible = true;
                            p2_lb.Visible = true;
                            /////////////////////////////////////////
                            if (session_w.nbr_player >= 3)
                            {
                                p3_name.Text = data_client[2].Split(':')[0];
                                if (data_client[2].Split(':')[1] == "waiting")
                                {
                                    p3_lb.Text = "En attente";
                                    p3_lb.ForeColor = Color.Red;
                                }
                                else if (data_client[2].Split(':')[1] == "ready")
                                {
                                    p3_lb.Text = "Prét";
                                    p3_lb.ForeColor = Color.Green;
                                }

                                if (p3_name.Text == session_w.user && p3_lb.Text == "En attente")
                                    launch_partie.Enabled = true;

                                p3_name.Visible = true;
                                p3_lb.Visible = true;
                            }

                            ///////////////////////////////////////////////
                            if (session_w.nbr_player >= 4)
                            {
                                p4_name.Text = data_client[3].Split(':')[0];
                                if (data_client[3].Split(':')[1] == "waiting")
                                {
                                    p4_lb.Text = "En attente";
                                    p4_lb.ForeColor = Color.Red;
                                }
                                else if (data_client[3].Split(':')[1] == "ready")
                                {
                                    p4_lb.Text = "Prét";
                                    p4_lb.ForeColor = Color.Green;
                                }

                                if (p4_name.Text == session_w.user && p4_lb.Text == "En attente")
                                    launch_partie.Enabled = true;

                                p4_name.Visible = true;
                                p4_lb.Visible = true;
                            }
                        }
                        else if (data[3] == "Occupe")
                        {
                            if (cmd_checker.Enabled == false)
                                cmd_checker.Enabled = true;
                        }
                    }
                }
                else if (data[1] == "droped")
                {
                    destroySession();
                    MessageBox.Show(this, "Vous avez été exclus de la partie.\nmerci de choisir une autre table.", "Erreur de session", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void client_syn2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
            {
                if (data[1] == "error")
                {
                    if (data[2] == "session lifetimeout")
                    {
                        syn_network.Enabled = false;
                        chat_checker.Enabled = false;
                        MessageBox.Show(this, "Vous avez été déconnecté suite a un trop nombre d'attente sans réponse\nmerci de vous connecter a nouveau", "Erreur de session", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (IsConnected())
                        {
                            header();
                            if (client_syn1.IsBusy == false)
                                client_syn1.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signout=1&id_app=" + session_w.id_app));
                            else if (client_syn2.IsBusy == false)
                                client_syn2.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signout=1&id_app=" + session_w.id_app));
                            else
                            {
                                //MessageBox.Show("ERROR 1");
                                //noWebClientfree();
                            }
                        }
                        else
                        {
                            destroySession();
                            MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        void client_syn1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
            {
                if (data[1] == "error")
                {
                    if (data[2] == "session lifetimeout")
                    {
                        syn_network.Enabled = false;
                        chat_checker.Enabled = false;
                        MessageBox.Show(this, "Vous avez été déconnecté suite a un trop nombre d'attente sans réponse\nmerci de vous connecter a nouveau", "Erreur de session", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (IsConnected())
                        {
                            header();
                            if (client_syn1.IsBusy == false)
                                client_syn1.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signout=1&id_app=" + session_w.id_app));
                            else if (client_syn2.IsBusy == false)
                                client_syn2.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signout=1&id_app=" + session_w.id_app));
                            else
                            {
                                //MessageBox.Show("ERROR 1");
                                //noWebClientfree();
                            }
                        }
                        else
                        {
                            destroySession();
                            MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        //////////////// LOAD PROFILE
        private void profile_load()
        {
            if (File.Exists("save\\rondaR.xml"))
            {
                XmlTextReader xml_profile = new XmlTextReader("save\\rondaR.xml");
                xml_profile.WhitespaceHandling = WhitespaceHandling.None;

                while (xml_profile.Read())
                {
                    if (xml_profile.LocalName == "dos")
                    {
                        load_profileR.dos = Int16.Parse(xml_profile.ReadString());
                        if (launched == true)
                        {
                            if (load_profileR.dos == 1)
                                toolStripMenuItem7_Click(null, null);
                            else if (load_profileR.dos == 2)
                                toolStripMenuItem8_Click(null, null);
                            else if (load_profileR.dos == 3)
                                toolStripMenuItem9_Click(null, null);
                        }

                        load_profileR.tapis = Properties.Settings.Default.tapis;

                        if (load_profileR.tapis == 1)
                            toolStripMenuItem14_Click(null, null);
                        else if (load_profileR.tapis == 2)
                            toolStripMenuItem15_Click(null, null);
                        else if (load_profileR.tapis == 3)
                            toolStripMenuItem16_Click(null, null);
                        else if (load_profileR.tapis == 4)
                            toolStripMenuItem17_Click(null, null);
                        else if (load_profileR.tapis == 5)
                            toolStripMenuItem18_Click(null, null);

                        xml_profile.Read();
                    }

                    if (xml_profile.LocalName == "son")
                    {
                        load_profileR.son = Int16.Parse(xml_profile.ReadString());

                        if (load_profileR.son == 1)
                        {
                            toolStripButton1.Image = Properties.Resources.volumeOk;
                            load_profileR.son = 1;
                        }
                        else
                        {
                            toolStripButton1.Image = Properties.Resources.volumeKo;
                            load_profileR.son = 0;
                        }
                        xml_profile.Read();
                    }
                }

                xml_profile.Close();
                load_pro = true;
            }
            else
            {
                MessageBox.Show(this, "Le fichier de sauveguard est introuvable,\nmerci de ré-installer l'application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        /////////////// save profile \\\\\\\\\\\\\\\\\\
        private void profile_save()
        {
            if (File.Exists("save\\rondaR.xml"))
            {
                XmlTextWriter xml_profile = new XmlTextWriter("save\\rondaR.xml", null);
                xml_profile.WriteStartDocument();
                xml_profile.WriteStartElement("params");
                xml_profile.WriteWhitespace("\n");

                xml_profile.WriteStartElement("dos");
                xml_profile.WriteString(load_profileR.dos.ToString());
                xml_profile.WriteEndElement();
                xml_profile.WriteWhitespace("\n");

                Properties.Settings.Default.tapis = load_profileR.tapis;
                Properties.Settings.Default.Save();

                xml_profile.WriteStartElement("son");
                xml_profile.WriteString(load_profileR.son.ToString());
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

        private void global_load()
        {
            if (File.Exists("save\\global.xml"))
            {
                XmlTextReader xml_global = new XmlTextReader("save\\global.xml");
                xml_global.WhitespaceHandling = WhitespaceHandling.None;
                int option_loaded = 0;
                int speed_loaded = 0;
                while (xml_global.Read())
                {
                    if (xml_global.LocalName == "option")
                    {
                        option_loaded = Int16.Parse(xml_global.ReadString());
                        xml_global.Read();
                    }

                    if (xml_global.LocalName == "speed")
                    {
                        speed_loaded = Int16.Parse(xml_global.ReadString());
                        xml_global.Read();
                    }
                }
                xml_global.Close();

                ////////////// option ///////////
                if (option_loaded == 1)
                {
                    chat_checker.Interval = 4000;
                    cmd_checker.Interval = 4000;
                }
                else if (option_loaded == 2)
                {

                }
                else if (option_loaded == 3)
                {
                    chat_checker.Interval = speed_loaded * 1000;
                    cmd_checker.Interval = speed_loaded * 1000;
                }
            }
            else
            {
                MessageBox.Show(this, "Le fichier de sauveguard est introuvable,\nmerci de réinstaller l'application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        ////////////////////  sound launcher  ////////////////////////
        private void sound_launcher(int pl, string extraS)
        {
            if (block == false)
                if (load_profileR.son == 1)
                {
                    Random rnd = new Random();
                    DirectoryInfo di_files = new DirectoryInfo(@"profiles\ronda\" + p[pl].profile + @"\" + extraS + @"\");
                    FileInfo[] fi_files = di_files.GetFiles();
                    bool valid_dir = false;

                    for (int cnt = 0; cnt < fi_files.Length; cnt++)
                        if (fi_files[cnt].Extension == ".wav" || fi_files[cnt].Extension == ".mp3" || fi_files[cnt].Extension == ".au")
                        {
                            valid_dir = true;
                            break;
                        }

                    if (valid_dir == true)
                        PlaySound(@"profiles\ronda\" + p[pl].profile + @"\" + extraS + @"\" + fi_files[rnd.Next(fi_files.Length)].Name, 0, 0x00020000);
                    else
                    {
                        MessageBox.Show("Aucun média n'a été trouvé sur la bibliotheque\n\tRéinstaller l'application svp");
                        this.Close();
                    }
                }
        }

        //////// organisation des cartes
        private void organiser(int player)
        {
            for (int cnt = 0; cnt < 4; cnt++)
            {
                if (convert_pos[player] == 0)
                {
                    if (p[convert_pos[player]].cards[cnt] != "null")
                    {
                        p_card[convert_pos[player], cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[convert_pos[player]].cards[cnt] + ".jpg");
                        p_card[convert_pos[player], cnt].BringToFront();
                        p_card[convert_pos[player], cnt].Visible = true;
                    }
                    else
                        p_card[convert_pos[player], cnt].Visible = false;
                }
                else if (convert_pos[player] == 1)
                {
                    if (p[convert_pos[player]].cards[cnt] == "x")
                    {
                        p_card[convert_pos[player], cnt].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                        p_card[convert_pos[player], cnt].BringToFront();
                        p_card[convert_pos[player], cnt].Visible = true;
                    }
                    else
                        p_card[convert_pos[player], cnt].Visible = false;
                }
                else if (convert_pos[player] == 2)
                {
                    if (p[convert_pos[player]].cards[cnt] == "x")
                    {
                        p_card[convert_pos[player], cnt].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                        p_card[convert_pos[player], cnt].Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                        p_card[convert_pos[player], cnt].BringToFront();
                        p_card[convert_pos[player], cnt].Visible = true;
                    }
                    else
                        p_card[convert_pos[player], cnt].Visible = false;
                }
                else if (convert_pos[player] == 3)
                {
                    if (p[convert_pos[player]].cards[cnt] == "x")
                    {
                        p_card[convert_pos[player], cnt].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                        p_card[convert_pos[player], cnt].Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        p_card[convert_pos[player], cnt].BringToFront();
                        p_card[convert_pos[player], cnt].Visible = true;
                    }
                    else
                        p_card[convert_pos[player], cnt].Visible = false;
                }
            }
        }

        // a supprimer
        ////////// click sur le tas des cartes a gauche
        void all_card_Click(object sender, EventArgs e)
        {
            if (v_la_main == 0 && block_me == false)
            {
                PictureBox tmp = (PictureBox)sender;
                int count_p_cards = 0;

                for (int cnt = 0; cnt < max_cards; cnt++)
                {
                    if (p[0].cards[cnt] == "null")
                        break;
                    else
                        count_p_cards++;
                }

                if (count_p_cards < max_cards)
                {
                    // ajout d'une carte par webclient
                    if (IsConnected())
                    {
                        header();
                        if (client1.IsBusy == false)
                        {
                            client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?give=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                            block_me = true;
                            chrono.Enabled = false;
                            chrono_lb.Visible = false;
                        }
                        else if (client2.IsBusy == false)
                        {
                            client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?give=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                            block_me = true;
                            chrono.Enabled = false;
                            chrono_lb.Visible = false;
                        }
                        else if (client3.IsBusy == false)
                        {
                            client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?give=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                            block_me = true;
                            chrono.Enabled = false;
                            chrono_lb.Visible = false;
                        }
                        else
                        {
                            evenement_area.Text = evenement_area.Text + "\n" + @"ERREUR\>Requette non éffectué,E1";
                            //MessageBox.Show("ERROR 2");
                            //noWebClientfree();
                        }
                        /*block_me = true;
                        chrono.Enabled = false;
                        chrono_lb.Visible = false;*/
                    }
                    else
                    {
                        destroySession();
                        MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // lancement du son si le player a 15 cartes

                }
            }
        }

        //////////////////////  check the winner  //////////////////
        private void check_winner(int player)
        {
            /////// nétoyage de l'écrant \\\\\
            for (int win_cnt_p = 0; win_cnt_p < session_w.nbr_player; win_cnt_p++)
                for (int win_cnt_card = 0; win_cnt_card < p[win_cnt_p].cards.Length; win_cnt_card++)
                    p_card[win_cnt_p, win_cnt_card].Visible = false;

            for (int cnt = 0; cnt < 40; cnt++)
                all_card[cnt].Visible = false;

            milieu[0].Visible = false;
            lb_all_card.Visible = false;
            /////////////////////////////////
            if (player == 0)
            {
                winner_pic.Image = (Bitmap)Image.FromFile(@"img\trophe.gif");
                winner_lb.Text = "Vous avez gagné la partie";
                if (load_profileR.son == 1)
                {
                    backsound.SoundLocation = @"son\applaud.wav";
                    backsound.Play();
                }
            }
            else
            {
                winner_pic.Image = (Bitmap)Image.FromFile(@"img\GameOver.gif");
                winner_lb.Text = p[player].nom.ToUpper() + " a gagné la partie";
                if (load_profileR.son == 1)
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
            winner_pic.Show();

            /////// param du label winner commun \\\\\
            winner_lb.Font = new Font("Verdana", 30F, System.Drawing.FontStyle.Bold);
            winner_lb.ForeColor = Color.Red;
            winner_lb.BackColor = Color.Transparent;
            winner_lb.AutoSize = true;
            backimg.Controls.Add(winner_lb);
            winner_lb_timer.Enabled = true;
            winner_lb.Location = new Point((this.Width / 2) - (winner_lb.Width / 2), winner_pic.Location.Y + winner_pic.Height + 20); //this.Height - 200
            winner_lb.BringToFront();
            winner_lb.Show();

            block_me = true;

            // récupération des points
            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                {
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_pts=1&id_app=" + session_w.id_app));
                    block_me = true;
                }
                else if (client2.IsBusy == false)
                {
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_pts=1&id_app=" + session_w.id_app));
                    block_me = true;
                }
                else if (client3.IsBusy == false)
                {
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_pts=1&id_app=" + session_w.id_app));
                    block_me = true;
                }
                else
                {
                    evenement_area.Text = evenement_area.Text + "\n" + @"ERREUR\>Winner check error";
                    //noWebClientfree();
                }
                //block_me = true;
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // affichage de l'animation de la fin (thumbs colorés)
            if (player == 0)
            {
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
        }

        //////////////////// partie null  ////////////////////////////
        private void null_party(int v_la_main)
        {/*
            /////// nétoyage de l'écrant \\\\\
            for (int win_cnt_p = 0; win_cnt_p < session_w.nbr_player; win_cnt_p++)
                for (int win_cnt_card = 0; win_cnt_card < p[win_cnt_p].cards.Length; win_cnt_card++)
                    p_card[win_cnt_p, win_cnt_card].Visible = false;

            for (int cnt = 0; cnt < 40; cnt++)
                all_card[cnt].Visible = false;

            milieu[0].Visible = false;
            lb_all_card.Visible = false;

            winner_pic.Image = (Bitmap)Image.FromFile(@"img\forfait.gif");
            winner_lb.Text = "Match null";

            //////// param winner_pic commun \\\\\\
            winner_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            winner_pic.BackColor = Color.Transparent;
            backimg.Controls.Add(winner_pic);
            winner_pic.Location = new Point((this.Width / 2) - (winner_pic.Width / 2), (this.Height / 2) - (winner_pic.Height / 2) - 100);
            winner_pic.BringToFront();
            winner_pic.Show();

            ////////// fond sonor  ////////////
            backsound.SoundLocation = @"son\forfait.wav";
            backsound.Play();


            /////// param du label winner commun \\\\\\\
            winner_lb.Font = new Font("Verdana", 30F, System.Drawing.FontStyle.Bold);
            winner_lb.ForeColor = Color.Red;
            winner_lb.BackColor = Color.Transparent;
            winner_lb.AutoSize = true;
            winner_lb.BringToFront();
            backimg.Controls.Add(winner_lb);
            winner_lb_timer.Enabled = true;
            winner_lb.Location = new Point((this.Width / 2) - (winner_lb.Width / 2), winner_pic.Location.Y + winner_pic.Height + 20); //this.Height - 200
            winner_lb.Show();

            block_me = true;*/
        }

        private void destroySession()
        {
            syn_network.Enabled = false;
            chat_checker.Enabled = false;
            session_w.logged = false;
        }

        private bool IsConnected()
        {
            try
            {
                System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient("localhost", 80);
                clnt.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        private void header()
        {
            client1.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client2.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client3.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_syn1.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_syn2.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_chat_checker1.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_chat_checker2.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
        }

        ////////// NEW GAME MENU
        private void new_game_menu()
        {
            this.SuspendLayout();
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

            // add a new Tabcontrol
            ptab.Height = 200;
            ptab.Location = new Point((ngpan.Width - ptab.Width) >> 1, nplab.Location.Y + 20);
            ngpan.Controls.Add(ptab);
            tab_add(session_w.nbr_player);

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

        private void all_card_function()
        {
            for (int cnt = 0; cnt < 40; cnt++)
            {
                if (cnt < nbr_cards)
                    all_card[cnt].Visible = true;
                else
                    all_card[cnt].Visible = false;
            }
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
                pnameT[cnt] = new Label();
                pnameT[cnt].Location = new Point(pnameL[cnt].Width + 16, 8);

                pnameT[cnt].Text = p[cnt].user;
                tab[cnt].Controls.Add(pnameT[cnt]);

                // avatar
                Random rnd = new Random();
                DirectoryInfo di_files = new DirectoryInfo(@"img\ronda\avatars");
                FileInfo[] fi_files = di_files.GetFiles();
                int rnd2 = rnd.Next(fi_files.Length);
                bool valid_dir = false;

                for (int cnt_files = 0; cnt_files < fi_files.Length; cnt_files++)
                    if (fi_files[cnt_files].Extension == ".jpg")
                    {
                        valid_dir = true;
                        break;
                    }

                if (valid_dir == true)
                    while (fi_files[rnd2].Extension != ".jpg")
                        rnd2 = rnd.Next(fi_files.Length);
                else
                {
                    MessageBox.Show(this, "Vous n'avez aucun avatar installé sur votre bibliotheque\n\t\t ré-installer l'application svp", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.Close();
                    break;
                }

                //image d'avatar
                avatarPB[cnt] = new PictureBox();
                avatarPB[cnt].SizeMode = PictureBoxSizeMode.StretchImage;
                avatarPB[cnt].Location = new Point(16, 40);
                avatarPB[cnt].Size = new Size(80, 70);

                if (valid_dir == true)
                {
                    if (p[cnt].avatar == "null")
                        avatarPB[cnt].Image = (Bitmap)Image.FromFile(@"img\ronda\avatars\" + p[cnt].profile + ".jpg");
                    else
                        avatarPB[cnt].Image = (Bitmap)Image.FromFile(@"temps\thumbs_wan\" + p[cnt].avatar);
                }

                tab[cnt].Controls.Add(avatarPB[cnt]);

                //combobox d'avatar
                avatarLB[cnt] = new Label();
                avatarLB[cnt].Font = new Font(avatarLB[cnt].Font.Name, 9, FontStyle.Bold);
                avatarLB[cnt].Size = new Size(86, 25);
                avatarLB[cnt].Location = new Point((avatarPB[cnt].Width) + 18, avatarPB[cnt].Location.Y + (avatarPB[cnt].Width - avatarLB[cnt].Height) - 10);
                tab[cnt].Controls.Add(avatarLB[cnt]);
                avatarLB[cnt].Text = p[cnt].profile;

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

                backsndLB[cnt].Font = new Font(backsndLB[cnt].Font.Name, 9, FontStyle.Bold);
                backsndLB[cnt].Size = new Size(104, 30);
                backsndLB[cnt].Location = new Point(backsndL[cnt].Location.X + backsndL[cnt].Width, backsndL[cnt].Location.Y - 2);
                tab[cnt].Controls.Add(backsndLB[cnt]);
                backsndLB[cnt].SelectedIndex = 0;
            }
            return 0;
        }

        // btn fermer du panneau
        void cancelBTN_Click(object sender, EventArgs e)
        {
            ngpan.Hide();
        }

        // LAUNCH GAME
        void launchBTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            loading_panel.Hide();
            ngpan.Hide();
            set_po(ptab.TabCount);
            start();
            panel_stats_function();
            session_w.playing = true;
        }

        // initialisation des position des players
        private int set_po(int nbr)
        {
            int[,] pos = { { 200, this.Height - 248 }, { 200, shortcuts.Location.Y + shortcuts.Height }, { this.Width - 200, shortcuts.Location.Y + shortcuts.Height + 50 }, { 0, shortcuts.Location.Y + shortcuts.Height + 50 } };

            for (int cnt = 0; cnt < nbr; cnt++)
            {
                // affichage du paneau du player nbr
                player_pan[cnt] = new Panel();
                player_pan[cnt].Size = new Size(92, 140);
                player_pan[cnt].Location = new Point(pos[cnt, 0], pos[cnt, 1]);
                player_pan[cnt].BackColor = Color.LightGray;
                this.Controls.Add(player_pan[cnt]);
                player_pan[cnt].BringToFront();

                // affichage de l'avatar
                avatar[cnt] = new PictureBox();
                if (p[cnt].avatar == "null")
                    avatar[cnt].Image = (Bitmap)Image.FromFile(@"img\ronda\avatars\" + p[cnt].profile + ".jpg");
                else
                    avatar[cnt].Image = (Bitmap)Image.FromFile(@"temps\thumbs_wan\" + p[cnt].avatar);
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
                nom[cnt].Location = new Point(4, avatar[cnt].Height + 2);
                player_pan[cnt].Controls.Add(nom[cnt]);

                //affichage des points
                p_pts[cnt] = new Label();
                p_pts[cnt].AutoSize = true;
                p_pts[cnt].Text = p[cnt].points + " Points";
                p_pts[cnt].Font = new Font(p_pts[cnt].Font.Name, 9, FontStyle.Bold);
                p_pts[cnt].ForeColor = Color.Red;
                p_pts[cnt].BackColor = Color.Transparent;
                p_pts[cnt].Location = new Point(4, nom[cnt].Location.Y + 14);
                player_pan[cnt].Controls.Add(p_pts[cnt]);

                // affichage de la ville
                ville[cnt] = new Label();
                ville[cnt].AutoSize = true;
                ville[cnt].Text = p[cnt].ville;
                ville[cnt].Font = new Font(ville[cnt].Font.Name, 9, FontStyle.Bold);
                ville[cnt].ForeColor = Color.Red;
                ville[cnt].BackColor = Color.Transparent;
                ville[cnt].Location = new Point(4, p_pts[cnt].Location.Y + 12);
                player_pan[cnt].Controls.Add(ville[cnt]);

                // affichage de la team
                team[cnt] = new Label();
                team[cnt].AutoSize = true;
                team[cnt].Text = p[cnt].team;
                team[cnt].Font = new Font(team[cnt].Font.Name, 9, FontStyle.Bold);
                team[cnt].ForeColor = Color.Red;
                team[cnt].BackColor = Color.Transparent;
                team[cnt].Location = new Point(4, ville[cnt].Location.Y + 12);
                player_pan[cnt].Controls.Add(team[cnt]);
            }
            return 0;
        }

        // initialisation da la matrice
        private void start()
        {
            // position des cartes
            int sp = 0;
            int[,] pos = { { 300, this.Height - 278 }, { 300, shortcuts.Location.Y + shortcuts.Height }, { this.Width - 180, shortcuts.Location.Y + shortcuts.Height + 230 }, { 0, shortcuts.Location.Y + shortcuts.Height + 230 } };

            // création des cartes a tirer
            for (int cnt = 0; cnt < 40; cnt++)
            {
                all_card[cnt] = new PictureBox();
                all_card[cnt].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                all_card[cnt].SizeMode = PictureBoxSizeMode.AutoSize;
                all_card[cnt].Location = new Point(200 + sp, ((this.Height - all_card[cnt].Height) >> 1) - 60 + sp);
                backimg.Controls.Add(all_card[cnt]);
                all_card[cnt].Name = cnt.ToString();
                all_card[cnt].BringToFront();
                sp += 2;
                all_card[cnt].Click += new EventHandler(all_card_Click);
                this.all_card[cnt].Cursor = Cursors.Hand;
                if (cnt > (39 - (4 * session_w.nbr_player)))
                    all_card[cnt].Visible = false;
            }

            // label du comptage des cartes
            lb_all_card.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lb_all_card.ForeColor = Color.DarkOrange;
            lb_all_card.Text = "Il reste " + nbr_cards + " cartes";
            lb_all_card.AutoSize = true;
            lb_all_card.Location = new Point(all_card[0].Location.X, all_card[0].Location.Y - 16);
            lb_all_card.Visible = true;
            lb_all_card.BackColor = Color.Transparent;
            lb_all_card.Parent = backimg;
            backimg.Controls.Add(lb_all_card);
            lb_all_card.BringToFront();

            // création des carte du milieu
            for (int cnt_m = 0; cnt_m < 10; cnt_m++)
            {
                milieu[cnt_m] = new PictureBox();
                //milieu[cnt_m].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");
                milieu[cnt_m].SizeMode = PictureBoxSizeMode.StretchImage;
                milieu[cnt_m].Size = new Size(80, 160);
                milieu[cnt_m].Location = new Point((this.Width >> 1) - (milieu[cnt_m].Width >> 1), (this.Height >> 1) - (milieu[cnt_m].Height >> 1) - 30);
                milieu[cnt_m].BackColor = Color.Transparent;
                backimg.Controls.Add(milieu[cnt_m]);
                milieu[cnt_m].BringToFront();
                //milieu[cnt_m].Visible = true;
            }
            rand_pos_cards_middle();

            for (int cnt = 0; cnt < session_w.nbr_player; cnt++)
                add_cards_once(pos[cnt, 0], pos[cnt, 1], cnt, v_front_card, v_dos_card, cnt);
        }

        ///////// DYSPLAY OF 4 CARDS * NUMBER OF PLAYERS
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
                first_pos = 100;
                second_pos = 0;
            }

            for (int cnt2 = 0; cnt2 < max_cards; cnt2++)
            {
                p_card[player, cnt2] = new PictureBox();

                if (z == 0)
                {
                    p_card[player, cnt2].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[player].cards[cnt2] + ".jpg");
                    p_card[player, cnt2].Size = new Size(100, 170);
                }
                else
                {
                    p_card[player, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));

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
                    p_card[player, cnt2].Click += new EventHandler(p_card_Click);
                    p_card[player, cnt2].Cursor = Cursors.Hand;
                }
            }
            launched = true;
            avantToolStripMenuItem.Enabled = true;
            éclairéToolStripMenuItem.Enabled = true;
            tamiséToolStripMenuItem.Enabled = true;

            ////// mutiplié par le nombre des joueurs
            if (style1ToolStripMenuItem1.Checked == true)
            {
                styled_draw(1);
            }
        }

        private void panel_stats_function()
        {
            if (session_w.nbr_player == 2)
            {
                /// p1 stats
                p_stats[convert_pos[0]].Location = new Point(p_card[convert_pos[0], 3].Location.X + p_card[convert_pos[0], 3].Width + 10, p_card[convert_pos[0], 3].Location.Y + 32);
                p_stats[convert_pos[0]].BackgroundImage = (Bitmap)(Properties.Resources.backpanelstats1);
                p_stats[convert_pos[0]].Visible = true;

                /// p2 stats
                p_stats[convert_pos[1]].Location = new Point(p_card[convert_pos[1], 3].Location.X + p_card[convert_pos[1], 3].Width + 10, p_card[convert_pos[1], 3].Location.Y + 32);
                p_stats[convert_pos[1]].BackgroundImage = (Bitmap)(Properties.Resources.backpanelstats2);
                p_stats[convert_pos[1]].Visible = true;
            }
            else if (session_w.nbr_player == 4)
            {
                for (int cnt = 0; cnt < session_w.nbr_player; cnt++)
                {
                    if (convert_pos[cnt] < 2)
                    {
                        p_stats[convert_pos[cnt]].Location = new Point(p_card[convert_pos[cnt], 3].Location.X + p_card[convert_pos[cnt], 3].Width + 10, p_card[convert_pos[cnt], 3].Location.Y + 32);
                        p_stats[convert_pos[cnt]].BackgroundImage = (Bitmap)(Properties.Resources.backpanelstats1);
                        p_stats[convert_pos[cnt]].Visible = true;
                    }
                    else
                    {
                        p_stats[convert_pos[cnt]].Location = new Point(p_card[convert_pos[cnt], 3].Location.X + 10, p_card[convert_pos[cnt], 3].Location.Y + p_card[convert_pos[cnt], 3].Height + 32);
                        p_stats[convert_pos[cnt]].BackgroundImage = (Bitmap)(Properties.Resources.backpanelstats2);
                        p_stats[convert_pos[cnt]].Visible = true;
                    }
                }


                /*/// p1 stats
                p_stats[convert_pos[0]].Location = new Point(p_card[convert_pos[0], 3].Location.X + p_card[convert_pos[0], 3].Width + 10, p_card[convert_pos[0], 3].Location.Y + 32);
                p_stats[convert_pos[0]].BackgroundImage = (Bitmap)(Properties.Resources.backpanelstats1);
                p_stats[convert_pos[0]].Visible = true;*/

                /// p2 stats
                /*p_stats[convert_pos[1]].Location = new Point(p_card[convert_pos[1], 3].Location.X + p_card[convert_pos[1], 3].Width + 10, p_card[convert_pos[1], 3].Location.Y + 32);
                p_stats[convert_pos[1]].BackgroundImage = (Bitmap)(Properties.Resources.backpanelstats1);
                p_stats[convert_pos[1]].Visible = true;*/

                /*/// p3 stats
                p_stats[convert_pos[2]].Location = new Point(p_card[convert_pos[2], 0].Location.X + 50, p_card[convert_pos[2], 3].Location.Y + p_card[convert_pos[2], 3].Height + 10);
                p_stats[convert_pos[2]].BackgroundImage = (Bitmap)(Properties.Resources.backpanelstats2);
                p_stats[convert_pos[2]].Visible = true;

                /// p4 stats
                p_stats[convert_pos[3]].Location = new Point(p_card[convert_pos[3], 0].Location.X + 10, p_card[convert_pos[3], 3].Location.Y + p_card[convert_pos[3], 3].Height + 10);
                p_stats[convert_pos[3]].BackgroundImage = (Bitmap)(Properties.Resources.backpanelstats2);
                p_stats[convert_pos[3]].Visible = true;*/
            }
        }

        ///////// evenement MOUSEOUT
        private void p_card_MouseLeave(object sender, EventArgs e)
        {
            block_pcard = 0;
            PictureBox tmp = (PictureBox)sender;
            tmp.Size = new Size(100, 170);
            tmp.Location = new Point(tmp.Location.X, this.Height - 278);
        }

        ///////// evenement MOUSEOVER
        private void p_card_MouseEnter(object sender, EventArgs e)
        {
            if (v_la_main == 0 && block_me == false)
                if (block_pcard < 30)
                {
                    PictureBox tmp = (PictureBox)sender;
                    tmp.Location = new Point(tmp.Location.X, tmp.Location.Y - 45);
                    tmp.Size = new Size(130, 220);
                    block_pcard++;
                }
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
                    p_card[0, cnt].Location = new Point(p_card[0, cnt].Location.X, this.Height - 278);
                }

                // Si la condition est correcte
                if (IsConnected())
                {
                    header();
                    if (client1.IsBusy == false)
                    {
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?take=" + p[0].cards[pic_number] + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        block_me = true;
                    }
                    else if (client2.IsBusy == false)
                    {
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?take=" + p[0].cards[pic_number] + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        block_me = true;
                    }
                    else if (client3.IsBusy == false)
                    {
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/mind.php?take=" + p[0].cards[pic_number] + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        block_me = true;
                    }
                    else
                    {
                        evenement_area.Text = evenement_area.Text + "\n" + @"ERREUR\>Requette non éffectué,E2";
                        //MessageBox.Show("ERROR 3");
                        //noWebClientfree();
                    }
                    //block_me = true;

                    if (Int16.Parse(p[0].cards[pic_number].Split('-')[0]) != session_w.extra4)
                    {
                        chrono.Enabled = false;
                        chrono_lb.Visible = false;
                    }
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        ///////// ADD CARDS
        private void add_cards(int player, int nbr)
        {/*
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
                                p_card[player, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                            else if (player == 2)
                            {
                                p_card[player, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
                                p_card[player, cnt2].Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                            }
                            else
                            {
                                p_card[player, cnt2].Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(v_dos_card));
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
                MessageBox.Show(this, "Aucune carte trouvé !!!\n reporter le bug SVP (Erreur 31)", "Erreur interne", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
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
            for (int cnt = 1; cnt < session_w.nbr_player; cnt++)
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
                milieu[0].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");
                for (int cnt = 0; p[0].cards[cnt] != null; cnt++)
                    p_card[0, cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[0].cards[cnt] + ".jpg");
            }
            else
            {
                milieu[0].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + v_milieu + ".jpg");
                for (int cnt = 0; p[0].cards[cnt] != null; cnt++)
                    p_card[0, cnt].Image = (Bitmap)Image.FromFile(@"img\cartes\" + v_front_card + @"\" + p[0].cards[cnt] + ".jpg");
            }
        }

        private void aProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutbox = new AboutBox1();
            aboutbox.Show();
        }

        private void rondaR_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            chat_checker.Enabled = false;
            t_tour.Enabled = false;
            block = true;
            if (session_w.playing == true)
            {
                if (IsConnected())
                {
                    header();
                    if (client1.IsBusy == false)
                    {
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?quite=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        block_me = true;
                    }
                    else if (client2.IsBusy == false)
                    {
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?quite=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        block_me = true;
                    }
                    else if (client3.IsBusy == false)
                    {
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?quite=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon));
                        block_me = true;
                    }
                    else
                    {
                        //MessageBox.Show("ERROR 8");
                        //noWebClientfree();
                    }
                    //block_me = true;
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            this.Dispose();
            GC.Collect();
        }

        // code si aucun wbclient n'est libre
        private void noWebClientfree()
        {
            MessageBox.Show(this, "Une erreur interne est survenu,votre connexion semble être saturé,\n merci de relencer l'application", "Erreur interne", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //this.Close();
        }

        private void chat_msg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (IsConnected())
                {
                    if (chat_msg.Text != "")
                    {
                        header();
                        string msg = chat_msg.Text;
                        msg = msg.Replace(":", ";");
                        msg = msg.Replace("/", "|");
                        msg = msg.Replace("#", "%");

                        if (client_chat_checker1.IsBusy == false)
                            client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_msg.Text + "&salon=" + session_w.salon));
                        else if (client_chat_checker2.IsBusy == false)
                            client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_msg.Text + "&salon=" + session_w.salon));

                        chat_msg.Text = "";
                    }
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void launch_partie_Click(object sender, EventArgs e)
        {
            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                {
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=3&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon + "&client=1"));
                    launch_partie.Enabled = false;
                }
                else if (client2.IsBusy == false)
                {
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=3&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon + "&client=1"));
                    launch_partie.Enabled = false;
                }
                else if (client3.IsBusy == false)
                {
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=3&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon + "&client=1"));
                    launch_partie.Enabled = false;
                }
                else
                {
                    //MessageBox.Show("ERROR 11");
                    //noWebClientfree();
                }
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void selfplay()
        {
            bool found = false;
            for (int cnt = 0; cnt < 4; cnt++)//it was 15 not 4
            {
                if (p[0].cards[cnt] != "null")
                {
                    if ((p[0].cards[cnt].Split('-')[0] == v_milieu[0].Split('-')[0] || p[0].cards[cnt].Split('-')[1] == v_milieu[0].Split('-')[1]) && Int16.Parse(p[0].cards[cnt].Split('-')[0]) != session_w.extra4)
                    {
                        p_card_Click(p_card[0, cnt], null);
                        found = true;
                        break;
                    }
                }
                else
                    break;
            }

            if (found == false)
                all_card_Click(null, null);
        }

        private void launch_partie_Click_1(object sender, EventArgs e)
        {
            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                {
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=3&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon + "&client=1"));
                    launch_partie.Enabled = false;
                }
                else if (client2.IsBusy == false)
                {
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=3&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon + "&client=1"));
                    launch_partie.Enabled = false;
                }
                else if (client3.IsBusy == false)
                {
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=3&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + session_w.salon + "&client=1"));
                    launch_partie.Enabled = false;
                }
                else
                {
                    //MessageBox.Show("ERROR 11");
                    //noWebClientfree();
                }
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rand_pos_cards_middle()
        {
            // cadre
            int[] pos_x = { 400, this.Width - 240 };
            int[] pos_y = { 200, panel1.Location.Y - 300 };
            Random rnd_m = new Random();

            for (int cnt = 0; cnt < 10; cnt++)
                milieu[cnt].Location = new Point(rnd_m.Next(pos_x[0], pos_x[1]), rnd_m.Next(pos_y[0], pos_y[1]));
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            rand_pos_cards_middle();
        }

        private void dispersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rand_pos_cards_middle();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            toolStripMenuItem7.Checked = true;
            toolStripMenuItem8.Checked = false;
            toolStripMenuItem9.Checked = false;
            v_dos_card = "dos_1";
            if (load_pro == true)
            {
                load_profileR.dos = 1;
                profile_save();
            }
            change_back_card();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            toolStripMenuItem7.Checked = false;
            toolStripMenuItem8.Checked = true;
            toolStripMenuItem9.Checked = false;
            v_dos_card = "dos_2";
            if (load_pro == true)
            {
                load_profileR.dos = 2;
                profile_save();
            }
            change_back_card();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            toolStripMenuItem7.Checked = false;
            toolStripMenuItem8.Checked = false;
            toolStripMenuItem9.Checked = true;
            v_dos_card = "dos_3";
            if (load_pro == true)
            {
                load_profileR.dos = 3;
                profile_save();
            }
            change_back_card();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            v_front_card = "tamisé";
            toolStripMenuItem11.Checked = true;
            toolStripMenuItem12.Checked = false;
            change_front_card("tamisé");
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            v_front_card = "éclairé";
            toolStripMenuItem12.Checked = true;
            toolStripMenuItem11.Checked = false;
            change_front_card("éclairé");
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\1.jpg");
            toolStripMenuItem14.Checked = true;
            toolStripMenuItem15.Checked = false;
            toolStripMenuItem16.Checked = false;
            toolStripMenuItem17.Checked = false;
            toolStripMenuItem18.Checked = false;
            if (load_pro == true)
            {
                load_profileR.tapis = 1;
                profile_save();
            }
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\2.jpg");
            toolStripMenuItem15.Checked = true;
            toolStripMenuItem14.Checked = false;
            toolStripMenuItem16.Checked = false;
            toolStripMenuItem17.Checked = false;
            toolStripMenuItem18.Checked = false;
            if (load_pro == true)
            {
                load_profileR.tapis = 2;
                profile_save();
            }
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\3.jpg");
            toolStripMenuItem16.Checked = true;
            toolStripMenuItem14.Checked = false;
            toolStripMenuItem15.Checked = false;
            toolStripMenuItem17.Checked = false;
            toolStripMenuItem18.Checked = false;
            if (load_pro == true)
            {
                load_profileR.tapis = 3;
                profile_save();
            }
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\4.jpg");
            toolStripMenuItem17.Checked = true;
            toolStripMenuItem14.Checked = false;
            toolStripMenuItem15.Checked = false;
            toolStripMenuItem16.Checked = false;
            toolStripMenuItem17.Checked = false;
            if (load_pro == true)
            {
                load_profileR.tapis = 4;
                profile_save();
            }
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            backimg.Image = (Bitmap)Image.FromFile(@"img\tapis\5.jpg");
            toolStripMenuItem18.Checked = true;
            toolStripMenuItem14.Checked = false;
            toolStripMenuItem15.Checked = false;
            toolStripMenuItem16.Checked = false;
            toolStripMenuItem17.Checked = false;
            if (load_pro == true)
            {
                load_profileR.tapis = 5;
                profile_save();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutbox = new AboutBox1();
            aboutbox.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (load_profileR.son == 1)
            {
                toolStripButton1.Image = Properties.Resources.volumeKo;

                load_profileR.son = 0;
                profile_save();
            }
            else
            {
                toolStripButton1.Image = Properties.Resources.volumeOk;

                load_profileR.son = 1;
                profile_save();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            toolStripProgressBar2.Increment(-10);
            toolStripLabel1.Text = toolStripProgressBar2.Value + " %";
            this.Opacity = double.Parse(toolStripProgressBar2.Value.ToString()) / 100;
            toolStripButton4.Enabled = true;
            if (toolStripProgressBar2.Value == 30)
                toolStripButton3.Enabled = false;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            toolStripProgressBar2.Increment(10);
            toolStripLabel1.Text = toolStripProgressBar2.Value + " %";
            this.Opacity = double.Parse(toolStripProgressBar2.Value.ToString()) / 100;
            toolStripButton3.Enabled = true;
            if (toolStripProgressBar2.Value == 100)
                toolStripButton4.Enabled = false;
        }

        private void style1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            styled_draw(1);
        }

        private void styled_draw(int s)
        {
            if (s == 1)
            {
                p1_bnt_style.Image = (Bitmap)Image.FromFile(@"img\pieces_rnd\style1\1.gif");
                p1_kha_style.Image = (Bitmap)Image.FromFile(@"img\pieces_rnd\style1\5.gif");

                p2_bnt_style.Image = (Bitmap)Image.FromFile(@"img\pieces_rnd\style1\1.gif");
                p2_kha_style.Image = (Bitmap)Image.FromFile(@"img\pieces_rnd\style1\5.gif");

                p3_bnt_style.Image = (Bitmap)Image.FromFile(@"img\pieces_rnd\style1\1.gif");
                p3_kha_style.Image = (Bitmap)Image.FromFile(@"img\pieces_rnd\style1\5.gif");

                p4_bnt_style.Image = (Bitmap)Image.FromFile(@"img\pieces_rnd\style1\1.gif");
                p4_kha_style.Image = (Bitmap)Image.FromFile(@"img\pieces_rnd\style1\5.gif");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            p1_bnt_won.Text = "0";
            p2_bnt_won.Text = "pop";
            //p1_bnt_won.Text = "a";
            //p_pts_won[convert_pos[0]].Text = "a";
        }

        private void button2_Click(object sender, EventArgs e)
        {
           //p2_bnt_won.Text = "b";
            p_pts_won[0].Text = "b";
        }
    }
}
