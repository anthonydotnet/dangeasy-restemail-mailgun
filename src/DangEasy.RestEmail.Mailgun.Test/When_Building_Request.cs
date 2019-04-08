using System;
using System.Collections.Generic;
using DangEasy.Interfaces.RestEmail;
using DangEasy.RestEmail.Mailgun;
using DangEasy.RestEmail.Mailgun.Models;
using Xunit;

namespace DangEasy.RestEmail.Test
{
    public class When_Building_Request : IDisposable
    {
        IRequestBuilder _requestBuilder;

        public When_Building_Request()
        {
            _requestBuilder = new RequestBuilder("mydomain.com");
        }

        public void Dispose()
        {
        }


        [Fact]
        public void Basic_Parameters_Are_Hydrated()
        {
            var request = _requestBuilder.BuildRequest(
                sender: "from@test.com",
                recipent: "to1@test.com",
                subject: "Subject",
                htmlBody: "<html>Hello</html>",
                textBody: "Hello"
                );

            var items = ((MailgunRestRequest)request).RestSharpRequest.Parameters;

            Assert.True(items.Exists(x => x.Name == "from" && (string)x.Value == "from@test.com"));
            Assert.True(items.Exists(x => x.Name == "to" && (string)x.Value == "to1@test.com"));
            Assert.True(items.Exists(x => x.Name == "subject" && (string)x.Value == "Subject"));
            Assert.True(items.Exists(x => x.Name == "html" && (string)x.Value == "<html>Hello</html>"));
            Assert.True(items.Exists(x => x.Name == "text" && (string)x.Value == "Hello"));
        }


        [Fact]
        public void Complex_Parameters_Are_Hydrated()
        {
            var request = _requestBuilder.BuildRequest(
                 sender: "from@test.com",
                 recipents: new List<string> { "to1@test.com", "to2@test.com" },
                 subject: "Subject",
                 htmlBody: "<html>Hello</html>",
                 textBody: "Hello",
                 ccs: new List<string> { "cc1@test.com", "cc2@test.com" },
                 bccs: new List<string> { "bcc1@test.com", "bcc2@test.com" },
                 attachments: new List<IFileAttachment>
                    {
                        new FileAttachment { AttachmentType = AttachmentType.Attachment, Filename = "test.jpg", Bytes = new byte [128] }
                    }
                 );

            var items = ((MailgunRestRequest)request).RestSharpRequest.Parameters;
            var fileItems = ((MailgunRestRequest)request).RestSharpRequest.Files;

            Assert.True(items.Exists(x => x.Name == "from" && (string)x.Value == "from@test.com"));
            var recipients = items.FindAll(x => x.Name == "to");
            Assert.Equal("to1@test.com", (string)recipients[0].Value);
            Assert.Equal("to2@test.com", (string)recipients[1].Value);

            Assert.True(items.Exists(x => x.Name == "subject" && (string)x.Value == "Subject"));
            Assert.True(items.Exists(x => x.Name == "html" && (string)x.Value == "<html>Hello</html>"));
            Assert.True(items.Exists(x => x.Name == "text" && (string)x.Value == "Hello"));

            var ccs = items.FindAll(x => x.Name == "cc");
            Assert.Equal("cc1@test.com", (string)ccs[0].Value);
            Assert.Equal("cc2@test.com", (string)ccs[1].Value);

            var bccs = items.FindAll(x => x.Name == "bcc");
            Assert.Equal("bcc1@test.com", (string)bccs[0].Value);
            Assert.Equal("bcc2@test.com", (string)bccs[1].Value);

            Assert.Equal("test.jpg", (string)fileItems[0].FileName);
            Assert.Equal("attachment", (string)fileItems[0].Name);
        }
    }
}
