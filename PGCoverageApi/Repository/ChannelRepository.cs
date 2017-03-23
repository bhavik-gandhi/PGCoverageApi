using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.DataContext;
using PGCoverageApi.Models;

namespace PGCoverageApi.Repository
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly CoverageContext _context;

        public ChannelRepository(CoverageContext context)
        {
            _context = context;
        }

        public IEnumerable<Channel> GetAll()
        {
            return _context.ChannelItems.ToList();
        }

        public void Add(Channel item)
        {
            _context.ChannelItems.Add(item);
            _context.SaveChanges();
        }

        public Channel Find(long key)
        {
            return _context.ChannelItems.FirstOrDefault(t => t.ChannelId == key);
        }

        public void Remove(long key)
        {
            var entity = _context.ChannelItems.First(t => t.ChannelId == key);
            _context.ChannelItems.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Channel item)
        {
            _context.ChannelItems.Update(item);
            _context.SaveChanges();
        }
    }
}

