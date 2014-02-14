using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SeekMail.Domain;
using SeekMail.Models;
using SeekMail.Services;

namespace SeekMail.Controllers
{
    public class MessagesController : ApiController
    {
        public const int PageSize = 10;

        readonly IMessagesRepository _repository;
        readonly IMessageTemplateRepository _templateRepository;
        readonly IMailService _mailService;

        public MessagesController(IMessagesRepository repository,
                                  IMessageTemplateRepository templateRepository,
                                  IMailService mailService)
        {
            _repository = repository;
            _templateRepository = templateRepository;
            _mailService = mailService;
        }

        /// <summary>
        /// Get a page of messages that have been sent.
        /// Next and previous page links are included in HTML headers when appropriate.
        /// </summary>
        public HttpResponseMessage Get(int page = 1)
        {
            if (page < 1)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Page parameter must be greater than zero.");

            var results = _repository.GetPage(page, PageSize);
            var response = Request.CreateResponse(HttpStatusCode.OK, results.ToList());

            if (results.HasNextPage)
                response.Headers.Add("Link", string.Format("<{0}>; rel=next", Url.Link("MessagesApi", new { page = page + 1 })));
            if (results.HasPreviousPage)
                response.Headers.Add("Link", string.Format("<{0}>; rel=previous", Url.Link("MessagesApi", new { page = page - 1 })));

            return response;
        }

        /// <summary>
        /// Send a new message for the specified template to the given email.
        /// This also saves the new message to the repository.
        /// Ideally, the service that sends messages would instead fire an event, and a handler would log the message.
        /// </summary>
        public IHttpActionResult Post(int templateId, [FromBody] PostMessageModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var template = _templateRepository.Get(templateId);
            if (template == null)
                return NotFound();

            // Would be better to handle log by using an event handler
            var message = _mailService.Send(template, model.EmailAddress);
            _repository.Add(message);
            _repository.SaveChanges();

            return Ok();
        }
    }
}