using TFST.SharedKernel.Domain.Entities;

namespace TFST.Modules.ProfessionalProfiles.Domain.Entities;

public class ProfessionalProfile : IdentifiableEntity
{
    public Guid UserId { get; set; }

    public string Profession { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
