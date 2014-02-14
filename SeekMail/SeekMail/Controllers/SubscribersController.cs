using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SeekMail.Domain;
using SeekMail.Models;

namespace SeekMail.Controllers
{
    public class SubscribersController : ApiController
    {
        readonly ISubscriberRepository _repository;

        public SubscribersController(ISubscriberRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get full list of subscribers.  Obviously paging would be needed here.
        /// </summary>
        public IEnumerable<SubscriberModel> Get()
        {
            return _repository.GetAll().Select(x => new SubscriberModel(x));
        }

        /// <summary>
        /// Add a new subscriber.
        /// </summary>
        public IHttpActionResult Post([FromBody] SubscriberModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingSubscriber = _repository.Get(model.EmailAddress);
            if (existingSubscriber != null)
                return Conflict();

            var entity = new Subscriber {EmailAddress = model.EmailAddress};
            _repository.Add(entity);
            _repository.SaveChanges();

            return Ok(new SubscriberModel(entity));
        }

        /// <summary>
        /// Delete a subscriber.
        /// Note: Email addresses cannot be part of the URI itself, but must be URIEncoded and included in the query string.
        /// </summary>
        public IHttpActionResult Delete(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                return BadRequest();

            var entity = _repository.Get(emailAddress);
            if (entity == null) return NotFound();

            _repository.Delete(entity);
            _repository.SaveChanges();

            return Ok();
        }
    }
}