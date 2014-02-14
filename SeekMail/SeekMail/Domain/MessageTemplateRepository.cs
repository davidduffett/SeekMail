using System.Collections.Generic;
using System.Linq;

namespace SeekMail.Domain
{
    public interface IMessageTemplateRepository
    {
        IEnumerable<MessageTemplate> GetAll();
        MessageTemplate Get(int id);
        MessageTemplate Add(MessageTemplate item);
        void Delete(MessageTemplate item);
        void SaveChanges();
    }

    public class MessageTemplateRepository : IMessageTemplateRepository
    {
        readonly SeekMailContext _context;

        public MessageTemplateRepository(SeekMailContext context)
        {
            _context = context;
        }

        public IEnumerable<MessageTemplate> GetAll()
        {
            return _context.MessageTemplates;
        }

        public MessageTemplate Get(int id)
        {
            return _context.MessageTemplates.SingleOrDefault(x => x.Id == id);
        }

        public MessageTemplate Add(MessageTemplate item)
        {
            return _context.MessageTemplates.Add(item);
        }

        public void Delete(MessageTemplate item)
        {
            _context.MessageTemplates.Remove(item);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}