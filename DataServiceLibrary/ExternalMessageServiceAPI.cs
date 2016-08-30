using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DataServiceLibrary
{
    public class ExternalMessageServiceAPI
    {
        public async Task<string> SubmitMessage(string url)
        {
            using (HttpClient httpclient = new HttpClient())
            {
              
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await httpclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var submitid=await response.Content.ReadAsStringAsync();
                return submitid;
            }
        }
        public async Task<string> GetMessageStatus(string url)
        {
            using (HttpClient httpclient = new HttpClient())
            {  
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await httpclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync(); 
            }
        }
        public static string SubmitMessageApiformaturl()
        {
            string apiformaturl = ConfigUtility.SubmitMessageApiformaturl();
            apiformaturl = apiformaturl + "&dest_mobileno={0}&message={1}&response=Y";
            return apiformaturl;
        }

        public static string GetMessageDeliveryReportUrl()
        {
            string url = ConfigUtility.GetMessageDeliveryReportUrl();
              return url;
        }

        public bool IsMessageSubmitted(string submitid)
        {
            if (submitid.Contains("ES1001") || submitid.Contains("ES1002") || submitid.Contains("ES1003") || submitid.Contains("ES1004")
                  || submitid.Contains("ES1005") || submitid.Contains("ES1006") || submitid.Contains("ES1007") || submitid.Contains("ES1008")
                  || submitid.Contains("ES1009") || submitid.Contains("ES1010") || submitid.Contains("ES1011") || submitid.Contains("ES1012")
                  || submitid.Contains("ES1013") || submitid.Contains("ES1014") || submitid.Contains("ES1015") || submitid.Contains("ES1016")
                  || submitid.Contains("ES1017") || submitid.Contains("ES1018") || submitid.Contains("Your Request has been not proceed...!")
                  || submitid.Contains("You have Exceeded your SMS Limit") || submitid.Contains("Account is Expire"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
