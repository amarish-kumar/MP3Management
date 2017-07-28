using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MP3Management.Data;
using MP3Management.Models;
using Newtonsoft.Json;

namespace MP3Management.Controllers
{
    public class PlaylistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Playlists
        public string Index()
        {
            var data = db.Playlist.Include(path => path.MP3Files).ToList();
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        // GET: Playlists/Details/5
        public string PlaylistDetails(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var data = db.Playlist.Where(s => s.PlaylistID == id).Include(m => m.MP3Files).FirstOrDefault();

            if (data == null)
            {
                //return HttpNotFound();
            }
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
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
            throw new HttpException(400, "There were errors in your model");
        }
        // POST: Playlists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public string Edit([Bind(Include = "PlaylistID,Name,Description")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playlist).State = EntityState.Modified;
                db.SaveChanges();
                return JsonConvert.SerializeObject(playlist, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            throw new HttpException(400, "There were errors in your model");
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Playlist playlist = db.Playlist.Find(id);
                db.Playlist.Remove(playlist);
                db.SaveChanges();
                return Json("Playlist deleted!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error deleting Playlist", JsonRequestBehavior.AllowGet);
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

        public string RemoveMp3FromPlaylist(int mp3Id, int playlistId)
        {
            //try
            //{
            MP3File mp3file = db.MP3File.Find(mp3Id);
            Playlist playlist = db.Playlist.Find(playlistId);
            if (playlist.MP3Files.Contains(mp3file))
            {
                playlist.MP3Files.Remove(mp3file);
                db.Entry(playlist).State = EntityState.Modified;
                db.SaveChanges();
                //return Json("Predmet added to student list!", JsonRequestBehavior.AllowGet);
            }
            return JsonConvert.SerializeObject(mp3file, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }); //and push it to the $scope

            //}
            //catch (Exception ex)
            //{
            //return JsonConvert.SerializeObject("Error adding mp3 to playlist!", Formatting.Indented);

            //}
        }
    }
}
