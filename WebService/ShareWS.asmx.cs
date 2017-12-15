using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using BLL;
using Share;

namespace WebService
{
    [ToolboxItem(false), WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ShareWS : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetUpdateFileVersion(string strUrl, string FileName)
        {
            string result;
            try
            {
                string strPath = base.Server.MapPath(ConfigurationManager.AppSettings[strUrl].ToString() + "\\" + FileName);
                if (!File.Exists(strPath))
                {
                    result = string.Empty;
                }
                else
                {
                    FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(strPath);
                    result = myFileVersionInfo.FileVersion;
                }
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            return result;
        }

        [WebMethod]
        public byte[] GetUpdateFileByte(string strUrl, string FileName)
        {
            FileHelper fileHelper = new FileHelper();
            FileName = base.Server.MapPath(ConfigurationManager.AppSettings[strUrl].ToString() + "\\" + FileName);
            return fileHelper.FileRead(FileName);
        }

        [WebMethod]
        public byte[] FileRead(string strUrl, string FileName)
        {
            FileHelper fileHelper = new FileHelper();
            FileName = base.Server.MapPath(ConfigurationManager.AppSettings[strUrl].ToString() + "\\" + FileName);
            return fileHelper.FileRead(FileName);
        }

        [WebMethod]
        public void FileSave(byte[] retBytes, string FileName, string ImgUrl, string BigImgUrl, string SmallImgUrl, string RecentSmallImgUrl, string RecentBigImgUrl, string RecentOrgImgUrl)
        {
            FileHelper fileHelper = new FileHelper();
            string InFile = base.Server.MapPath(ConfigurationManager.AppSettings[ImgUrl].ToString() + "\\" + FileName);
            string BigFileName = base.Server.MapPath(ConfigurationManager.AppSettings[BigImgUrl].ToString() + "\\" + FileName);
            string SmallFileName = base.Server.MapPath(ConfigurationManager.AppSettings[SmallImgUrl].ToString() + "\\" + FileName);
            string RecentSmallFName = base.Server.MapPath(ConfigurationManager.AppSettings[RecentSmallImgUrl].ToString() + "\\" + FileName);
            string RecentBigFName = base.Server.MapPath(ConfigurationManager.AppSettings[RecentBigImgUrl].ToString() + "\\" + FileName);
            string RecentOrgFName = base.Server.MapPath(ConfigurationManager.AppSettings[RecentOrgImgUrl].ToString() + "\\" + FileName);
            fileHelper.FileSave(retBytes, InFile, BigFileName, SmallFileName, RecentOrgFName, RecentSmallFName, RecentBigFName);
        }

        [WebMethod]
        public object ExecStoredProc(string strConnectionString, string strSqlSPName, string strParaKeys, string[] strParaVals, string strRetType)
        {
            ShareSqlManager sql = new ShareSqlManager(strConnectionString);
            return sql.ExecStoredProc(strSqlSPName, strParaKeys, strParaVals, strRetType);
        }

        [WebMethod]
        public DataTable GetTableByStoredProc(string strConnectionString, string strSqlSPName, string strParaKeys, string[] strParaVals)
        {
            ShareSqlManager sql = new ShareSqlManager(strConnectionString);
            DataTable dt = (DataTable)sql.ExecStoredProc(strSqlSPName, strParaKeys, strParaVals, "Table");
            dt.TableName = strSqlSPName;
            return dt;
        }

        [WebMethod]
        public DataSet GetDataSetByStoredProc(string strConnectionString, string strSqlSPName, string strParaKeys, string[] strParaVals)
        {
            ShareSqlManager sql = new ShareSqlManager(strConnectionString);
            return (DataSet)sql.ExecStoredProc(strSqlSPName, strParaKeys, strParaVals, "dataSet");
        }
    }
}
