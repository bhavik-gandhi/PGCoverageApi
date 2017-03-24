using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PGCoverageApi.Models;
using PGCoverageApi.Repository;
using PGCoverageApi.Utilities;
using Serilog;
using Microsoft.Extensions.Logging;

namespace PGCoverageApi.Controllers
{
    [Route("api/[controller]")]
    public class CoverageController : Controller
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IRepRepository _repRepository;
        private readonly ILogger<CoverageController> _log;

        public CoverageController(ILogger<CoverageController> log, IChannelRepository channelRepository, IRegionRepository regionRepository, IBranchRepository branchRepository, IRepRepository repRepository)
        {

            _log = log;
            _channelRepository = channelRepository;
            _regionRepository = regionRepository;
            _branchRepository = branchRepository;
            _repRepository = repRepository;
            
        }

        [HttpPatch]
        public IActionResult CreateBulk(int channelCount = 1, int regionCount = 2, int branchCount = 3, int repCount = 4)
        {

            var channels = CoverageDataSetup.FetchChannels(channelCount);
            var regions = CoverageDataSetup.FetchRegions(regionCount);
            var branches = CoverageDataSetup.FetchBranches(branchCount);
            var reps = CoverageDataSetup.FetchReps(repCount);

            //_log.LogInformation("RepCount - {0}", repCount.ToString());

            var startTime = DateTime.UtcNow;

            //Channel
            var startTimeChannel = DateTime.UtcNow;

            //foreach (Channel item in channels)
            //    _channelRepository.Add(item);
            _channelRepository.AddBulk(channels);
            var endTimeChannel = DateTime.UtcNow;

            //Region
            var startTimeRegion = DateTime.UtcNow;

            //foreach (Region item in regions)
            //    _regionRepository.Add(item);
            _regionRepository.AddBulk(regions);
            var endTimeRegion = DateTime.UtcNow;

            //Branch
            var startTimeBranch= DateTime.UtcNow;

            //foreach (Branch item in branches)
            //    _branchRepository.Add(item);
            _branchRepository.AddBulk(branches);
            var endTimeBranch = DateTime.UtcNow;

            //Rep
            var startTimeRep = DateTime.UtcNow;

            //foreach (Rep item in reps)
            //  _repRepository.Add(item);
            _repRepository.AddBulk(reps);
            var endTimeRep = DateTime.UtcNow;



            var endTime = DateTime.UtcNow;

            TimeSpan durationChannel = endTimeChannel - startTimeChannel;
            TimeSpan durationRegion = endTimeRegion - startTimeRegion;
            TimeSpan durationBranch = endTimeBranch - startTimeBranch;
            TimeSpan durationRep = endTimeRep - startTimeRep;
            TimeSpan durationTotalTime = endTime - startTime;

            var msgChannel = string.Format("Channel created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}",
                                    channels.Count, durationChannel.TotalSeconds.ToString(), startTimeChannel.ToString(), endTimeChannel.ToString());

            var msgRegion = string.Format("Region created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}",
                                    regions.Count, durationRegion.TotalSeconds.ToString(), startTimeRegion.ToString(), endTimeRegion.ToString());


            var msgBranch = string.Format("Branch created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}",
                                    branches.Count, durationBranch.TotalSeconds.ToString(), startTimeBranch.ToString(), endTimeBranch.ToString());


            var msgRep = string.Format("Rep created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}",
                                    reps.Count, durationRep.TotalSeconds.ToString(), startTimeRep.ToString(), endTimeRep.ToString());

            var msgTotalTime = string.Format("Total time taken(secs): {0}", durationTotalTime);


            var msg = msgTotalTime + Environment.NewLine + msgChannel + Environment.NewLine + msgRegion + Environment.NewLine + msgBranch + Environment.NewLine + msgRep + Environment.NewLine;
            var msg1 = channelCount.ToString();
            return Content(msg);
        }
    }
}