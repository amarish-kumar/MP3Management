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
        public ActionResult Index()
        {
            var data = db.MP3File.Include(p => p.Playlists).ToList();
            return Content(JsonConvert.SerializeObject(data, Formatting.Indented, 
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
        }

        public ActionResult Search(string SearchString, string SearchBy)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.ToLower();

                if (SearchBy.Equals("name"))
                {
                    var data = db.MP3File.Where(s => s.Name.ToLower().Contains(SearchString)).Include(p => p.Playlists).ToList();
                    return Content(JsonConvert.SerializeObject(data, Formatting.Indented, 
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
                }
                else if (SearchBy.Equals("author"))
                {
                    var data1 = db.MP3File.Where(d => d.Author.ToLower().Contains(SearchString)).Include(p => p.Playlists).ToList();
                    return Content(JsonConvert.SerializeObject(data1, Formatting.Indented, 
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
                }
                else
                {
                    var playlist = db.Playlist.Where(p => p.Name.ToLower().Contains(SearchString)).Include(m => m.MP3Files);
                    var data2 = playlist.SelectMany(s => s.MP3Files).ToList();
                    return Content(JsonConvert.SerializeObject(data2, Formatting.Indented, 
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);

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
                return Content(JsonConvert.SerializeObject(result, Formatting.Indented, 
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
            }
            return HttpNotFound();
            */
            #endregion
        }

        // GET: MP3File/Details/5
        public ActionResult MP3Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.MP3File.Where(s => s.MP3FileID == id).Include(m => m.Playlists).FirstOrDefault();
            if (data == null)
            {
                //HttpStatusCode.BadRequest
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return Content(JsonConvert.SerializeObject(data, Formatting.Indented, 
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
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
                return Content(JsonConvert.SerializeObject(mP3File, Formatting.Indented, 
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
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
                    return Content(JsonConvert.SerializeObject(mp3file, Formatting.Indented,
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
