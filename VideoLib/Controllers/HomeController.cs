using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VideoLib.Data;
using VideoLib.Models;

namespace VideoLib.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            SearchModel sm = new SearchModel();

            return View(sm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SearchModel postedata)
        {

            if (!string.IsNullOrEmpty(postedata.SearchTerm))
            {
                postedata.Searched = true;

                //search for results
                postedata.SearchResults = await _context.VideoAssets
                                                    .AsNoTracking()
                                                    .Where(x => x.Name.ToLower().StartsWith(postedata.SearchTerm.ToLower())).ToListAsync();
            }
            else
            {
                postedata.Message = "To search you must enter a term";
            }

            return View(postedata);
        }

        public IActionResult AddVideo()
        {

            VideoModel vm = new VideoModel();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo(VideoModel postedata)
        {
            bool oksofar = true;

            //basic validation
            if (string.IsNullOrEmpty(postedata.Name))
            {
                oksofar = false;
                postedata.Message = "You must enter a name for the video, ";
            }
            if(postedata.Rating < 1 || postedata.Rating > 5)
            {
                oksofar = false;
                postedata.Message += "You must enter a valid rating for the video";
            }
            if (oksofar)
            {
                VideoAsset va = new VideoAsset
                {
                    VideoAssetId = Guid.NewGuid(),
                    Name = postedata.Name,
                    Rating = postedata.Rating
                };
                _context.Entry(va).State = EntityState.Added;
                await _context.SaveChangesAsync();

                return RedirectToAction("index", "home");
            }
            else
            {
                //return back to page
                return View(postedata);
            }

            
        }

    }
}