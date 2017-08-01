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
    public class PlaylistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Playlists
        public JsonResult Index()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return this.Json((from obj in db.Playlist
                              select new
                              {
                                  PlaylistID = obj.PlaylistID,
                                  Name = obj.Name,
                                  Description = obj.Description,
                                  MP3Files = obj.MP3Files,
                              }), JsonRequestBehavior.AllowGet);
        }

        // GET: Playlists/Details/5
        public JsonResult PlaylistDetails(int? id)
        {
            if (id == null)
            {
                return Json(new { statuscode = new HttpStatusCodeResult(HttpStatusCode.BadRequest) });
            }

            db.Configuration.ProxyCreationEnabled = false;
            var data = db.Playlist.Select(p => new
            {
                PlaylistID = p.PlaylistID,
                Name = p.Name,
                Description = p.Description,
                MP3Files = p.MP3Files
            }).Where(s => s.PlaylistID == id).FirstOrDefault();

            if (data == null)
            {
                return Json(new { statuscode = new HttpStatusCodeResult(HttpStatusCode.NotFound) });
            }
            return this.Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Playlist.Add(playlist);
                db.SaveChanges();
                return Json(playlist, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "PlaylistID,Name,Description")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playlist).State = EntityState.Modified;
                db.SaveChanges();

                db.Configuration.ProxyCreationEnabled = false;
                var data = db.Playlist.Select(p => new
                {
                    PlaylistID = p.PlaylistID,
                    Name = p.Name,
                    Description = p.Description,
                    MP3Files = p.MP3Files
                }).Where(s => s.PlaylistID == playlist.PlaylistID).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Playlist playlist = db.Playlist.Find(id);
                db.Playlist.Remove(playlist);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(ex.HResult);
            }
        }

        public ActionResult RemoveMp3FromPlaylist(int mp3Id, int playlistId)
        {
            try
            {
                MP3File mp3file = db.MP3File.Find(mp3Id);
                Playlist playlist = db.Playlist.Find(playlistId);
                if (playlist.MP3Files.Contains(mp3file))
                {
                    playlist.MP3Files.Remove(mp3file);
                    db.Entry(playlist).State = EntityState.Modified;
                    db.SaveChanges();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

                    //response
                    var data = db.MP3File.Select(p => new
                    {
                        MP3FileID = p.MP3FileID,
                        Name = p.Name,
                        Author = p.Author,
                        AlbumName = p.AlbumName,
                    }).Where(s => s.MP3FileID == mp3Id).FirstOrDefault();
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
