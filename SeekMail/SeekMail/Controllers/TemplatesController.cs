using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SeekMail.Domain;
using SeekMail.Models;

namespace SeekMail.Controllers
{
    public class TemplatesController : ApiController
    {
        readonly IMessageTemplateRepository _repository;

        public TemplatesController(IMessageTemplateRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get a list of templates.  Obviously paging would be needed here.
        /// </summary>
        public IEnumerable<MessageTemplateModel> Get()
        {
            return _repository.GetAll().Select(x => new MessageTemplateModel(x));
        }

        /// <summary>
        /// Get a specific template's details.
        /// </summary>
        public IHttpActionResult Get(int id)
        {
            var entity = _repository.Get(id);
            if (entity == null) return NotFound();

            return Ok(new MessageTemplateModel(entity));
        }

        /// <summary>
        /// Add a new template.
        /// </summary>
        public IHttpActionResult Post([FromBody] MessageTemplateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new MessageTemplate
            {
                Created = DateTime.Now,
                Name = model.Name,
                FromEmail = model.FromEmail,
                FromName = model.FromName,
                Subject = model.Subject,
                HtmlBody = model.HtmlBody,
                TextBody = model.TextBody
            };
            _repository.Add(entity);
            _repository.SaveChanges();

            return Created(Url.Link("DefaultApi", new { id = entity.Id }), new MessageTemplateModel(entity));
        }

        /// <summary>
        /// Update an existing template.
        /// </summary>
        public IHttpActionResult Put(int id, [FromBody] MessageTemplateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _repository.Get(id);
            if (entity == null) return NotFound();

            entity.Name = model.Name;
            entity.FromEmail = model.FromEmail;
            entity.FromName = model.FromName;
            entity.Subject = model.Subject;
            entity.TextBody = model.TextBody;
            entity.HtmlBody = model.HtmlBody;

            _repository.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Delete a template.
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            var entity = _repository.Get(id);
            if (entity == null) return NotFound();

            _repository.Delete(entity);
            _repository.SaveChanges();

            return Ok();
        }
    }
}