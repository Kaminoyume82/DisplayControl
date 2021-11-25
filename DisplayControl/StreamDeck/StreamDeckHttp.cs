using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media;
using DisplayControl.Extensions;
using DisplayControl.Log;

namespace DisplayControl.StreamDeck
{
    public class StreamDeckHttp
    {
        private readonly Logger logger;
        private readonly UriBuilder streamDeckUriBuilder;

        public StreamDeckHttp(IPEndPoint host, Logger logger)
        {
            this.logger = logger;

            streamDeckUriBuilder = new UriBuilder();
            streamDeckUriBuilder.Scheme = "http";
            streamDeckUriBuilder.Host = host.Address.ToString();
            streamDeckUriBuilder.Port = host.Port;
        }

        public void SetButtonBackground(int page, int button, Color bgcolor)
        {
            streamDeckUriBuilder.Path = $"/style/bank/{page}/{button}/";
            streamDeckUriBuilder.Query = $"bgcolor={HttpUtility.UrlEncode(bgcolor.ToRGB())}";

            WebRequest webRequest = WebRequest.Create(streamDeckUriBuilder.Uri);
            HttpWebResponse webResponse;

            try
            {
                webResponse = webRequest.GetResponse() as HttpWebResponse;
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    this.logger.LogError($"SetButtonBackground http error: {webResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"SetButtonBackground exception: {ex.Message}");
            }
        }
    }
}
