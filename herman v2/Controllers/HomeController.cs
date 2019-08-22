using System.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Linq;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;
using herman_v2.Models;
using System.Collections.Generic;
using System.Text;

namespace herman_v2.Controllers
{
    public class HomeController : Controller
    {
        private Models.Entities db = new Models.Entities();


        public ActionResult Index(string search)
        {
            Dashboard dash = new Dashboard();

            var movies = (from v in db.Videos select v);

            dash.CountTotal = movies.Count();
            dash.CountVHS = movies.Where(v => v.VHS == true).Count();
            dash.CountDVD = movies.Where(v => v.DVD == true).Count();
            dash.CountBluray = movies.Where(v => v.BLURAY == true).Count();
            dash.CountDIGITAL = movies.Where(v => v.DIGITAL == true).Count();


            var top5 = (from a in db.top5s select a).ToList();

            dash.Top5 = top5;

            var searchval = "false";

            if (search != null)
            {
                dash.Vmm = from v in db.Videos
                           join d in db.directors on v.Director equals d.dir_id
                           where v.Video_Name.Contains(search)
                           select new VideoViewModel
                           {
                               video_id = v.video_id,
                               Video_Name = v.Video_Name,
                               VHS = v.VHS,
                               DVD = v.DVD,
                               BLURAY = v.BLURAY,
                               DIGITAL = v.DIGITAL,
                               Release_Date = v.Release_Date,
                               Plot = v.Plot,
                               Box_Cover = v.Box_Cover,
                               dir_id = v.Director,
                               dir_first_name = d.dir_first_name,
                               dir_last_name = d.dir_last_name,
                               dir_name = d.dir_name,
                               Tagline = v.Tagline
                           };

                searchval = "true";
            }
            else
            {
                dash.Vmm = from v in db.Videos
                           where v.featured == true
                           join d in db.directors on v.Director equals d.dir_id
                           select new VideoViewModel
                           {
                               video_id = v.video_id,
                               Video_Name = v.Video_Name,
                               VHS = v.VHS,
                               DVD = v.DVD,
                               BLURAY = v.BLURAY,
                               DIGITAL = v.DIGITAL,
                               Release_Date = v.Release_Date,
                               Plot = v.Plot,
                               Box_Cover = v.Box_Cover,
                               dir_first_name = d.dir_first_name,
                               dir_last_name = d.dir_last_name,
                               dir_id = v.Director,
                               dir_name = d.dir_name,
                               Tagline = v.Tagline

                           };

                dash.Recent = (from v in db.Videos
                               join d in db.directors on v.Director equals d.dir_id
                               orderby v.video_id descending
                               select new VideoViewModel
                               {
                                   video_id = v.video_id,
                                   Video_Name = v.Video_Name,
                                   VHS = v.VHS,
                                   DVD = v.DVD,
                                   BLURAY = v.BLURAY,
                                   DIGITAL = v.DIGITAL,
                                   Release_Date = v.Release_Date,
                                   Plot = v.Plot,
                                   Box_Cover = v.Box_Cover,
                                   dir_first_name = d.dir_first_name,
                                   dir_last_name = d.dir_last_name,
                                   dir_id = v.Director,
                                   dir_name = d.dir_name,
                                   Tagline = v.Tagline

                               }).Take(5);

            }
            dash.search = searchval;

            return View(dash);
        }

        [Authorize]
        public ActionResult Logon(string gotourl)
        {
            return new RedirectResult(gotourl);
        }

