using MediatR;

public class UnShareAssetCommand : IRequest
{
    public Guid SharedAssetId { get; }
    public Guid UserId { get; }

    public UnShareAssetCommand(Guid sharedAssetId, Guid userId)
    {
        SharedAssetId = sharedAssetId;
        UserId = userId;
    }
}
