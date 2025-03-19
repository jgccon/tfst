using TFST.SharedKernel.Domain.Entities;

namespace TFST.SharedKernel.Domain.Services;

public interface IMonikerService
{
    Task<string> GenerateMonikerAsync<T>(string baseText) where T : IdentifiableEntity;
}
