using System;
using System.Collections.Generic;
using System.Net;
using DangEasy.Interfaces.RestEmail;
using DangEasy.RestEmail.Mailgun.Models;
using Xunit;

namespace DangEasy.RestEmail.Mailgun.Test.Integration
{ 
    public class When_Sending : BaseIntegration
    {
        IRequestBuilder _requestBuilder;
        Client _client;
       
        const string ToEmail = "burner.a0dbafc6@tryninja.io"; // your recipient email
        const string FromEmail = "no-reply@test.com";

        const string TextBody = "Hello";
        const string HtmlBody = "<html><h1>Hello</h1></html>";
        const string ExampleBase64Image = "/9j/4AAQSkZJRgABAQEAYABgAAD//gA+Q1JFQVRPUjogZ2QtanBlZyB2MS4wICh1c2luZyBJSkcg\nSlBFRyB2ODApLCBkZWZhdWx0IHF1YWxpdHkK/9sAQwAIBgYHBgUIBwcHCQkICgwUDQwLCwwZEhMP\nFB0aHx4dGhwcICQuJyAiLCMcHCg3KSwwMTQ0NB8nOT04MjwuMzQy/9sAQwEJCQkMCwwYDQ0YMiEc\nITIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIy/8AAEQgA\nQABAAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMC\nBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYn\nKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeY\nmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5\n+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwAB\nAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpD\nREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ip\nqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMR\nAD8A9/orwW18Z694w8VtbyeJpvDtncOyafstg0chBxtLZHzdPXk444rtf+EE8Zf9FHu//AIf/F0A\nei0V51/wgnjL/oo93/4BD/4uj/hBPGX/AEUe7/8AAIf/ABdAHotFedf8IJ4y/wCij3f/AIBD/wCL\no/4QTxl/0Ue7/wDAIf8AxdAHotFedf8ACCeMv+ij3f8A4BD/AOLrirrxnr3g/wAVrbx+JpvEVnbu\nqahvtgscZJxtDZPzdfTkY55oA3fh94Y07xZ8LZ9P1CPIN9M0Uq/fifjDKf8AOa1fDHifUfDWsR+E\nPF8mZjxp+pN9y5ToFJP8Xbn6HnBMnwX/AORFf/r+m/pXVeJ/DGneLNHfT9QjyD80Uq/fifsyn/Oa\nANqivN/DHifUfDWsR+EPF8mZjxp+pN9y5ToFJP8AF25+h5wT6RQAUUV5v4n8T6j4l1iTwh4QkxMO\nNQ1JfuWydCoI/i7cfQc5IADxP4n1HxLrEnhDwhJiYcahqS/ctk6FQR/F24+g5yRlfEHwxp3hP4Ww\nafp8eAL6FpZW+/K/OWY/5xXofhjwxp3hPR00/T48AfNLK335X7sx/wA4rlfjR/yIqf8AX9D/AFoA\nPgv/AMiK/wD1/Tf0rrPE3ibTvCmjyajqMuFHEca/flbsqj1/lXnfw98Tad4U+Fs+o6jLhRfTCONf\nvytxhVHr/KtPwz4Z1HxTrEfi/wAXxYYc6dpjfct16hmB/i78/U9gACjZeB7/AOIIn17xi81sZ4yu\nn2MTEfZEPRj/ALXTg9e/YCfR/Gl34Iuj4d8cvIEjH+haqEZ0nj7BsZOR6/n6n1Cobm0tr2Ew3VvF\nPEeSkqBl/I0Aea6x40u/G90PDvgZ5Ckg/wBN1UoyJBH3C5wcn1/L1EF74Hv/AIfCDXvBzzXJgjC6\nhYysT9rQdWH+114HTt3B9RtrS2soRDa28UEQ5CRIFX8hU1AGP4Z8Tad4r0ePUdOlyp4kjb78Td1Y\nev8AOuT+NH/Iip/1/Q/1qLxN4Z1HwtrEni/whFljzqOmL9y4XqWUD+Lvx9R3BzPiF4m07xX8LYNR\n06XKm+hEkbffibnKsPX+dAGDbeC9e8I+KzPL4an8R2Ns7SaeEuQkaEnO4rg/N07DkZ5wK7b/AITz\nxj/0Te8/8Dh/8br0SigDzv8A4Tzxj/0Te8/8Dh/8bo/4Tzxj/wBE3vP/AAOH/wAbr0SigDzv/hPP\nGP8A0Te8/wDA4f8Axuj/AITzxj/0Te8/8Dh/8br0SigDzv8A4Tzxj/0Te8/8Dh/8bribnwXr3i7x\nWJ4vDU/hyxuXWTUA9yHjcg53BcD5uvY8nPGTXvVFAH//2Q==";


        public When_Sending()
        {
            _requestBuilder = new RequestBuilder(Configuration["AppSettings:Domain"]);
            _client = new Client(Configuration["AppSettings:ApiKey"], _requestBuilder);
        }



        [Fact]
        public void Simple_Email_Is_Sent()
        {
            var response = _client.SendAsync(FromEmail, ToEmail, "MailGun Test 1", HtmlBody, TextBody).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public void Email_With_Image_Is_Sent()
        {
            // email with attachment using the request builder
            var attachments = new List<IFileAttachment>
            {
                new FileAttachment {
                    AttachmentType = AttachmentType.Attachment,
                    Filename = "email.jpg",
                    Bytes = Convert.FromBase64String(ExampleBase64Image)
                }
            };

            var request = _client.RequestBuilder.BuildRequest(FromEmail, new List<string> { ToEmail }, "MailGun Test with image", HtmlBody, TextBody, attachments: attachments);

            var response = _client.SendAsync(request).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }



        [Fact]
        public void Email_With_Inline_Image_Is_Sent()
        {
            // email with attachment using the request builder
            var inlineAttachments = new List<IFileAttachment>
            {
                new FileAttachment {
                    AttachmentType = AttachmentType.Inline,
                    Filename = "email.jpg",
                    Bytes = Convert.FromBase64String(ExampleBase64Image)
                }
            };

            var htmlBodyInline = $"<html><h1>Hello</h1><div>Before image... <img src=\"cid:email.jpg\" /> ...after image</div></html>";

            var request = _client.RequestBuilder.BuildRequest(FromEmail, new List<string> { ToEmail }, "MailGun Test with inline image", htmlBodyInline, TextBody, attachments: inlineAttachments);

            var response = _client.SendAsync(request).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
