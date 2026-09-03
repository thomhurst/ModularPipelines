using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Configuration;

internal class RoleDetector(IOptions<DistributedOptions> options)
{
    private readonly DistributedOptions _options = options.Value;

    public DistributedRole DetectRole()
    {
        if (_options.Role != DistributedRole.Auto)
        {
            return _options.Role;
        }

        return _options.InstanceIndex == 0 ? DistributedRole.Master : DistributedRole.Worker;
    }
}
