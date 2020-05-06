using System;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C4RT4
{
    public class _player
    {
        public string nom;
        public string avatar;
        public string level;
        public string[] cards=new string[15];
        public int block = 0;
        public string type;
        public int position;
        public bool sound;
        public zriwita _zriwita = null;
        
        

        // contructeur
        public _player(zriwita _form)
        {
            _zriwita = _form;
        }

        ~_player()
        {
            nom = null;
            avatar = null;
            level = null;
            cards = null;
            block = 0;
            type = null;
            
            GC.Collect();
        }

        public void destructor()
        {
            nom = null;
            avatar = null;
            level = null;
            block = 0;

            for (int cnt = 0; cnt < cards.Length; cnt++)
            {
                cards[cnt] = null;
                _zriwita.p_card[position, cnt].Visible = false;
            }

            System.GC.Collect();
        }

        public void destructor2()
        {
            for (int cnt = 0; cards[cnt]!=null; cnt++)
            {
                cards[cnt] = null;
                _zriwita.p_card[position, cnt].Visible = false;
            }
            System.GC.Collect();
        }

    }
}
