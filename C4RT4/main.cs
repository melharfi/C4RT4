using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Net.Cache;
using System.Threading;
using System.IO;
using System.Xml;

namespace C4RT4
{
    public partial class main : Form
    {
        internal static main _main = null;
        private System.Media.SoundPlayer backsound = new System.Media.SoundPlayer();
        private WebClient client1 = new WebClient(), client2 = new WebClient(), client3 = new WebClient();
        private WebClient client_syn1= new WebClient(), client_syn2=new WebClient();
        private WebClient client_users_checker1 = new WebClient(), client_users_checker2=new WebClient();
        private WebClient client_chat_checker1 = new WebClient(), client_chat_checker2 = new WebClient();
        private WebClient avatar_wc1 = new WebClient(), avatar_wc2 = new WebClient(), avatar_wc3 = new WebClient(), avatar_wc4 = new WebClient();
        private session_wan session_w = new session_wan();
        private _playerR[] p = new _playerR[4];
        internal string hosted_server;
        internal static int option_loaded = 0;
        internal static int speed_loaded = 0;
        private int speed_cnt = 0;
        private bool lanched_game = false;

        internal main()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (br1.Checked == true)
            {
                switch (gameListe.SelectedIndex)
                {
                    case -1:
                        MessageBox.Show("Veillez choisir un jeux");
                        break;
                    case 0:
                        C4RT4.zriwita _zriwita = new C4RT4.zriwita();
                        _main = this;
                        this.Hide();
                        stopBackSound();
                        _zriwita.Show();
                        break;
                }
            }
            else
            {
                /*switch (gameListe.SelectedIndex)
                {
                    case -1:
                        MessageBox.Show("Veillez choisir un jeux");
                        break;
                    case 0:
                        C4RT4.zriwitaR _zriwitaR = new C4RT4.zriwitaR(null);
                        _main = this;
                        this.Hide();
                        stopBackSound();
                        _zriwitaR.Show();
                        break;
                }*/
            }
        }

