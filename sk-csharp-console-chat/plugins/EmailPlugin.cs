using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Plugins;

internal class EmailPlugin
{
    [KernelFunction]
    [Description("Sends an email to a recipient.")]
    public async Task SendEmailAsync(
        Kernel kernel,
        [Description("Semicolon delimitated list of emails of the recipients. Ask for valid email address, do not generate")] string recipientEmails,
        string subject,
        string body
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        await Task.Delay(500);
        Console.WriteLine("Recipient Emails: " + recipientEmails);
        Console.WriteLine("Subject: " + subject);
        Console.WriteLine("Body: " + body);
        Console.WriteLine("....Email sent!....");
    }
}
