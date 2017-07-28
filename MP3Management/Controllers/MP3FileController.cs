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
    public class MP3FileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MP3File
        public string Index()
        {
            var data = db.MP3File.Include(p => p.Playlists).ToList();
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public string Search(string SearchString, string SearchBy)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.ToLower();

                if (SearchBy.Equals("name"))
                {
                    var data = db.MP3File.Where(s => s.Name.ToLower().Contains(SearchString)).Include(p => p.Playlists).ToList();
                    return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                else if (SearchBy.Equals("author"))
                {
                    var data1 = db.MP3File.Where(d => d.Author.ToLower().Contains(SearchString)).Include(p => p.Playlists).ToList();
                    return JsonConvert.SerializeObject(data1, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                else
                {
                    var playlist = db.Playlist.Where(p => p.Name.ToLower().Contains(SearchString)).Include(m => m.MP3Files);
                    var data2 = playlist.SelectMany(s => s.MP3Files).ToList();
                    return JsonConvert.SerializeObject(data2, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
            }
            throw new HttpException(400, "There were errors in your model");

            #region *********  search without ComboBox selection "Search by"  *********
            /*
            if (!String.IsNullOrEmpty(SearchString))
            {
                // name
                var data = db.MP3File.Where(s => s.Name.ToLower().Contains(SearchString)).Include(p => p.Playlists);
                // author
                var data1 = db.MP3File.Where(d => d.Author.ToLower().Contains(SearchString)).Include(p => p.Playlists);
                //playlist
                var playlist = db.Playlist.Where(p => p.Name.ToLower().Contains(SearchString)).Include(m => m.MP3Files);
                var data2 = playlist.SelectMany(s => s.MP3Files);

                var result = data.Union(data1).Union(data2);
                return JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            throw new HttpException(400, "There were errors in your model");
            */
            #endregion
        }

        // GET: MP3File/Details/5
        public string MP3Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.MP3File.Where(s => s.MP3FileID == id).Include(m => m.Playlists).FirstOrDefault();
            if (data == null)
            {
                //return HttpNotFound();
            }
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
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
