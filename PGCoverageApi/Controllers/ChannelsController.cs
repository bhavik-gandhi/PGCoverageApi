using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PGCoverageApi.Models;
using PGCoverageApi.Repository;
using PGCoverageApi.Utilities;

namespace PGCoverageApi.Controllers
{
    [Route("api/[controller]")]
    public class ChannelsController : Controller
    {
        private readonly IChannelRepository _channelRepository;

        public ChannelsController(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        [HttpGet]
        public IEnumerable<Channel> GetAll()
        {
            return _channelRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetChannel")]
        public IActionResult GetById(long id)
        {
            var item = _channelRepository.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Channel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _channelRepository.Add(item);

            return CreatedAtRoute("GetChannel", new { id = item.ChannelId }, item);
        }

        [HttpPatch]
        public IActionResult CreateBulk(int channelCount)
        {

            var channels = CoverageDataSetup.FetchChannels(channelCount);

            var startTime = DateTime.UtcNow;

            foreach (Channel item in channels)
                _channelRepository.Add(item);

            var endTime = DateTime.UtcNow;

            TimeSpan duration = endTime - startTime;

            var msg = string.Format("Channel created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}",
                                    channels.Count, duration.TotalSeconds.ToString(), startTime.ToString(), endTime.ToString());

            var msg1 = channelCount.ToString();
            return Content(msg);
        }
    }
}