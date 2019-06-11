using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LocationPolling
{
    public class AppService
    {
        public async Task<string> RestartVMs()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url;
                    HttpResponseMessage response;

                    url = Constants.RestartVMEndPoints;
                    response = await client.PostAsync(url, null);

                    if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                        return "Success";
                    else
                        return "Fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
