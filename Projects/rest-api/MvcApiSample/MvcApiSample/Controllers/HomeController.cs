using Microsoft.AspNetCore.Mvc;
using MvcApiSample.ViewModels.Home;

namespace MvcApiSample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // ViewResult
            //return View();

            //return View() is similar to:

            #region Correct Code

            //ViewData["somedata"] = "This is some data from the Action";
            //ViewResult res = new ViewResult();
            //res.ViewName = "/Views/Home/Index.cshtml";

            ////res.ViewName = "/Views/Whatever/Iwassowhatever.cshtml";
            ////res.ViewData["someData"] = "This is some data from the Action"; NullReference Error

            //res.ViewData = this.ViewData;
            //return res;
            #endregion Correct Code

            // The framework takes the ViewName and the ViewData and renders them

            // The rows above are like:
            // ViewResult res = this.View(); // the View method of the controller; takes Home + Index; takes the controller ViewData and sets it ad the ViewResult ViewData

            //ViewData["somedata"] = "This is some data from the Action"; // this return an array
            //return new ViewResult()
            //{
            //    ViewName = "/Views/Home/Index.cshtml" // property initializer
            //};

            #region JSON Result
            IndexVM model = new IndexVM();
            model.Title = "Index page";
            model.Content = "This is some content";
            JsonResult res = new JsonResult(model);
            return res;
            // This JSON can be taken with a JS code, then be deserialized and afterwards work can be done with it
            #endregion
        }
    }
}
