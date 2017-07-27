using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MP3Management.Data;
using MP3Management.Models;
using Newtonsoft.Json;

namespace MP3Management.Controllers
{
    public class MP3FileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MP3File
        public ActionResult Index()
        {
            var data = db.MP3File.Select(p => new
            {
                MP3FileID = p.MP3FileID,
                Name = p.Name,
                Author = p.Author,
                AlbumName = p.AlbumName
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: MP3File/Details/5
        public ActionResult MP3Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.MP3File.Select(p => new
            {
                MP3FileID = p.MP3FileID,
                Name = p.Name,
                Author = p.Author,
                AlbumName = p.AlbumName
            }).Where(s => s.MP3FileID == id).FirstOrDefault();

            if (data == null)
            {
                return HttpNotFound();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(MP3File mP3File)
        {
            if (ModelState.IsValid)
            {
                db.MP3File.Add(mP3File);
                db.SaveChanges();
                return Json(mP3File, JsonRequestBehavior.AllowGet); ;
            }
            throw new HttpException(400, "There were errors in your model");
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "MP3FileID,Name,Author,AlbumName")] MP3File mP3File)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mP3File).State = EntityState.Modified;
                db.SaveChanges();
                return Json("MP3 edited!", JsonRequestBehavior.AllowGet);
            }
            return Json("Error editing MP3", JsonRequestBehavior.AllowGet);
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
                return Json("MP3 deleted!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error deleting MP3", JsonRequestBehavior.AllowGet);
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
        public ActionResult Playlists()
        {
            var data = db.Playlist.Select(p => new
            {
                PlaylistID = p.PlaylistID,
                Name = p.Name,
                Description = p.Description
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string AddToPlaylist(int mp3Id, int playlistId)
        {
            //try
            //{
                MP3File mp3file = db.MP3File.Find(mp3Id);
                Playlist playlist = db.Playlist.Find(playlistId);
                if (!playlist.MP3Files.Contains(mp3file))
                {
                    playlist.MP3Files.Add(mp3file);
                    db.Entry(playlist).State = EntityState.Modified;
                    db.SaveChanges();
                    //return Json("Predmet added to student list!", JsonRequestBehavior.AllowGet);
                }
                return JsonConvert.SerializeObject(mp3file, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }); //and push it to the $scope

            //}
            //catch (Exception ex)
            //{
            //return JsonConvert.SerializeObject("Error adding mp3 to playlist!", Formatting.Indented);

            //}
        }
    }
}
