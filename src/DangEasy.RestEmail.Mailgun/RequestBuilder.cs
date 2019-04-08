using System.Collections.Generic;
using DangEasy.Interfaces.RestEmail;
using RestSharp;

namespace DangEasy.RestEmail.Mailgun
{
    public class RequestBuilder : IRequestBuilder
    {
        private readonly string _domainName;

        public RequestBuilder(string domain)
        {
            _domainName = domain;
        }

        public Interfaces.RestEmail.IRestRequest BuildRequest(string sender, string recipent, string subject, string htmlBody, string textBody)
        {
            var request = BuildRequest(sender, new List<string> { recipent }, subject, htmlBody, textBody);

            return request;
        }


        public Interfaces.RestEmail.IRestRequest BuildRequest(string sender, List<string> recipents, string subject, string htmlBody, string textBody = "", List<string> ccs = null, List<string> bccs = null, List<IFileAttachment> attachments = null)
        {
            var request = BuildRequest(sender, recipents, subject, htmlBody, textBody);

            if (ccs != null)
            {
                ccs.ForEach(x => request.AddParameter("cc", x));
            }
            if (bccs != null)
            {
                bccs.ForEach(x => request.AddParameter("bcc", x));
            }

            if (attachments != null)
            {
                attachments.ForEach(x => request.AddFile(x.AttachmentType.ToLower(), x.Bytes, x.Filename));
            }

            return request;
        }


        protected Interfaces.RestEmail.IRestRequest BuildRequest(string sender, List<string> recipents, string subject, string htmlBody, string textBody)
        {
            var request = new MailgunRestRequest($"{_domainName}/messages", Method.POST);
           // request.AddParameter("domain", _domainName, ParameterType.UrlSegment);
            request.AddParameter("from", sender);
            recipents.ForEach(x => request.AddParameter("to", x));

            request.AddParameter("subject", subject);
            request.AddParameter("html", htmlBody);
            request.AddParameter("text", textBody);

            return request;
        }
    }
}
