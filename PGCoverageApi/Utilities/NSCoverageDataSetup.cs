﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCoverageApi.Models;
using Serilog;

namespace PGCoverageApi.Utilities
{
    public class NSCoverageDataSetup
    {
        private static readonly int _clientId = 1;
        private static readonly int _companyId = 101;
        private static ICollection<EntityCode> _entityCodes;

        public static string _EntityCodeChannel = "CH";
        public static string _EntityCodeRegion = "RG";
        public static string _EntityCodeBranch = "BR";
        public static string _EntityCodeRep = "R";
        public static string _EntityCodeInvestorGroup = "InvGrp";
        public static string _EntityCodeInvestor = "Inv";

        public static string _entityCodeTypeGroup = "Group";
        public static string _entityCodeTypeInvestor = "Investor";

        private static ICollection<Group> _channels;
        private static ICollection<Group> _regions;
        private static ICollection<Group> _branches;
        private static ICollection<Group> _reps;
        private static ICollection<GroupRelation> _channelRelation;
        private static ICollection<GroupRelation> _regionsRelation;
        private static ICollection<GroupRelation> _branchesRelation;
        private static ICollection<GroupRelation> _repsRelation;

        private static ICollection<Investor> _investorGroup;
        private static ICollection<Investor> _investor;

        private static ICollection<InvestorRelation> _investorRelation;
        private static ICollection<InvestorRelation> _investorGroupRelation;
        private static ILogger _log = new LoggerConfiguration().WriteTo.File(@"C:\Temp\logs\NSCoverageDataSetup.log").CreateLogger();

        public static ICollection<EntityCode> FetchEntityCode()
        {
            if (_entityCodes == null || !_entityCodes.Any())
            {
                BuildEntityCode();
            }

            return _entityCodes;
        }

        public static ICollection<Group> FetchChannels(long channelCount = 5, long startId = 1, ICollection<Group> parentGroupList = null)
        {
            if (_channels == null || !_channels.Any())
            {
                _channels = BuildGroup(_EntityCodeChannel, channelCount, startId);
            }

            return _channels;
        }


        public static ICollection<Group> FetchRegions(long regionCount = 15, long startId = 1, ICollection<Group> parentGroupList = null)
        {

            if (_regions == null || !_regions.Any())
            {
                _regions = BuildGroup(_EntityCodeRegion, regionCount, startId);
            }
            return _regions;
        }

        public static ICollection<Group> FetchBranches(long branchCount = 250, long startId = 1, ICollection<Group> parentGroupList = null)
        {
            if (_branches == null || !_branches.Any())
            {
                _branches = BuildGroup(_EntityCodeBranch, branchCount, startId);
            }
            return _branches;
        }

        public static ICollection<Group> FetchReps(long repCount = 100, long startId = 1, ICollection<Group> parentGroupList = null)
        {
            if (_reps == null || !_reps.Any())
            {
                //_log.Information("rep is null");
                _reps = BuildGroup(_EntityCodeRep, repCount, startId);
            }
            return _reps;
        }

        public static ICollection<Investor> FetchInvestorGroup(long investorGroupCount = 100, long startId = 1)
        {
            if (_investorGroup == null || !_investorGroup.Any())
            {
                //_log.Information("rep is null");
                _investorGroup = BuildInvestor(_EntityCodeInvestorGroup, investorGroupCount, startId);
            }
            return _investorGroup;
        }

        public static ICollection<Investor> FetchInvestor(long investorCount = 25, long startId = 1)
        {
            if (_investor == null || !_investor.Any())
            {
                //_log.Information("rep is null");
                _investor = BuildInvestor(_EntityCodeInvestor, investorCount, startId);
            }
            return _investor;
        }

        public static ICollection<GroupRelation> FetchChannelRelations(ICollection<Group> parent, ICollection<Group> child, long startId)
        {

            if (_channelRelation == null || !_channelRelation.Any())
            {
                _channelRelation = BuildGroupRelation(parent, child, startId);
            }
            return _channelRelation;
        }
        public static ICollection<GroupRelation> FetchRegionRelations(ICollection<Group> parent, ICollection<Group> child, long startId)
        {

            if (_regionsRelation == null || !_regionsRelation.Any())
            {
                _regionsRelation = BuildGroupRelation(parent, child, startId);
            }
            return _regionsRelation;
        }

        public static ICollection<GroupRelation> FetchBranchRelations(ICollection<Group> parent, ICollection<Group> child, long startId)
        {

            if (_branchesRelation == null || !_branchesRelation.Any())
            {
                _branchesRelation = BuildGroupRelation(parent, child, startId);
            }
            return _branchesRelation;
        }

        public static ICollection<GroupRelation> FetchRepRelations(ICollection<Group> parent, ICollection<Group> child, long startId)
        {

            if (_repsRelation == null || !_repsRelation.Any())
            {
                _repsRelation = BuildGroupRelation(parent, child, startId);
            }
            return _repsRelation;
        }

