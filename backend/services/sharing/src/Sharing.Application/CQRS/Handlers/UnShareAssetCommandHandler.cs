using MediatR;
using Sharing.Domain.Repositories;

public class UnShareAssetCommandHandler : IRequestHandler<UnShareAssetCommand>
{
    private readonly ISharedAssetRepository _shareRepository;

    public UnShareAssetCommandHandler(ISharedAssetRepository shareRepository)
    {
        _shareRepository = shareRepository;
    }

    public async Task Handle(UnShareAssetCommand request, CancellationToken cancellationToken)
    {
        var sharedAsset = await _shareRepository.GetByIdAsync(request.SharedAssetId);
        if (sharedAsset == null)
            throw new Exception("Shared asset not found");

        if (sharedAsset.OwnerUserId != request.UserId)
            throw new Exception("Forbidden");

        await _shareRepository.DeleteAsync(sharedAsset.Id);
    }
}
