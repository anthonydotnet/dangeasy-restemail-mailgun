using System;
using System.Net;

namespace DangEasy.RestEmail.Mailgun
{
    public class MailgunRestResponse : Interfaces.RestEmail.IRestResponse
    {
        private RestSharp.IRestResponse _restSharpResponse;

        public MailgunRestResponse(RestSharp.IRestResponse restSharpResponse)
        {
            _restSharpResponse = restSharpResponse;
        }

        public HttpStatusCode StatusCode => _restSharpResponse.StatusCode;

        public string Content => _restSharpResponse.Content;

        public Exception Exception => _restSharpResponse.ErrorException;
    }
}