        public static void UpdateGroupRelationDataForRep(ICollection<GroupRelation> ansectorRelations)
        {
            _log.Information("In UpdateGroupRelationDataForRep");

            if (ansectorRelations != null && _repsRelation != null && _repsRelation.Any())
            {
                _log.Information("In UpdateGroupRelationDataForRep inside if");
                foreach (GroupRelation r in _repsRelation)
                {
                    _log.Information("In UpdateGroupRelationDataForRep - ForGroupID - {0}", r.Group.GroupId);
                    r.GroupRelationData = GetGroupRelationData(_EntityCodeRep, r.Group.GroupId, ansectorRelations, r);
                    _log.Information("In UpdateGroupRelationDataForRep - ForGroupID - {0} Done", r.Group.GroupId);
                }
            }
        }
        public static ICollection<InvestorRelation> FetchInvestorRelations(ICollection<Investor> parent, ICollection<Investor> child, long startId)
        {

            if (_investorRelation == null || !_investorRelation.Any())
            {
                _investorRelation = BuildInvestorRelation(parent, child, startId);
            }
            return _investorRelation;
        }

        public static ICollection<InvestorRelation> FetchInvestorGroupRelations(ICollection<Investor> parent, ICollection<InvestorRelation> invRelation, long startId)
        {

            if (_investorGroupRelation == null || !_investorGroupRelation.Any())
            {
                _investorGroupRelation = BuildInvestorGroupRelation(parent, invRelation, startId);
            }
            return _investorGroupRelation;
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
            
            for (long cnt = 1; cnt <= groupCnt; cnt++)
            {
                //_log.Information("FetchGroup - GroupCount - {0}", cnt.ToString());

                var group = new Group()
                {
                    GroupId = startId + cnt,
                    ClientId = _clientId,
                    CompanyId = _companyId,
                    GroupCode = RandomString(5, false) + entityCode,
                    GroupName = RandomString(20, true),
                    GroupIndex = Convert.ToInt64(RandomNumber(5)),
                    ActiveInd = true,
                    EntityCode = entity
                };
                

                _groups.Add(group);
            }

            return _groups;
        }

        private static ICollection<Investor> BuildInvestor(string entityCode, long groupCnt = 10, long startId = 1)
        {
            var _inv = new List<Investor>();
            var entity = _entityCodes.FirstOrDefault<EntityCode>(e => e.EntityCd == entityCode);

            for (long cnt = 1; cnt <= groupCnt; cnt++)
            {
              //  _log.Information("FetchGroup - GroupCount - {0}", cnt.ToString());

                var investor = new Investor()
                {
                    InvestorId = startId + cnt,
                    ClientId = _clientId,
                    CompanyId = _companyId,
                    InvestorCode = RandomString(5, false) + entityCode,
                    InvestorName = RandomString(20, true),
                    InvestorIndex = Convert.ToInt64(RandomNumber(5)),
                    ActiveInd = true,
                    EntityCode = entity
                };

                _inv.Add(investor);
            }

            return _inv;
        }
  
        private static ICollection<GroupRelation> BuildGroupRelation(ICollection<Group> parentGroupList, ICollection<Group> groupList, long startId)
        {
            var _groupRelation = new List<GroupRelation>();
            
            foreach (Group g in groupList)
            {
                _log.Information("FetchGroupRelation - GroupCount - {0}", g.GroupId.ToString());
                startId++;

                var grpRel = new GroupRelation()
                {
                    GroupRelationId = startId,
                    Group = g,
                    GroupParent = RandomlyFetchParentGroup(parentGroupList),
                };

                _groupRelation.Add(grpRel);
            }

            return _groupRelation;
        }
        //GroupRelationData = GetGroupRelationData(currentEntityCodeType, g.GroupId, ansectors)
        private static ICollection<InvestorRelation> BuildInvestorRelation(ICollection<Investor> parentInvestorList, ICollection<Investor> investorList, long startId)
        {
            var _investorRelation = new List<InvestorRelation>();

            foreach (Investor i in investorList)
            {
              //  _log.Information("FetchInvestorRelation - InvestorCount - {0}", i.InvestorId.ToString());
                startId++;

                var invRel = new InvestorRelation()
                {
                    InvestorRelationId = startId,
                    Investor = i,
                    InvestorParent = RandomlyFetchInvestorGroup(parentInvestorList),
                };

                _investorRelation.Add(invRel);
            }

            return _investorRelation;
        }

        private static ICollection<InvestorRelation> BuildInvestorGroupRelation(ICollection<Investor> invGroups, ICollection<InvestorRelation> invRelation, long startId)
        {
            var _investorRelation = new List<InvestorRelation>();

            foreach (Investor i in invGroups)
            {
                //_log.Information("FetchInvestorRelation - InvestorCount - {0}", i.InvestorId.ToString());
                startId++;

                var invRel = new InvestorRelation()
                {
                    InvestorRelationId = startId,
                    Investor = i,
                    InvestorRelationData = GetInvestorGroupRelationData(i.InvestorId, invRelation),
                };

                _investorRelation.Add(invRel);
            }

            return _investorRelation;
        }

