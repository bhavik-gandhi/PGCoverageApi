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
        public IActionResult CreateBulk(long channelCount = 1, long regionCount = 2, long branchCount = 3, long repCount = 4, long investorGroupCount = 5, long investorCount = 5, bool onlyPOMStyleData = true, long batchSize = 10000)
        {
            string connectionString = "Server=bgpostgresmaster.cachftxgju6f.us-east-1.rds.amazonaws.com;User Id=bg;Password=ipreo1359;Database=Orders;Port=5432;Pooling=true;";

            NSCoverageDataSetup.FetchEntityCode();

            var channels = NSCoverageDataSetup.FetchChannels(channelCount, 0);

            _log.Information("channel_maxid is:" + channels.Max<Group>(i => i.GroupId));
            
            var regions = NSCoverageDataSetup.FetchRegions(regionCount, channels.Max<Group>(i => i.GroupId));

            _log.Information("region_maxid is:" + regions.Max<Group>(i => i.GroupId));

            var branches = NSCoverageDataSetup.FetchBranches(branchCount, regions.Max<Group>(i => i.GroupId));

            _log.Information("branch_maxid is:" + branches.Max<Group>(i => i.GroupId));

            var reps = NSCoverageDataSetup.FetchReps(repCount, branches.Max<Group>(i => i.GroupId));

            var investorGroup = NSCoverageDataSetup.FetchInvestorGroup(investorGroupCount, 0);

            var investor = NSCoverageDataSetup.FetchInvestor(investorCount, investorGroup.Max<Investor>(i => i.InvestorId));

            var startTime = DateTime.UtcNow;

            //Channel
            var startTimeChannel = DateTime.UtcNow;
            _groupRepository.AddBulk(connectionString, channels, batchSize);

            var endTimeChannel = DateTime.UtcNow;

            ////Region
            var startTimeRegion = DateTime.UtcNow;
            _groupRepository.AddBulk(connectionString, regions, batchSize);
            var endTimeRegion = DateTime.UtcNow;

            ////Branch
            var startTimeBranch= DateTime.UtcNow;
            _groupRepository.AddBulk(connectionString, branches, batchSize);
            var endTimeBranch = DateTime.UtcNow;

            ////Rep
            var startTimeRep = DateTime.UtcNow;
            _groupRepository.AddBulk(connectionString, reps, batchSize);
            var endTimeRep = DateTime.UtcNow;

            ////InvestorGroup
            var startTimeInvestorGroup = DateTime.UtcNow;
            _investorRepository.AddBulk(connectionString, investorGroup, batchSize);
            var endTimeInvestorGroup = DateTime.UtcNow;


            ////Rep
            var startTimeInvestor = DateTime.UtcNow;
            _investorRepository.AddBulk(connectionString, investor, batchSize);
            var endTimeInvestor = DateTime.UtcNow;


            var endTime = DateTime.UtcNow;

            TimeSpan durationChannel = endTimeChannel - startTimeChannel;
            TimeSpan durationRegion = endTimeRegion - startTimeRegion;
            TimeSpan durationBranch = endTimeBranch - startTimeBranch;
            TimeSpan durationRep = endTimeRep - startTimeRep;
            TimeSpan durationInvestorGroup = endTimeInvestorGroup - startTimeInvestorGroup;
            TimeSpan durationInvestor = endTimeInvestor - startTimeInvestor;
            TimeSpan durationTotalTime = endTime - startTime;

            var msgChannel = string.Format("Channel created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    channels.Count, durationChannel.TotalSeconds.ToString(), startTimeChannel.ToString(), endTimeChannel.ToString(), (channels.Count / durationChannel.TotalSeconds).ToString());

            var msgRegion = string.Format("Region created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    regions.Count, durationRegion.TotalSeconds.ToString(), startTimeRegion.ToString(), endTimeRegion.ToString(), (regions.Count / durationRegion.TotalSeconds).ToString());


            var msgBranch = string.Format("Branch created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    branches.Count, durationBranch.TotalSeconds.ToString(), startTimeBranch.ToString(), endTimeBranch.ToString(), (branches.Count / durationBranch.TotalSeconds).ToString());


            var msgRep = string.Format("Rep created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    reps.Count, durationRep.TotalSeconds.ToString(), startTimeRep.ToString(), endTimeRep.ToString(), (reps.Count / durationRep.TotalSeconds).ToString());

            var msgInvestorGroup = string.Format("InvestorGroup created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    investorGroup.Count, durationInvestorGroup.TotalSeconds.ToString(), startTimeInvestorGroup.ToString(), endTimeInvestorGroup.ToString(), (investorGroup.Count / durationInvestorGroup.TotalSeconds).ToString());

            var msgInvestor = string.Format("Investor created: {0}, Time taken(secs): {1}, Start Time (utc): {2}, End Time(utc): {3}, Record Count per sec:{4}",
                                    investor.Count, durationInvestor.TotalSeconds.ToString(), startTimeInvestor.ToString(), endTimeInvestor.ToString(), (investor.Count / durationInvestor.TotalSeconds).ToString());


            long totalCount = channels.Count + regions.Count + branches.Count + reps.Count;

            var msgTotalTime = string.Format("Total time taken(secs): {0}, total record count: {1}, Record Count per sec:{2}", durationTotalTime.TotalSeconds.ToString(), totalCount.ToString(), (totalCount / durationTotalTime.TotalSeconds).ToString());


            var msg = msgTotalTime + Environment.NewLine + msgChannel + Environment.NewLine + msgRegion + Environment.NewLine + msgBranch + Environment.NewLine + msgRep + Environment.NewLine + msgInvestorGroup + Environment.NewLine + msgInvestor + Environment.NewLine;
            var msg1 = channelCount.ToString();

            return Content(msg);

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