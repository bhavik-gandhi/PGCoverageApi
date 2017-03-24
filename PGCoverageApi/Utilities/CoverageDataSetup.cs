using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PGCoverageApi.Models;
using Serilog;
using Microsoft.Extensions.Logging;

namespace PGCoverageApi.Utilities
{
    public class CoverageDataSetup
    {
        private static readonly int _clientId = 1;
        private static string _channelCode = "CH";
        private static string _regionCode = "RG";
        private static string _branchCode = "BR";
        private static string _repCode = "R";
        private static ICollection<Channel> _channels;
        private static ICollection<Region> _regions;
        private static ICollection<Branch> _branches;
        private static ICollection<Rep> _reps;
      

        public static ICollection<Channel> FetchChannels(int channelCount = 5)
        {
            if (_channels == null || !_channels.Any())
            {
                BuildChannels(channelCount);
            }

            return _channels;
        }

        public static ICollection<Region> FetchRegions(int regionCount = 15)
        {
            
            if (_regions == null || !_regions.Any())
            {
                BuildRegions(regionCount);
            }
            return _regions;
            
        }

        public static ICollection<Branch> FetchBranches(int branchCount = 250)
        {
            if (_branches == null || !_branches.Any())
            {
                BuildBranches(branchCount);
            }
            return _branches;
        }

        public static ICollection<Rep> FetchReps(int repCount = 100)
        {
            //var _log = new LoggerConfiguration().WriteTo.File(@"C:\Temp\abc1.log").CreateLogger();

            //_log.Information("FetchReps - repCount - {0}", repCount.ToString());

            if (_reps == null || !_reps.Any())
            {
                //_log.Information("rep is null");
                BuildReps(repCount);
            }
            return _reps;
        }


        public static void BuildCoverage(int channel = 5, int region = 15, int branch = 250, int rep = 100)
        {
            BuildChannels(channel);
            BuildRegions(region);
            BuildBranches(branch);
            BuildReps(rep);

        }

        private static void BuildChannels(int channelCnt = 10)
        {
            _channels = new List<Channel>();

            for (int cnt = 1 ; cnt <= channelCnt; cnt++ )
            {
                var channel = new Channel()
                {
                    ClientId = _clientId,
                    ChannelId = cnt,
                    ChannelCode = _channelCode + RandomString(5),
                    ChannelName = RandomString(20),
                    ActiveInd = true,
                    LastModifiedUserId = Convert.ToInt64(RandomNumber(10)),
                    LastModifiedUtcDateTime = DateTime.UtcNow
                };

                _channels.Add(channel);

            }
        }

        private static void BuildRegions(int regionCnt = 100)
        {
            _regions = new List<Region>();
            int regionId = 0;

            foreach(var channel in _channels)
            {
                for (int cnt = 1; cnt <= regionCnt; cnt++)
                {
                    var region = new Region()
                    {
                        ClientId = _clientId,
                        RegionId = ++regionId,
                        RegionCode = _regionCode + RandomString(5),
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

        private static void BuildBranches(int branchCnt = 1000)
        {
            _branches = new List<Branch>();
            var branchId = 0;

            foreach (var region in _regions)
            {
                for (int cnt = 1; cnt <= branchCnt; cnt++)
                {
                    var branch = new Branch()
                    {
                        ClientId = _clientId,
                        BranchId = ++branchId,
                        BranchCode = _branchCode + RandomString(5),
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

        private static void BuildReps(int repCnt = 10000)
        {
            var _log = new LoggerConfiguration().WriteTo.File(@"C:\Temp\abc.log").CreateLogger();

            //loggerFactory.AddFile("Logs/myapp-{Date}.txt");

            _reps = new List<Rep>();
            var repId = 0;
            

            foreach (var branch in _branches)
            {
                //_log.Information("Branch - {0}", branch.BranchId.ToString());

                for (int cnt = 1; cnt <= repCnt; cnt++)
                {
                    var rep = new Rep()
                    {
                        ClientId = _clientId,
                        RepId = ++repId,
                        RepCode = _repCode + RandomString(5),
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
