using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace herman_v2.Models
{
    public class ActorVideo
    {
        public int video_id { get; set; }
        public string Video_Name { get; set; }
        public Nullable<int> Release_Date { get; set; }

        public string char_first_name { get; set; }
        public string char_mi { get; set; }
        public string char_last_name { get; set; }
        public string char_alias { get; set; }

        public string char_name { get; set; }

        public string char_fullname
        {
            get
            {
                var fullname = "";

                fullname = char_first_name + ' ';
                if (char_mi != null)
                {
                    fullname = fullname + char_mi + ' ';
                }

                fullname = fullname + char_last_name;

                if (char_alias != null)
                {
                    fullname = fullname + ' ' + '(' + char_alias + ')';
                }

                return fullname;
            }
        }
    }
}