        private static string GetInvestorGroupRelationData(long invGroupId, ICollection<InvestorRelation> invRelation)
        {
            //_log.Information("here -GetInvestorGroupRelationData - {0}", invGroupId);

            //var invList = invRelation.Where<InvestorRelation>(x => x.InvestorParent.InvestorId == invGroupId).Select< InvestorRelation, Investor>(x => x.Investor);
            var invList = invRelation.Where<InvestorRelation>(x => x.InvestorParent.InvestorId == invGroupId).Select<InvestorRelation, long>(x => x.Investor.InvestorId);
            //_log.Information("GetInvestorGroupRelationData - InvestorCount - {0}", invList.LongCount().ToString());

            if (invList == null || !invList.Any())
                return string.Empty;


            StringBuilder sb = new StringBuilder();
            sb.Append("{\"investors\":\"[");

            foreach (long inv in invList)
            {
                sb.Append(inv.ToString());
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]\"}");
            
            return sb.ToString();
        }

        private static string GetGroupRelationData(string currentEntityCodeType, long groupId, ICollection<GroupRelation> ansectorRelations, GroupRelation current)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _log.Information("In GetGroupRelationData - ForGroupID - {0}", groupId);
                string ancestorList = GetAncestorListFortheGroup(ansectorRelations, current);
                _log.Information("In GetGroupRelationData - ForGroupID - {0} Done", groupId);

                string investorGroupList = GetInvestorGroupListFortheGroup();
                string userList = GetUserListFortheGroup();

                if (string.IsNullOrEmpty(ancestorList) && string.IsNullOrEmpty(investorGroupList) && string.IsNullOrEmpty(userList))
                    return string.Empty;

                sb.Append("{");

                if (!string.IsNullOrEmpty(ancestorList))
                {
                    sb.Append("\"ancestors\":\"[");
                    sb.Append(ancestorList);
                    sb.Append("]\",");
                }

                if (!string.IsNullOrEmpty(investorGroupList))
                {
                    sb.Append("\"investorgroups\":\"[");
                    sb.Append(investorGroupList);
                    sb.Append("]\",");
                }

                if (!string.IsNullOrEmpty(userList))
                {
                    sb.Append("\"users\":\"[");
                    sb.Append(userList);
                    sb.Append("]\",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
            }
            catch(Exception ex)
            {
                _log.Information(ex.ToString());
            }
            return sb.ToString();
        }

        private static string GetAncestorListFortheGroup(ICollection<GroupRelation> ansectorsRelations, GroupRelation current)
        {
            StringBuilder list = new StringBuilder();

            var ancestorsIds = FindAllAncestors(ansectorsRelations, current).Select(x => x.Group.GroupId);

            foreach (long l in ancestorsIds)
            {
                list.Append(l.ToString());
                list.Append(",");
            }
            list.Remove(list.Length - 1, 1);

            return list.ToString();
        }

        private static IEnumerable<GroupRelation> FindAllAncestors(ICollection<GroupRelation> ansectorsRelations, GroupRelation current)
        {
            // http://stackoverflow.com/questions/34730349/finding-parents-in-a-tree-hierarchy-for-a-given-child-linq-lambda-expression

            if(ansectorsRelations == null)
                return Enumerable.Empty<GroupRelation>();

            var parent = ansectorsRelations.FirstOrDefault(x => x.Group.GroupId == current.GroupParent.GroupId);

            if (parent == null)
                return Enumerable.Empty<GroupRelation>();

            return new[] { parent }.Concat(FindAllAncestors(ansectorsRelations, parent));
        }

        private static string GetInvestorGroupListFortheGroup()
        {
            string list = string.Empty;

            return list;
        }

        private static string GetUserListFortheGroup()
        {
            string list = string.Empty;

            return list;
        }

        private static Random randomParentGroup = new Random();
        private static Group RandomlyFetchParentGroup(ICollection<Group> parentGroupList)
        {
            if (parentGroupList == null || !parentGroupList.Any())
                return null;

            int parentId = randomParentGroup.Next((int)parentGroupList.Min<Group>(x => x.GroupId), (int)parentGroupList.Max<Group>(x => x.GroupId));

            return parentGroupList.FirstOrDefault<Group>(x => x.GroupId == parentId);
        }

        private static Random randomInvestor = new Random();
        private static Investor RandomlyFetchInvestorGroup(ICollection<Investor> parentInvestorList)
        {
            if (parentInvestorList == null || !parentInvestorList.Any())
                return null;

            int parentId = randomInvestor.Next((int)parentInvestorList.Min<Investor>(x => x.InvestorId), (int)parentInvestorList.Max<Investor>(x => x.InvestorId));

            return parentInvestorList.FirstOrDefault<Investor>(x => x.InvestorId == parentId);
        }

        private static Random randomString = new Random();
        private static string RandomString(int length, bool isSpaceAllowed)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + (isSpaceAllowed ? " " : string.Empty);
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[randomString.Next(s.Length)]).ToArray());
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
