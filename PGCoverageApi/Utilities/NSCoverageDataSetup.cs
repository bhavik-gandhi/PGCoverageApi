using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;
using Serilog;
using Microsoft.Extensions.Logging;

namespace PGCoverageApi.Utilities
{
    public class NSCoverageDataSetup
    {
        private static readonly int _clientId = 1;
        private static readonly int _companyId = 101;
        private static ICollection<EntityCode> _entityCodes;

        private static string _EntityCodeChannel = "CH";
        private static string _EntityCodeRegion = "RG";
        private static string _EntityCodeBranch = "BR";
        private static string _EntityCodeRep = "R";
        private static string _EntityCodeInvestorGroup = "InvGrp";
        private static string _EntityCodeInvestor = "Inv";

        private static string _entityCodeTypeGroup = "Group";
        private static string _entityCodeTypeInvestor = "Investor";

        private static ICollection<Channel> _channels;
        private static ICollection<Region> _regions;
        private static ICollection<Branch> _branches;
        private static ICollection<Rep> _reps;

        public static ICollection<EntityCode> FetchEntityCode()
        {
            if (_entityCodes == null || !_entityCodes.Any())
            {
                BuildEntityCode();
            }

            return _entityCodes;
        }

        public static ICollection<Channel> FetchChannels(long channelCount = 5, long startId = 1)
        {
            if (_channels == null || !_channels.Any())
            {
                BuildChannels(channelCount, startId);
            }

            return _channels;
        }


        public static ICollection<Region> FetchRegions(long regionCount = 15, long startId = 1)
        {

            if (_regions == null || !_regions.Any())
            {
                BuildRegions(regionCount, startId);
            }
            return _regions;
            
        }

        public static ICollection<Branch> FetchBranches(long branchCount = 250, long startId = 1)
        {
            if (_branches == null || !_branches.Any())
            {
                BuildBranches(branchCount, startId);
            }
            return _branches;
        }

        public static ICollection<Rep> FetchReps(long repCount = 100, long startId = 1)
        {
            //var _log = new LoggerConfiguration().WriteTo.File(@"C:\Temp\abc1.log").CreateLogger();

            //_log.Information("FetchReps - repCount - {0}", repCount.ToString());

            if (_reps == null || !_reps.Any())
            {
                //_log.Information("rep is null");
                BuildReps(repCount, startId);
            }
            return _reps;
        }

        public static void BuildCoverage(long channel = 5, long region = 15, long branch = 200, long rep = 75)
        {
            BuildChannels(channel);
            BuildRegions(region);
            BuildBranches(branch);
            BuildReps(rep);

        }

        private static void BuildEntityCode()
        {
            _entityCodes = new List<EntityCode>();

            var entityCode = new EntityCode();
            entityCode.EntityCodeId = 1;
            entityCode.EntityCd = _EntityCodeChannel;
            entityCode.EntityCodeName = "Channel";
            entityCode.EntityCodeType = _entityCodeTypeGroup;
            entityCode.ActiveInd= true;
            _entityCodes.Add(entityCode);

            entityCode.EntityCodeId = 2;
            entityCode.EntityCd = _EntityCodeRegion;
            entityCode.EntityCodeName = "Region";
            entityCode.EntityCodeType = _entityCodeTypeGroup;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

            entityCode.EntityCodeId = 3;
            entityCode.EntityCd = _EntityCodeBranch;
            entityCode.EntityCodeName = "Branch";
            entityCode.EntityCodeType = _entityCodeTypeGroup;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

            entityCode.EntityCodeId = 4;
            entityCode.EntityCd = _EntityCodeRep;
            entityCode.EntityCodeName = "Rep";
            entityCode.EntityCodeType = _entityCodeTypeGroup;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

            entityCode.EntityCodeId = 5;
            entityCode.EntityCd = _EntityCodeInvestorGroup;
            entityCode.EntityCodeName = "InvestorGroup";
            entityCode.EntityCodeType = _entityCodeTypeInvestor;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

            entityCode.EntityCodeId = 6;
            entityCode.EntityCd = _EntityCodeInvestor;
            entityCode.EntityCodeName = "Investor";
            entityCode.EntityCodeType = _entityCodeTypeInvestor;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

        }

        private static void BuildChannels(long channelCnt = 10, long startId = 1)
        {
            _channels = new List<Channel>();

            for (int cnt = 0 ; cnt < channelCnt; cnt++ )
            {
                var channel = new Channel()
                {
                    ClientId = _clientId,
                    ChannelId = startId + cnt,
                    ChannelCode = _EntityCodeChannel + RandomString(5),
                    ChannelName = RandomString(20),
                    ActiveInd = true,
                    LastModifiedUserId = Convert.ToInt64(RandomNumber(10)),
                    LastModifiedUtcDateTime = DateTime.UtcNow
                };

                _channels.Add(channel);

            }
        }

        private static void BuildRegions(long regionCnt = 100, long startId = 1)
        {
            _regions = new List<Region>();
            var regionId = startId;

            foreach(var channel in _channels)
            {
                for (int cnt = 1; cnt <= regionCnt; cnt++)
                {
                    var region = new Region()
                    {
                        ClientId = _clientId,
                        RegionId = ++regionId,
                        RegionCode = _EntityCodeRegion + RandomString(5),
                        RegionName = RandomString(20),
                        RegionRankIndex = Convert.ToInt64(RandomNumber(5)),
                        ActiveInd = true,
                        LastModifiedUserId = Convert.ToInt64(RandomNumber(10)),
                        LastModifiedUtcDateTime = DateTime.UtcNow,
                        Channel = channel
                    };

                    _regions.Add(region);
                }
            }

        }

        private static void BuildBranches(long branchCnt = 1000, long startId = 1)
        {
            _branches = new List<Branch>();
            var branchId = startId;

            foreach (var region in _regions)
            {
                for (int cnt = 1; cnt <= branchCnt; cnt++)
                {
                    var branch = new Branch()
                    {
                        ClientId = _clientId,
                        BranchId = ++branchId,
                        BranchCode = _EntityCodeBranch + RandomString(5),
                        BranchName = RandomString(20),
                        BranchRankIndex = Convert.ToInt64(RandomNumber(5)),
                        ActiveInd = true,
                        LastModifiedUserId = Convert.ToInt64(RandomNumber(10)),
                        LastModifiedUtcDateTime = DateTime.UtcNow,
                        Region = region
                    };

                    _branches.Add(branch);
                }
            }
        }

        private static void BuildReps(long repCnt = 10000, long startId = 1)
        {
            _reps = new List<Rep>();
            var repId = startId;
            

            foreach (var branch in _branches)
            {
                for (int cnt = 1; cnt <= repCnt; cnt++)
                {
                    var rep = new Rep()
                    {
                        ClientId = _clientId,
                        RepId = ++repId,
                        RepCode = _EntityCodeRep + RandomString(5),
                        RepName = RandomString(20),
                        RepRankIndex = Convert.ToInt64(RandomNumber(5)),
                        ActiveInd = true,
                        LastModifiedUserId = Convert.ToInt64(RandomNumber(10)),
                        LastModifiedUtcDateTime = DateTime.UtcNow,
                        Branch = branch
                    };

                    _reps.Add(rep);
                }
            }
             
        }


       
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static Random randomNumber = new Random();
        private static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[randomNumber.Next(s.Length)]).ToArray());
        }
    }
}
