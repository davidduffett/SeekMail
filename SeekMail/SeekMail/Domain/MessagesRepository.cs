using System.Linq;
using PagedList;

namespace SeekMail.Domain
{
    public interface IMessagesRepository
    {
        IPagedList<Message> GetPage(int page, int pageSize);
        Message Add(Message entity);
        void SaveChanges();
    }

    public class MessagesRepository : IMessagesRepository
    {
        readonly SeekMailContext _context;

        public MessagesRepository(SeekMailContext context)
        {
            _context = context;
        }

        public IPagedList<Message> GetPage(int page, int pageSize)
        {
            return _context.Messages
                .OrderByDescending(x => x.Id)
                .ToPagedList(page, pageSize);
        }

        public Message Add(Message entity)
        {
            return _context.Messages.Add(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}