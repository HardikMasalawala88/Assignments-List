using lab3.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace lab3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMovie(Movie movieData)
        {
            //var localpath = "D:\\Projects\\MDT_Projects";
            //var localpath1 = movieData.MovieVideo.ToString();
            var fileName = string.Empty;//movieData.MovieVideo.Split("\\").Last();

            var data = S3Service.sendMyFileToS3("", "ec2studio_sample_bucket", "", movieData.MovieVideo.FileName);

            return View(data);
            //if (data)
            //{
            //    MessageBox.Show("File uploaded successfully on S3 Bucket Name: " + comboBucketList.SelectedValue.ToString());
            //}
            //if (fetchBucketFiles)//NOT GETTING RESPONSE FROM S3 AND APPLICATION GETTING STUCK IF IT IS TRUE THEN IT GOES INSIDE IF AND FETCH BUCKET FILES
            //{
            //    fetchBucketData(comboBucketList.SelectedValue.ToString());
            //}


        }
    }
}