        private void main_Load(object sender, EventArgs e)
        {
            // initialisation de session
            session_w.logged = false;
            client1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client1_DownloadStringCompleted);
            client2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client2_DownloadStringCompleted);
            client3.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client3_DownloadStringCompleted);
            client_syn1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_syn1_DownloadStringCompleted);
            client_syn2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_syn2_DownloadStringCompleted);
            client_users_checker1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_users_checker1_DownloadStringCompleted);
            client_users_checker2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_users_checker2_DownloadStringCompleted);
            client_chat_checker1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_chat_checker1_DownloadStringCompleted);
            client_chat_checker2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_chat_checker2_DownloadStringCompleted);
            backsound.SoundLocation = @"son\Grease_Monkey.wav";
            //hosted_server = "http://127.0.0.1";// "http://c4rt4-v2.freehostia.com/app";
            
            if(bs_cb.Checked==true)
                playBackSound();

            gameListe.SelectedIndex = 0;

            this.BringToFront();
            global_load();
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

                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            if (session_w.index_chat < index_chat)
                            {
                                for (int cnt = session_w.index_chat; cnt < all_data.Length; cnt++)
                                {
                                    string[] all_msg = all_data[cnt].Split(':');

                                    if (session_w.index_chat < Int16.Parse(all_msg[0]))
                                    {
                                        session_w.index_chat = Int16.Parse(all_msg[0]);

                                        if (br3.Checked)
                                        {
                                            if (chat_area_server_z.Text == "")
                                                chat_area_server_z.Text = all_msg[1] + ">dit: " + all_msg[2];
                                            else
                                                chat_area_server_z.Text = chat_area_server_z.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];

                                            chat_area_server_z.SelectionStart = chat_area_server_z.Text.Length;
                                            chat_area_server_z.ScrollToCaret();
                                            chat_area_server_z.Refresh();
                                        }
                                        else
                                        {
                                            if (chat_area_client_z.Text == "")
                                                chat_area_client_z.Text = all_msg[1] + ">dit: " + all_msg[2];
                                            else
                                                chat_area_client_z.Text = chat_area_client_z.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];

                                            chat_area_client_z.SelectionStart = chat_area_client_z.Text.Length;
                                            chat_area_client_z.ScrollToCaret();
                                            chat_area_client_z.Refresh();
                                        }
                                    }
                                    else
                                        break;
                                }
                            }

                            if (br3.Checked)
                                etat_host_lb_z.Text = data[5];
                            else
                            {
                                etat_client_lb_z.Text = data[5];
                                if (data[5] == "Initialisation")
                                {
                                    step1_client_zriwita();
                                }
                            }
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            if (session_w.index_chat < index_chat)
                            {
                                for (int cnt = session_w.index_chat; cnt < all_data.Length; cnt++)
                                {
                                    string[] all_msg = all_data[cnt].Split(':');

                                    if (session_w.index_chat < Int16.Parse(all_msg[0]))
                                    {
                                        session_w.index_chat = Int16.Parse(all_msg[0]);

                                        if (br3.Checked)
                                        {
                                            if (chat_area_server_r.Text == "")
                                                chat_area_server_r.Text = all_msg[1] + ">dit: " + all_msg[2];
                                            else
                                                chat_area_server_r.Text = chat_area_server_r.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];

                                            chat_area_server_r.SelectionStart = chat_area_server_r.Text.Length;
                                            chat_area_server_r.ScrollToCaret();
                                            chat_area_server_r.Refresh();
                                        }
                                        else
                                        {
                                            if (chat_area_client_r.Text == "")
                                                chat_area_client_r.Text = all_msg[1] + ">dit: " + all_msg[2];
                                            else
                                                chat_area_client_r.Text = chat_area_client_r.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];

                                            chat_area_client_r.SelectionStart = chat_area_client_r.Text.Length;
                                            chat_area_client_r.ScrollToCaret();
                                            chat_area_client_r.Refresh();
                                        }
                                    }
                                    else
                                        break;
                                }
                            }

                            if (br3.Checked)
                                etat_host_lb_r.Text = data[5];
                            else
                            {
                                etat_client_lb_r.Text = data[5];
                                if (data[5] == "Initialisation")
                                {
                                    step1_client_ronda();
                                }
                            }
                        }
                    }
                    else if (data[2] == "nothing")
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            if (br3.Checked)
                                etat_host_lb_z.Text = data[3];
                            else
                            {
                                etat_client_lb_z.Text = data[3];
                                if (data[3] == "Initialisation")
                                    step1_client_zriwita();
                            }
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            if (br3.Checked)
                                etat_host_lb_r.Text = data[3];
                            else
                            {
                                etat_client_lb_r.Text = data[3];
                                if (data[3] == "Initialisation")
                                    step1_client_ronda();
                            }
                        }
                    }
                }
                else if (data[1] == "droped")
                {
                    if(gameListe.SelectedItem.ToString()=="ZRIWITA")
                        destroyClientPartie_zriwita();
                    else if(gameListe.SelectedItem.ToString()=="RONDA")
                        destroyClientPartie_ronda();
                    MessageBox.Show(this, "Vous n'avez pas été séléctionné, merci de choisir une autre table", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (data[1] == "error")
                {
                    if (data[2] == "party not found" && br4.Checked == true)
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            panel_RP_z.Visible = false;
                            rejoindre_btn_z.Text = "S'assoir";
                            nom_tbl_RP_z.Enabled = true;
                            nom_tbl_RP_z.Text = "";
                            chat_checker.Enabled = false;
                            users_checker.Enabled = false;

                            MessageBox.Show(this, "La table viens d'être annulé\nMerci de choisir une autre", "Table annulé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (br3.Checked == true)
                                destroyHostPartie_zriwita();
                            else
                                destroyClientPartie_zriwita();
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            panel_RP_r.Visible = false;
                            rejoindre_btn_r.Text = "S'assoir";
                            nom_tbl_RP_r.Enabled = true;
                            nom_tbl_RP_r.Text = "";
                            chat_checker.Enabled = false;
                            users_checker.Enabled = false;

                            MessageBox.Show(this, "La table viens d'être annulé\nMerci de choisir une autre", "Table annulé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (br3.Checked == true)
                                destroyHostPartie_ronda();
                            else
                                destroyClientPartie_ronda();
                        }
                    }
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

                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            if (session_w.index_chat < index_chat)
                            {
                                for (int cnt = session_w.index_chat; cnt < all_data.Length; cnt++)
                                {
                                    string[] all_msg = all_data[cnt].Split(':');

                                    if (session_w.index_chat < Int16.Parse(all_msg[0]))
                                    {
                                        session_w.index_chat = Int16.Parse(all_msg[0]);

                                        if (br3.Checked)
                                        {
                                            if (chat_area_server_z.Text == "")
                                                chat_area_server_z.Text = all_msg[1] + ">dit: " + all_msg[2];
                                            else
                                                chat_area_server_z.Text = chat_area_server_z.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];

                                            chat_area_server_z.SelectionStart = chat_area_server_z.Text.Length;
                                            chat_area_server_z.ScrollToCaret();
                                            chat_area_server_z.Refresh();
                                        }
                                        else
                                        {
                                            if (chat_area_client_z.Text == "")
                                                chat_area_client_z.Text = all_msg[1] + ">dit: " + all_msg[2];
                                            else
                                                chat_area_client_z.Text = chat_area_client_z.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];

                                            chat_area_client_z.SelectionStart = chat_area_client_z.Text.Length;
                                            chat_area_client_z.ScrollToCaret();
                                            chat_area_client_z.Refresh();
                                        }
                                    }
                                    else
                                        break;
                                }
                            }

                            if (br3.Checked)
                                etat_host_lb_z.Text = data[5];
                            else
                            {
                                etat_client_lb_z.Text = data[5];
                                if (data[5] == "Initialisation")
                                {
                                    step1_client_zriwita();
                                }
                            }
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            if (session_w.index_chat < index_chat)
                            {
                                for (int cnt = session_w.index_chat; cnt < all_data.Length; cnt++)
                                {
                                    string[] all_msg = all_data[cnt].Split(':');

                                    if (session_w.index_chat < Int16.Parse(all_msg[0]))
                                    {
                                        session_w.index_chat = Int16.Parse(all_msg[0]);

                                        if (br3.Checked)
                                        {
                                            if (chat_area_server_r.Text == "")
                                                chat_area_server_r.Text = all_msg[1] + ">dit: " + all_msg[2];
                                            else
                                                chat_area_server_r.Text = chat_area_server_r.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];

                                            chat_area_server_r.SelectionStart = chat_area_server_r.Text.Length;
                                            chat_area_server_r.ScrollToCaret();
                                            chat_area_server_r.Refresh();
                                        }
                                        else
                                        {
                                            if (chat_area_client_r.Text == "")
                                                chat_area_client_r.Text = all_msg[1] + ">dit: " + all_msg[2];
                                            else
                                                chat_area_client_r.Text = chat_area_client_r.Text + "\n" + all_msg[1] + ">dit: " + all_msg[2];

                                            chat_area_client_r.SelectionStart = chat_area_client_r.Text.Length;
                                            chat_area_client_r.ScrollToCaret();
                                            chat_area_client_r.Refresh();
                                        }
                                    }
                                    else
                                        break;
                                }
                            }

                            if (br3.Checked)
                                etat_host_lb_r.Text = data[5];
                            else
                            {
                                etat_client_lb_r.Text = data[5];
                                if (data[5] == "Initialisation")
                                {
                                    step1_client_ronda();
                                }
                            }
                        }
                    }
                    else if (data[2] == "nothing")
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            if (br3.Checked)
                                etat_host_lb_z.Text = data[3];
                            else
                            {
                                etat_client_lb_z.Text = data[3];
                                if (data[3] == "Initialisation")
                                    step1_client_zriwita();
                            }
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            if (br3.Checked)
                                etat_host_lb_r.Text = data[3];
                            else
                            {
                                etat_client_lb_r.Text = data[3];
                                if (data[3] == "Initialisation")
                                    step1_client_ronda();
                            }
                        }
                    }
                }
                else if (data[1] == "droped")
                {
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        destroyClientPartie_zriwita();
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                        destroyClientPartie_ronda();
                    MessageBox.Show(this, "Vous n'avez pas été séléctionné, merci de choisir une autre table", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (data[1] == "error")
                {
                    if (data[2] == "party not found" && br4.Checked == true)
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            panel_RP_z.Visible = false;
                            rejoindre_btn_z.Text = "S'assoir";
                            nom_tbl_RP_z.Enabled = true;
                            nom_tbl_RP_z.Text = "";
                            chat_checker.Enabled = false;
                            users_checker.Enabled = false;

                            MessageBox.Show(this, "La table viens d'être annulé\nMerci de choisir une autre", "Table annulé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (br3.Checked == true)
                                destroyHostPartie_zriwita();
                            else
                                destroyClientPartie_zriwita();
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            panel_RP_r.Visible = false;
                            rejoindre_btn_r.Text = "S'assoir";
                            nom_tbl_RP_r.Enabled = true;
                            nom_tbl_RP_r.Text = "";
                            chat_checker.Enabled = false;
                            users_checker.Enabled = false;

                            MessageBox.Show(this, "La table viens d'être annulé\nMerci de choisir une autre", "Table annulé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (br3.Checked == true)
                                destroyHostPartie_ronda();
                            else
                                destroyClientPartie_ronda();
                        }
                    }
                }
            }
        }

        void client_users_checker1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
                if (data[1] == "client")
                    if (data[2] == "list users")
                    {
                        if (br3.Checked == true)
                            updateClientIPList(data[3]);
                        else
                            updateClientRPList(data[3]);
                    }
        }

        void client_users_checker2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
                if (data[1] == "client")
                    if (data[2] == "list users")
                    {
                        if (br3.Checked == true)
                            updateClientIPList(data[3]);
                        else
                            updateClientRPList(data[3]);
                    }
        }

        void client1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
            {
                if (data[1] == "party")
                {
                    if (data[2] == "done")
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            session_w.salon = salon_host_z.SelectedItem.ToString();
                            session_w.nom_table = data[3];
                            session_w.index_chat = 0;
                            session_w.p_pos = 0;
                            nom_table_z.Text = session_w.nom_table;
                            creer_tbl_btn_z.Enabled = false;
                            annuler_tbl_btn_z.Enabled = true;
                            lancer_partie_btn_z.Enabled = true;
                            chrono_z.Enabled = false;
                            br4.Enabled = false;
                            br1.Enabled = false;
                            salon_host_z.Enabled = false;
                            sonor_Host_z.Enabled = false;
                            session_w.extra1 = Int16.Parse(extra1HP_z.SelectedItem.ToString());
                            session_w.extra2 = Int16.Parse(extra2HP_z.SelectedItem.ToString());
                            session_w.extra3 = Int16.Parse(extra3HP_z.SelectedItem.ToString());
                            session_w.extra4 = Int16.Parse(extra4HP_z.SelectedItem.ToString());

                            extra1IP_z.Text = session_w.extra1.ToString();
                            extra2IP_z.Text = session_w.extra2.ToString();
                            extra3IP_z.Text = session_w.extra3.ToString();
                            extra4IP_z.Text = session_w.extra4.ToString();

                            nbr_player_IP_z.Text = session_w.nbr_player.ToString();

                            if (session_w.vip_zriwita == 1)
                            {
                                vip_boxIP_z.Checked = true;
                                vip_boxIP_z.Visible = true;
                            }
                            else
                                vip_boxIP_z.Checked = false;

                            vip_box_z.Enabled = false;

                            panel_IP_z.Visible = true;
                            PChatServer_z.Visible = true;
                            ////////////////////////////////

                            //// verouillage des controles
                            vip_box_z.Enabled = false;
                            p2HP_z.Enabled = false;
                            p3HP_z.Enabled = false;
                            p4HP_z.Enabled = false;
                            extra1HP_z.Enabled = false;
                            extra2HP_z.Enabled = false;
                            extra3HP_z.Enabled = false;
                            extra4HP_z.Enabled = false;
                            lock_partie_btn_z.Enabled = true;
                            P1IP_z.Text = session_w.user + " {" + session_w.nom + "}";

                            if (session_w.nbr_player == 2)
                                P2IP_z.Enabled = true;
                            else if (session_w.nbr_player == 3)
                            {
                                P2IP_z.Enabled = true;
                                P3IP_z.Enabled = true;
                            }
                            else if (session_w.nbr_player == 4)
                            {
                                P2IP_z.Enabled = true;
                                P3IP_z.Enabled = true;
                                P4IP_z.Enabled = true;
                            }

                            P2IP_z.Items.Clear();
                            P3IP_z.Items.Clear();
                            P4IP_z.Items.Clear();

                            P2IP_z.Items.AddRange(new object[] { "(aucun)" });
                            P3IP_z.Items.AddRange(new object[] { "(aucun)" });
                            P4IP_z.Items.AddRange(new object[] { "(aucun)" });

                            P2IP_z.SelectedIndex = 0;
                            P3IP_z.SelectedIndex = 0;
                            P4IP_z.SelectedIndex = 0;

                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;

                            loading_panel.Visible = false;
                            loading_panel.Visible = false;
                            partie_mode_host_zriwita.Visible = true;
                            partie_mode_host_zriwita.SelectTab(1);
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            session_w.salon = salon_host_r.SelectedItem.ToString();
                            session_w.nom_table = data[3];
                            session_w.index_chat = 0;
                            session_w.p_pos = 0;
                            nom_table_r.Text = session_w.nom_table;
                            creer_tbl_btn_r.Enabled = false;
                            annuler_tbl_btn_r.Enabled = true;
                            lancer_partie_btn_r.Enabled = true;
                            chrono_r.Enabled = false;
                            br4.Enabled = false;
                            br1.Enabled = false;
                            salon_host_r.Enabled = false;
                            sonor_Host_r.Enabled = false;

                            nbr_player_IP_r.Text = session_w.nbr_player.ToString();

                            if (session_w.vip_ronda == 1)
                            {
                                vip_boxIP_r.Checked = true;
                                vip_boxIP_r.Visible = true;
                            }
                            else
                                vip_boxIP_r.Checked = false;

                            vip_box_r.Enabled = false;

                            panel_IP_r.Visible = true;
                            PChatServer_r.Visible = true;
                            ////////////////////////////////

                            //// verouillage des controles
                            vip_box_r.Enabled = false;
                            p2HP_r.Enabled = false;
                            p4HP_r.Enabled = false;
                            lock_partie_btn_r.Enabled = true;
                            P1IP_r.Text = session_w.user + " {" + session_w.nom + "}";

                            if (session_w.nbr_player == 2)
                                P2IP_r.Enabled = true;
                            else if (session_w.nbr_player == 4)
                            {
                                P2IP_r.Enabled = true;
                                P3IP_r.Enabled = true;
                                P4IP_r.Enabled = true;
                            }

                            P2IP_r.Items.Clear();
                            P3IP_r.Items.Clear();
                            P4IP_r.Items.Clear();

                            P2IP_r.Items.AddRange(new object[] { "(aucun)" });
                            P3IP_r.Items.AddRange(new object[] { "(aucun)" });
                            P4IP_r.Items.AddRange(new object[] { "(aucun)" });

                            P2IP_r.SelectedIndex = 0;
                            P3IP_r.SelectedIndex = 0;
                            P4IP_r.SelectedIndex = 0;

                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;

                            loading_panel.Visible = false;
                            loading_panel.Visible = false;
                            partie_mode_host_ronda.Visible = true;
                            partie_mode_host_ronda.SelectTab(1);
                        }
                    }
                    else if (data[2] == "destroyed")
                    {
                        if(gameListe.SelectedItem.ToString()=="ZRIWITA")
                            destroyHostPartie_zriwita();
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                            destroyHostPartie_ronda();
                    }
                    else if (data[2] == "etat")
                    {
                        if (data[3] == "busy")
                        {
                            /*MessageBox.Show("done");
                            step3();*/
                        }
                        else if (data[3] == "encapsulate")
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                step3_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                step3_ronda();
                        }
                    }
                    else if (data[2] == "not enough vip")
                    {
                        loading_panel.Visible = false;
                        MessageBox.Show(this, "Désolé, mais vous n'avez pas assez de points VIP.\nPensez a recharger votre compte s'il le faut.", "Points VIP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (br3.Checked)
                        {
                            if(gameListe.SelectedItem.ToString()=="ZRIWITA")
                                destroyHostPartie_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                destroyHostPartie_ronda();
                            partie_mode_host_zriwita.Visible = true;
                        }
                        else
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyClientPartie_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                destroyClientPartie_ronda();
                            partie_mode_client_zriwita.Visible = true;
                        }
                    }
                }
                else if (data[1] == "client")
                {
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (data[2] == "disconnected")
                        {
                            // traitement pour la deconnexion
                            panel_RP_z.Visible = false;
                            rejoindre_btn_z.Text = "S'assoir";
                            nom_tbl_RP_z.Enabled = true;
                            nom_tbl_RP_z.Text = "";
                            destroyClientPartie_zriwita();
                        }
                        else if (data[2] == "added")
                        {
                            // traitement lorsque le joueurs a été ajouté.
                            br3.Enabled = false;
                            session_w.salon = salon_client_z.SelectedItem.ToString();
                            session_w.nom_table = data[5];
                            session_w.nbr_player = Int16.Parse(data[6]);
                            session_w.index_chat = Int16.Parse(data[7]);
                            session_w.chrono = Int16.Parse(data[8]);
                            if (session_w.chrono == 1)
                                chrono_img_z.Visible = true;
                            else
                                chrono_img_z.Visible = false;
                            // ajouter un controle si la version est vip, on lui attribue 0 pour avoir tt l'historque de chat.
                            //session_w.index_chat = 0;
                            nom_tbl_RP_z.Text = session_w.nom_table;
                            nom_tbl_RP_z.Enabled = false;
                            salon_client_z.Enabled = false;
                            sonor_Client_z.Enabled = false;
                            nbr_player_RP_z.Text = session_w.nbr_player.ToString();
                            rejoindre_btn_z.Text = "Quiter";
                            rejoindre_btn_z.Enabled = true;
                            panel_RP_z.Visible = true;

                            session_w.vip_zriwita = Int16.Parse(data[4]);
                            if (session_w.vip_zriwita == 0)
                                vipRP_z.Visible = false;
                            string[] data_extra = data[3].Split('-');
                            session_w.extra1 = Int16.Parse(data_extra[0]);
                            session_w.extra2 = Int16.Parse(data_extra[1]);
                            session_w.extra3 = Int16.Parse(data_extra[2]);
                            session_w.extra4 = Int16.Parse(data_extra[3]);

                            // affichage des données dans RP
                            extra1RP_z.Text = session_w.extra1.ToString();
                            extra2RP_z.Text = session_w.extra2.ToString();
                            extra3RP_z.Text = session_w.extra3.ToString();
                            extra4RP_z.Text = session_w.extra4.ToString();

                            if (session_w.vip_zriwita == 1)
                                vipRP_z.Checked = true;
                            else
                                vipRP_z.Checked = false;

                            // envoie d'une requette pour avoir les stats des joueurs TIMER
                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;
                        }
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (data[2] == "disconnected")
                        {
                            // traitement pour la deconnexion
                            panel_RP_r.Visible = false;
                            rejoindre_btn_r.Text = "S'assoir";
                            nom_tbl_RP_r.Enabled = true;
                            nom_tbl_RP_r.Text = "";
                            destroyClientPartie_ronda();
                        }
                        else if (data[2] == "added")
                        {
                            // traitement lorsque le joueurs a été ajouté.
                            br3.Enabled = false;
                            session_w.salon = salon_client_r.SelectedItem.ToString();
                            session_w.nom_table = data[5];
                            session_w.nbr_player = Int16.Parse(data[6]);
                            session_w.index_chat = Int16.Parse(data[7]);
                            session_w.chrono = Int16.Parse(data[8]);
                            if (session_w.chrono == 1)
                                chrono_img_r.Visible = true;
                            else
                                chrono_img_r.Visible = false;
                            // ajouter un controle si la version est vip, on lui attribue 0 pour avoir tt l'historque de chat.
                            //session_w.index_chat = 0;
                            nom_tbl_RP_r.Text = session_w.nom_table;
                            nom_tbl_RP_r.Enabled = false;
                            salon_client_r.Enabled = false;
                            sonor_Client_r.Enabled = false;
                            nbr_player_RP_r.Text = session_w.nbr_player.ToString();
                            rejoindre_btn_r.Text = "Quiter";
                            rejoindre_btn_r.Enabled = true;
                            panel_RP_r.Visible = true;

                            session_w.vip_ronda = Int16.Parse(data[4]);
                            if (session_w.vip_ronda == 0)
                                vipRP_r.Visible = false;

                            if (session_w.vip_ronda == 1)
                                vipRP_r.Checked = true;
                            else
                                vipRP_r.Checked = false;

                            // envoie d'une requette pour avoir les stats des joueurs TIMER
                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;
                        }
                    }
                    
                }
                else if (data[1] == "host party")
                {
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (data[2] == "locked")
                            lock_partie_btn_z.Text = "Déverrouiller la partie";
                        else if (data[2] == "unlocked")
                            lock_partie_btn_z.Text = "Vérrouiller la partie";
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (data[2] == "locked")
                            lock_partie_btn_r.Text = "Déverrouiller la partie";
                        else if (data[2] == "unlocked")
                            lock_partie_btn_r.Text = "Vérrouiller la partie";
                    }
                }
                else if (data[1] == "client party")
                {
                    if (data[2] == "locked")
                        MessageBox.Show("La table est vérrouillé", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "not found")
                        MessageBox.Show("La table n'existe pas", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "full")
                        MessageBox.Show(this, "Table pleine, merci de choisir une autre", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "destroyed")
                    {
                        users_checker.Enabled = false;

                        if (br3.Checked == true)
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyHostPartie_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                destroyHostPartie_ronda();
                            MessageBox.Show(this, "Table viens d’être supprimé, certainement à cause d’un très nombre d’attente\nVérifié votre connexion svp.", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyClientPartie_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                destroyClientPartie_ronda();
                            MessageBox.Show(this, "Table viens d'être supprimé, merci de choisir une autre table", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else if (data[1] == "get_players_info")
                {
                    int inc = 0;
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (br3.Checked)
                        {
                            p[0] = new _playerR();
                            p[0].nom = session_w.nom;
                            p[0].user = session_w.user;
                            p[0].points = session_w.points_z;
                            p[0].points_vip_zriwta = session_w.points_vip_zriwita;
                            p[0].avatar = session_w.avatar;
                            // avatar

                            p[0].won = session_w.won_z;
                            p[0].lose = session_w.lose_z;
                            p[0].profile = sonor_Host_z.SelectedItem.ToString();
                            p[0].position = 0;
                            p[0].ville = session_w.ville;
                            p[0].team = session_w.team;
                            inc = 1;
                        }

                        // implementation des infos users
                        string[] players_info = data[2].Split('|');
                        for (int cnt = 0; cnt < players_info.Length; cnt++)
                        {
                            string[] data_player = players_info[cnt].Split(':');
                            p[cnt + inc] = new _playerR();
                            p[cnt + inc].nom = data_player[0];
                            p[cnt + inc].user = data_player[1];
                            p[cnt + inc].points = Int16.Parse(data_player[2]);
                            p[cnt + inc].points_vip_zriwta = Int16.Parse(data_player[3]);
                            p[cnt + inc].avatar = data_player[4];

                            // avatar
                            if (p[cnt + inc].avatar != "null")
                            {
                                if (avatar_wc1.IsBusy)
                                    avatar_wc2.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc2.IsBusy)
                                    avatar_wc3.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc3.IsBusy)
                                    avatar_wc2.DownloadFile(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                            }

                            p[cnt + inc].won = Int16.Parse(data_player[5].Split('/')[0]);
                            p[cnt + inc].lose = Int16.Parse(data_player[5].Split('/')[1]);
                            p[cnt + inc].profile = data_player[6];
                            p[cnt + inc].position = cnt + inc;
                            p[cnt + inc].ville = data_player[7];
                            p[cnt + inc].team = data_player[8];
                            if (session_w.user == data_player[1])
                                session_w.p_pos = cnt + inc;
                        }

                        if (br3.Checked)
                            step2_host_zriwita();
                        else
                            step3_zriwita();
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (br3.Checked)
                        {
                            p[0] = new _playerR();
                            p[0].nom = session_w.nom;
                            p[0].user = session_w.user;
                            p[0].points = session_w.points_r;
                            p[0].points_vip_ronda = session_w.points_vip_ronda;
                            p[0].avatar = session_w.avatar;
                            // avatar

                            p[0].won = session_w.won_r;
                            p[0].lose = session_w.lose_r;
                            p[0].profile = sonor_Host_r.SelectedItem.ToString();
                            p[0].position = 0;
                            p[0].ville = session_w.ville;
                            p[0].team = session_w.team;
                            inc = 1;
                        }

                        // implementation des infos users
                        string[] players_info = data[2].Split('|');
                        for (int cnt = 0; cnt < players_info.Length; cnt++)
                        {
                            string[] data_player = players_info[cnt].Split(':');
                            p[cnt + inc] = new _playerR();
                            p[cnt + inc].nom = data_player[0];
                            p[cnt + inc].user = data_player[1];
                            p[cnt + inc].points = Int16.Parse(data_player[2]);
                            p[cnt + inc].points_vip_ronda = Int16.Parse(data_player[3]);
                            p[cnt + inc].avatar = data_player[4];

                            // avatar
                            if (p[cnt + inc].avatar != "null")
                            {
                                if (avatar_wc1.IsBusy)
                                    avatar_wc2.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc2.IsBusy)
                                    avatar_wc3.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc3.IsBusy)
                                    avatar_wc2.DownloadFile(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                            }

                            p[cnt + inc].won = Int16.Parse(data_player[5].Split('/')[0]);
                            p[cnt + inc].lose = Int16.Parse(data_player[5].Split('/')[1]);
                            p[cnt + inc].profile = data_player[6];
                            p[cnt + inc].position = cnt + inc;
                            p[cnt + inc].ville = data_player[7];
                            p[cnt + inc].team = data_player[8];
                            if (session_w.user == data_player[1])
                                session_w.p_pos = cnt + inc;
                        }

                        if (br3.Checked)
                            step2_host_ronda();
                        else
                            step3_ronda();
                    }
                    
                }
                else if (data[1] == "fatal error")
                {
                    if (data[2] == "not connected")
                        destroySession();
                    else if (data[2] == "salon not found")
                    {
                        loading_panel.Visible = false;
                        MessageBox.Show(this, "Ce salon n'existe pas, merci de choisir un autre", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        partie_mode_host_zriwita.Visible = true;
                    }
                }
            }
            else
            {
                //traitement pour une cmd non reconnu.
            }
        }

        void client2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
            {
                if (data[1] == "party")
                {
                    if (data[2] == "done")
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            session_w.salon = salon_host_z.SelectedItem.ToString();
                            session_w.nom_table = data[3];
                            session_w.index_chat = 0;
                            session_w.p_pos = 0;
                            nom_table_z.Text = session_w.nom_table;
                            creer_tbl_btn_z.Enabled = false;
                            annuler_tbl_btn_z.Enabled = true;
                            lancer_partie_btn_z.Enabled = true;
                            chrono_z.Enabled = false;
                            br4.Enabled = false;
                            br1.Enabled = false;
                            salon_host_z.Enabled = false;
                            sonor_Host_z.Enabled = false;
                            session_w.extra1 = Int16.Parse(extra1HP_z.SelectedItem.ToString());
                            session_w.extra2 = Int16.Parse(extra2HP_z.SelectedItem.ToString());
                            session_w.extra3 = Int16.Parse(extra3HP_z.SelectedItem.ToString());
                            session_w.extra4 = Int16.Parse(extra4HP_z.SelectedItem.ToString());

                            extra1IP_z.Text = session_w.extra1.ToString();
                            extra2IP_z.Text = session_w.extra2.ToString();
                            extra3IP_z.Text = session_w.extra3.ToString();
                            extra4IP_z.Text = session_w.extra4.ToString();

                            nbr_player_IP_z.Text = session_w.nbr_player.ToString();

                            if (session_w.vip_zriwita == 1)
                            {
                                vip_boxIP_z.Checked = true;
                                vip_boxIP_z.Visible = true;
                            }
                            else
                                vip_boxIP_z.Checked = false;

                            vip_box_z.Enabled = false;

                            panel_IP_z.Visible = true;
                            PChatServer_z.Visible = true;
                            ////////////////////////////////

                            //// verouillage des controles
                            vip_box_z.Enabled = false;
                            p2HP_z.Enabled = false;
                            p3HP_z.Enabled = false;
                            p4HP_z.Enabled = false;
                            extra1HP_z.Enabled = false;
                            extra2HP_z.Enabled = false;
                            extra3HP_z.Enabled = false;
                            extra4HP_z.Enabled = false;
                            lock_partie_btn_z.Enabled = true;
                            P1IP_z.Text = session_w.user + " {" + session_w.nom + "}";

                            if (session_w.nbr_player == 2)
                                P2IP_z.Enabled = true;
                            else if (session_w.nbr_player == 3)
                            {
                                P2IP_z.Enabled = true;
                                P3IP_z.Enabled = true;
                            }
                            else if (session_w.nbr_player == 4)
                            {
                                P2IP_z.Enabled = true;
                                P3IP_z.Enabled = true;
                                P4IP_z.Enabled = true;
                            }

                            P2IP_z.Items.Clear();
                            P3IP_z.Items.Clear();
                            P4IP_z.Items.Clear();

                            P2IP_z.Items.AddRange(new object[] { "(aucun)" });
                            P3IP_z.Items.AddRange(new object[] { "(aucun)" });
                            P4IP_z.Items.AddRange(new object[] { "(aucun)" });

                            P2IP_z.SelectedIndex = 0;
                            P3IP_z.SelectedIndex = 0;
                            P4IP_z.SelectedIndex = 0;

                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;

                            loading_panel.Visible = false;
                            loading_panel.Visible = false;
                            partie_mode_host_zriwita.Visible = true;
                            partie_mode_host_zriwita.SelectTab(1);
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            session_w.salon = salon_host_r.SelectedItem.ToString();
                            session_w.nom_table = data[3];
                            session_w.index_chat = 0;
                            session_w.p_pos = 0;
                            nom_table_r.Text = session_w.nom_table;
                            creer_tbl_btn_r.Enabled = false;
                            annuler_tbl_btn_r.Enabled = true;
                            lancer_partie_btn_r.Enabled = true;
                            chrono_r.Enabled = false;
                            br4.Enabled = false;
                            br1.Enabled = false;
                            salon_host_r.Enabled = false;
                            sonor_Host_r.Enabled = false;

                            nbr_player_IP_r.Text = session_w.nbr_player.ToString();

                            if (session_w.vip_ronda == 1)
                            {
                                vip_boxIP_r.Checked = true;
                                vip_boxIP_r.Visible = true;
                            }
                            else
                                vip_boxIP_r.Checked = false;

                            vip_box_r.Enabled = false;

                            panel_IP_r.Visible = true;
                            PChatServer_r.Visible = true;
                            ////////////////////////////////

                            //// verouillage des controles
                            vip_box_r.Enabled = false;
                            p2HP_r.Enabled = false;
                            p4HP_r.Enabled = false;
                            lock_partie_btn_r.Enabled = true;
                            P1IP_r.Text = session_w.user + " {" + session_w.nom + "}";

                            if (session_w.nbr_player == 2)
                                P2IP_r.Enabled = true;
                            else if (session_w.nbr_player == 4)
                            {
                                P2IP_r.Enabled = true;
                                P3IP_r.Enabled = true;
                                P4IP_r.Enabled = true;
                            }

                            P2IP_r.Items.Clear();
                            P3IP_r.Items.Clear();
                            P4IP_r.Items.Clear();

                            P2IP_r.Items.AddRange(new object[] { "(aucun)" });
                            P3IP_r.Items.AddRange(new object[] { "(aucun)" });
                            P4IP_r.Items.AddRange(new object[] { "(aucun)" });

                            P2IP_r.SelectedIndex = 0;
                            P3IP_r.SelectedIndex = 0;
                            P4IP_r.SelectedIndex = 0;

                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;

                            loading_panel.Visible = false;
                            loading_panel.Visible = false;
                            partie_mode_host_ronda.Visible = true;
                            partie_mode_host_ronda.SelectTab(1);
                        }
                    }
                    else if (data[2] == "destroyed")
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                            destroyHostPartie_zriwita();
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                            destroyHostPartie_ronda();
                    }
                    else if (data[2] == "etat")
                    {
                        if (data[3] == "busy")
                        {
                            /*MessageBox.Show("done");
                            step3();*/
                        }
                        else if (data[3] == "encapsulate")
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                step3_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                step3_ronda();
                        }
                    }
                    else if (data[2] == "not enough vip")
                    {
                        loading_panel.Visible = false;
                        MessageBox.Show(this, "Désolé, mais vous n'avez pas assez de points VIP.\nPensez a recharger votre compte s'il le faut.", "Points VIP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (br3.Checked)
                        {
                            destroyHostPartie_zriwita();
                            partie_mode_host_zriwita.Visible = true;
                        }
                        else
                        {
                            destroyClientPartie_zriwita();
                            partie_mode_client_zriwita.Visible = true;
                        }
                    }
                }
                else if (data[1] == "client")
                {
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (data[2] == "disconnected")
                        {
                            // traitement pour la deconnexion
                            panel_RP_z.Visible = false;
                            rejoindre_btn_z.Text = "S'assoir";
                            nom_tbl_RP_z.Enabled = true;
                            nom_tbl_RP_z.Text = "";
                            destroyClientPartie_zriwita();
                        }
                        else if (data[2] == "added")
                        {
                            // traitement lorsque le joueurs a été ajouté.
                            br3.Enabled = false;
                            session_w.salon = salon_client_z.SelectedItem.ToString();
                            session_w.nom_table = data[5];
                            session_w.nbr_player = Int16.Parse(data[6]);
                            session_w.index_chat = Int16.Parse(data[7]);
                            session_w.chrono = Int16.Parse(data[8]);
                            if (session_w.chrono == 1)
                                chrono_img_z.Visible = true;
                            else
                                chrono_img_z.Visible = false;
                            // ajouter un controle si la version est vip, on lui attribue 0 pour avoir tt l'historque de chat.
                            //session_w.index_chat = 0;
                            nom_tbl_RP_z.Text = session_w.nom_table;
                            nom_tbl_RP_z.Enabled = false;
                            salon_client_z.Enabled = false;
                            sonor_Client_z.Enabled = false;
                            nbr_player_RP_z.Text = session_w.nbr_player.ToString();
                            rejoindre_btn_z.Text = "Quiter";
                            rejoindre_btn_z.Enabled = true;
                            panel_RP_z.Visible = true;

                            session_w.vip_zriwita = Int16.Parse(data[4]);
                            if (session_w.vip_zriwita == 0)
                                vipRP_z.Visible = false;
                            string[] data_extra = data[3].Split('-');
                            session_w.extra1 = Int16.Parse(data_extra[0]);
                            session_w.extra2 = Int16.Parse(data_extra[1]);
                            session_w.extra3 = Int16.Parse(data_extra[2]);
                            session_w.extra4 = Int16.Parse(data_extra[3]);

                            // affichage des données dans RP
                            extra1RP_z.Text = session_w.extra1.ToString();
                            extra2RP_z.Text = session_w.extra2.ToString();
                            extra3RP_z.Text = session_w.extra3.ToString();
                            extra4RP_z.Text = session_w.extra4.ToString();

                            if (session_w.vip_zriwita == 1)
                                vipRP_z.Checked = true;
                            else
                                vipRP_z.Checked = false;

                            // envoie d'une requette pour avoir les stats des joueurs TIMER
                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;
                        }
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (data[2] == "disconnected")
                        {
                            // traitement pour la deconnexion
                            panel_RP_r.Visible = false;
                            rejoindre_btn_r.Text = "S'assoir";
                            nom_tbl_RP_r.Enabled = true;
                            nom_tbl_RP_r.Text = "";
                            destroyClientPartie_ronda();
                        }
                        else if (data[2] == "added")
                        {
                            // traitement lorsque le joueurs a été ajouté.
                            br3.Enabled = false;
                            session_w.salon = salon_client_r.SelectedItem.ToString();
                            session_w.nom_table = data[5];
                            session_w.nbr_player = Int16.Parse(data[6]);
                            session_w.index_chat = Int16.Parse(data[7]);
                            session_w.chrono = Int16.Parse(data[8]);
                            if (session_w.chrono == 1)
                                chrono_img_r.Visible = true;
                            else
                                chrono_img_r.Visible = false;
                            // ajouter un controle si la version est vip, on lui attribue 0 pour avoir tt l'historque de chat.
                            //session_w.index_chat = 0;
                            nom_tbl_RP_r.Text = session_w.nom_table;
                            nom_tbl_RP_r.Enabled = false;
                            salon_client_r.Enabled = false;
                            sonor_Client_r.Enabled = false;
                            nbr_player_RP_r.Text = session_w.nbr_player.ToString();
                            rejoindre_btn_r.Text = "Quiter";
                            rejoindre_btn_r.Enabled = true;
                            panel_RP_r.Visible = true;

                            session_w.vip_ronda = Int16.Parse(data[4]);
                            if (session_w.vip_ronda == 0)
                                vipRP_r.Visible = false;
                            /*string[] data_extra = data[3].Split('-');
                            session_w.extra1 = Int16.Parse(data_extra[0]);
                            session_w.extra2 = Int16.Parse(data_extra[1]);
                            session_w.extra3 = Int16.Parse(data_extra[2]);
                            session_w.extra4 = Int16.Parse(data_extra[3]);

                            // affichage des données dans RP
                            extra1RP_z.Text = session_w.extra1.ToString();
                            extra2RP_z.Text = session_w.extra2.ToString();
                            extra3RP_z.Text = session_w.extra3.ToString();
                            extra4RP_z.Text = session_w.extra4.ToString();*/

                            if (session_w.vip_ronda == 1)
                                vipRP_r.Checked = true;
                            else
                                vipRP_r.Checked = false;

                            // envoie d'une requette pour avoir les stats des joueurs TIMER
                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;
                        }
                    }

                }
                else if (data[1] == "host party")
                {
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (data[2] == "locked")
                            lock_partie_btn_z.Text = "Déverrouiller la partie";
                        else if (data[2] == "unlocked")
                            lock_partie_btn_z.Text = "Vérrouiller la partie";
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (data[2] == "locked")
                            lock_partie_btn_r.Text = "Déverrouiller la partie";
                        else if (data[2] == "unlocked")
                            lock_partie_btn_r.Text = "Vérrouiller la partie";
                    }
                }
                else if (data[1] == "client party")
                {
                    if (data[2] == "locked")
                        MessageBox.Show("La table est vérrouillé", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "not found")
                        MessageBox.Show("La table n'existe pas", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "full")
                        MessageBox.Show(this, "Table pleine, merci de choisir une autre", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "destroyed")
                    {
                        users_checker.Enabled = false;

                        if (br3.Checked == true)
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyHostPartie_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyHostPartie_ronda();
                            MessageBox.Show(this, "Table viens d’être supprimé, certainement à cause d’un très nombre d’attente\nVérifié votre connexion svp.", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyClientPartie_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                destroyClientPartie_ronda();
                            MessageBox.Show(this, "Table viens d'être supprimé, merci de choisir une autre table", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else if (data[1] == "get_players_info")
                {
                    int inc = 0;
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (br3.Checked)
                        {
                            p[0] = new _playerR();
                            p[0].nom = session_w.nom;
                            p[0].user = session_w.user;
                            p[0].points = session_w.points_z;
                            p[0].points_vip_zriwta = session_w.points_vip_zriwita;
                            p[0].avatar = session_w.avatar;
                            // avatar

                            p[0].won = session_w.won_z;
                            p[0].lose = session_w.lose_z;
                            p[0].profile = sonor_Host_z.SelectedItem.ToString();
                            p[0].position = 0;
                            p[0].ville = session_w.ville;
                            p[0].team = session_w.team;
                            inc = 1;
                        }

                        // implementation des infos users
                        string[] players_info = data[2].Split('|');
                        for (int cnt = 0; cnt < players_info.Length; cnt++)
                        {
                            string[] data_player = players_info[cnt].Split(':');
                            p[cnt + inc] = new _playerR();
                            p[cnt + inc].nom = data_player[0];
                            p[cnt + inc].user = data_player[1];
                            p[cnt + inc].points = Int16.Parse(data_player[2]);
                            p[cnt + inc].points_vip_zriwta = Int16.Parse(data_player[3]);
                            p[cnt + inc].avatar = data_player[4];

                            // avatar
                            if (p[cnt + inc].avatar != "null")
                            {
                                if (avatar_wc1.IsBusy)
                                    avatar_wc2.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc2.IsBusy)
                                    avatar_wc3.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc3.IsBusy)
                                    avatar_wc2.DownloadFile(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                            }

                            p[cnt + inc].won = Int16.Parse(data_player[5].Split('/')[0]);
                            p[cnt + inc].lose = Int16.Parse(data_player[5].Split('/')[1]);
                            p[cnt + inc].profile = data_player[6];
                            p[cnt + inc].position = cnt + inc;
                            p[cnt + inc].ville = data_player[7];
                            p[cnt + inc].team = data_player[8];
                            if (session_w.user == data_player[1])
                                session_w.p_pos = cnt + inc;
                        }

                        if (br3.Checked)
                            step2_host_zriwita();
                        else
                            step3_zriwita();
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (br3.Checked)
                        {
                            p[0] = new _playerR();
                            p[0].nom = session_w.nom;
                            p[0].user = session_w.user;
                            p[0].points = session_w.points_r;
                            //p[0].points_vip_zriwta = session_w.points_vip_zriwita;
                            p[0].points_vip_ronda = session_w.points_vip_ronda;
                            p[0].avatar = session_w.avatar;
                            // avatar

                            p[0].won = session_w.won_r;
                            p[0].lose = session_w.lose_r;
                            p[0].profile = sonor_Host_r.SelectedItem.ToString();
                            p[0].position = 0;
                            p[0].ville = session_w.ville;
                            p[0].team = session_w.team;
                            inc = 1;
                        }

                        // implementation des infos users
                        string[] players_info = data[2].Split('|');
                        for (int cnt = 0; cnt < players_info.Length; cnt++)
                        {
                            string[] data_player = players_info[cnt].Split(':');
                            p[cnt + inc] = new _playerR();
                            p[cnt + inc].nom = data_player[0];
                            p[cnt + inc].user = data_player[1];
                            p[cnt + inc].points = Int16.Parse(data_player[2]);
                            p[cnt + inc].points_vip_ronda = Int16.Parse(data_player[3]);
                            p[cnt + inc].avatar = data_player[4];

                            // avatar
                            if (p[cnt + inc].avatar != "null")
                            {
                                if (avatar_wc1.IsBusy)
                                    avatar_wc2.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc2.IsBusy)
                                    avatar_wc3.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc3.IsBusy)
                                    avatar_wc2.DownloadFile(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                            }

                            p[cnt + inc].won = Int16.Parse(data_player[5].Split('/')[0]);
                            p[cnt + inc].lose = Int16.Parse(data_player[5].Split('/')[1]);
                            p[cnt + inc].profile = data_player[6];
                            p[cnt + inc].position = cnt + inc;
                            p[cnt + inc].ville = data_player[7];
                            p[cnt + inc].team = data_player[8];
                            if (session_w.user == data_player[1])
                                session_w.p_pos = cnt + inc;
                        }

                        if (br3.Checked)
                            step2_host_ronda();
                        else
                            step3_ronda();
                    }

                }
                else if (data[1] == "fatal error")
                {
                    if (data[2] == "not connected")
                        destroySession();
                    else if (data[2] == "salon not found")
                    {
                        loading_panel.Visible = false;
                        MessageBox.Show(this, "Ce salon n'existe pas, merci de choisir un autre", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        partie_mode_host_zriwita.Visible = true;
                    }
                }
            }
            else
            {
                //traitement pour une cmd non reconnu.
            }
        }

        void client3_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
            {
                if (data[1] == "party")
                {
                    if (data[2] == "done")
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                        {
                            session_w.salon = salon_host_z.SelectedItem.ToString();
                            session_w.nom_table = data[3];
                            session_w.index_chat = 0;
                            session_w.p_pos = 0;
                            nom_table_z.Text = session_w.nom_table;
                            creer_tbl_btn_z.Enabled = false;
                            annuler_tbl_btn_z.Enabled = true;
                            lancer_partie_btn_z.Enabled = true;
                            chrono_z.Enabled = false;
                            br4.Enabled = false;
                            br1.Enabled = false;
                            salon_host_z.Enabled = false;
                            sonor_Host_z.Enabled = false;
                            session_w.extra1 = Int16.Parse(extra1HP_z.SelectedItem.ToString());
                            session_w.extra2 = Int16.Parse(extra2HP_z.SelectedItem.ToString());
                            session_w.extra3 = Int16.Parse(extra3HP_z.SelectedItem.ToString());
                            session_w.extra4 = Int16.Parse(extra4HP_z.SelectedItem.ToString());

                            extra1IP_z.Text = session_w.extra1.ToString();
                            extra2IP_z.Text = session_w.extra2.ToString();
                            extra3IP_z.Text = session_w.extra3.ToString();
                            extra4IP_z.Text = session_w.extra4.ToString();

                            nbr_player_IP_z.Text = session_w.nbr_player.ToString();

                            if (session_w.vip_zriwita == 1)
                            {
                                vip_boxIP_z.Checked = true;
                                vip_boxIP_z.Visible = true;
                            }
                            else
                                vip_boxIP_z.Checked = false;

                            vip_box_z.Enabled = false;

                            panel_IP_z.Visible = true;
                            PChatServer_z.Visible = true;
                            ////////////////////////////////

                            //// verouillage des controles
                            vip_box_z.Enabled = false;
                            p2HP_z.Enabled = false;
                            p3HP_z.Enabled = false;
                            p4HP_z.Enabled = false;
                            extra1HP_z.Enabled = false;
                            extra2HP_z.Enabled = false;
                            extra3HP_z.Enabled = false;
                            extra4HP_z.Enabled = false;
                            lock_partie_btn_z.Enabled = true;
                            P1IP_z.Text = session_w.user + " {" + session_w.nom + "}";

                            if (session_w.nbr_player == 2)
                                P2IP_z.Enabled = true;
                            else if (session_w.nbr_player == 3)
                            {
                                P2IP_z.Enabled = true;
                                P3IP_z.Enabled = true;
                            }
                            else if (session_w.nbr_player == 4)
                            {
                                P2IP_z.Enabled = true;
                                P3IP_z.Enabled = true;
                                P4IP_z.Enabled = true;
                            }

                            P2IP_z.Items.Clear();
                            P3IP_z.Items.Clear();
                            P4IP_z.Items.Clear();

                            P2IP_z.Items.AddRange(new object[] { "(aucun)" });
                            P3IP_z.Items.AddRange(new object[] { "(aucun)" });
                            P4IP_z.Items.AddRange(new object[] { "(aucun)" });

                            P2IP_z.SelectedIndex = 0;
                            P3IP_z.SelectedIndex = 0;
                            P4IP_z.SelectedIndex = 0;

                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;

                            loading_panel.Visible = false;
                            loading_panel.Visible = false;
                            partie_mode_host_zriwita.Visible = true;
                            partie_mode_host_zriwita.SelectTab(1);
                        }
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                        {
                            session_w.salon = salon_host_r.SelectedItem.ToString();
                            session_w.nom_table = data[3];
                            session_w.index_chat = 0;
                            session_w.p_pos = 0;
                            nom_table_r.Text = session_w.nom_table;
                            creer_tbl_btn_r.Enabled = false;
                            annuler_tbl_btn_r.Enabled = true;
                            lancer_partie_btn_r.Enabled = true;
                            chrono_r.Enabled = false;
                            br4.Enabled = false;
                            br1.Enabled = false;
                            salon_host_r.Enabled = false;
                            sonor_Host_r.Enabled = false;

                            nbr_player_IP_r.Text = session_w.nbr_player.ToString();

                            if (session_w.vip_ronda == 1)
                            {
                                vip_boxIP_r.Checked = true;
                                vip_boxIP_r.Visible = true;
                            }
                            else
                                vip_boxIP_r.Checked = false;

                            vip_box_r.Enabled = false;

                            panel_IP_r.Visible = true;
                            PChatServer_r.Visible = true;
                            ////////////////////////////////

                            //// verouillage des controles
                            vip_box_r.Enabled = false;
                            p2HP_r.Enabled = false;
                            p4HP_r.Enabled = false;
                            lock_partie_btn_r.Enabled = true;
                            P1IP_r.Text = session_w.user + " {" + session_w.nom + "}";

                            if (session_w.nbr_player == 2)
                                P2IP_r.Enabled = true;
                            else if (session_w.nbr_player == 4)
                            {
                                P2IP_r.Enabled = true;
                                P3IP_r.Enabled = true;
                                P4IP_r.Enabled = true;
                            }

                            P2IP_r.Items.Clear();
                            P3IP_r.Items.Clear();
                            P4IP_r.Items.Clear();

                            P2IP_r.Items.AddRange(new object[] { "(aucun)" });
                            P3IP_r.Items.AddRange(new object[] { "(aucun)" });
                            P4IP_r.Items.AddRange(new object[] { "(aucun)" });

                            P2IP_r.SelectedIndex = 0;
                            P3IP_r.SelectedIndex = 0;
                            P4IP_r.SelectedIndex = 0;

                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;

                            loading_panel.Visible = false;
                            loading_panel.Visible = false;
                            partie_mode_host_ronda.Visible = true;
                            partie_mode_host_ronda.SelectTab(1);
                        }
                    }
                    else if (data[2] == "destroyed")
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                            destroyHostPartie_zriwita();
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                            destroyHostPartie_ronda();
                    }
                    else if (data[2] == "etat")
                    {
                        if (data[3] == "busy")
                        {
                            /*MessageBox.Show("done");
                            step3();*/
                        }
                        else if (data[3] == "encapsulate")
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                step3_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                step3_ronda();
                        }
                    }
                    else if (data[2] == "not enough vip")
                    {
                        loading_panel.Visible = false;
                        MessageBox.Show(this, "Désolé, mais vous n'avez pas assez de points VIP.\nPensez a recharger votre compte s'il le faut.", "Points VIP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (br3.Checked)
                        {
                            destroyHostPartie_zriwita();
                            partie_mode_host_zriwita.Visible = true;
                        }
                        else
                        {
                            destroyClientPartie_zriwita();
                            partie_mode_client_zriwita.Visible = true;
                        }
                    }
                }
                else if (data[1] == "client")
                {
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (data[2] == "disconnected")
                        {
                            // traitement pour la deconnexion
                            panel_RP_z.Visible = false;
                            rejoindre_btn_z.Text = "S'assoir";
                            nom_tbl_RP_z.Enabled = true;
                            nom_tbl_RP_z.Text = "";
                            destroyClientPartie_zriwita();
                        }
                        else if (data[2] == "added")
                        {
                            // traitement lorsque le joueurs a été ajouté.
                            br3.Enabled = false;
                            session_w.salon = salon_client_z.SelectedItem.ToString();
                            session_w.nom_table = data[5];
                            session_w.nbr_player = Int16.Parse(data[6]);
                            session_w.index_chat = Int16.Parse(data[7]);
                            session_w.chrono = Int16.Parse(data[8]);
                            if (session_w.chrono == 1)
                                chrono_img_z.Visible = true;
                            else
                                chrono_img_z.Visible = false;
                            // ajouter un controle si la version est vip, on lui attribue 0 pour avoir tt l'historque de chat.
                            //session_w.index_chat = 0;
                            nom_tbl_RP_z.Text = session_w.nom_table;
                            nom_tbl_RP_z.Enabled = false;
                            salon_client_z.Enabled = false;
                            sonor_Client_z.Enabled = false;
                            nbr_player_RP_z.Text = session_w.nbr_player.ToString();
                            rejoindre_btn_z.Text = "Quiter";
                            rejoindre_btn_z.Enabled = true;
                            panel_RP_z.Visible = true;

                            session_w.vip_zriwita = Int16.Parse(data[4]);
                            if (session_w.vip_zriwita == 0)
                                vipRP_z.Visible = false;
                            string[] data_extra = data[3].Split('-');
                            session_w.extra1 = Int16.Parse(data_extra[0]);
                            session_w.extra2 = Int16.Parse(data_extra[1]);
                            session_w.extra3 = Int16.Parse(data_extra[2]);
                            session_w.extra4 = Int16.Parse(data_extra[3]);

                            // affichage des données dans RP
                            extra1RP_z.Text = session_w.extra1.ToString();
                            extra2RP_z.Text = session_w.extra2.ToString();
                            extra3RP_z.Text = session_w.extra3.ToString();
                            extra4RP_z.Text = session_w.extra4.ToString();

                            if (session_w.vip_zriwita == 1)
                                vipRP_z.Checked = true;
                            else
                                vipRP_z.Checked = false;

                            // envoie d'une requette pour avoir les stats des joueurs TIMER
                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;
                        }
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (data[2] == "disconnected")
                        {
                            // traitement pour la deconnexion
                            panel_RP_r.Visible = false;
                            rejoindre_btn_r.Text = "S'assoir";
                            nom_tbl_RP_r.Enabled = true;
                            nom_tbl_RP_r.Text = "";
                            destroyClientPartie_ronda();
                        }
                        else if (data[2] == "added")
                        {
                            // traitement lorsque le joueurs a été ajouté.
                            br3.Enabled = false;
                            session_w.salon = salon_client_r.SelectedItem.ToString();
                            session_w.nom_table = data[5];
                            session_w.nbr_player = Int16.Parse(data[6]);
                            session_w.index_chat = Int16.Parse(data[7]);
                            session_w.chrono = Int16.Parse(data[8]);
                            if (session_w.chrono == 1)
                                chrono_img_r.Visible = true;
                            else
                                chrono_img_r.Visible = false;
                            // ajouter un controle si la version est vip, on lui attribue 0 pour avoir tt l'historque de chat.
                            //session_w.index_chat = 0;
                            nom_tbl_RP_r.Text = session_w.nom_table;
                            nom_tbl_RP_r.Enabled = false;
                            salon_client_r.Enabled = false;
                            sonor_Client_r.Enabled = false;
                            nbr_player_RP_r.Text = session_w.nbr_player.ToString();
                            rejoindre_btn_r.Text = "Quiter";
                            rejoindre_btn_r.Enabled = true;
                            panel_RP_r.Visible = true;

                            session_w.vip_ronda = Int16.Parse(data[4]);
                            if (session_w.vip_ronda == 0)
                                vipRP_r.Visible = false;
                            /*string[] data_extra = data[3].Split('-');
                            session_w.extra1 = Int16.Parse(data_extra[0]);
                            session_w.extra2 = Int16.Parse(data_extra[1]);
                            session_w.extra3 = Int16.Parse(data_extra[2]);
                            session_w.extra4 = Int16.Parse(data_extra[3]);

                            // affichage des données dans RP
                            extra1RP_z.Text = session_w.extra1.ToString();
                            extra2RP_z.Text = session_w.extra2.ToString();
                            extra3RP_z.Text = session_w.extra3.ToString();
                            extra4RP_z.Text = session_w.extra4.ToString();*/

                            if (session_w.vip_ronda == 1)
                                vipRP_r.Checked = true;
                            else
                                vipRP_r.Checked = false;

                            // envoie d'une requette pour avoir les stats des joueurs TIMER
                            users_checker.Enabled = true;
                            chat_checker.Enabled = true;
                        }
                    }

                }
                else if (data[1] == "host party")
                {
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (data[2] == "locked")
                            lock_partie_btn_z.Text = "Déverrouiller la partie";
                        else if (data[2] == "unlocked")
                            lock_partie_btn_z.Text = "Vérrouiller la partie";
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (data[2] == "locked")
                            lock_partie_btn_r.Text = "Déverrouiller la partie";
                        else if (data[2] == "unlocked")
                            lock_partie_btn_r.Text = "Vérrouiller la partie";
                    }
                }
                else if (data[1] == "client party")
                {
                    if (data[2] == "locked")
                        MessageBox.Show("La table est vérrouillé", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "not found")
                        MessageBox.Show("La table n'existe pas", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "full")
                        MessageBox.Show(this, "Table pleine, merci de choisir une autre", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (data[2] == "destroyed")
                    {
                        users_checker.Enabled = false;

                        if (br3.Checked == true)
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyHostPartie_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyHostPartie_ronda();
                            MessageBox.Show(this, "Table viens d’être supprimé, certainement à cause d’un très nombre d’attente\nVérifié votre connexion svp.", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                destroyClientPartie_zriwita();
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                destroyClientPartie_ronda();
                            MessageBox.Show(this, "Table viens d'être supprimé, merci de choisir une autre table", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else if (data[1] == "get_players_info")
                {
                    int inc = 0;
                    if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                    {
                        if (br3.Checked)
                        {
                            p[0] = new _playerR();
                            p[0].nom = session_w.nom;
                            p[0].user = session_w.user;
                            p[0].points = session_w.points_z;
                            p[0].points_vip_zriwta = session_w.points_vip_zriwita;
                            p[0].avatar = session_w.avatar;
                            // avatar

                            p[0].won = session_w.won_z;
                            p[0].lose = session_w.lose_z;
                            p[0].profile = sonor_Host_z.SelectedItem.ToString();
                            p[0].position = 0;
                            p[0].ville = session_w.ville;
                            p[0].team = session_w.team;
                            inc = 1;
                        }

                        // implementation des infos users
                        string[] players_info = data[2].Split('|');
                        for (int cnt = 0; cnt < players_info.Length; cnt++)
                        {
                            string[] data_player = players_info[cnt].Split(':');
                            p[cnt + inc] = new _playerR();
                            p[cnt + inc].nom = data_player[0];
                            p[cnt + inc].user = data_player[1];
                            p[cnt + inc].points = Int16.Parse(data_player[2]);
                            p[cnt + inc].points_vip_zriwta = Int16.Parse(data_player[3]);
                            p[cnt + inc].avatar = data_player[4];

                            // avatar
                            if (p[cnt + inc].avatar != "null")
                            {
                                if (avatar_wc1.IsBusy)
                                    avatar_wc2.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc2.IsBusy)
                                    avatar_wc3.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc3.IsBusy)
                                    avatar_wc2.DownloadFile(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                            }

                            p[cnt + inc].won = Int16.Parse(data_player[5].Split('/')[0]);
                            p[cnt + inc].lose = Int16.Parse(data_player[5].Split('/')[1]);
                            p[cnt + inc].profile = data_player[6];
                            p[cnt + inc].position = cnt + inc;
                            p[cnt + inc].ville = data_player[7];
                            p[cnt + inc].team = data_player[8];
                            if (session_w.user == data_player[1])
                                session_w.p_pos = cnt + inc;
                        }

                        if (br3.Checked)
                            step2_host_zriwita();
                        else
                            step3_zriwita();
                    }
                    else if (gameListe.SelectedItem.ToString() == "RONDA")
                    {
                        if (br3.Checked)
                        {
                            p[0] = new _playerR();
                            p[0].nom = session_w.nom;
                            p[0].user = session_w.user;
                            p[0].points = session_w.points_r;
                            //p[0].points_vip_zriwta = session_w.points_vip_zriwita;
                            p[0].points_vip_ronda = session_w.points_vip_ronda;
                            p[0].avatar = session_w.avatar;
                            // avatar

                            p[0].won = session_w.won_r;
                            p[0].lose = session_w.lose_r;
                            p[0].profile = sonor_Host_r.SelectedItem.ToString();
                            p[0].position = 0;
                            p[0].ville = session_w.ville;
                            p[0].team = session_w.team;
                            inc = 1;
                        }

                        // implementation des infos users
                        string[] players_info = data[2].Split('|');
                        for (int cnt = 0; cnt < players_info.Length; cnt++)
                        {
                            string[] data_player = players_info[cnt].Split(':');
                            p[cnt + inc] = new _playerR();
                            p[cnt + inc].nom = data_player[0];
                            p[cnt + inc].user = data_player[1];
                            p[cnt + inc].points = Int16.Parse(data_player[2]);
                            p[cnt + inc].points_vip_ronda = Int16.Parse(data_player[3]);
                            p[cnt + inc].avatar = data_player[4];

                            // avatar
                            if (p[cnt + inc].avatar != "null")
                            {
                                if (avatar_wc1.IsBusy)
                                    avatar_wc2.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc2.IsBusy)
                                    avatar_wc3.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                                else if (avatar_wc3.IsBusy)
                                    avatar_wc2.DownloadFile(new Uri(hosted_server + "/media/avatars/" + p[cnt + inc].avatar), "temps/thumbs_wan/" + p[cnt + inc].avatar);
                            }

                            p[cnt + inc].won = Int16.Parse(data_player[5].Split('/')[0]);
                            p[cnt + inc].lose = Int16.Parse(data_player[5].Split('/')[1]);
                            p[cnt + inc].profile = data_player[6];
                            p[cnt + inc].position = cnt + inc;
                            p[cnt + inc].ville = data_player[7];
                            p[cnt + inc].team = data_player[8];
                            if (session_w.user == data_player[1])
                                session_w.p_pos = cnt + inc;
                        }

                        if (br3.Checked)
                            step2_host_ronda();
                        else
                            step3_ronda();
                    }

                }
                else if (data[1] == "fatal error")
                {
                    if (data[2] == "not connected")
                        destroySession();
                    else if (data[2] == "salon not found")
                    {
                        loading_panel.Visible = false;
                        MessageBox.Show(this, "Ce salon n'existe pas, merci de choisir un autre", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        partie_mode_host_zriwita.Visible = true;
                    }
                }
            }
            else
            {
                //traitement pour une cmd non reconnu.
            }
        }

        void client_syn1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
            {
                if (data[1] == "welcome")
                {
                    session_w.logged = true;
                    session_w.id_app = data[2];
                    session_w.nom = data[3];
                    session_w.user = data[4];
                    session_w.points_z = Int16.Parse(data[5]);
                    session_w.won_z = Int16.Parse(data[6].Split('/')[0]);
                    session_w.lose_z = Int16.Parse(data[6].Split('/')[1]);
                    session_w.points_vip_zriwita = Int16.Parse(data[7]);
                    session_w.ville = data[9];
                    session_w.team = data[10];
                    session_w.avatar = data[11];
                    ville_lb.Text = session_w.ville;
                    team_lb.Text = session_w.team;
                    offline_pan.Visible = false;
                    online_user_lb.Text = "[" + session_w.nom + "]";
                    online_pan.Location = new Point(this.Width - 250, 2);
                    stats_pts_z.Text = session_w.points_z + " Pts";
                    session_w.points_r = Int16.Parse(data[12]);
                    stats_pts_r.Text = session_w.points_r + "Pts";
                    session_w.won_r = Int16.Parse(data[13].Split('/')[0]);
                    session_w.lose_r = Int16.Parse(data[13].Split('/')[1]);
                    session_w.points_vip_ronda = Int16.Parse(data[14]);

                    if (session_w.points_vip_zriwita > 0)
                    {
                        stats_vip_pts_z.Text = session_w.points_vip_zriwita + " VIP";
                        stats_vip_pts_z.Visible = true;
                        vip_box_z.Enabled = true;
                        vip_box_z.Checked = true;
                        vip_box_z.Visible = true;
                        vip_pic_z.Visible = true;
                    }
                    else
                    {
                        vip_box_z.Enabled = false;
                        vip_box_z.Checked = false;
                    }

                    if (session_w.points_vip_ronda > 0)
                    {
                        stats_vip_pts_r.Text = session_w.points_vip_ronda + " VIP";
                        stats_vip_pts_r.Visible = true;
                        vip_box_r.Enabled = true;
                        vip_box_r.Checked = true;
                        vip_box_r.Visible = true;
                        vip_pic_r.Visible = true;
                    }
                    else
                    {
                        vip_box_r.Enabled = false;
                        vip_box_r.Checked = false;
                    }

                    stats_parties_z.Text = session_w.won_z + " G / " + session_w.lose_z + " P";
                    stats_parties_r.Text = session_w.won_r + " G / " + session_w.lose_r + " P";
                    online_pan.Visible = true;
                    br2.Enabled = true;
                    syn_network.Enabled = true;
                    string[] data_client = data[8].Split('/');

                    // ajout des nom des tables
                    for (int cnt = 0; cnt < data_client.Length; cnt++)
                    {
                        salon_host_z.Items.AddRange(new object[] { data_client[cnt] });
                        salon_client_z.Items.AddRange(new object[] { data_client[cnt] });

                        salon_host_r.Items.AddRange(new object[] { data_client[cnt] });
                        salon_client_r.Items.AddRange(new object[] { data_client[cnt] });
                    }
                    
                    salon_host_z.SelectedIndex = 0;
                    salon_client_z.SelectedIndex = 0;

                    salon_host_r.SelectedIndex = 0;
                    salon_client_r.SelectedIndex = 0;

                    // listage des fond sonors
                    Random rnd = new Random();
                    int rnd2 = rnd.Next(2);
                    sonor_Host_z.SelectedIndex = rnd2;
                    rnd2 = rnd.Next(2);
                    sonor_Host_r.SelectedIndex = rnd2;
                    rnd2 = rnd.Next(2);
                    sonor_Client_z.SelectedIndex = rnd2;
                    rnd2 = rnd.Next(2);
                    sonor_Client_r.SelectedIndex = rnd2;

                    // download de l'avatar
                    if (session_w.avatar != "null")
                        avatar_wc1.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + data[11]), "temps/thumbs_wan/" + data[11]);
                    
                    if (option_loaded == 2)
                    {
                        speed_timer.Enabled = false;
                        
                        if (speed_cnt < 2)
                        {
                            speed_loaded = 2000;
                            users_checker.Interval = speed_loaded;
                            chat_checker.Interval = speed_loaded;
                        }
                        else if (speed_cnt > 8)
                        {
                            // code pour sign out
                            speed_loaded = 8 * 1000;
                            users_checker.Interval = speed_loaded;
                            chat_checker.Interval = speed_loaded;
                        }
                        else
                        {
                            speed_loaded = speed_cnt * 1000;
                            users_checker.Interval = speed_loaded;
                            chat_checker.Interval = speed_loaded;
                        }
                    }
                    else if (option_loaded == 3)
                    {
                        users_checker.Interval = main.speed_loaded;
                        chat_checker.Interval = main.speed_loaded;
                        
                    }
                }
                else if (data[1] == "signed out done")
                {
                    destroyHostPartie_zriwita();
                    destroyClientPartie_zriwita();
                    session_w.logged = false;
                    br1.Enabled = true;
                    session_w.id_app = null;
                    session_w.nom = null;
                    online_pan.Visible = false;
                    offline_pan.Visible = true;
                    br2.Enabled = false;
                    br4.Enabled = true;
                    br1.Checked = true;
                    loading_panel.Visible = false;
                    partie_mode_host_zriwita.Visible = false;
                    partie_mode_client_zriwita.Visible = false;
                    check_connexion_btn.Enabled = true;
                }
                else if (data[1]== "session reset done")
                {
                    stats_pts_z.Text = data[2] + " Pts";
                    stats_pts_r.Text = data[3] + " Pts";
                }
                else if (data[1] == "error")
                {
                    if (data[2] == "failed to log")
                    {
                        MessageBox.Show(this, "Login ou mot de passe sont incorrecte", "Erreur d'authentification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        user.Focus();
                        check_connexion_btn.Enabled = true;
                    }
                    else if (data[2] == "session lifetimeout")
                    {
                        syn_network.Enabled = false;
                        users_checker.Enabled = false;
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
                                noWebClientfree();
                        }
                        else
                        {
                            destroySession();
                            MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (data[2] == "not connected")
                    {
                        MessageBox.Show(this, "Action non autorisé", "Command erroné", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        check_connexion_btn.Enabled = true;
                    }
                    else if (data[2] == "user already signed")
                    {
                        MessageBox.Show(this, "Un autre utilisateur est déja connecté avec ces identifiants\nSi vous pensez que vous êtes victime d'un Hack, pensez a changer le mot de passe :-)\n", "Erreur d'authentification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        check_connexion_btn.Enabled = true;
                    }
                    else if (data[2] == "user already signed out")
                    {
                        MessageBox.Show(this, "Vous êtes déja déconnecté\nSoit autaumatiquement, soit un autre utilisateur vous a déconnecté", "Erreur de session", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        check_connexion_btn.Enabled = true;
                    }
                }
                else if (data[1] == "fatal error")
                {
                    if (data[2] == "not connected")
                    {
                        destroySession();
                        MessageBox.Show("Vous n'êtes pas loggé sur le site\nSoit vous avez oublié de vous déloggé, soit vous êtes un CHEATER :-)", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        check_connexion_btn.Enabled = true;
                    }
                }
            }
        }

        // //////////////////////// 2eme instance  client_syn 2
        void client_syn2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //MessageBox.Show(e.Result);
            string[] data;
            data = e.Result.Split('#');

            if (data[0] == "C4RT4")
            {
                if (data[1] == "welcome")
                {
                    session_w.logged = true;
                    session_w.id_app = data[2];
                    session_w.nom = data[3];
                    session_w.user = data[4];
                    session_w.points_z = Int16.Parse(data[5]);
                    session_w.won_z = Int16.Parse(data[6].Split('/')[0]);
                    session_w.lose_z = Int16.Parse(data[6].Split('/')[1]);
                    session_w.points_vip_zriwita = Int16.Parse(data[7]);
                    session_w.ville = data[9];
                    session_w.team = data[10];
                    session_w.avatar = data[11];
                    ville_lb.Text = session_w.ville;
                    team_lb.Text = session_w.team;
                    offline_pan.Visible = false;
                    online_user_lb.Text = "[" + session_w.nom + "]";
                    online_pan.Location = new Point(this.Width - 250, 2);
                    stats_pts_z.Text = session_w.points_z + " Pts";

                    if (session_w.points_vip_zriwita > 0)
                    {
                        stats_vip_pts_z.Text = session_w.points_vip_zriwita + " VIP";
                        stats_vip_pts_z.Visible = true;
                        vip_box_z.Enabled = true;
                        vip_box_z.Checked = true;
                        vip_box_z.Visible = true;
                        vip_pic_z.Visible = true;
                    }
                    else
                    {
                        vip_box_z.Enabled = false;
                        vip_box_z.Checked = false;
                    }

                    stats_parties_z.Text = session_w.won_z + " G / " + session_w.lose_z + " P";
                    online_pan.Visible = true;
                    br2.Enabled = true;
                    syn_network.Enabled = true;
                    string[] data_client = data[8].Split('/');

                    // ajout des nom des tables
                    for (int cnt = 0; cnt < data_client.Length; cnt++)
                    {
                        salon_host_z.Items.AddRange(new object[] { data_client[cnt] });
                        salon_client_z.Items.AddRange(new object[] { data_client[cnt] });
                    }

                    salon_host_z.SelectedIndex = 0;
                    salon_client_z.SelectedIndex = 0;
                    // listage des fond sonors
                    Random rnd = new Random();
                    int rnd2 = rnd.Next(2);
                    sonor_Host_z.SelectedIndex = rnd2;
                    rnd2 = rnd.Next(2);
                    sonor_Client_z.SelectedIndex = rnd2;

                    // download de l'avatar
                    if (session_w.avatar != "null")
                    {

                        avatar_wc1.DownloadFileAsync(new Uri(hosted_server + "/media/avatars/" + data[11]), "temps/thumbs_wan/" + data[11]);
                    }
                }
                else if (data[1] == "signed out done")
                {
                    destroyHostPartie_zriwita();
                    destroyClientPartie_zriwita();
                    session_w.logged = false;
                    br1.Enabled = true;
                    session_w.id_app = null;
                    session_w.nom = null;
                    online_pan.Visible = false;
                    offline_pan.Visible = true;
                    br2.Enabled = false;
                    br4.Enabled = true;
                    br1.Checked = true;
                    loading_panel.Visible = false;
                    partie_mode_host_zriwita.Visible = false;
                    partie_mode_client_zriwita.Visible = false;
                    check_connexion_btn.Enabled = true;
                }
                else if (data[1] == "session reset done")
                {
                    stats_pts_z.Text = data[2] + " Pts";
                }
                else if (data[1] == "error")
                {
                    if (data[2] == "failed to log")
                    {
                        MessageBox.Show(this, "Login ou mot de passe sont incorrecte", "Erreur d'authentification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        user.Focus();
                        check_connexion_btn.Enabled = true;
                    }
                    else if (data[2] == "session lifetimeout")
                    {
                        syn_network.Enabled = false;
                        users_checker.Enabled = false;
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
                                noWebClientfree();
                        }
                        else
                        {
                            destroySession();
                            MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (data[2] == "not connected")
                    {
                        MessageBox.Show(this, "Action non autorisé", "Command erroné", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        check_connexion_btn.Enabled = true;
                    }
                    else if (data[2] == "user already signed")
                    {
                        MessageBox.Show(this, "Un autre utilisateur est déja connecté avec ces identifiants\nSi vous pensez que vous êtes victime d'un Hack, pensez a changer le mot de passe :-)\n", "Erreur d'authentification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        check_connexion_btn.Enabled = true;
                    }
                    else if (data[2] == "user already signed out")
                    {
                        MessageBox.Show(this, "Vous êtes déja déconnecté\nSoit autaumatiquement, soit un autre utilisateur vous a déconnecté", "Erreur de session", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        check_connexion_btn.Enabled = true;
                    }
                }
                else if (data[1] == "fatal error")
                {
                    if (data[2] == "not connected")
                    {
                        destroySession();
                        MessageBox.Show("Vous n'êtes pas loggé sur le site\nSoit vous avez oublié de vous déloggé, soit vous êtes un CHEATER :-)", "Erreur de chargement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        check_connexion_btn.Enabled = true;
                    }
                }
            }
        }

        private void header()
        {
            client1.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client2.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client3.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_syn1.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_syn2.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_users_checker1.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_users_checker2.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_chat_checker1.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
            client_chat_checker2.Headers["User-Agent"] = "NDMzNDUyNTQzNDJjNzA2Zjc3NjU3MjIwNjk3MzIwNzQ2ODY1MjA2ZDZmNmU2NTc5MmMyMDZkNmY2ZTY1NzkyMDY5NzMyMDc0Njg2NTIwNzA2Zjc3NjU3MjJjMjA3NzY1MjA2ZDYxNjQ2NTIwNjk3NDIwNDYzMDU4MmMyODU0NjgzMzJkNmQzMDUyNzA0ODMzNTIyMDNhMjk";
        }

        private void destroyHostPartie_zriwita()
        {
            users_checker.Enabled = false;
            chat_checker.Enabled = false;

            br1.Enabled = true;
            br2.Enabled = true;
            br3.Enabled = true;
            br4.Enabled = true;
            //////// onglet heberger une partie
            creer_tbl_btn_z.Enabled = true;
            annuler_tbl_btn_z.Enabled = false;
            lancer_partie_btn_z.Enabled = false;
            chrono_z.Enabled = true;
            etat_host_lb_z.Text = "Non disponible";
            vip_box_z.Enabled = true;
            p2HP_z.Enabled = true;
            p3HP_z.Enabled = true;
            p4HP_z.Enabled = true;
            extra1HP_z.Enabled = true;
            extra2HP_z.Enabled = true;
            extra3HP_z.Enabled = true;
            extra4HP_z.Enabled = true;
            lock_partie_btn_z.Text = "Vérrouiller la partie";
            lock_partie_btn_z.Enabled = false;
            panel_IP_z.Visible = false;
            PChatServer_z.Visible = false;

            ////////// onglet info parametres
            P1IP_z.Text = "0 Points";
            P2IP_z.Enabled = false;
            P3IP_z.Enabled = false;
            P4IP_z.Enabled = false;
            total_players_server_z.Text = "1 joueurs au total";
            nbr_player_chat_z.Text = "1 joueurs";
            chat_area_server_z.Clear();
            vip_box_z.Enabled = true;
            sonor_Host_z.Enabled = true;
            salon_host_z.Enabled = true;
        }

        private void destroyClientPartie_zriwita()
        {
            users_checker.Enabled = false;
            chat_checker.Enabled = false;

            panel_RP_z.Visible = false;
            br1.Enabled = true;
            br2.Enabled = true;
            br3.Enabled = true;
            br4.Enabled = true;
            nom_tbl_RP_z.Enabled = true;
            nom_tbl_RP_z.Clear();
            salon_client_z.Enabled = true;
            sonor_Client_z.Enabled = true;
            etat_client_lb_z.Text = "Non disponible";
            nbr_player_RP_z.Text = "0";
            vipRP_z.Checked = false;
            extra1RP_z.Text = "0";
            extra2RP_z.Text = "0";
            extra3RP_z.Text = "0";
            extra4RP_z.Text = "0";
            liste_playerRP_z.Items.Clear();
            chat_area_client_z.Clear();
            chat_textbox_client_z.Clear();
            rejoindre_btn_z.Text = "S'assoir";
        }

        private void destroyHostPartie_ronda()
        {
            users_checker.Enabled = false;
            chat_checker.Enabled = false;

            br1.Enabled = true;
            br2.Enabled = true;
            br3.Enabled = true;
            br4.Enabled = true;
            //////// onglet heberger une partie
            creer_tbl_btn_r.Enabled = true;
            annuler_tbl_btn_r.Enabled = false;
            lancer_partie_btn_r.Enabled = false;
            chrono_r.Enabled = true;
            etat_host_lb_r.Text = "Non disponible";
            vip_box_r.Enabled = true;
            p2HP_r.Enabled = true;
            p4HP_r.Enabled = true;
            lock_partie_btn_r.Text = "Vérrouiller la partie";
            lock_partie_btn_r.Enabled = false;
            panel_IP_r.Visible = false;
            PChatServer_r.Visible = false;

            ////////// onglet info parametres
            P1IP_r.Text = "0 Points";
            P2IP_r.Enabled = false;
            P3IP_r.Enabled = false;
            P4IP_r.Enabled = false;
            total_players_server_r.Text = "1 joueurs au total";
            nbr_player_chat_r.Text = "1 joueurs";
            chat_area_server_r.Clear();
            vip_box_r.Enabled = true;
            sonor_Host_r.Enabled = true;
            salon_host_r.Enabled = true;
        }

        private void destroyClientPartie_ronda()
        {
            users_checker.Enabled = false;
            chat_checker.Enabled = false;

            panel_RP_r.Visible = false;
            br1.Enabled = true;
            br2.Enabled = true;
            br3.Enabled = true;
            br4.Enabled = true;
            nom_tbl_RP_r.Enabled = true;
            nom_tbl_RP_r.Clear();
            salon_client_r.Enabled = true;
            sonor_Client_r.Enabled = true;
            etat_client_lb_r.Text = "Non disponible";
            nbr_player_RP_r.Text = "0";
            vipRP_r.Checked = false;
            liste_playerRP_r.Items.Clear();
            chat_area_client_r.Clear();
            chat_textbox_client_r.Clear();
            rejoindre_btn_r.Text = "S'assoir";
        }

        private void destroySession()
        {
            syn_network.Enabled = false;
            chat_checker.Enabled = false;
            users_checker.Enabled = false;
            destroyHostPartie_zriwita();
            destroyClientPartie_zriwita();
            destroyClientPartie_ronda();
            destroyHostPartie_ronda();
            session_w.logged = false;
            br1.Enabled = true;
            session_w.id_app = null;
            session_w.nom = null;
            online_pan.Visible = false;
            offline_pan.Visible = true;
            br2.Enabled = false;
            br4.Enabled = true;
            br1.Checked = true;
            chrono_z.Enabled = true;
            chrono_r.Enabled = true;
        }

        private void updateClientIPList(string data)
        {
            string selectedP2 = "(aucun)";
            string selectedP3 = "(aucun)";
            string selectedP4 = "(aucun)";

            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
            {
                if (P2IP_z.Enabled == true && P2IP_z.Items.Count > 1)
                    selectedP2 = P2IP_z.SelectedItem.ToString();

                if (P3IP_z.Enabled == true && P3IP_z.Items.Count > 1)
                    selectedP3 = P3IP_z.SelectedItem.ToString();

                if (P4IP_z.Enabled == true && P4IP_z.Items.Count > 1)
                    selectedP4 = P4IP_z.SelectedItem.ToString();

                P2IP_z.Items.Clear();
                P3IP_z.Items.Clear();
                P4IP_z.Items.Clear();

                P2IP_z.Items.AddRange(new object[] { "(aucun)" });
                P3IP_z.Items.AddRange(new object[] { "(aucun)" });
                P4IP_z.Items.AddRange(new object[] { "(aucun)" });

                P2IP_z.SelectedIndex = 0;
                P3IP_z.SelectedIndex = 0;
                P4IP_z.SelectedIndex = 0;

                string[] data_all = data.Split('/');

                if (data_all.Length > 1)
                {
                    total_players_server_z.ForeColor = Color.Green;
                    nbr_player_chat_z.ForeColor = Color.Green;
                }
                else
                {
                    total_players_server_z.ForeColor = Color.Red;
                    nbr_player_chat_z.ForeColor = Color.Red;
                }

                total_players_server_z.Text = data_all.Length + " joueurs au total";
                nbr_player_chat_z.Text = data_all.Length + " joueurs";

                ////// alimentation des noms d'utilisateurs
                for (int cnt = 1; cnt < data_all.Length; cnt++)
                {
                    string[] data_user = data_all[cnt].Split(':');

                    if (P2IP_z.Enabled == true)
                        P2IP_z.Items.AddRange(new object[] { data_user[0] + " (" + data_user[1] + " Pts)" });

                    if (P3IP_z.Enabled == true)
                        P3IP_z.Items.AddRange(new object[] { data_user[0] + " (" + data_user[1] + " Pts)" });

                    if (P4IP_z.Enabled == true)
                        P4IP_z.Items.AddRange(new object[] { data_user[0] + " (" + data_user[1] + " Pts)" });
                }

                if (P2IP_z.Enabled == true)
                    P2IP_z.SelectedItem = selectedP2;
                if (P3IP_z.Enabled == true)
                    P3IP_z.SelectedItem = selectedP3;
                if (P4IP_z.Enabled == true)
                    P4IP_z.SelectedItem = selectedP4;
            }
            else if (gameListe.SelectedItem.ToString() == "RONDA")
            {
                if (P2IP_r.Enabled == true && P2IP_r.Items.Count > 1)
                    selectedP2 = P2IP_r.SelectedItem.ToString();

                if (P3IP_r.Enabled == true && P3IP_r.Items.Count > 1)
                    selectedP3 = P3IP_r.SelectedItem.ToString();

                if (P4IP_r.Enabled == true && P4IP_r.Items.Count > 1)
                    selectedP4 = P4IP_r.SelectedItem.ToString();

                P2IP_r.Items.Clear();
                P3IP_r.Items.Clear();
                P4IP_r.Items.Clear();

                P2IP_r.Items.AddRange(new object[] { "(aucun)" });
                P3IP_r.Items.AddRange(new object[] { "(aucun)" });
                P4IP_r.Items.AddRange(new object[] { "(aucun)" });

                P2IP_r.SelectedIndex = 0;
                P3IP_r.SelectedIndex = 0;
                P4IP_r.SelectedIndex = 0;

                string[] data_all = data.Split('/');

                if (data_all.Length > 1)
                {
                    total_players_server_r.ForeColor = Color.Green;
                    nbr_player_chat_r.ForeColor = Color.Green;
                }
                else
                {
                    total_players_server_r.ForeColor = Color.Red;
                    nbr_player_chat_r.ForeColor = Color.Red;
                }

                total_players_server_r.Text = data_all.Length + " joueurs au total";
                nbr_player_chat_r.Text = data_all.Length + " joueurs";

                ////// alimentation des noms d'utilisateurs
                for (int cnt = 1; cnt < data_all.Length; cnt++)
                {
                    string[] data_user = data_all[cnt].Split(':');

                    if (P2IP_r.Enabled == true)
                        P2IP_r.Items.AddRange(new object[] { data_user[0] + " (" + data_user[1] + " Pts)" });

                    if (P3IP_r.Enabled == true)
                        P3IP_r.Items.AddRange(new object[] { data_user[0] + " (" + data_user[1] + " Pts)" });

                    if (P4IP_r.Enabled == true)
                        P4IP_r.Items.AddRange(new object[] { data_user[0] + " (" + data_user[1] + " Pts)" });
                }

                if (P2IP_r.Enabled == true)
                    P2IP_r.SelectedItem = selectedP2;
                if (P3IP_r.Enabled == true)
                    P3IP_r.SelectedItem = selectedP3;
                if (P4IP_r.Enabled == true)
                    P4IP_r.SelectedItem = selectedP4;
            }
        }

        private void updateClientRPList(string data1)
        {
            ////// alimentation des noms utilisateurs
            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
            {
                liste_playerRP_z.Items.Clear();
                string[] data_all = data1.Split('/');

                if (data_all.Length > 0)
                    nbr_player_RP_z.ForeColor = Color.Green;
                else
                    nbr_player_RP_z.ForeColor = Color.Red;

                nbr_player_RP_z.Text = data_all.Length.ToString();

                for (int cnt = 0; cnt < data_all.Length; cnt++)
                {
                    string[] data_user = data_all[cnt].Split(':');

                    if (data_user[0] != session_w.user)
                        liste_playerRP_z.Items.AddRange(new object[] { data_user[0] + " (" + data_user[1] + " Pts)" });
                }
                if (liste_playerRP_z.Items.Count > 0)
                    liste_playerRP_z.SelectedIndex = 0;
            }
            else if (gameListe.SelectedItem.ToString() == "RONDA")
            {
                liste_playerRP_r.Items.Clear();
                string[] data_all = data1.Split('/');

                if (data_all.Length > 0)
                    nbr_player_RP_r.ForeColor = Color.Green;
                else
                    nbr_player_RP_r.ForeColor = Color.Red;

                nbr_player_RP_r.Text = data_all.Length.ToString();

                for (int cnt = 0; cnt < data_all.Length; cnt++)
                {
                    string[] data_user = data_all[cnt].Split(':');

                    if (data_user[0] != session_w.user)
                        liste_playerRP_r.Items.AddRange(new object[] { data_user[0] + " (" + data_user[1] + " Pts)" });
                }
                if (liste_playerRP_r.Items.Count > 0)
                    liste_playerRP_r.SelectedIndex = 0;
            }
        }

        internal void playBackSound()
        {
            backsound.Play();
            backsound.PlayLooping();
        }

        internal void updatePts(int pts)
        {
            session_w.points_z = pts;
            stats_pts_z.Text = pts + " Pts";
        }

        private void stopBackSound()
        {
            backsound.Stop();
        }

        private void bs_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (bs_cb.Checked == true)
                playBackSound();
            else
                stopBackSound();
        }

        private void br2_CheckedChanged(object sender, EventArgs e)
        {
            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
            {
                partie_mode_host_zriwita.Visible = true;
                launchGameBTN.Visible = false;
                panel2.Visible = true;
                br3_CheckedChanged(null, null);
            }
            else if (gameListe.SelectedItem.ToString() == "RONDA")
            {
                partie_mode_host_ronda.Visible = true;
                launchGameBTN.Visible = false;
                panel2.Visible = true;
                br3_CheckedChanged(null, null);
            }
            
        }

        private void br1_CheckedChanged(object sender, EventArgs e)
        {
            br3.Checked = true;
            partie_mode_host_zriwita.Visible = false;
            partie_mode_host_ronda.Visible = false;
            partie_mode_client_zriwita.Visible = false;
            partie_mode_client_ronda.Visible = false;
            launchGameBTN.Visible = true;
            panel2.Visible = false;
        }

        private void check_connexion_btn_Click(object sender, EventArgs e)
        {
            // sign in
            if (IsConnected())
            {
                header();
                if (client_syn1.IsBusy == false)
                {
                    client_syn1.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signin=1&login=" + user.Text + "&pwd=" + toMD5(password.Text)));
                    check_connexion_btn.Enabled = false;
                    if (option_loaded == 2)
                        speed_timer.Enabled = true;
                }
                else if (client_syn2.IsBusy == false)
                {
                    client_syn2.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signin=1&login=" + user.Text + "&pwd=" + toMD5(password.Text))) ;
                    check_connexion_btn.Enabled = false;
                    if (option_loaded == 2)
                        speed_timer.Enabled = true;
                }
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void logout_pic_Click(object sender, EventArgs e)
        {
            // sign out
            syn_network.Enabled = false;
            chat_checker.Enabled = false;
            users_checker.Enabled = false;
            speed_timer.Enabled = false;
            if (IsConnected())
            {
                header();
                if(client_syn1.IsBusy==false)
                    client_syn1.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signout=1&id_app=" + session_w.id_app));
                else if(client_syn2.IsBusy==false)
                    client_syn2.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signout=1&id_app=" + session_w.id_app));
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // sign out
            syn_network.Enabled = false;
            chat_checker.Enabled = false;
            users_checker.Enabled = false;
            speed_timer.Enabled = false;
            if (online_pan.Visible == true)
            {
                if (IsConnected())
                {
                    header();
                    if (client_syn1.IsBusy == false)
                        client_syn1.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signout=1&id_app=" + session_w.id_app));
                    else if (client_syn2.IsBusy == false)
                        client_syn2.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?signout=1&id_app=" + session_w.id_app));
                    /*else
                        noWebClientfree();*/
                    Thread.Sleep(1000);
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void syn_network_Tick(object sender, EventArgs e)
        {
            // synchronisation
            if (IsConnected())
            {
                header();
                if (client_syn1.IsBusy == false)
                {
                    client_syn1.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?syn=1&id_app=" + session_w.id_app));
                    if (option_loaded == 2)
                    {
                        speed_cnt = 0;
                        speed_timer.Enabled = true;
                    }
                }
                else if (client_syn2.IsBusy == false)
                {
                    client_syn2.DownloadStringAsync(new Uri(hosted_server + "/system/interroge_app_auth.php?syn=1&id_app=" + session_w.id_app));
                    if (option_loaded == 2)
                    {
                        speed_cnt = 0;
                        speed_timer.Enabled = true;
                    }
                }
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void creer_tbl_btn_Click(object sender, EventArgs e)
        {
            // sauveguard des extras
            Properties.Settings.Default.Set_extra1 = Int16.Parse(extra1HP_z.SelectedItem.ToString());
            Properties.Settings.Default.Set_extra2 = Int16.Parse(extra2HP_z.SelectedItem.ToString());
            Properties.Settings.Default.Set_extra3 = Int16.Parse(extra3HP_z.SelectedItem.ToString());
            Properties.Settings.Default.Set_extra4 = Int16.Parse(extra4HP_z.SelectedItem.ToString());

            partie_mode_host_zriwita.Visible = false;
            loading_lb_value.Text = "Creation de la partie, veillez patienter.\nMerci de se reconnecter si c'est lent";
            loading_panel.Visible = true;
            loading_panel.BringToFront();

            if (p2HP_z.Checked == true)
                session_w.nbr_player = 2;
            else if (p3HP_z.Checked == true)
                session_w.nbr_player = 3;
            else
                session_w.nbr_player = 4;

            if (vip_box_z.Checked == true)
                session_w.vip_zriwita = 1;
            else
                session_w.vip_zriwita = 0;

            if (chrono_z.Checked == true)
                session_w.chrono = 1;
            else
                session_w.chrono = 0;

            // création d'une partie.
            if (IsConnected())
            {
                header();
                int vip_stats;
                if(vip_box_z.Checked==true)
                    vip_stats=1;
                else vip_stats=0;

                if (client1.IsBusy==false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?make=1&id_app=" + session_w.id_app + "&players=" + session_w.nbr_player + "&vip=" + vip_stats + "&extra1=" + Properties.Settings.Default.Set_extra1 + "&extra2=" + Properties.Settings.Default.Set_extra2 + "&extra3=" + Properties.Settings.Default.Set_extra3 + "&extra4=" + Properties.Settings.Default.Set_extra4 + "&salon=" + salon_host_z.SelectedItem + "&profile=" + sonor_Host_z.SelectedItem + "&chrono=" + session_w.chrono));
                else if (client2.IsBusy==false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?make=1&id_app=" + session_w.id_app + "&players=" + session_w.nbr_player + "&vip=" + vip_stats + "&extra1=" + Properties.Settings.Default.Set_extra1 + "&extra2=" + Properties.Settings.Default.Set_extra2 + "&extra3=" + Properties.Settings.Default.Set_extra3 + "&extra4=" + Properties.Settings.Default.Set_extra4 + "&salon=" + salon_host_z.SelectedItem + "&profile=" + sonor_Host_z.SelectedItem + "&chrono=" + session_w.chrono));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?make=1&id_app=" + session_w.id_app + "&players=" + session_w.nbr_player + "&vip=" + vip_stats + "&extra1=" + Properties.Settings.Default.Set_extra1 + "&extra2=" + Properties.Settings.Default.Set_extra2 + "&extra3=" + Properties.Settings.Default.Set_extra3 + "&extra4=" + Properties.Settings.Default.Set_extra4 + "&salon=" + salon_host_z.SelectedItem + "&profile=" + sonor_Host_z.SelectedItem + "&chrono=" + session_w.chrono));
                else
                    noWebClientfree();
            }
            else
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet, merci de vérifier", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void annuler_tbl_btn_Click(object sender, EventArgs e)
        {
            // supprimer la partie.
            if (IsConnected())
            {
                header();
                if(client1.IsBusy==false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?destroy=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else if(client2.IsBusy==false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?destroy=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else if(client3.IsBusy==false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?destroy=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void br3_CheckedChanged(object sender, EventArgs e)
        {
            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
            {
                partie_mode_host_zriwita.Visible = true;
                partie_mode_client_zriwita.Visible = false;

                ////// extra mode hebergeur
                if (Properties.Settings.Default.Set_extra1 > 9)
                    extra1HP_z.SelectedIndex = Properties.Settings.Default.Set_extra1 - 3;
                else
                    extra1HP_z.SelectedIndex = Properties.Settings.Default.Set_extra1 - 1;

                if (Properties.Settings.Default.Set_extra2 > 9)
                    extra2HP_z.SelectedIndex = Properties.Settings.Default.Set_extra2 - 3;
                else
                    extra2HP_z.SelectedIndex = Properties.Settings.Default.Set_extra2 - 1;

                if (Properties.Settings.Default.Set_extra3 > 9)
                    extra3HP_z.SelectedIndex = Properties.Settings.Default.Set_extra3 - 3;
                else
                    extra3HP_z.SelectedIndex = Properties.Settings.Default.Set_extra3 - 1;

                if (Properties.Settings.Default.Set_extra4 > 9)
                    extra4HP_z.SelectedIndex = Properties.Settings.Default.Set_extra4 - 3;
                else
                    extra4HP_z.SelectedIndex = Properties.Settings.Default.Set_extra4 - 1;

                /////// extra mode hebergeur stats
                extra1IP_z.Text = (Properties.Settings.Default.Set_extra1).ToString();
                extra2IP_z.Text = (Properties.Settings.Default.Set_extra2).ToString();
                extra3IP_z.Text = (Properties.Settings.Default.Set_extra3).ToString();
                extra4IP_z.Text = (Properties.Settings.Default.Set_extra4).ToString();
            }
            else if (gameListe.SelectedItem.ToString() == "RONDA")
            {
                if (br3.Checked == true)
                {
                    partie_mode_host_ronda.Visible = true;
                    partie_mode_client_ronda.Visible = false;
                }
            }
        }

        private void br4_CheckedChanged(object sender, EventArgs e)
        {
            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
            {
                partie_mode_host_zriwita.Visible = false;
                partie_mode_client_zriwita.Visible = true;
            }
            else if (gameListe.SelectedItem.ToString() == "RONDA")
            {
                if (br4.Checked == true)
                {
                    partie_mode_host_ronda.Visible = false;
                    partie_mode_client_ronda.Visible = true;
                }
            }
        }

        private void lock_partie_btn_Click(object sender, EventArgs e)
        {
            if (lock_partie_btn_z.Text == "Vérrouiller la partie")
            {
                if (IsConnected())
                {
                    header();
                    if(client1.IsBusy==false)
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?locked=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else if(client2.IsBusy==false)
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?locked=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else if (client3.IsBusy == false)
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?locked=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else
                        noWebClientfree();
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (IsConnected())
                {
                    header();
                    if(client1.IsBusy==false)
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?locked=0&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else if(client2.IsBusy==false)
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?locked=0&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else if (client3.IsBusy == false)
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?locked=0&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else
                        noWebClientfree();
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void rejoindre_btn_Click(object sender, EventArgs e)
        {
            if (rejoindre_btn_z.Text == "S'assoir")
            {
                if (nom_tbl_RP_z.Text != "" && nom_tbl_RP_z.Text.Length>6 && nom_tbl_RP_z.Text.Substring(0,6)=="table_")
                {
                    if (IsConnected())
                    {
                        header();
                        if (client1.IsBusy == false)
                        {
                            client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem + "&profile=" + sonor_Client_z.SelectedItem));
                            rejoindre_btn_z.Enabled = false;
                        }
                        else if (client2.IsBusy == false)
                        {
                            client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem + "&profile=" + sonor_Client_z.SelectedItem));
                            rejoindre_btn_z.Enabled = false;
                        }
                        else if (client3.IsBusy == false)
                        {
                            client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem + "&profile=" + sonor_Client_z.SelectedItem));
                            rejoindre_btn_z.Enabled = false;
                        }
                        else
                            noWebClientfree();
                        //rejoindre_btn.Enabled = false;
                    }
                    else
                    {
                        destroySession();
                        MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (nom_tbl_RP_z.Text != "")
                {
                    if (IsConnected())
                    {
                        header();
                        if(client1.IsBusy==false)
                            client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=table_" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem + "&profile=" + sonor_Client_z.SelectedItem));
                        else if(client2.IsBusy==false)
                            client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=table_" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem + "&profile=" + sonor_Client_z.SelectedItem));
                        else if (client3.IsBusy == false)
                            client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=table_" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem + "&profile=" + sonor_Client_z.SelectedItem));
                        else
                            noWebClientfree();
                    }
                    else
                    {
                        destroySession();
                        MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(this,"Veillez donner un nom valide à votre partie", "Erreur de formulaire", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nom_tbl_RP_z.Focus();
                }
            }
            else
            {
                // requette pour sortir de la table
                if (IsConnected())
                {
                    header();
                    if(client1.IsBusy==false)
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?disconnect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem));
                    else if(client2.IsBusy==false)
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?disconnect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem));
                    else if (client3.IsBusy == false)
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?disconnect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_z.Text + "&salon=" + salon_client_z.SelectedItem));
                    else
                        noWebClientfree();
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private bool IsConnected()
        {
            try
            {
                System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient(hosted_server.Substring(7), 80);
                clnt.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        private void users_cherker_Tick(object sender, EventArgs e)
        {
            ////// timer qui cherche les nouveau membres ajoutés a la partie
            if (br3.Checked == true)
            {
                if (IsConnected())
                {
                    header();
                    if (client_users_checker1.IsBusy == false)
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                            client_users_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                            client_users_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                    }
                    else if (client_users_checker2.IsBusy == false)
                    {
                        if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                            client_users_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                        else if (gameListe.SelectedItem.ToString() == "RONDA")
                            client_users_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                    }
                    else
                    {
                        //noWebClientfree();
                        if (br3.Checked)
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                chat_area_server_z.Text = chat_area_server_z.Text + "\n" + @"SYSTEM\>Impossible de récuperer la liste des joueurs";
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                chat_area_server_r.Text = chat_area_server_r.Text + "\n" + @"SYSTEM\>Impossible de récuperer la liste des joueurs";
                        }
                        else
                        {
                            if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                                chat_area_client_z.Text = chat_area_client_z.Text + "\n" + @"SYSTEM\>Impossible de récuperer la liste des joueurs";
                            else if (gameListe.SelectedItem.ToString() == "RONDA")
                                chat_area_client_r.Text = chat_area_client_r.Text + "\n" + @"SYSTEM\>Impossible de récuperer la liste des joueurs";
                        }
                    }
                }
                else
                {
                    //syn_network.Enabled = false;
                    //chat_checker.Enabled = false;
                    //users_checker.Enabled = false;
                    //speed_timer.Enabled = false;
                    //MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet, votre session se fermera automatiquement\nMerci de verifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                {
                    if (IsConnected())
                    {
                        header();
                        if (client_users_checker1.IsBusy == false)
                            client_users_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_client_z.SelectedItem));
                        else if (client_users_checker2.IsBusy == false)
                            client_users_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_client_z.SelectedItem));
                        else
                        {
                            //noWebClientfree();
                            if (br3.Checked)
                                chat_area_server_z.Text = chat_area_server_z.Text + "\n" + @"SYSTEM\>Impossible de récuperer la liste des joueurs";
                            else
                                chat_area_client_z.Text = chat_area_client_z.Text + "\n" + @"SYSTEM\>Impossible de récuperer la liste des joueurs";
                        }
                    }
                    else
                    {
                        //syn_network.Enabled = false;
                        //chat_checker.Enabled = false;
                        //users_checker.Enabled = false;
                        //speed_timer.Enabled = false;
                        //MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet, votre session se fermera automatiquement\nMerci de verifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (gameListe.SelectedItem.ToString() == "RONDA")
                {
                    if (IsConnected())
                    {
                        header();
                        if (client_users_checker1.IsBusy == false)
                            client_users_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_client_r.SelectedItem));
                        else if (client_users_checker2.IsBusy == false)
                            client_users_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_client_r.SelectedItem));
                        else
                        {
                            //noWebClientfree();
                            if (br3.Checked)
                                chat_area_server_r.Text = chat_area_server_r.Text + "\n" + @"SYSTEM\>Impossible de récuperer la liste des joueurs";
                            else
                                chat_area_client_r.Text = chat_area_client_r.Text + "\n" + @"SYSTEM\>Impossible de récuperer la liste des joueurs";
                        }
                    }
                    else
                    {
                        //syn_network.Enabled = false;
                        //chat_checker.Enabled = false;
                        //users_checker.Enabled = false;
                        //speed_timer.Enabled = false;
                        //MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet, votre session se fermera automatiquement\nMerci de verifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private string toMD5(string strText)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(strText);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        private void chat_checker_Tick(object sender, EventArgs e)
        {
            ////// timer qui cherche les nouveau membres ajoutés a la partie
            if (IsConnected())
            {
                header();
                if (gameListe.SelectedItem.ToString() == "ZRIWITA")
                {
                    if (br3.Checked == true)
                    {
                        if (client_chat_checker1.IsBusy == false)
                            client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                        else if (client_chat_checker2.IsBusy == false)
                            client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                        else
                        {
                            //noWebClientfree();
                            if (br3.Checked)
                                chat_area_server_z.Text = chat_area_server_z.Text + "\n" + @"SYSTEM\>Impossible de récuperer l'historique de conversation";
                            else
                                chat_area_client_z.Text = chat_area_client_z.Text + "\n" + @"SYSTEM\>Impossible de récuperer l'historique de conversation";
                        }
                    }
                    else
                    {
                        if (client_chat_checker1.IsBusy == false)
                            client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_client_z.SelectedItem));
                        else if (client_chat_checker2.IsBusy == false)
                            client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_client_z.SelectedItem));
                        else
                        {
                            //noWebClientfree();
                            if (br3.Checked)
                                chat_area_server_z.Text = chat_area_server_z.Text + "\n" + @"SYSTEM\>Impossible de récuperer l'historique de conversation";
                            else
                                chat_area_client_z.Text = chat_area_client_z.Text + "\n" + @"SYSTEM\>Impossible de récuperer l'historique de conversation";
                        }
                    }
                }
                else if (gameListe.SelectedItem.ToString() == "RONDA")
                {
                    if (br3.Checked == true)
                    {
                        if (client_chat_checker1.IsBusy == false)
                            client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                        else if (client_chat_checker2.IsBusy == false)
                            client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                        else
                        {
                            //noWebClientfree();
                            if (br3.Checked)
                                chat_area_server_r.Text = chat_area_server_r.Text + "\n" + @"SYSTEM\>Impossible de récuperer l'historique de conversation";
                            else
                                chat_area_client_r.Text = chat_area_client_r.Text + "\n" + @"SYSTEM\>Impossible de récuperer l'historique de conversation";
                        }
                    }
                    else
                    {
                        if (client_chat_checker1.IsBusy == false)
                            client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_client_r.SelectedItem));
                        else if (client_chat_checker2.IsBusy == false)
                            client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?get_msg=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_client_r.SelectedItem));
                        else
                        {
                            //noWebClientfree();
                            if (br3.Checked)
                                chat_area_server_r.Text = chat_area_server_r.Text + "\n" + @"SYSTEM\>Impossible de récuperer l'historique de conversation";
                            else
                                chat_area_client_r.Text = chat_area_client_r.Text + "\n" + @"SYSTEM\>Impossible de récuperer l'historique de conversation";
                        }
                    }
                }
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chat_send_btn_Click(object sender, EventArgs e)
        {
            if (IsConnected())
            {
                if (chat_textbox_client_z.Text != "")
                {
                    header();
                    string msg = chat_textbox_client_z.Text;
                    msg = msg.Replace(":", ";");
                    msg = msg.Replace("/", "|");
                    msg = msg.Replace("#", "%");
                    
                    if(client_chat_checker1.IsBusy==false)
                        client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_textbox_client_z.Text + "&salon=" + salon_host_z.SelectedItem));
                    else if (client_chat_checker2.IsBusy == false)
                        client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_textbox_client_z.Text + "&salon=" + salon_host_z.SelectedItem));
                    else
                    {
                        //noWebClientfree();
                        if (br3.Checked)
                            chat_area_server_z.Text = chat_area_server_z.Text + "\n" + @"SYSTEM\>Impossible de soumettre votre message";
                        else
                            chat_area_client_z.Text = chat_area_client_z.Text + "\n" + @"SYSTEM\>Impossible de soumettre votre message";
                    }
                    
                    chat_textbox_client_z.Text = "";
                }
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chat_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                chat_send_btn_Click(null, null);
        }

        private void chat_send_btn_server_Click(object sender, EventArgs e)
        {
            if (IsConnected())
            {
                if (chat_textbox_server_z.Text != "")
                {
                    header();
                    string msg = chat_textbox_server_z.Text;
                    msg = msg.Replace(":", ";");
                    msg = msg.Replace("/", "|");
                    msg = msg.Replace("#", "!");

                    if(client_chat_checker1.IsBusy==false)
                        client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_textbox_server_z.Text + "&salon=" + salon_host_z.SelectedItem));
                    else if(client_chat_checker2.IsBusy==false)
                        client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_textbox_server_z.Text + "&salon=" + salon_host_z.SelectedItem));
                    
                    chat_textbox_server_z.Text = "";
                }
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chat_textbox_server_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                chat_send_btn_server_Click(null, null);
        }

        private void lancer_partie_btn_Click(object sender, EventArgs e)
        {
            // controle des données
            if (P2IP_z.SelectedItem.ToString() == "(aucun)" && P3IP_z.SelectedItem.ToString() == "(aucun)" && P2IP_z.SelectedItem.ToString() == "(aucun)")
            {
                MessageBox.Show(this, "Veillez choisir un joueur valide", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                partie_mode_host_zriwita.SelectTab(1);
            }
            else
            {
                bool ok = true;
                if (session_w.nbr_player == 3 && P2IP_z.SelectedIndex == P3IP_z.SelectedIndex)
                    ok = false;
                else if (session_w.nbr_player == 4 && P2IP_z.SelectedIndex == P3IP_z.SelectedIndex && P2IP_z.SelectedIndex != P4IP_z.SelectedIndex && P3IP_z.SelectedIndex != P4IP_z.SelectedIndex)
                    ok = false;

                if (ok == true)
                {
                    partie_mode_host_zriwita.Visible = false;
                    loading_lb_value.Text = "Initialisation en cours ...";
                    loading_panel.Visible = true;
                    loading_panel.BringToFront();
                    step1_host_zriwita();
                }
                else
                    MessageBox.Show(this, "Vous avez choisie un joueur en double, mérci de rectifier", "Erreur de formulaire", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void step1_client_zriwita()
        {
            partie_mode_client_zriwita.Visible = false;
            loading_lb_value.Text = "Initialisation des données en cours ...";
            loading_panel.Visible = true;
            step2_client_zriwita();
        }

        private void step1_host_zriwita()
        {
            if (IsConnected())
            {
                string users = "";

                if (session_w.nbr_player == 2)
                    users = P2IP_z.SelectedItem.ToString().Split(' ')[0];
                else if (session_w.nbr_player == 3)
                    users = P2IP_z.SelectedItem.ToString().Split(' ')[0] + '/' + P3IP_z.SelectedItem.ToString().Split(' ')[0];
                else if (session_w.nbr_player == 4)
                    users = P2IP_z.SelectedItem.ToString().Split(' ')[0] + '/' + P3IP_z.SelectedItem.ToString().Split(' ')[0] + '/' + P4IP_z.SelectedItem.ToString().Split(' ')[0];
                
                header();
                if(client1.IsBusy==false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players_info=1&users=" + users + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else if(client2.IsBusy==false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players_info=1&users=" + users + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players_info=1&users=" + users + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void step2_client_zriwita()
        {
            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players_info=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&client=1" + "&salon=" + salon_client_z.SelectedItem));
                else if (client2.IsBusy == false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players_info=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&client=1" + "&salon=" + salon_client_z.SelectedItem));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/connect_client.php?get_players_info=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&client=1" + "&salon=" + salon_client_z.SelectedItem));
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void step2_host_zriwita()
        {
            loading_lb_value.Text = "Importation des données réussit";

            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?etat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&lock=1&salon=" + salon_host_z.SelectedItem));
                else if (client2.IsBusy == false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?etat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&lock=1&salon=" + salon_host_z.SelectedItem));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/zriwita/make_partie.php?etat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&lock=1&salon=" + salon_host_z.SelectedItem));
                else
                    noWebClientfree();
            }
            else
            {
                loading_panel.Visible = false;
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void step3_zriwita()
        {
            if (lanched_game == false)
            {
                lanched_game = true;
                syn_network.Enabled = false;
                chat_checker.Enabled = false;
                users_checker.Enabled = false;
                loading_lb_value.Text = "Lancement de la partie, patienter ...";
                C4RT4.zriwitaR _zriwitaR = new C4RT4.zriwitaR(session_w, p[0], p[1], p[2], p[3]);
                _main = this;
                this.Hide();
                stopBackSound();
                _zriwitaR.Show();
                loading_panel.Visible = false;
                if (br3.Checked == true)
                {
                    destroyHostPartie_zriwita();
                    br3_CheckedChanged(null, null);
                }
                else
                {
                    destroyClientPartie_zriwita();
                    br4_CheckedChanged(null, null);
                }
            }
        }

        private void step1_client_ronda()
        {
            partie_mode_client_ronda.Visible = false;
            loading_lb_value.Text = "Initialisation des données en cours ...";
            loading_panel.Visible = true;
            step2_client_ronda();
        }

        private void step1_host_ronda()
        {
            if (IsConnected())
            {
                string users = "";

                if (session_w.nbr_player == 2)
                    users = P2IP_r.SelectedItem.ToString().Split(' ')[0];
                else if (session_w.nbr_player == 3)
                    users = P2IP_r.SelectedItem.ToString().Split(' ')[0] + '/' + P3IP_r.SelectedItem.ToString().Split(' ')[0];
                else if (session_w.nbr_player == 4)
                    users = P2IP_r.SelectedItem.ToString().Split(' ')[0] + '/' + P3IP_r.SelectedItem.ToString().Split(' ')[0] + '/' + P4IP_r.SelectedItem.ToString().Split(' ')[0];

                header();
                if (client1.IsBusy == false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players_info=1&users=" + users + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                else if (client2.IsBusy == false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players_info=1&users=" + users + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players_info=1&users=" + users + "&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void step2_client_ronda()
        {
            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players_info=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&client=1" + "&salon=" + salon_client_r.SelectedItem));
                else if (client2.IsBusy == false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players_info=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&client=1" + "&salon=" + salon_client_r.SelectedItem));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?get_players_info=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&client=1" + "&salon=" + salon_client_r.SelectedItem));
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void step2_host_ronda()
        {
            loading_lb_value.Text = "Importation des données réussit";

            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&lock=1&salon=" + salon_host_r.SelectedItem));
                else if (client2.IsBusy == false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&lock=1&salon=" + salon_host_r.SelectedItem));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?etat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&lock=1&salon=" + salon_host_r.SelectedItem));
                else
                    noWebClientfree();
            }
            else
            {
                loading_panel.Visible = false;
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void step3_ronda()
        {
            if (lanched_game == false)
            {
                lanched_game = true;
                syn_network.Enabled = false;
                chat_checker.Enabled = false;
                users_checker.Enabled = false;
                loading_lb_value.Text = "Lancement de la partie, patienter ...";
                C4RT4.rondaR _rondaR = new C4RT4.rondaR(session_w, p[0], p[1], p[2], p[3]);
                _main = this;
                this.Hide();
                stopBackSound();
                _rondaR.Show();
                loading_panel.Visible = false;
                if (br3.Checked == true)
                {
                    destroyHostPartie_ronda();
                    br3_CheckedChanged(null, null);
                }
                else
                {
                    destroyClientPartie_ronda();
                    br4_CheckedChanged(null, null);
                }
            }
        }

        private void nom_tbl_RP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                rejoindre_btn_Click(null, null);
        }

        private void vip_box_CheckStateChanged(object sender, EventArgs e)
        {
            if (vip_box_z.Checked == true)
                vip_pic_z.Image = Properties.Resources.vip;
            else
                vip_pic_z.Image = Properties.Resources.vipGray;
        }

        private void chat_textbox_server_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                chat_send_btn_server_Click(null, null);
        }

        private void noWebClientfree()
        {
            MessageBox.Show(this, "Une erreur interne est survenu,votre connexion semble être saturé,\n merci de relencer l'application", "Erreur interne", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            parametresR _parametersR = new parametresR(this);
            _parametersR.Show();
            this.Enabled = false;
        }

        internal void global_load()
        {
            if (File.Exists("save\\global.xml"))
            {
                XmlTextReader xml_global = new XmlTextReader("save\\global.xml");
                xml_global.WhitespaceHandling = WhitespaceHandling.None;
                while (xml_global.Read())
                {
                    if (xml_global.LocalName == "option")
                    {
                        option_loaded = Int16.Parse(xml_global.ReadString());
                        xml_global.Read();
                    }

                    if (xml_global.LocalName == "speed")
                    {
                        speed_loaded = Int16.Parse(xml_global.ReadString()) * 1000;
                        xml_global.Read();
                    }

                    if (xml_global.LocalName == "url")
                    {
                        hosted_server = xml_global.ReadString();
                        xml_global.Read();
                    }

                    if (xml_global.LocalName == "dns")
                        xml_global.Read();
                }
                xml_global.Close();

                ////////////// option ///////////
                if (option_loaded == 1)
                {
                    users_checker.Interval = 4000;
                    chat_checker.Interval = 4000;
                }
                else if (option_loaded == 3)
                {
                    users_checker.Interval = speed_loaded * 1000;
                    chat_checker.Interval = speed_loaded * 1000;
                }
            }
            else
            {
                MessageBox.Show(this, "Le fichier de sauveguard est introuvable,\nmerci de réinstaller l'application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void speed_timer_Tick(object sender, EventArgs e)
        {
            speed_cnt++;
        }

        private void gameListe_SelectedIndexChanged(object sender, EventArgs e)
        {
            partie_mode_client_ronda.Visible = false;
            partie_mode_client_zriwita.Visible = false;
            partie_mode_host_ronda.Visible = false;
            partie_mode_host_zriwita.Visible = false;

            if (br2.Checked == true)
            {
                br3_CheckedChanged(null, null);
                br3.Checked = true;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            partie_mode_host_ronda.Visible = false;
            loading_lb_value.Text = "Création de la partie, veillez patienter.";
            loading_panel.Visible = true;
            loading_panel.BringToFront();

            if (p2HP_r.Checked == true)
                session_w.nbr_player = 2;
            else
                session_w.nbr_player = 4;

            if (vip_box_r.Checked == true)
                session_w.vip_ronda = 1;
            else
                session_w.vip_ronda = 0;

            if (chrono_r.Checked == true)
                session_w.chrono = 1;
            else
                session_w.chrono = 0;

            // création d'une partie.
            if (IsConnected())
            {
                header();
                int vip_stats;
                if (vip_box_r.Checked == true)
                    vip_stats = 1;
                else vip_stats = 0;

                if (client1.IsBusy == false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?make=1&id_app=" + session_w.id_app + "&players=" + session_w.nbr_player + "&vip=" + vip_stats + "&salon=" + salon_host_r.SelectedItem + "&profile=" + sonor_Host_r.SelectedItem + "&chrono=" + session_w.chrono));
                else if (client2.IsBusy == false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?make=1&id_app=" + session_w.id_app + "&players=" + session_w.nbr_player + "&vip=" + vip_stats + "&salon=" + salon_host_r.SelectedItem + "&profile=" + sonor_Host_r.SelectedItem + "&chrono=" + session_w.chrono));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?make=1&id_app=" + session_w.id_app + "&players=" + session_w.nbr_player + "&vip=" + vip_stats + "&salon=" + salon_host_r.SelectedItem + "&profile=" + sonor_Host_r.SelectedItem + "&chrono=" + session_w.chrono));
                else
                    noWebClientfree();
            }
            else
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet, merci de vérifier", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // supprimer la partie.
            if (IsConnected())
            {
                header();
                if (client1.IsBusy == false)
                    client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?destroy=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else if (client2.IsBusy == false)
                    client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?destroy=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else if (client3.IsBusy == false)
                    client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?destroy=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                else
                    noWebClientfree();
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // boutton verouiller
        private void button12_Click(object sender, EventArgs e)
        {
            if (lock_partie_btn_r.Text == "Vérrouiller la partie")
            {
                if (IsConnected())
                {
                    header();
                    if (client1.IsBusy == false)
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?locked=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                    else if (client2.IsBusy == false)
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?locked=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                    else if (client3.IsBusy == false)
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?locked=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_r.SelectedItem));
                    else
                        noWebClientfree();
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (IsConnected())
                {
                    header();
                    if (client1.IsBusy == false)
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?locked=0&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else if (client2.IsBusy == false)
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?locked=0&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else if (client3.IsBusy == false)
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/make_partie.php?locked=0&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&salon=" + salon_host_z.SelectedItem));
                    else
                        noWebClientfree();
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet.\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // controle des données
            
            if (P2IP_r.SelectedItem.ToString() == "(aucun)" && P3IP_r.SelectedItem.ToString() == "(aucun)" && P2IP_r.SelectedItem.ToString() == "(aucun)")
            {
                MessageBox.Show(this, "Veillez choisir un joueur valide.", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                partie_mode_host_ronda.SelectTab(1);
            }
            else
            {
                bool ok = true;
                if (session_w.nbr_player == 3 && P2IP_r.SelectedIndex == P3IP_r.SelectedIndex)
                    ok = false;
                else if (session_w.nbr_player == 4 && P2IP_r.SelectedIndex == P3IP_r.SelectedIndex && P2IP_r.SelectedIndex != P4IP_r.SelectedIndex && P3IP_r.SelectedIndex != P4IP_r.SelectedIndex)
                    ok = false;

                if (ok == true)
                {
                    partie_mode_host_ronda.Visible = false;
                    loading_lb_value.Text = "Initialisation en cours ...";
                    loading_panel.Visible = true;
                    loading_panel.BringToFront();
                    step1_host_ronda();
                }
                else
                    MessageBox.Show(this, "Vous avez choisie un joueur en double, mérci de réctifier", "Erreur de formulaire", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void vip_box_r_CheckStateChanged(object sender, EventArgs e)
        {
            if (vip_box_r.Checked == true)
                vip_pic_r.Image = Properties.Resources.vip;
            else
                vip_pic_r.Image = Properties.Resources.vipGray;
        }

        private void chat_send_btn_server_r_Click(object sender, EventArgs e)
        {
            if (IsConnected())
            {
                if (chat_textbox_server_r.Text != "")
                {
                    header();
                    string msg = chat_textbox_server_r.Text;
                    msg = msg.Replace(":", ";");
                    msg = msg.Replace("/", "|");
                    msg = msg.Replace("#", "!");

                    if (client_chat_checker1.IsBusy == false)
                        client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_textbox_server_r.Text + "&salon=" + salon_host_r.SelectedItem));
                    else if (client_chat_checker2.IsBusy == false)
                        client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_textbox_server_r.Text + "&salon=" + salon_host_r.SelectedItem));

                    chat_textbox_server_r.Text = "";
                }
            }
            else
            {
                destroySession();
                MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chat_textbox_server_r_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                chat_send_btn_server_r_Click(null, null);
        }

        private void rejoindre_btn_r_Click(object sender, EventArgs e)
        {
            if (rejoindre_btn_r.Text == "S'assoir")
            {
                if (nom_tbl_RP_r.Text != "" && nom_tbl_RP_r.Text.Length > 6 && nom_tbl_RP_r.Text.Substring(0, 6) == "table_")
                {
                    if (IsConnected())
                    {
                        header();
                        if (client1.IsBusy == false)
                        {
                            client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem + "&profile=" + sonor_Client_r.SelectedItem));
                            rejoindre_btn_r.Enabled = false;
                        }
                        else if (client2.IsBusy == false)
                        {
                            client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem + "&profile=" + sonor_Client_r.SelectedItem));
                            rejoindre_btn_r.Enabled = false;
                        }
                        else if (client3.IsBusy == false)
                        {
                            client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem + "&profile=" + sonor_Client_r.SelectedItem));
                            rejoindre_btn_r.Enabled = false;
                        }
                        else
                            noWebClientfree();
                        //rejoindre_btn.Enabled = false;
                    }
                    else
                    {
                        destroySession();
                        MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (nom_tbl_RP_r.Text != "")
                {
                    if (IsConnected())
                    {
                        header();
                        if (client1.IsBusy == false)
                            client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=table_" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem + "&profile=" + sonor_Client_r.SelectedItem));
                        else if (client2.IsBusy == false)
                            client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=table_" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem + "&profile=" + sonor_Client_r.SelectedItem));
                        else if (client3.IsBusy == false)
                            client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?connect=1&id_app=" + session_w.id_app + "&partie=table_" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem + "&profile=" + sonor_Client_r.SelectedItem));
                        else
                            noWebClientfree();
                    }
                    else
                    {
                        destroySession();
                        MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Veillez donner un nom valide à votre partie", "Erreur de formulaire", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nom_tbl_RP_r.Focus();
                }
            }
            else
            {
                // requette pour sortir de la table
                if (IsConnected())
                {
                    header();
                    if (client1.IsBusy == false)
                        client1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?disconnect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem));
                    else if (client2.IsBusy == false)
                        client2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?disconnect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem));
                    else if (client3.IsBusy == false)
                        client3.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/connect_client.php?disconnect=1&id_app=" + session_w.id_app + "&partie=" + nom_tbl_RP_r.Text + "&salon=" + salon_client_r.SelectedItem));
                    else
                        noWebClientfree();
                }
                else
                {
                    destroySession();
                    MessageBox.Show(this, "Probleme de connexion internet\nVous n'êtes pas connecter sur internet,\nVotre session va être terminé, merci de vérifier votre connexion", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void chat_textbox_client_r_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                chat_send_btn_client_r_Click(null, null);
        }

        private void chat_send_btn_client_r_Click(object sender, EventArgs e)
        {
            if (IsConnected())
            {
                if (chat_textbox_client_r.Text != "")
                {
                    header();
                    string msg = chat_textbox_client_r.Text;
                    msg = msg.Replace(":", ";");
                    msg = msg.Replace("/", "|");
                    msg = msg.Replace("#", "%");

                    if (client_chat_checker1.IsBusy == false)
                        client_chat_checker1.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_textbox_client_r.Text + "&salon=" + salon_host_r.SelectedItem));
                    else if (client_chat_checker2.IsBusy == false)
                        client_chat_checker2.DownloadStringAsync(new Uri(hosted_server + "/system/ronda/chat.php?chat=1&id_app=" + session_w.id_app + "&partie=" + session_w.nom_table + "&msg=" + chat_textbox_client_r.Text + "&salon=" + salon_host_r.SelectedItem));
                    else
                    {
                        //noWebClientfree();
                        if (br3.Checked)
                            chat_area_server_r.Text = chat_area_server_r.Text + "\n" + @"SYSTEM\>Impossible de soumettre votre message";
                        else
                            chat_area_client_r.Text = chat_area_client_r.Text + "\n" + @"SYSTEM\>Impossible de soumettre votre message";
                    }

                    chat_textbox_client_r.Text = "";
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