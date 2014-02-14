using System.Collections.Generic;
using System.Linq;

namespace SeekMail.Domain
{
    public interface ISubscriberRepository
    {
        IEnumerable<Subscriber> GetAll();
        Subscriber Get(string emailAddress);
        Subscriber Add(Subscriber subscriber);
        void Delete(Subscriber subscriber);
        void SaveChanges();
    }

    public class SubscriberRepository : ISubscriberRepository
    {
        readonly SeekMailContext _context;

        public SubscriberRepository(SeekMailContext context)
        {
            _context = context;
        }

        public IEnumerable<Subscriber> GetAll()
        {
            return _context.Subscribers;
        }

        public Subscriber Get(string emailAddress)
        {
            return _context.Subscribers.SingleOrDefault(x => x.EmailAddress == emailAddress);
        }

        public Subscriber Add(Subscriber subscriber)
        {
            return _context.Subscribers.Add(subscriber);
        }

        public void Delete(Subscriber subscriber)
        {
            _context.Subscribers.Remove(subscriber);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}