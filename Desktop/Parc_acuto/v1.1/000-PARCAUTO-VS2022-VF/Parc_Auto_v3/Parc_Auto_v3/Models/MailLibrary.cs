using System.Net.Mail;
using System.Net.Mime;

namespace Parc_Auto_v3.Models
{
    public static class MailLibrary
    {
        public static bool CreateMail(string _server, string _from, string _to, string _subject, string _body, Attachment _attachment, ref string _message, bool _IsBodyHtml = false)
        {
            try
            {
                //_to = "sami_essid@bt.com.tn,riadh.benhassine@bt.com.tn";
                //_from = "BTDevinSSISService@BT.COM.TN";
                MailMessage message = new MailMessage(_from, _to);
                message.Subject = _subject;
                message.Body = _body;
                message.IsBodyHtml = _IsBodyHtml;
                SmtpClient client = new SmtpClient(_server);
                // client.UseDefaultCredentials = true;
                if (_attachment != null)
                {
                    message.Attachments.Add(_attachment);
                }

                client.Send(message);

                return true;

            }
            catch (Exception ex)
            {
                _message = "Exception:  " + ex.Message;

                return false;
            }
        }
        public static Attachment WrapExcelBytesInAttachment(byte[] excelContent)
        {
            try
            {
                //text/csv   
                Stream stream = new MemoryStream(excelContent);
                //Attachment attachment = new Attachment(stream, "fileName.xls");
                //attachment.ContentType = new ContentType("application/vnd.ms-excel");
                Attachment attachment = new Attachment(stream, "fileName" + DateTime.Now + ".csv");
                attachment.ContentType = new ContentType("text/csv");
                return attachment;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Attachment ConvertBytesToCSVAttachment(byte[] excelContent, string _destinationName)
        {
            try
            {
              

                Stream stream = new MemoryStream(excelContent);
                //Attachment attachment = new Attachment(stream, "fileName.xls");
                //attachment.ContentType = new ContentType("application/vnd.ms-excel");
                Attachment attachment = new Attachment(stream, _destinationName + ".csv");
                attachment.ContentType = new ContentType("text/csv");
                return attachment;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }

}
