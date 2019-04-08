using RestSharp;

namespace DangEasy.RestEmail.Mailgun
{
    public class MailgunRestRequest : Interfaces.RestEmail.IRestRequest
    {
        public RestRequest RestSharpRequest;

        public MailgunRestRequest(string resource, Method method)
        {
            RestSharpRequest = new RestRequest(resource, method);
        }

        public void AddFile(string name, byte[] bytes, string fileName)
        {
            RestSharpRequest.AddFile(name, bytes, fileName);
        }

        public void AddParameter(string name, object value)
        {
            RestSharpRequest.AddParameter(name, value);
        }
    }
}
