using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HDCustomer.Service.Mapper;
using HDCustomer.Models;

namespace HDCustomer.Controllers
{
    public class HDCustomerController : Controller
    {
        // GET: HDCustomer
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            string userID = formCollection["txtUserName"];
            string password = formCollection["txtPassword"];
            HDCustomerMapper hdCustomerMapper = new HDCustomerMapper();
            bool isValidUser = hdCustomerMapper.ValidateLoginCredentials(userID, password);
            if (isValidUser)
                return RedirectToAction("OfferSection", new { userID = userID });
            return View(isValidUser);
        }

        public ActionResult OfferSection(string userID)
        {
            HDCustomerMapper hdCustomerMapper = new HDCustomerMapper();
            OffersModelList offers = hdCustomerMapper.MapOffersModel(userID);
            if (offers != null)
                offers.UserID = userID;
            return View(offers);
        }

        public ActionResult UpgradeOfferSection(string userName, string customerType)
        {
            HDCustomerMapper hdCustomerMapper = new HDCustomerMapper();
            OffersModelList offers = hdCustomerMapper.UpgradeOfferSectionModel(userName, customerType);
            if (offers != null)
                offers.UserID = userName;
            return View(offers);
        }

        public JsonResult SaveProducts(string userID, string selectedPrd)
        {
            HDCustomerMapper hdCustomerMapper = new HDCustomerMapper();
            string customerType = hdCustomerMapper.SaveProducts(userID, selectedPrd);
            return Json(customerType, JsonRequestBehavior.AllowGet);
        }
    }
}