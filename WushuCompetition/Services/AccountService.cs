using Microsoft.AspNetCore.Identity;
using WushuCompetition.Dto;
using WushuCompetition.Repository.Interfaces;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMatchRepository _matchRepository;

        public AccountService(IAccountRepository accountRepository, IMatchRepository matchRepository)
        {
            _accountRepository = accountRepository;
            _matchRepository = matchRepository;
        }

        public async Task DistributeReferees()
        {
            var referees = await _accountRepository.GetReferees();
            referees = RandomReferee(referees);
            var matchesNumber = await _matchRepository.GetNumberOfMatchesNoReferee();

            if (referees.Count() > matchesNumber.Count())
            {
                await AddRefereesInMatches(referees, matchesNumber);
            }

            if (referees.Count() == matchesNumber.Count())
            {
                await AddRefereesInMatches(referees, matchesNumber);
            }

            if (referees.Count() < matchesNumber.Count())
            {
                await AddRefereesInMatchesNoRefereesGraterNoMatches(referees, matchesNumber);
            }
        }

        private IEnumerable<IdentityUser> RandomReferee(IEnumerable<IdentityUser> referees)
        {
            var refereesRandom = referees.OrderBy(elem => Guid.NewGuid()).ToList();
            return refereesRandom;
        }

        private async Task AddRefereesInMatches(IEnumerable<IdentityUser> referees, IEnumerable<MatchDto> matches)
        {
            for (int elem = 0; elem < matches.Count(); elem++)
            {
                await _matchRepository.AddRefereeInMatches(matches.ElementAt(elem).Id,
                    referees.ElementAt(elem).Id);
            }
        }

        private async Task AddRefereesInMatchesNoRefereesGraterNoMatches(IEnumerable<IdentityUser> referees, IEnumerable<MatchDto> matches)
        {
            int refereeIndex = referees.Count()-1;
            int matchesIndex = matches.Count()-1;

            while (matchesIndex != -1)
            {
                   
                await _matchRepository.AddRefereeInMatches(matches.ElementAt(matchesIndex).Id,
                    referees.ElementAt(refereeIndex).Id);
                refereeIndex--;
                matchesIndex--;
                if (refereeIndex < 0)
                {
                    refereeIndex = referees.Count() - 1;
                }

            }
            
        }

    }
}
