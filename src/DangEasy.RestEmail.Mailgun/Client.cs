using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DangEasy.Interfaces.RestEmail;
using DangEasy.RestEmail.Mailgun;
using RestSharp;
using RestSharp.Authenticators;

namespace DangEasy.RestEmail
{
    public class Client : IRestEmailClient
    {
        public IRequestBuilder RequestBuilder { get; private set; }

        private readonly IRestClient _client;

        public Client(string apiKey, IRequestBuilder requestBuilder)
        {
            _client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", apiKey)
            };

            RequestBuilder = requestBuilder;
        }


        public async Task<Interfaces.RestEmail.IRestResponse> SendAsync(string sender, string recipents, string subject, string htmlBody, string textBody = "", List<string> ccs = null, List<string> bccs = null, List<IFileAttachment> attachments = null)
        {
            var request = RequestBuilder.BuildRequest(sender, new List<string> { recipents }, subject, htmlBody, textBody, ccs, bccs, attachments);

            return await SendAsync(request);
        }


        public async Task<Interfaces.RestEmail.IRestResponse> SendAsync(Interfaces.RestEmail.IRestRequest request)
        {
            var restSharpRequest = ((MailgunRestRequest)request).RestSharpRequest;

            var restSharpResponse = await _client.ExecuteTaskAsync(restSharpRequest);

            var response = new MailgunRestResponse(restSharpResponse);

            return response;
        }
    }
}
