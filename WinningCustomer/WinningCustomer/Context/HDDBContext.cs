using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using HDCustomer.Models;

namespace HDCustomer.Context
{
    public class HDDBContext
    {
        public string CheckValidUser(string userID, string pwd)
        {
            string result = string.Empty;
            string Sqlconnectionstring = ConfigurationManager.ConnectionStrings["HDCConnection"].ConnectionString;
            try
            {
                SqlConnection sqlcon = new SqlConnection(Sqlconnectionstring);
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand("SELECT CASE WHEN EXISTS (SELECT * FROM Tbl_Login WHERE UserID = '" + userID + "' and Credential ='" + pwd + "')THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS _result", sqlcon);
                SqlDataReader SqlDR = cmd.ExecuteReader();

                while (SqlDR.Read())
                {
                    result = SqlDR["_result"].ToString();
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public OffersModelList GetUserProfile(string userID)
        {
            int activetype = 1;
            OffersModelList _OffersModelList = new OffersModelList();
            _OffersModelList.OffersList = new List<OffersModel>();
            string Sqlconnectionstring = ConfigurationManager.ConnectionStrings["HDCConnection"].ConnectionString;
            try
            {
                SqlConnection sqlcon = new SqlConnection(Sqlconnectionstring);
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT [CustUsageID]
                                                          ,[Cust_FirstName]
                                                          ,[Cust_LastName]
                                                          
                                                          ,[DataImage]
                                                          
                                                          ,A.[TariffID]
	                                                      ,B.TariffDescription
	                                                      ,B.TariffImage
	                                                      ,B.TariffCost
                                                          ,B.serviceType
                                                      FROM [dbo].[Tbl_CustUsage] A
                                                      inner join [dbo].[tbl_TariffMaster] B On A.TariffID = B.TariffID
                                                      Where A.userID = '" + userID + "' and A.isactive='" + activetype + "'", sqlcon);

                SqlDataReader SqlDR = cmd.ExecuteReader();

                while (SqlDR.Read())
                {
                    OffersModel _OffersModel = new OffersModel();
                    _OffersModel.ProductID = SqlDR["TariffID"].ToString();
                    _OffersModel.Description = SqlDR["TariffDescription"].ToString();
                    _OffersModel.Cost = SqlDR["TariffCost"].ToString();
                    _OffersModel.Image = SqlDR["TariffImage"].ToString();
                    _OffersModelList.ServiceType = SqlDR["serviceType"].ToString();
                    _OffersModelList.OffersList.Add(_OffersModel);
                }
                SqlDR.Close();
                SqlCommand cmd1 = new SqlCommand(@"CREATE TABLE #tmpCust (CustomerID INT,Cust_FirstName nCHAR(10),Cust_LastName nCHAR(10),strStatus VARCHAR(50))

                                                INSERT INTO #tmpCust(CustomerID,Cust_FirstName,Cust_LastName)
                                                SELECT CustomerID,Cust_FirstName,Cust_LastName FROM
                                                (
                                                SELECT  [CustomerID] , A.Cust_FirstName,A.Cust_LastName
     	                                                ,SUM(B.TariffCost) AS TariffCost
                                                FROM [dbo].[Tbl_CustUsage] A
                                                inner join [dbo].[tbl_TariffMaster] B On A.TariffID = B.TariffID
                                                group by A.customerID, A.Cust_FirstName,A.Cust_LastName 
                                                ) A
                                                ORDER BY A.TariffCost DESC

                                                UPDATE TOP (25) PERCENT tmp SET tmp.strStatus = 'Gold'
                                                FROM #tmpCust tmp WHERE tmp.strStatus IS NULL

                                                UPDATE TOP (25) PERCENT tmp SET tmp.strStatus = 'Silver'
                                                FROM #tmpCust tmp WHERE tmp.strStatus IS NULL

                                                UPDATE TOP (25) PERCENT tmp SET tmp.strStatus = 'Platinum'
                                                FROM #tmpCust tmp WHERE tmp.strStatus IS NULL

                                                UPDATE TOP (25) PERCENT tmp SET tmp.strStatus = 'Bronze'
                                                FROM #tmpCust tmp WHERE tmp.strStatus IS NULL

                                                SELECT * FROM #tmpCust where CustomerID=(Select DISTINCT CustomerID  from Tbl_Login Where  UserID= '" + userID + "')  DROP TABLE #tmpCust", sqlcon);

                SqlDataReader SqlDr1 = cmd1.ExecuteReader();
                while (SqlDr1.Read())
                {
                    _OffersModelList.CustomerType = SqlDr1["strStatus"].ToString();
                }
                SqlDr1.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {

            }
            return _OffersModelList;
        }

        public OffersModelList UpgradeOfferDetails(string userID, string customerType = "")
        {
            int activetype = 1;

            if (customerType == "inactive")
            {
                activetype = 0;
            }

            OffersModelList _OffersModelList = new OffersModelList();
            _OffersModelList.OffersList = new List<OffersModel>();
            string Sqlconnectionstring = ConfigurationManager.ConnectionStrings["HDCConnection"].ConnectionString;
            try
            {
                SqlConnection sqlcon = new SqlConnection(Sqlconnectionstring);
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT 
                                                         A.CustUsageID AS CustUsageID
                                                          ,[Cust_FirstName]
                                                          ,[Cust_LastName]                                                         
                                                          ,[DataImage]                                                                                                                   
	                                                      ,TariffDescription
	                                                      ,TariffImage
	                                                      ,TariffCost
                                                          ,serviceType
                                                      FROM [dbo].Tbl_CustUsage A inner join
                                                      [dbo].Tbl_Tariffmaster B On A.TariffID = B.TariffID
                                                      Where A.userID = '" + userID + "' and A.isactive='" + activetype + "'", sqlcon);
                                                      

                SqlDataReader SqlDR = cmd.ExecuteReader();

                while (SqlDR.Read())
                {
                    OffersModel _OffersModel = new OffersModel();
                    _OffersModel.ProductID = SqlDR["CustUsageID"].ToString();
                    _OffersModel.Description = SqlDR["TariffDescription"].ToString();
                    _OffersModel.Cost = SqlDR["TariffCost"].ToString();
                    _OffersModel.Image = SqlDR["TariffImage"].ToString();
                    _OffersModelList.ServiceType = SqlDR["serviceType"].ToString();
                    _OffersModelList.OffersList.Add(_OffersModel);
                }
               sqlcon.Close();
            }
            catch (Exception ex)
            {

            }
            return _OffersModelList;
        }

        public OffersModelList GetUserTarrif(string userID)
        {
            OffersModelList _OffersModelList = new OffersModelList();
            OffersModel _OffersModel = new OffersModel();
            string Sqlconnectionstring = ConfigurationManager.ConnectionStrings["HDCConnection"].ConnectionString;
            try
            {
                SqlConnection sqlcon = new SqlConnection(Sqlconnectionstring);
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT [Cust_FirstName]
                                                          ,[Cust_LastName]                                                          
                                                          ,[DataImage]                                                          
                                                          ,A.[TariffID]
	                                                      ,B.TariffDescription
	                                                      ,B.TariffImage
	                                                      ,B.TariffCost
                                                          ,B.serviceType
                                                      FROM [dbo].[Tbl_CustUsage] A
                                                      inner join [dbo].[tbl_TariffMaster] B On A.TariffID = B.TariffID
                                                      Where A.CustomerID = " + userID + "", sqlcon);
                SqlDataReader SqlDR = cmd.ExecuteReader();

                while (SqlDR.Read())
                {
                    _OffersModel.ProductID = SqlDR["TariffID"].ToString();
                    _OffersModel.Description = SqlDR["TariffDescription"].ToString();
                    _OffersModel.Cost = SqlDR["TariffCost"].ToString();
                    _OffersModel.Image = SqlDR["TariffImage"].ToString();
                    _OffersModelList.ServiceType = SqlDR["serviceType"].ToString();
                    _OffersModelList.CustomerType = SqlDR["customerType"].ToString();
                    _OffersModelList.OffersList.Add(_OffersModel);
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {

            }
            return _OffersModelList;
        }


        public string SaveOfferDetails(string userID, string selectedPrd)
        {
            string result = string.Empty;
             
            string Sqlconnectionstring = ConfigurationManager.ConnectionStrings["HDCConnection"].ConnectionString;
            try
            {
                SqlConnection sqlcon = new SqlConnection(Sqlconnectionstring);
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand("SAVEPRODUCTS", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@CustUsageID", selectedPrd);

                int Execquery = cmd.ExecuteNonQuery();
                if(Execquery > 0)
                {
                    result = "Success";
                }
                else
                {
                    result = "Failure";
                }
                sqlcon.Close();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}