using DashiNovel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DashiNovel.Controllers
{
    public class NovelsController : Controller
    {
        DashiNovelContext context = new DashiNovelContext();
        public IActionResult Index(string search)
        {
            List<Novel> novels = context.Novels.Include(n => n.Genres).ToList();
            if (search != null)
            {
                novels = novels.Where(n => n.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(novels);
        }
        public IActionResult Work()
        {
            List<Publish> publishes = context.Publishes.Where(c => c.UserId == HttpContext.Session.GetInt32("UserId")).ToList();
            if (publishes.Count > 0)
            {
                List<Novel> novels = context.Novels.Where(c => publishes.Select(c => c.NovelId).ToList().Contains(c.NovelId)).ToList();
                ViewBag.novels = novels;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Publish()
        {
            List<Genre> genres = context.Genres.ToList();
            return View(genres);
        }

        [HttpPost]
        public IActionResult Publish(string title, string author, IFormFile image, string type, string status, string sypnosis, int[] genres)
        {
            Novel newNovel = new Novel();
            newNovel.Title = title;
            newNovel.Author = author;
            newNovel.Type = bool.Parse(type);
            newNovel.State = bool.Parse(status);
            newNovel.Description = sypnosis;
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    byte[] binaryData = memoryStream.ToArray();
                    newNovel.Image = binaryData;
                }
            }
            List<Genre> novelGenres = context.Genres.Where(c => genres.Contains(c.GenreId)).ToList();
            newNovel.Genres = novelGenres;
            context.Novels.Add(newNovel);
            context.SaveChanges();
            Publish publish = new Publish();
            publish.NovelId = newNovel.NovelId;
            publish.UserId = HttpContext.Session.GetInt32("UserId");
            context.Publishes.Add(publish);
            context.SaveChanges();

            return RedirectToAction("EditNovel", "Novels", new { novelId = newNovel.NovelId });
        }


        [HttpGet]
        public IActionResult EditNovel(int novelId)
        {
            List<Genre> genres = context.Genres.ToList();
            Novel novel = context.Novels.Include(n => n.Genres).Include(n => n.Chapters).FirstOrDefault(n => n.NovelId == novelId);
            ViewBag.novel = novel;
            ViewBag.novelGenres = novel.Genres.Select(c => c.GenreId).ToList();
            return View(genres);
        }

        [HttpPost]
        public IActionResult EditNovel(int novelId, string title, string author, IFormFile image, string type, string status, string sypnosis, int[] genres)
        {
            Novel novel = context.Novels.Include(n => n.Genres).FirstOrDefault(n => n.NovelId == novelId);
            novel.Title = title;
            novel.Author = author;
            novel.Type = bool.Parse(type);
            novel.State = bool.Parse(status);
            novel.Description = sypnosis;
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    byte[] binaryData = memoryStream.ToArray();
                    novel.Image = binaryData;
                }
            }
            List<Genre> novelGenres = context.Genres.Where(c => genres.Contains(c.GenreId)).ToList();
            novel.Genres = novelGenres;
            context.Novels.Update(novel);
            context.SaveChanges();
            return RedirectToAction("EditNovel", "Novels", new { novelId = novelId });
        }
        [HttpGet]
        public IActionResult EditChapter(int novelId, int chapterId)
        {
            Novel novel = context.Novels.Include(n => n.Genres).Include(n => n.Chapters).FirstOrDefault(n => n.NovelId == novelId);
            ViewBag.novel = novel;
            ViewBag.chapter = context.Chapters.FirstOrDefault(c => c.ChapterId == chapterId);
            return View();
        }

        [HttpPost]
        public IActionResult EditChapter(int novelId, int chapterId, int number, string title, string content)
        {
            Chapter chapter = context.Chapters.FirstOrDefault(x => x.ChapterId == chapterId);
            chapter.Title = title;
            chapter.Content = content;
            context.SaveChanges();
            return RedirectToAction("EditChapter", "Novels", new { novelId, chapterId });
        }

        public IActionResult Add(int novelId, int count)
        {
            Chapter newChapter = new Chapter();
            newChapter.NovelId = novelId;
            newChapter.Title = "Default Title";
            newChapter.Content = "Default Content";
            newChapter.ChapterNumber = count + 1;
            context.Chapters.Add(newChapter);
            context.SaveChanges();
            return RedirectToAction("EditChapter", "Novels", new { novelId = novelId, chapterId = newChapter.ChapterId });
        }

        public IActionResult Details(int novelId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            Novel novel = context.Novels.Include(n => n.Chapters).Include(n => n.Genres).Include(n => n.Reviews.OrderByDescending(r => r.Date)).ThenInclude(r => r.User).FirstOrDefault(n => n.NovelId == novelId);
            if (userId != null)
            {
                Reading read = context.Readings.FirstOrDefault(r => r.UserId == userId && r.NovelId == novelId);
                if (read != null)
                {
                    ViewBag.reading = novel.Chapters.FirstOrDefault(c => c.ChapterNumber == read.ChapterNumber);
                }
                ViewBag.canReview = context.Reviews.FirstOrDefault(r => r.NovelId == novelId && r.UserId == userId);
            }
            return View(novel);
        }

        public IActionResult Read(int chapterId, int novelId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            List<Chapter> chapters = context.Chapters.Include(c => c.Comments.OrderByDescending(c => c.Date)).ThenInclude(c => c.User).Where(c => c.NovelId == novelId).ToList();
            Chapter chapter = chapters.FirstOrDefault(c => c.ChapterId == chapterId);
            if (chapter.ChapterNumber > 1)
            {
                ViewBag.prev = chapters.FirstOrDefault(c => c.ChapterNumber == chapter.ChapterNumber - 1 && c.NovelId == novelId).ChapterId;
            }
            if (chapter.ChapterNumber < chapters.Count)
            {
                ViewBag.next = chapters.FirstOrDefault(c => c.ChapterNumber == chapter.ChapterNumber + 1 && c.NovelId == novelId).ChapterId;
            }
            if (userId != null)
            {
                Reading read = context.Readings.FirstOrDefault(r => r.UserId == userId && r.NovelId == novelId);
                if (read != null)
                {
                    read.ChapterNumber = chapter.ChapterNumber;
                    context.Readings.Update(read);
                    context.SaveChanges();
                }
                else
                {
                    read = new Reading();
                    read.ChapterNumber = chapter.ChapterNumber;
                    read.NovelId = novelId;
                    read.UserId = (int)userId;
                    context.Readings.Add(read);
                    context.SaveChanges();
                }
            }
            ViewBag.chapters = chapters;
            return View(chapter);
        }

        public IActionResult Comment(int chapterId, int novelId, string comment)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                Comment com = new Comment();
                com.Comment1 = comment;
                com.ChapterId = chapterId;
                com.UserId = userId;
                com.User = null;
                context.Comments.Add(com);
                context.SaveChanges();
            }
            return RedirectToAction("Read", "Novels", new { novelId = novelId, chapterId = chapterId });
        }

        public IActionResult Review(int novelId, bool rate, string review)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                Review rv = new Review();
                rv.User = null;
                rv.NovelId = novelId;
                rv.UserId = (int)userId;
                rv.Rate = rate;
                rv.Review1 = review;
                context.Reviews.Add(rv);
                context.SaveChanges();
            }
            return RedirectToAction("Details", "Novels", new { novelId = novelId });
        }

        public IActionResult Reading()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                List<Reading> readings = context.Readings.Include(r => r.Novel).ThenInclude(n => n.Chapters).Where(r => r.UserId == userId).ToList();
                ViewBag.readings = readings;
            }
            return View();
        }

        public IActionResult Delete(int novelId)
        {
            Novel novel = context.Novels.FirstOrDefault(n => n.NovelId == novelId);
            context.Novels.Remove(novel);
            context.SaveChanges();
            return RedirectToAction("Work", "Novels");
        }
    }
}