        public static bool CanEdit()
        {
            var su = System.Configuration.ConfigurationManager.AppSettings.GetValues("AdSuperUsers").ToList();
            var cu = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            var val = false;

            foreach (var user in su)
            {
                if (user.Contains(cu) && cu != "")
                {
                    val = true;
                }
            }

            if (val)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult VideoDetails(int id)
        {
            var getvideo = (from v in db.Videos
                            where v.video_id == id
                            join d in db.directors on v.Director equals d.dir_id
                            join c in db.categories on v.Category equals c.category_id
                            select new VideoViewModel
                            {
                                video_id = v.video_id,
                                Video_Name = v.Video_Name,
                                VHS = v.VHS,
                                DVD = v.DVD,
                                BLURAY = v.BLURAY,
                                DIGITAL = v.DIGITAL,
                                Release_Date = v.Release_Date,
                                Plot = v.Plot,
                                Box_Cover = v.Box_Cover,
                                dir_first_name = d.dir_first_name,
                                dir_last_name = d.dir_last_name,
                                dir_mi = d.dir_mi,
                                dir_name = d.dir_name,
                                dir_id = d.dir_id,
                                length = v.length,
                                rating = v.rating,
                                category_name = c.category_name,
                                tmdb_id = v.tmdb_id
                            }).SingleOrDefault();

            var getcast = (from a2 in db.actor2movie
                           where a2.video_id == id
                           join a in db.actors on a2.actor_id equals a.actor_id
                           join c in db.characters on a2.char_id equals c.char_id
                           select new CastList
                           {
                               actor = a,
                               character = c
                           });

            getvideo.VideoCastList = getcast.ToList();

            return View(getvideo);
        }

        public ActionResult ViewActor(int id)
        {

            var client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;

            var jsonString = "";

            var getactor = (from a in db.actors
                            where a.actor_id == id
                            select a).SingleOrDefault();

            getactor.mymovies = (from a2 in db.actor2movie
                                 where a2.actor_id == id
                                 join v in db.Videos on a2.video_id equals v.video_id
                                 join c in db.characters on a2.char_id equals c.char_id
                                 orderby v.Release_Date descending
                                 select new ActorVideo
                                 {
                                     video_id = v.video_id,
                                     Video_Name = v.Video_Name,
                                     Release_Date = v.Release_Date,
                                     char_first_name = c == null ? "" : c.char_first_name,
                                     char_mi = c == null ? "" : c.char_mi,
                                     char_last_name = c == null ? "" : c.char_last_name,
                                     char_alias = c == null ? "" : c.char_alias,
                                     char_name = c.char_name,
                                     inCollection = true
                                 }).ToList();

            getactor.CollectionCount = getactor.mymovies.Count();

            jsonString = client.DownloadString("https://api.themoviedb.org/3/person/" + getactor.tmdb_id + "/movie_credits?api_key=20b7f0072c71ea8a098653d0a11b5b46&language=en-US");

            var tmdbVideosearch = JsonConvert.DeserializeObject<Credits>(jsonString);

            foreach (var video in tmdbVideosearch.cast)
            {
                ActorVideo av = new ActorVideo();

                av.Video_Name = video.title;
                if (video.release_date != "" && video.release_date != null)
                {
                    av.Release_Date = DateTime.Parse(video.release_date).Year;
                }
                av.char_first_name = video.character;

                bool dontown = true;

                foreach (ActorVideo cav in getactor.mymovies)
                {
                    if (cav.Video_Name.Equals(video.title))
                    {
                        dontown = false;
                    }
                }

                if(dontown == true && av.Release_Date != null && !av.char_first_name.Contains("imself"))
                {
                    av.inCollection = false;
                    getactor.mymovies.Add(av);
                }

            }

            getactor.mymovies = getactor.mymovies.OrderByDescending(m => m.Release_Date).ToList();

            getactor.FilmCount = getactor.mymovies.Count();

            return View(getactor);
        }

        public ActionResult ViewDirector(int id)
        {
            var getdir = (from d in db.directors
                          where d.dir_id == id
                          select d).SingleOrDefault();

            getdir.mymovies = (from v in db.Videos
                               where v.Director == id
                               orderby v.Release_Date descending
                               select new ActorVideo
                               {
                                   video_id = v.video_id,
                                   Video_Name = v.Video_Name,
                                   Release_Date = v.Release_Date
                               }).ToList();

            return View(getdir);
        }

        public List<TVResult> TVSearchResults(string search)
        {
            var client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;

            var jsonString = client.DownloadString("https://api.themoviedb.org/3/search/tv?api_key=20b7f0072c71ea8a098653d0a11b5b46&language=en-US&page=1&include_adult=false&query=" + Url.Encode(search));

            tmdbTVSearch tmdbtvsearch = JsonConvert.DeserializeObject<tmdbTVSearch>(jsonString);

            return tmdbtvsearch.results;
        }

        public List<tmdbVideoResult> VideoSearchResults(string search)
        {
            var client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;

            var jsonString = client.DownloadString("https://api.themoviedb.org/3/search/movie?api_key=20b7f0072c71ea8a098653d0a11b5b46&language=en-US&page=1&include_adult=false&query=" + Url.Encode(search));

            tmdbVideoSearch tmdbvidsearch = JsonConvert.DeserializeObject<tmdbVideoSearch>(jsonString);

            return tmdbvidsearch.results;
        }

        public tmdbVideoDetails ActorSearchResults(string tmdbid)
        {
            var client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;

            var jsonString = "";

            //this one is searching the actor api directly
            //var jsonString = new WebClient().DownloadString("https://api.themoviedb.org/3/search/person?api_key=20b7f0072c71ea8a098653d0a11b5b46&language=en-US&page=1&include_adult=false&query=" + Url.Encode(search));

                //this one is searching the movie api
                jsonString = client.DownloadString("https://api.themoviedb.org/3/movie/" + tmdbid + "?api_key=20b7f0072c71ea8a098653d0a11b5b46&language=en-US&append_to_response=credits");

            var tmdbVideosearch = JsonConvert.DeserializeObject<tmdbVideoDetails>(jsonString);

                return tmdbVideosearch;
        }

        public tmdbTVDetails TVActorSearchResults(string tmdbid)
        {
            var client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;

            var jsonString = "";

                //this one is searching the tv api
                jsonString = client.DownloadString("https://api.themoviedb.org/3/tv/" + tmdbid + "?api_key=20b7f0072c71ea8a098653d0a11b5b46&language=en-US&append_to_response=credits");
                var tmdbTVS = JsonConvert.DeserializeObject<tmdbTVDetails>(jsonString);

                return tmdbTVS;
            }

        public ActionResult SearchForTV(string search)
        {
            var ret = TVSearchResults(search);

            return PartialView(ret);
        }

        public ActionResult SearchForVideo(string search)
        {
            var ret = VideoSearchResults(search);

            return PartialView(ret);
        }

        public ActionResult SearchForTVActor(string videoid, string tmdbid, string rating)
        {
                var ret = TVActorSearchResults(tmdbid);

                ret.video_id = videoid;

                return View(ret);
        }

        public ActionResult SearchForActor(string videoid, string tmdbid, string rating)
        {
                var ret = ActorSearchResults(tmdbid);

                ret.video_id = videoid;

                return View(ret);
        }

        public tmdbVideoDetails GetTMDBVideoDetailfromApi(string id, int video_id)
        {
            var client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;

            string jsonString = client.DownloadString("https://api.themoviedb.org/3/movie/" + id + "?api_key=20b7f0072c71ea8a098653d0a11b5b46&language=en-US&append_to_response=release_dates,credits");

            var tmdbsearch = JsonConvert.DeserializeObject<tmdbVideoDetails>(jsonString);

            //            tmdbsearch.dir_id = FindDir(imdbsearch.Director);
            //            tmdbsearch.id = video_id;

            return tmdbsearch;
        }

        public tmdbTVDetails GetTMDBTVDetailfromApi(string id, int video_id)
        {
            var client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;

            var jsonString = client.DownloadString("https://api.themoviedb.org/3/tv/" + id + "?api_key=20b7f0072c71ea8a098653d0a11b5b46&language=en-US&append_to_response=credits");

            var tmdbsearch = JsonConvert.DeserializeObject<tmdbTVDetails>(jsonString);

            return tmdbsearch;
        }

        public ActionResult GettmdbVideoDetails(string id, int video_id, bool VHS, bool DVD, bool BLURAY, bool DIGITAL)
        {
            var ret = GetTMDBVideoDetailfromApi(id, video_id);
            ret.video_id = video_id.ToString();
            ret.VHS = VHS;
            ret.DVD = DVD;
            ret.BLURAY = BLURAY;
            ret.DIGITAL = DIGITAL;
            return PartialView(ret);
        }

        public ActionResult GettmdbTVDetails(string id, int video_id, bool VHS, bool DVD, bool BLURAY, bool DIGITAL)
        {
            var ret = GetTMDBTVDetailfromApi(id, video_id);
            ret.video_id = video_id.ToString();
            ret.VHS = VHS;
            ret.DVD = DVD;
            ret.BLURAY = BLURAY;
            ret.DIGITAL = DIGITAL;
            return PartialView(ret);
        }

        public ActionResult AddVideo(string id, string search, bool vhs, bool dvd, bool bluray, bool digital)
        {
            ViewData["uvs"] = search;
            ViewData["vid"] = id;

            var ret = new Video();
            ret.video_id = Convert.ToInt32(id);
            ret.Video_Name = search;
            ret.VHS = vhs;
            ret.DVD = dvd;
            ret.BLURAY = bluray;
            ret.DIGITAL = digital;

            return View(ret);
        }

        public ActionResult AddActor(string vid, string id, string search)
        {
            ViewData["uas"] = search;
            ViewData["aid"] = id;
            ViewData["vid"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddActor(int id, actor a, character c)
        {
            actor2movie a2m = new actor2movie();

            if (a.actor_id == 0)
            {
                db.actors.Add(a);
                db.SaveChanges();
            }

            if (c.char_id == 0)
            {
                db.characters.Add(c);
                db.SaveChanges();
            }

            a2m.video_id = id;
            a2m.actor_id = a.actor_id;
            a2m.char_id = c.char_id;

            db.actor2movie.Add(a2m);
            db.SaveChanges();

            return RedirectToAction("VideoDetails", new { @id = id });
        }

        public ActionResult AddActorToVideo(int video_id, actor a, character c)
        {
            actor2movie a2m = new actor2movie();

            if (a.actor_id == 0)
            {
                var actr = (from ac in db.actors where a.actor_name == ac.actor_name || (a.actor_name.Contains(ac.actor_first_name) && a.actor_name.EndsWith(ac.actor_last_name)) select ac).FirstOrDefault();

                if (actr == null)
                {
                    db.actors.Add(a);
                    db.SaveChanges();
                    a2m.actor_id = a.actor_id;
                }
                else
                {
                    a2m.actor_id = actr.actor_id;
                    actr.actor_photo = a.actor_photo;
                    actr.tmdb_id = a.tmdb_id;
                }

            }
            if (c.char_id == 0)
            {
                var chr = (from ch in db.characters where c.char_name == ch.char_name select ch.char_id).FirstOrDefault();

                if (chr == 0)
                {
                    db.characters.Add(c);
                    db.SaveChanges();
                    a2m.char_id = c.char_id;
                }
                else
                {
                    a2m.char_id = chr;
                }
            }

            a2m.video_id = video_id;

            var a2mexists = (from a2 in db.actor2movie where a2m.actor_id == a2.actor_id && a2m.video_id == a2.video_id && a2m.char_id == a2.char_id select a2).FirstOrDefault();
            if (a2mexists == null)
            {
                db.actor2movie.Add(a2m);
            }
            db.SaveChanges();

            return RedirectToAction("VideoDetails", new { @id = video_id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddVideo(VideoViewModel vid, string video_id)
        {
            var dir = new director();
            var vidid = 0;

            int rating = 0;

            if (vid.ratingtxt == "PG")
            {
                rating = 2;
            }
            else if (vid.ratingtxt == "PG-13")
            {
                rating = 3;
            }
            else if (vid.ratingtxt == "R")
            {
                rating = 4;
            }
            else if (vid.ratingtxt == "TV")
            {
                rating = 5;
            }
            else
            {
                rating = 1;
            }

            int catid = (from c in db.categories where vid.category_name == c.category_name select c.category_id).FirstOrDefault();

            if(catid == 0)
            {
                var newcat = new category();
                newcat.category_id = Convert.ToInt32(vid.Category);
                newcat.category_name = vid.category_name;
                db.categories.Add(newcat);
                db.SaveChanges();

                catid = newcat.category_id;
            }

            var exvid = (from v in db.Videos where v.video_id.ToString() == video_id select v).FirstOrDefault();

            var dir_id = (from d in db.directors where d.dir_name == vid.dir_name select d.dir_id).FirstOrDefault();
            vid.Director = dir_id;

            if (exvid == null)
            {
                if (vid.Director == 0)
                {
                    if (vid.dir_name == null)
                    {
                        vid.Director = 5373;
                    }
                    else
                    {
                        dir.dir_first_name = vid.dir_name.Substring(0, vid.dir_name.IndexOf(" "));
                        dir.dir_last_name = (vid.dir_name.Substring((vid.dir_name.IndexOf(" ")), (vid.dir_name.Length - vid.dir_name.IndexOf(" ")))).Trim();
                        dir.dir_name = vid.dir_name;

                        db.directors.Add(dir);
                        db.SaveChanges();

                        vid.Director = dir.dir_id;
                    }
                }
            }

            if (exvid != null)
            {
                exvid.VHS = vid.VHS;
                exvid.DVD = vid.DVD;
                exvid.BLURAY = vid.BLURAY;
                exvid.DIGITAL = vid.DIGITAL;
                exvid.Release_Date = vid.Release_Date;
                exvid.rating = rating;
                exvid.length = Convert.ToInt32(vid.lengthtxt);
                exvid.Plot = vid.Plot;
                exvid.Tagline = vid.Tagline;
                exvid.Category = catid;
                exvid.Box_Cover = "https://image.tmdb.org/t/p/w220_and_h330_face" + vid.Box_Cover;
                exvid.imdb_id = vid.imdb_id;
                exvid.tmdb_id = vid.tmdb_id;

                vidid = exvid.video_id;
            }
            else
            {
                var newVideo = new Video();

                newVideo.Video_Name = vid.Video_Name;
                newVideo.VHS = vid.VHS;
                newVideo.DVD = vid.DVD;
                newVideo.BLURAY = vid.BLURAY;
                newVideo.DIGITAL = vid.DIGITAL;
                newVideo.Release_Date = vid.Release_Date;
                newVideo.rating = rating;
                newVideo.length = Convert.ToInt32(vid.lengthtxt);
                newVideo.Plot = vid.Plot;
                newVideo.Tagline = vid.Tagline;
                newVideo.Director = vid.Director;
                newVideo.Category = catid;
                newVideo.Box_Cover = "https://image.tmdb.org/t/p/w220_and_h330_face" + vid.Box_Cover;
                newVideo.featured = false;
                newVideo.imdb_id = vid.imdb_id;
                newVideo.tmdb_id = vid.tmdb_id;
                db.Videos.Add(newVideo);
                db.SaveChanges();

                vidid = newVideo.video_id;
            }

            try
            {

                if (CanEdit())
                {
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                Console.Write("err is " + ex);

                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            if (vidid != 0)
            {
                return RedirectToAction("VideoDetails", new { id = vidid });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult EditVideo(int id)
        {
            var vid = (from v in db.Videos
                       where v.video_id == id
                       select v).SingleOrDefault();

            return View(vid);
        }

        [HttpPost]
        public ActionResult EditVideo(Video vid)
        {
            var udvid = (from v in db.Videos where v.video_id == vid.video_id select v).SingleOrDefault();

            udvid.Video_Name = vid.Video_Name;
            udvid.VHS = vid.VHS;
            udvid.DVD = vid.DVD;
            udvid.BLURAY = vid.BLURAY;
            udvid.DIGITAL = vid.DIGITAL;
            udvid.Director = vid.Director;
            udvid.length = vid.length;
            udvid.rating = vid.rating;
            udvid.Category = vid.Category;
            udvid.Plot = vid.Plot;
            udvid.Release_Date = vid.Release_Date;
            udvid.Box_Cover = vid.Box_Cover;
            udvid.featured = vid.featured;

            if (CanEdit())
            {
                db.SaveChanges();
            }
            return RedirectToAction("VideoDetails", new { id = vid.video_id });
        }

        public ActionResult RemoveActor(int video_id, int actor_id, int char_id)
        {
            actor2movie a2m = (from a2 in db.actor2movie where a2.video_id == video_id && a2.actor_id == actor_id && a2.char_id == char_id select a2).SingleOrDefault();

            db.actor2movie.Remove(a2m);

            var ainm = from a2 in db.actor2movie where a2.actor_id == a2m.actor_id select a2;

            if (ainm.Count() < 2)
            {
                var actor = (from a in db.actors where a.actor_id == a2m.actor_id select a).FirstOrDefault();
                db.actors.Remove(actor);
            }

            var cinm = from a2 in db.actor2movie where a2.char_id == a2m.char_id select a2;

            if (cinm.Count() < 2)
            {
                var chr = (from c in db.characters where c.char_id == a2m.char_id select c).FirstOrDefault();
                db.characters.Remove(chr);
            }

            if (CanEdit())
            {
                db.SaveChanges();
            }

            return RedirectToAction("VideoDetails", new { @id = a2m.video_id });
        }
    }
}