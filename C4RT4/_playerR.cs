using System;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C4RT4
{
    public class _playerR
    {
        public string nom, user, id_app, avatar , profile, ville, team;
        public int points,points_vip_zriwta, points_vip_ronda, won, lose, position;
        public string[] cards = new string[15];
        public int block = 0;
        public bool sound;
        //public zriwitaR _zriwitaR = null;

        // contructeur
        public _playerR(/*zriwitaR _form*/)
        {
            //_zriwitaR = _form;
        }

        ~_playerR()
        {
            nom = null;
            avatar = null;
            cards = null;
            block = 0;
            
            GC.Collect();
        }

        public void destructor()
        {
            /*nom = null;
            avatar = null;
            block = 0;

            for (int cnt = 0; cnt < cards.Length; cnt++)
            {
                cards[cnt] = null;
                _zriwitaR.p_card[position, cnt].Visible = false;
            }

            System.GC.Collect();*/
        }

        public void destructor2()
        {
            /*for (int cnt = 0; cards[cnt] != null; cnt++)
            {
                cards[cnt] = null;
                _zriwitaR.p_card[position, cnt].Visible = false;
            }
            System.GC.Collect();*/
        }
    }
}
