using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Bulky.Utility;

public class EmailSender: IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //This is fake Email sender implementation ,,,Replace it with your specific provided email service
        return Task.CompletedTask;
    }
}