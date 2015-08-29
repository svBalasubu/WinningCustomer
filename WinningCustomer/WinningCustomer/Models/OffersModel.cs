using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDCustomer.Models
{
    public class OffersModel
    {
        public string ProductID { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Cost { get; set; } 
        
    }

    public class OffersModelList
    {
        public string ServiceType { get; set; }
        public string CustomerType { get; set; }
        public string UserID { get; set; }
        public List<OffersModel> OffersList { get; set; }
    }
}