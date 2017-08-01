using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MP3Management.Data;
using MP3Management.Models;
using Newtonsoft.Json;
using System.Net;

namespace MP3Management.Controllers
{
    public class MP3FileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MP3File
        public JsonResult Index()
        {
            db.Configuration.ProxyCreationEnabled = false;

            return this.Json((from obj in db.MP3File
                              select new
                              {
                                  MP3FileID = obj.MP3FileID,
                                  Name = obj.Name,
                                  Author = obj.Author,
                                  AlbumName = obj.AlbumName,
                                  Playlists = obj.Playlists
                              }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string SearchString, string SearchBy)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.ToLower();
                db.Configuration.ProxyCreationEnabled = false;

                if (SearchBy.Equals("name"))
                {
                    var data = db.MP3File.Select(p => new
                    {
                        MP3FileID = p.MP3FileID,
                        Name = p.Name,
                        Author = p.Author,
                        AlbumName = p.AlbumName,
                        Playlists = p.Playlists
                    }).Where(s => s.Name.ToLower().Contains(SearchString)).ToList();

                    return this.Json(data, JsonRequestBehavior.AllowGet);
                }
                else if (SearchBy.Equals("author"))
                {
                    var data1 = db.MP3File.Select(p => new
                    {
                        MP3FileID = p.MP3FileID,
                        Name = p.Name,
                        Author = p.Author,
                        AlbumName = p.AlbumName,
                        Playlists = p.Playlists
                    }).Where(s => s.Author.ToLower().Contains(SearchString)).ToList();

                    return this.Json(data1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var playlist = db.Playlist.Where(p => p.Name.ToLower().Contains(SearchString)).Include(m => m.MP3Files);

                    var data2 = playlist.SelectMany(s => s.MP3Files).Include(p => p.Playlists).ToList();
                    return this.Json((from obj in data2
                                      select new
                                      {
                                          MP3FileID = obj.MP3FileID,
                                          Name = obj.Name,
                                          Author = obj.Author,
                                          AlbumName = obj.AlbumName,
                                          Playlists = from p in obj.Playlists
                                                      select new
                                                      {
                                                          PlaylistID = p.PlaylistID,
                                                          Name = p.Name,
                                                          Description = p.Description
                                                      }
                                      }), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { statuscode = new HttpStatusCodeResult(HttpStatusCode.NotFound) });
        }

        public JsonResult MP3Details(int? id)
        {
            if (id == null)
            {
                return Json(new { statuscode = new HttpStatusCodeResult(HttpStatusCode.BadRequest) });
            }

            db.Configuration.ProxyCreationEnabled = false;
            var data = db.MP3File.Select(p => new
            {
                MP3FileID = p.MP3FileID,
                Name = p.Name,
                Author = p.Author,
                AlbumName = p.AlbumName,
                Playlists = p.Playlists
            }).Where(s => s.MP3FileID == id).FirstOrDefault();

            if (data == null)
            {
                return Json(new { statuscode = new HttpStatusCodeResult(HttpStatusCode.NotFound) });
            }
            return this.Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(MP3File mP3File)
        {
            if (ModelState.IsValid)
            {
                db.MP3File.Add(mP3File);
                db.SaveChanges();
                return Json(mP3File, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //Update
        [HttpPost]
        public ActionResult Edit([Bind(Include = "MP3FileID,Name,Author,AlbumName")] MP3File mP3File)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mP3File).State = EntityState.Modified;
                db.SaveChanges();

                db.Configuration.ProxyCreationEnabled = false;
                var data = db.MP3File.Select(p => new
                {
                    MP3FileID = p.MP3FileID,
                    Name = p.Name,
                    Author = p.Author,
                    AlbumName = p.AlbumName,
                    Playlists = p.Playlists
                }).Where(s => s.MP3FileID == mP3File.MP3FileID).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: MP3File/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                MP3File mP3File = db.MP3File.Find(id);
                db.MP3File.Remove(mP3File);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(ex.HResult);
            }
        }

        //add mp3 to playlist
        [HttpPost]
        public ActionResult AddToPlaylist(int mp3Id, int playlistId)
        {
            try
            {
                MP3File mp3file = db.MP3File.Find(mp3Id);
                Playlist playlist = db.Playlist.Find(playlistId);
                if (!playlist.MP3Files.Contains(mp3file))
                {
                    playlist.MP3Files.Add(mp3file);
                    db.Entry(playlist).State = EntityState.Modified;
                    db.SaveChanges();

                    //response without references
                    var data = db.Playlist.Select(p => new
                    {
                        PlaylistID = p.PlaylistID,
                        Name = p.Name,
                        Description = p.Description,
                    }).Where(s => s.PlaylistID == playlistId).FirstOrDefault();
                    return Content(JsonConvert.SerializeObject(data, Formatting.Indented,
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(ex.HResult);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
