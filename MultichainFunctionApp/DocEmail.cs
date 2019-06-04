using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;
using SendGrid;
using SendGrid.Client;
using System.Text.RegularExpressions;

namespace MultichainFunctionApp
{
    public static class DocEmail
    {
        [FunctionName("DocEmail")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log, Mail msg, [SendGrid(ApiKey = "MultichainDocApi")] IAsyncCollector<SendGridMessage> messageCollector)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            if (name == null)
            {
                // Get request body  of.
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data?.name;
            }


            //  var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            // var client = new SendGridClient(ApiKey);
            // var emailObject = JsonConvert.DeserializeObject<OutgoingEmail>(Encoding.UTF8.GetString(email.Body));

            /*msg = new Mail();
            

            string messageText = "Test email from Sendgrid";
            Content content = new Content
            {
                Type = "text/plain",
                Value = messageText
            };
            msg.AddContent(content);
            msg.ReplyTo = new Email("aniruddha.patil@hotmail.com") ;
            msg.From = new Email("ani.patil12@gmail.com");
            msg.Subject = messageText;*/

            var message = new SendGridMessage();
            string messageText = "Test email from Sendgrid";
            Content content = new Content
            {
                Type = "text/plain",
                Value = messageText
            };
            message.AddTo("aniruddha.patil@hotmail.com");
           // message. = content;
            message.From = new System.Net.Mail.MailAddress("ani.patil12@gmail.com");
            //message.From(new EmailAddress("a@gmail.com", "Azure Tips and Tricks"));
            message.Subject = "Thanks for your order";
            await messageCollector.AddAsync(message);

            // msg.Subject.ToString("Mail from Azure and SendGrid");

           //var response = await client.SendEmailAsync(msg);
                 return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
        /*  public static HttpResponseMessage RunMail(out Mail message)
         {
             // parse query parameter
             //string messageText = req.GetQueryNameValuePairs()
             //  .FirstOrDefault(q => string.Compare(q.Key, "message", true) == 0)
             //  .Value;

            message = new Mail();
             message.ReplyTo = "aniruddha.patil@hotmail.com";
             Content content = new Content
             {
                 Type = "text/plain",
                 Value = "Test email from Sendgrid"
             };
             message.AddContent(content);

             return req.CreateResponse(HttpStatusCode.Accepted);
    }*/
    }
}
