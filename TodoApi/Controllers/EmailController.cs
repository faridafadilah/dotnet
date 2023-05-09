using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;

namespace TodoApi.Controllers
{
  [ApiController]
  [Route("email")]
  public class EmailController : ControllerBase
  {
    private readonly ILogger<EmailController> _logger;

    public EmailController(ILogger<EmailController> logger)
    {
      _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest req)
    {
      var emailSender = new EmailSender();
      await emailSender.SendEmailAsync(req.Email, req.Subject, req.Message);
      return Ok("Berhasil Send!");
    }
  }
}