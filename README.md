# DangEasy-RestEmail-Mailgun
A simple easy to use implementation of Mailgun's REST API.

The goal of this library is to provide simple wrapper functionality for only a subset of features. 

I hope you find this helpful.

# Installation

Use NuGet to install the [package](https://www.nuget.org/packages/DangEasy.RestEmail.Mailgun/).

```
PM> Install-Package DangEasy.RestEmail.Mailgun
```


## Setup
```
var client = new Client("https://api.mailgun.net/v3", Your_ApiKey, new RequestBuilder(Your_Domain));
```

## Simple Email
```
var response = client.SendAsync(fromEmail, toEmail, "MailGun Test 1", htmlBody, textBody).Result;
System.Console.WriteLine(response.StatusCode);
```

## Email with image
```
var attachments = new List<IFileAttachment>
{
    new FileAttachment {
        AttachmentType = AttachmentType.Attachment,
        Filename = "email.jpg",
        Bytes = Convert.FromBase64String(ExampleBase64Image)
    }
};

var request = client.RequestBuilder.BuildRequest(fromEmail, new List<string> { toEmail }, 
                                                "MailGun Test with image", htmlBody, textBody, 
                                                attachments: attachments);

response = client.SendAsync(request).Result;
System.Console.WriteLine(response.StatusCode);
```

## Email with inline image
```
var inlineAttachments = new List<IFileAttachment>
{
    new FileAttachment {
        AttachmentType = AttachmentType.Inline,
        Filename = "email.jpg",
        Bytes = Convert.FromBase64String(ExampleBase64Image)
    }
};

var htmlBodyInline = $"<html><h1>Hello</h1><div>Inline image: <img src=\"cid:email.jpg\" /> end image</div></html>";
request = client.RequestBuilder.BuildRequest(fromEmail, new List<string> { toEmail }, 
                                            "MailGun Test with inline image", htmlBodyInline, textBody, 
                                            attachments: inlineAttachments);

response = client.SendAsync(request).Result;
System.Console.WriteLine(response.StatusCode);
```

