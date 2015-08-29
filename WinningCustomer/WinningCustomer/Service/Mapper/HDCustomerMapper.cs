using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HDCustomer.Models;
using HDCustomer.Context;


namespace HDCustomer.Service.Mapper
{
    public class HDCustomerMapper
    {

        public bool ValidateLoginCredentials(string userID, string password)
        {
            bool isValidUser = false;
            HDDBContext cxt = new HDDBContext();
            string result = cxt.CheckValidUser(userID, password);
            if (result == "True")
                isValidUser = true;
            return isValidUser;
        }

        public OffersModelList MapOffersModel(string userID)
        {
            HDDBContext _HDDBContext = new HDDBContext();
            OffersModelList _OffersModelList = _HDDBContext.GetUserProfile(userID);
            return _OffersModelList;
        }
        public OffersModelList UpgradeOfferSectionModel(string userID, string customerType)
        { 
            HDDBContext _HDDBContext = new HDDBContext();
            OffersModelList _OffersModelList = _HDDBContext.UpgradeOfferDetails(userID, customerType);
            return _OffersModelList;
        }

        public string SaveProducts(string userID, string selectedPrd)
        {
            string CustomerType = string.Empty;
            HDDBContext _HDDBContext = new HDDBContext();
            string Result = _HDDBContext.SaveOfferDetails(userID, selectedPrd);
            if (Result == "Success")
            {
                OffersModelList _OffersModelList = _HDDBContext.GetUserProfile(userID);
                CustomerType = _OffersModelList.CustomerType;
            }
            else
            {
                CustomerType = "Failure";
            }
            return CustomerType;
        }
    }
}