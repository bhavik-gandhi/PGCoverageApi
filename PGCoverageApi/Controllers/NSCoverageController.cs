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
    public class NSCoverageController : Controller
    {
        private readonly IEntityCodeRepository _entityCodeRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupRelationRepository _groupRelationRepository;
        private readonly IInvestorRepository _investorRepository;
        private readonly IInvestorRelationRepository _investorRelationRepository;
        private ILogger _log;

        public NSCoverageController(ILogger log, IEntityCodeRepository entityCodeRepository, IGroupRepository groupRepository, IGroupRelationRepository groupRelationRepository, IInvestorRepository investorRepository, IInvestorRelationRepository investorRelationRepository)
        {
            _log = log;
            _entityCodeRepository = entityCodeRepository;
            _groupRepository = groupRepository;
            _groupRelationRepository = groupRelationRepository;
            _investorRepository = investorRepository;
            _investorRelationRepository = investorRelationRepository;
        }

        [HttpPatch]
        public IActionResult CreateBulk(int channelCount = 1, int regionCount = 2, int branchCount = 3, int repCount = 4, bool onlyPOMStyleData = true, int batchSize = 10000)
        {
            string connectionString = "Server=bgpostgresmaster.cachftxgju6f.us-east-1.rds.amazonaws.com;User Id=bg;Password=ipreo1359;Database=Orders;Port=5432;Pooling=true;";
            var channels = NSCoverageDataSetup.FetchChannels(channelCount, 1);

            _log.Information("channel_maxid is:" + channels.Max<Channel>(i => i.ChannelId));

            var regions = CoverageDataSetup.FetchRegions(regionCount, channels.Max<Channel>(i => i.ChannelId));

            _log.Information("region_maxid is:" + regions.Max<Region>(i => i.RegionId));

            var branches = CoverageDataSetup.FetchBranches(branchCount, regions.Max<Region>(i => i.RegionId));

            _log.Information("branch_maxid is:" + branches.Max<Branch>(i => i.BranchId));
            var reps = CoverageDataSetup.FetchReps(repCount, branches.Max<Branch>(i => i.BranchId));

            
            var startTime = DateTime.UtcNow;

            ////Channel
            //var startTimeChannel = DateTime.UtcNow;
            //_channelRepository.AddBulk(connectionString, channels, storeDataAsJson, dataInSingleTable, batchSize);

            //var endTimeChannel = DateTime.UtcNow;

            ////Region
            //var startTimeRegion = DateTime.UtcNow;
            //_regionRepository.AddBulk(connectionString, regions, storeDataAsJson, dataInSingleTable, batchSize);
            //var endTimeRegion = DateTime.UtcNow;

            ////Branch
            //var startTimeBranch= DateTime.UtcNow;
            //_branchRepository.AddBulk(connectionString, branches, storeDataAsJson, dataInSingleTable, batchSize);
            //var endTimeBranch = DateTime.UtcNow;

            ////Rep
            //var startTimeRep = DateTime.UtcNow;
            //_repRepository.AddBulk(connectionString, reps, storeDataAsJson, dataInSingleTable, batchSize);
            //var endTimeRep = DateTime.UtcNow;

            //var endTime = DateTime.UtcNow;

            //TimeSpan durationChannel = endTimeChannel - startTimeChannel;
            //TimeSpan durationRegion = endTimeRegion - startTimeRegion;
            //TimeSpan durationBranch = endTimeBranch - startTimeBranch;
            //TimeSpan durationRep = endTimeRep - startTimeRep;
            //TimeSpan durationTotalTime = endTime - startTime;

            //var msgChannel = string.Format("Channel created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
            //                        channels.Count, durationChannel.TotalSeconds.ToString(), startTimeChannel.ToString(), endTimeChannel.ToString(), (channels.Count / durationChannel.TotalSeconds).ToString());

            //var msgRegion = string.Format("Region created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
            //                        regions.Count, durationRegion.TotalSeconds.ToString(), startTimeRegion.ToString(), endTimeRegion.ToString(), (regions.Count / durationRegion.TotalSeconds).ToString());


            //var msgBranch = string.Format("Branch created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
            //                        branches.Count, durationBranch.TotalSeconds.ToString(), startTimeBranch.ToString(), endTimeBranch.ToString(), (branches.Count / durationBranch.TotalSeconds).ToString());


            //var msgRep = string.Format("Rep created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
            //                        reps.Count, durationRep.TotalSeconds.ToString(), startTimeRep.ToString(), endTimeRep.ToString(), (reps.Count/ durationRep.TotalSeconds).ToString());

            //long totalCount = channels.Count + regions.Count + branches.Count + reps.Count;

            //var msgTotalTime = string.Format("Total time taken(secs): {0}, total record count: {1}, Record Count per sec:{2}", durationTotalTime.TotalSeconds.ToString(), totalCount.ToString(), (totalCount/ durationTotalTime.TotalSeconds).ToString());


            //var msg = msgTotalTime + Environment.NewLine + msgChannel + Environment.NewLine + msgRegion + Environment.NewLine + msgBranch + Environment.NewLine + msgRep + Environment.NewLine;
            //var msg1 = channelCount.ToString();

            //return Content(msg);
            return Content("done");

        }
        /*
        [HttpGet]
        public IActionResult GetCoverage(int selectionRepCount = 1000, int joinsCount = 1, bool dataInSingleTable = false, bool keepDataAsJson = false)
        {
            string connectionString = "Server=bgpostgresmaster.cachftxgju6f.us-east-1.rds.amazonaws.com;User Id=bg;Password=ipreo1359;Database=Orders;Port=5432;Pooling=true;";

            var startTime = DateTime.UtcNow;

            var repCodes = _repRepository.ListRepCodes(connectionString, selectionRepCount, joinsCount, dataInSingleTable, keepDataAsJson);

            _log.Information("done 1");
            //Rep
            var startTimeRep = DateTime.UtcNow;
            var reps = _repRepository.ListReps(connectionString, repCodes, joinsCount, dataInSingleTable);
            var endTimeRep = DateTime.UtcNow;

            var endTime = DateTime.UtcNow;

            _log.Information("done 2");

            TimeSpan durationRep = endTimeRep - startTimeRep;
            TimeSpan durationTotalTime = endTime - startTime;

            var msgRep = string.Format("Rep fetched: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    reps.Count<Rep>().ToString(), durationRep.TotalSeconds.ToString(), 
                                    startTimeRep.ToString(), endTimeRep.ToString(), 
                                    (reps.Count / durationRep.TotalSeconds).ToString());


            var msgTotalTime = string.Format("Total time taken(secs): {0}, total record count: {1}", durationTotalTime.TotalSeconds.ToString(), repCodes.Count.ToString());
            var msg = msgTotalTime + Environment.NewLine + msgRep + Environment.NewLine;
            

            return Content(msg);
        }
        */
    }
}