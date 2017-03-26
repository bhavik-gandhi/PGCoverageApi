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
using Microsoft.Extensions.Configuration;

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
            //string connectionString = "User ID = bg; Password = ipreo1359; Host = bgpostgresmaster.cachftxgju6f.us - east - 1.rds.amazonaws.com; Port = 5432; Database = Orders; Pooling = true; ";

            string connectionString = "Server=bgpostgresmaster.cachftxgju6f.us-east-1.rds.amazonaws.com;User Id=bg;Password=ipreo1359;Database=Orders;Port=5432;Pooling=true;";
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
            _channelRepository.AddBulk(connectionString, channels);
            var endTimeChannel = DateTime.UtcNow;

            //Region
            var startTimeRegion = DateTime.UtcNow;

            //foreach (Region item in regions)
            //    _regionRepository.Add(item);
            _regionRepository.AddBulk(connectionString, regions);
            var endTimeRegion = DateTime.UtcNow;

            //Branch
            var startTimeBranch= DateTime.UtcNow;

            //foreach (Branch item in branches)
            //    _branchRepository.Add(item);
            _branchRepository.AddBulk(connectionString, branches);
            var endTimeBranch = DateTime.UtcNow;

            //Rep
            var startTimeRep = DateTime.UtcNow;

            //foreach (Rep item in reps)
            //  _repRepository.Add(item);
            _repRepository.AddBulk(connectionString, reps);
            var endTimeRep = DateTime.UtcNow;



            var endTime = DateTime.UtcNow;

            TimeSpan durationChannel = endTimeChannel - startTimeChannel;
            TimeSpan durationRegion = endTimeRegion - startTimeRegion;
            TimeSpan durationBranch = endTimeBranch - startTimeBranch;
            TimeSpan durationRep = endTimeRep - startTimeRep;
            TimeSpan durationTotalTime = endTime - startTime;

            var msgChannel = string.Format("Channel created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    channels.Count, durationChannel.TotalSeconds.ToString(), startTimeChannel.ToString(), endTimeChannel.ToString(), (channels.Count / durationChannel.TotalSeconds).ToString());

            var msgRegion = string.Format("Region created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    regions.Count, durationRegion.TotalSeconds.ToString(), startTimeRegion.ToString(), endTimeRegion.ToString(), (regions.Count / durationRegion.TotalSeconds).ToString());


            var msgBranch = string.Format("Branch created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    branches.Count, durationBranch.TotalSeconds.ToString(), startTimeBranch.ToString(), endTimeBranch.ToString(), (branches.Count / durationBranch.TotalSeconds).ToString());


            var msgRep = string.Format("Rep created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    reps.Count, durationRep.TotalSeconds.ToString(), startTimeRep.ToString(), endTimeRep.ToString(), (reps.Count/ durationRep.TotalSeconds).ToString());

            long totalCount = channels.Count + regions.Count + branches.Count + reps.Count;

            var msgTotalTime = string.Format("Total time taken(secs): {0}, total record count: {1}, Record Count per sec:{2}", durationTotalTime.TotalSeconds.ToString(), totalCount.ToString(), (totalCount/ durationTotalTime.TotalSeconds).ToString());


            var msg = msgTotalTime + Environment.NewLine + msgChannel + Environment.NewLine + msgRegion + Environment.NewLine + msgBranch + Environment.NewLine + msgRep + Environment.NewLine;
            var msg1 = channelCount.ToString();
            return Content(msg);
        }
    }
}