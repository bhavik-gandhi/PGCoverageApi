using System;
using Microsoft.AspNetCore.Mvc;
using PGCoverageApi.Repository;
using PGCoverageApi.Utilities;
using Serilog;
using PGCoverageApi.Models;
using System.Linq;

namespace PGCoverageApi.Controllers
{
    [Route("api/[controller]")]
    public class CoverageController : Controller
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IRepRepository _repRepository;
        private ILogger _log;

        public CoverageController(ILogger log, IChannelRepository channelRepository, IRegionRepository regionRepository, IBranchRepository branchRepository, IRepRepository repRepository)
        {
            _log = log;
            _channelRepository = channelRepository;
            _regionRepository = regionRepository;
            _branchRepository = branchRepository;
            _repRepository = repRepository;
        }

        [HttpPatch]
        public IActionResult CreateBulk(int channelCount = 1, int regionCount = 2, int branchCount = 3, int repCount = 4, bool storeDataAsJson = false, bool dataInSingleTable = false)
        {
            string connectionString = "Server=bgpostgresmaster.cachftxgju6f.us-east-1.rds.amazonaws.com;User Id=bg;Password=ipreo1359;Database=Orders;Port=5432;Pooling=true;";
            var channels = CoverageDataSetup.FetchChannels(channelCount, 1);
            _log.Information("channel_maxid is:" + channels.Max<Channel>(i => i.ChannelId));
            var regions = CoverageDataSetup.FetchRegions(regionCount, channels.Max<Channel>(i => i.ChannelId));

            _log.Information("region_maxid is:" + regions.Max<Region>(i => i.RegionId));

            var branches = CoverageDataSetup.FetchBranches(branchCount, regions.Max<Region>(i => i.RegionId));

            _log.Information("branch_maxid is:" + branches.Max<Branch>(i => i.BranchId));
            var reps = CoverageDataSetup.FetchReps(repCount, branches.Max<Branch>(i => i.BranchId));

            
            var startTime = DateTime.UtcNow;

            //Channel
            var startTimeChannel = DateTime.UtcNow;
            _channelRepository.AddBulk(connectionString, channels, storeDataAsJson, dataInSingleTable);
            
            var endTimeChannel = DateTime.UtcNow;

            //Region
            var startTimeRegion = DateTime.UtcNow;
            _regionRepository.AddBulk(connectionString, regions, storeDataAsJson, dataInSingleTable);
            var endTimeRegion = DateTime.UtcNow;
            
            //Branch
            var startTimeBranch= DateTime.UtcNow;
            _branchRepository.AddBulk(connectionString, branches, storeDataAsJson, dataInSingleTable);
            var endTimeBranch = DateTime.UtcNow;
            
            //Rep
            var startTimeRep = DateTime.UtcNow;
            _repRepository.AddBulk(connectionString, reps, storeDataAsJson, dataInSingleTable);
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