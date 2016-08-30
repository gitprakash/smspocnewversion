namespace DataServiceLibrary
{
    using System;
    using System.Configuration;
    public static class ConfigUtility
    {
        public static string GetAppkeyvalues(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        public static string SubmitMessageApiformaturl()
        {
            //username=@username&pass=@password&senderid=@senderId&dest_mobileno=@mobileno&message=@message&response=Y

            string apiurl = ConfigUtility.GetAppkeyvalues("submitmessageapiurl");
            if (string.IsNullOrEmpty(apiurl))
            {
                throw new Exception("Problem in getting SubmitMessageApiformaturl ");
            }
            apiurl = apiurl + "username={0}&pass={1}&senderid={2}";
            string username = ConfigUtility.GetAppkeyvalues("username");
            string password = ConfigUtility.GetAppkeyvalues("password");
            string senderId = ConfigUtility.GetAppkeyvalues("senderId");
            apiurl= string.Format(apiurl, username, password, senderId);
            return apiurl;
        }
        public static string GetMessageDeliveryReportUrl()
        {
            string url = ConfigUtility.GetAppkeyvalues("messagestatusapiurl");
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("Problem in getting GetMessageDeliveryReportUrl ");
            }
            else
            {
                return url = url + " Scheduleid={0}"; ;
            }
        }
    }
}