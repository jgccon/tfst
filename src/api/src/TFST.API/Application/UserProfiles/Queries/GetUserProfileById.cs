using TheFullStackTeam.Application.UserProfiles.Models;
using TFST.Domain.Repositories.Query;
using MediatR;

namespace TheFullStackTeam.Application.UserProfiles.Queries;

public class GetUserProfileByIdQuery : IRequest<UserProfileModel>
{
    public Guid UserProfileId { get; }

    public GetUserProfileByIdQuery(Guid userProfileId)
    {
        UserProfileId = userProfileId;
    }
}

public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfileModel>
{
    private readonly IUserProfileViewQueryRepository _userProfileRepository;

    public GetUserProfileByIdQueryHandler(IUserProfileViewQueryRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    public async Task<UserProfileModel> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _userProfileRepository.GetByIdAsync(request.UserProfileId);

        if (profile == null)
        {
            throw new Exception($"Profile with ID {request.UserProfileId} not found");
        }

        return UserProfileModel.FromView(profile);
    }
}
