using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Exceptions;
using SothbeysKillerApi.Repository;

namespace SothbeysKillerApi.Services
{
    public interface IAuctionService
    {
        Task<List<AuctionResponse>> GetPastAuctionsAsync();
        Task<List<AuctionResponse>> GetActiveAuctionsAsync();
        Task<List<AuctionResponse>> GetFutureAuctionsAsync();
        Task<Guid> CreateAuctionAsync(AuctionCreateRequest request);
        Task<AuctionResponse> GetAuctionByIdAsync(Guid id);
        Task UpdateAuctionAsync(Guid id, AuctionUpdateRequest request);
        Task DeleteAuctionAsync(Guid id);
    }

    public class AuctionService : IAuctionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuctionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AuctionResponse>> GetPastAuctionsAsync()
        {
            var auctions = await _unitOfWork.AuctionRepository.GetPastAsync();
            return auctions.Select(a => new AuctionResponse(a.Id, a.Title, a.Start, a.Finish))
                          .OrderByDescending(a => a.Start)
                          .ToList();
        }

        public async Task<List<AuctionResponse>> GetActiveAuctionsAsync()
        {
            var auctions = await _unitOfWork.AuctionRepository.GetActiveAsync();
            return auctions.Select(a => new AuctionResponse(a.Id, a.Title, a.Start, a.Finish))
                          .OrderByDescending(a => a.Start)
                          .ToList();
        }

        public async Task<List<AuctionResponse>> GetFutureAuctionsAsync()
        {
            var auctions = await _unitOfWork.AuctionRepository.GetFutureAsync();
            return auctions.Select(a => new AuctionResponse(a.Id, a.Title, a.Start, a.Finish))
                          .OrderByDescending(a => a.Start)
                          .ToList();
        }

        public async Task<Guid> CreateAuctionAsync(AuctionCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length < 3 || request.Title.Length > 255)
            {
                throw new UserValidationException("Title", "Title must be between 3 and 255 characters.");
            }

            if (request.Start < DateTime.Now)
            {
                throw new UserValidationException("Start", "Start date cannot be in the past.");
            }

            if (request.Finish <= request.Start)
            {
                throw new UserValidationException("Finish", "Finish date must be after start date.");
            }

            var auction = new Auction
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Start = request.Start,
                Finish = request.Finish
            };

            await _unitOfWork.AuctionRepository.CreateAsync(auction);
            await _unitOfWork.CommitAsync();

            return auction.Id;
        }

        public async Task<AuctionResponse> GetAuctionByIdAsync(Guid id)
        {
            var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);
            if (auction == null)
            {
                throw new UserNotFoundException("Id", $"Auction with ID {id} not found.");
            }

            return new AuctionResponse(auction.Id, auction.Title, auction.Start, auction.Finish);
        }

        public async Task UpdateAuctionAsync(Guid id, AuctionUpdateRequest request)
        {
            var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);
            if (auction == null)
            {
                throw new UserNotFoundException("Id", $"Auction with ID {id} not found.");
            }

            if (auction.Start <= DateTime.Now)
            {
                throw new UserValidationException("Start", "Cannot update an auction that has already started.");
            }

            if (request.Start < DateTime.Now)
            {
                throw new UserValidationException("Start", "Start date cannot be in the past.");
            }

            if (request.Finish <= request.Start)
            {
                throw new UserValidationException("Finish", "Finish date must be after start date.");
            }

            auction.Start = request.Start;
            auction.Finish = request.Finish;

            await _unitOfWork.AuctionRepository.UpdateAsync(auction);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAuctionAsync(Guid id)
        {
            var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(id);
            if (auction == null)
            {
                throw new UserNotFoundException("Id", $"Auction with ID {id} not found.");
            }

            if (auction.Start <= DateTime.Now)
            {
                throw new UserValidationException("Start", "Cannot delete an auction that has already started.");
            }

            await _unitOfWork.AuctionRepository.DeleteAsync(auction);
            await _unitOfWork.CommitAsync();
        }
    }
}