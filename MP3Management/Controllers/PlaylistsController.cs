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
        public ActionResult Index()
        {
            var data = db.Playlist.Include(path => path.MP3Files).ToList();
            return Content(JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
        }

        // GET: Playlists/Details/5
        public ActionResult PlaylistDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var data = db.Playlist.Where(s => s.PlaylistID == id).Include(m => m.MP3Files).FirstOrDefault();

            if (data == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return Content(JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
        }

        [HttpPost]
        public ActionResult Create(Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Playlist.Add(playlist);
                db.SaveChanges();
                return Json(playlist, JsonRequestBehavior.AllowGet); ;
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
                return Content(JsonConvert.SerializeObject(playlist, Formatting.Indented,
                     new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
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
