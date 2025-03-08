using TFST.Domain.Base;

namespace TFST.Domain.Services;

public interface IMonikerService
{
    Task<string> GenerateMonikerAsync<T>(string baseText) where T : IdentifiableEntity;
}
