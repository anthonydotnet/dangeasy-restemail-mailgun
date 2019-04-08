using DangEasy.Interfaces.RestEmail;

namespace DangEasy.RestEmail.Mailgun.Models
{
    public class FileAttachment: IFileAttachment
    {
        public string Filename { get; set; }
        public byte[] Bytes { get; set; }
        public string AttachmentType { get; set; }
    }

    public class AttachmentType
    {
        public const string Attachment = "attachment";
        public const string Message = "message";
        public const string Inline = "inline";
    }
}
