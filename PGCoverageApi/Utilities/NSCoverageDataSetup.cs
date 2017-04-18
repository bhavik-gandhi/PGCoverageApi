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

        private static ICollection<Group> _channels;
        private static ICollection<Group> _regions;
        private static ICollection<Group> _branches;
        private static ICollection<Group> _reps;

        public static ICollection<EntityCode> FetchEntityCode()
        {
            if (_entityCodes == null || !_entityCodes.Any())
            {
                BuildEntityCode();
            }

            return _entityCodes;
        }

        public static ICollection<Group> FetchChannels(long channelCount = 5, long startId = 1)
        {
            if (_channels == null || !_channels.Any())
            {
                _channels = BuildGroup(_EntityCodeChannel, channelCount, startId);
            }

            return _channels;
        }


        public static ICollection<Group> FetchRegions(long regionCount = 15, long startId = 1)
        {

            if (_regions == null || !_regions.Any())
            {
                _regions = BuildGroup(_EntityCodeRegion, regionCount, startId);
            }
            return _regions;
            
        }

        public static ICollection<Group> FetchBranches(long branchCount = 250, long startId = 1)
        {
            if (_branches == null || !_branches.Any())
            {
                _branches = BuildGroup(_EntityCodeBranch, branchCount, startId);
            }
            return _branches;
        }

        public static ICollection<Group> FetchReps(long repCount = 100, long startId = 1)
        {
            //var _log = new LoggerConfiguration().WriteTo.File(@"C:\Temp\abc1.log").CreateLogger();

            //_log.Information("FetchReps - repCount - {0}", repCount.ToString());

            if (_reps == null || !_reps.Any())
            {
                //_log.Information("rep is null");
                _reps = BuildGroup(_EntityCodeRep, repCount, startId);
            }
            return _reps;
        }

        public static void BuildCoverage(long channel = 5, long region = 15, long branch = 200, long rep = 75)
        {
            BuildGroup(_EntityCodeChannel);
            BuildGroup(_EntityCodeRegion);
            BuildGroup(_EntityCodeBranch);
            BuildGroup(_EntityCodeRep);
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

            entityCode = new EntityCode();
            entityCode.EntityCodeId = 2;
            entityCode.EntityCd = _EntityCodeRegion;
            entityCode.EntityCodeName = "Region";
            entityCode.EntityCodeType = _entityCodeTypeGroup;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

            entityCode = new EntityCode();
            entityCode.EntityCodeId = 3;
            entityCode.EntityCd = _EntityCodeBranch;
            entityCode.EntityCodeName = "Branch";
            entityCode.EntityCodeType = _entityCodeTypeGroup;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

            entityCode = new EntityCode();
            entityCode.EntityCodeId = 4;
            entityCode.EntityCd = _EntityCodeRep;
            entityCode.EntityCodeName = "Rep";
            entityCode.EntityCodeType = _entityCodeTypeGroup;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

            entityCode = new EntityCode();
            entityCode.EntityCodeId = 5;
            entityCode.EntityCd = _EntityCodeInvestorGroup;
            entityCode.EntityCodeName = "InvestorGroup";
            entityCode.EntityCodeType = _entityCodeTypeInvestor;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

            entityCode = new EntityCode();
            entityCode.EntityCodeId = 6;
            entityCode.EntityCd = _EntityCodeInvestor;
            entityCode.EntityCodeName = "Investor";
            entityCode.EntityCodeType = _entityCodeTypeInvestor;
            entityCode.ActiveInd = true;
            _entityCodes.Add(entityCode);

        }

        private static ICollection<Group> BuildGroup(string entityCode, long groupCnt = 10, long startId = 1)
        {
            var _groups = new List<Group>();
            var entity = _entityCodes.FirstOrDefault<EntityCode>(e => e.EntityCd == entityCode);
            var _log = new LoggerConfiguration().WriteTo.File(@"C:\Temp\abc1.log").CreateLogger();

            for (long cnt = 1; cnt <= groupCnt; cnt++)
            {
                _log.Information("FetchGroup - GroupCount - {0}", cnt.ToString());

                var group = new Group()
                {
                    GroupId = startId + cnt,
                    ClientId = _clientId,
                    CompanyId = _companyId,
                    GroupCode = entityCode + RandomString(5, false),
                    GroupName = RandomString(20, true),
                    GroupIndex = Convert.ToInt64(RandomNumber(5)),
                    ActiveInd = true,
                    EntityCode = entity
                };

                _groups.Add(group);
            }

            return _groups;
        }
        
        private static void BuildGroupRelation(long channelCnt = 10, long startId = 1)
        {

        }

        private static Random random = new Random();
        private static string RandomString(int length, bool isSpaceAllowed)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + (isSpaceAllowed ? " " : string.Empty);
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
