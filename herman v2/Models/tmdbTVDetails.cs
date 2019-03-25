using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace herman_v2.Models
{

    public class LastEpisodeToAir
    {
        public string air_date { get; set; }
        public int episode_number { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string production_code { get; set; }
        public int season_number { get; set; }
        public int show_id { get; set; }
        public string still_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class Network
    {
        public string name { get; set; }
        public int id { get; set; }
        public string logo_path { get; set; }
        public string origin_country { get; set; }
    }

    public class Season
    {
        public string air_date { get; set; }
        public int episode_count { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public int season_number { get; set; }
    }

    public class TVCast
    {
        public string character { get; set; }
        public string credit_id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int gender { get; set; }
        public string profile_path { get; set; }
        public int order { get; set; }
    }

    public class TVCrew
    {
        public string credit_id { get; set; }
        public string department { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int gender { get; set; }
        public string job { get; set; }
        public string profile_path { get; set; }
    }

    public class TVCredits
    {
        public List<TVCast> cast { get; set; }
        public List<TVCrew> crew { get; set; }
    }

    public class tmdbTVDetails
    {
        public string backdrop_path { get; set; }
        public List<object> created_by { get; set; }
        public List<int> episode_run_time { get; set; }
        public string first_air_date { get; set; }
        public List<Genre> genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public bool in_production { get; set; }
        public List<string> languages { get; set; }
        public string last_air_date { get; set; }
        public LastEpisodeToAir last_episode_to_air { get; set; }
        public string name { get; set; }
        public object next_episode_to_air { get; set; }
        public List<Network> networks { get; set; }
        public int number_of_episodes { get; set; }
        public int number_of_seasons { get; set; }
        public List<string> origin_country { get; set; }
        public string original_language { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public List<ProductionCompany> production_companies { get; set; }
        public List<Season> seasons { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
        public TVCredits credits { get; set; }

        public string video_id { get; set; }

        public Nullable<bool> VHS { get; set; }
        public Nullable<bool> DVD { get; set; }
        public Nullable<bool> BLURAY { get; set; }
        public Nullable<bool> DIGITAL { get; set; }
    }
}