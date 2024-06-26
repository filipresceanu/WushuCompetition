﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Data;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;

namespace WushuCompetition.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public MatchRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task CreateMatch(Participant participantFirst, Participant participantSecond)
        {
            var match = new Match();
            match.CompetitorFirstId = participantFirst.Id;
            match.CompetitorSecondId = participantSecond.Id;
            _dataContext.Matches.Add(match);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MatchDto>> GetNumberOfMatchesNoReferee()
        {
            var matches = await _dataContext.Matches.Where(elem=>elem.Referee==null).ToListAsync();
            var matchesDto = matches.Select(match => _mapper.Map<MatchDto>(match));
            return matchesDto;
        }

        public async Task AddRefereeInMatches(Guid matchId,string refereeId)
        {
            var match = await _dataContext.Matches.FindAsync(matchId);
            match.Referee=refereeId;
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MatchDto>> GetMatchesForRefereeNoWinner(string refereeId)
        {
            var matches = await _dataContext.Matches.Where(elem => elem.Referee == refereeId &&
                                           elem.ParticipantWinnerId == null).ToListAsync();
            var matchesDto = matches.Select(match => _mapper.Map<MatchDto>(match));
            return matchesDto;
        }

        public async Task<IEnumerable<Match>> GetMatchesNoWinner()
        {
            var matches = await _dataContext.Matches.Where(elem => elem.ParticipantWinnerId == null).ToListAsync();
            return matches;
        }

        public async Task<Match> GetMatchWithId(Guid matchId)
        {
            var match = await _dataContext.Matches.FindAsync(matchId);
            return match;
        }

        public async Task SetWinnerInMatch(Guid matchId, Guid winnerId)
        {
            var match = await GetMatchWithId(matchId);
            match.ParticipantWinnerId=winnerId;
            await _dataContext.SaveChangesAsync();
        }
    }
}
