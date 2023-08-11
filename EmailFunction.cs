using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

public static class EmailFunction
{
    [FunctionName("EmailFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        log.LogInformation("Email  notification function started.");

        // Read the request
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        // assume request has email only
        string emailAddress = requestBody.Trim();

        // Initialize SendGrid API key and client
        var apiKey = "API_KEY";
        var client = new SendGridClient(apiKey);

        // Create the email message
        var msg = new SendGridMessage
        {
            From = new EmailAddress("test@email.com", "Your Name"),
            Subject = "Welcome Email",
            PlainTextContent = "Welcome! Have a great day"
        };
        msg.AddTo(new EmailAddress(emailAddress));

        // Send the email
        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode == HttpStatusCode.Accepted)
        {
            return new OkObjectResult("Email sent successfully.");
        }
        else
        {
            return new BadRequestObjectResult("Failed to send email.");
        }
    }
}
