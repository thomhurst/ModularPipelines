using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Configuration;

internal class RoleDetector(IOptions<DistributedOptions> options)
{
    private readonly DistributedOptions _options = options.Value;

    public DistributedRole DetectRole()
    {
        // Check environment variable override first
        var envInstance = System.Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");
        if (envInstance is not null && int.TryParse(envInstance, out var envIndex))
        {
            return envIndex == 0 ? DistributedRole.Master : DistributedRole.Worker;
        }

        return _options.InstanceIndex == 0 ? DistributedRole.Master : DistributedRole.Worker;
    }
}
