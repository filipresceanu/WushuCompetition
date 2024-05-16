using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Data;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Repository.Interfaces;

namespace WushuCompetition.Repository
{
    public class ParticipantRepository:IParticipantRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
    
        public ParticipantRepository(DataContext context,IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = context;
        }

        public async Task AddParticipantsInCompetition( Participant participant)
        {
            _dataContext.Participants.Add(participant);
            await Save();
        }

        public async Task DeleteParticipants(Guid participantId)
        {
            var participant=await GetParticipant(participantId);
            _dataContext.Participants.Remove(participant);
            await Save();
        }

        public   async Task<IEnumerable<Participant>> GetParticipantsShuffling()
        {
            var participants=await _dataContext.Participants.OrderBy(elem=>Guid.NewGuid()).ToListAsync();
            return participants;
        }

        public async Task<Participant> GetParticipant(Guid participantId)
        {
            var participant=await _dataContext.Participants.SingleOrDefaultAsync(elem=>elem.Id==participantId);
            
            return participant;
        }

        public async Task<string> GetParticipantName(Guid participantId)
        {
            var participant = await _dataContext.Participants.SingleOrDefaultAsync(elem => elem.Id == participantId);

            return participant.Name;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsDataForCompetitionId(Guid competitionId)
        {
           
            var participants=await _dataContext.Participants.Where(elem => 
                elem.Category.Competition.Id == competitionId).ToListAsync();

            return participants;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsForCategoryAndCompetition(Guid categoryId, Guid competitionId)
        {
            try { 
                var participants = await _dataContext.Participants.Where(participant=>
                    participant.Category.Id==categoryId &&
                    participant.Category.Competition.Id==competitionId).ToListAsync();

                return  participants;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ParticipantDto>> GetParticipantsForCompetitionId(Guid competitionId)
        {
          
            var participants =  _dataContext.Participants.Where(elem => 
                    elem.Category.Competition.Id == competitionId)
                .ProjectTo<ParticipantDto>(_mapper.ConfigurationProvider).ToListAsync();
                
            return await participants;
        }

        public async Task Save()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<int> GetParticipantNumberForCategoryAndCompetition(Guid categoryId, Guid competitionId)
        {
            try
            {
                var participants = await _dataContext.Participants.Where(participant => 
                    participant.Category.Id == categoryId &&
                    participant.Category.Competition.Id == competitionId).ToListAsync();

                return participants.Count();
            }
            catch (Exception ex)
            {
                return ex.HResult;
            }
        }

        public async Task<ParticipantDto>GetParticipantDto(Guid participantId)
        {
            var participants=await _dataContext.Participants.Where(element=>
                element.Id == participantId)
                .ProjectTo<ParticipantDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();

            return participants;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsWinnersForCategoryAndCompetition(Guid categoryId, Guid competitionId)
        {
            try
            {
                var participants = await _dataContext.Participants.Where(participant =>
                    participant.Category.Id == categoryId &&
                    participant.Category.Competition.Id == competitionId &&
                    participant.CompeteInNextMatch==true).ToListAsync();

                return participants;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task UpdateParticipantCompeteInNextMatch(Guid participantId,bool status)
        {
            var participant = await _dataContext.Participants.FindAsync(participantId);
            participant.CompeteInNextMatch=status;
            await Save();
        }
    }
}
