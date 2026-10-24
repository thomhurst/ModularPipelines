using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.DocumentationSnippets;

// docs/docs/distributed/capabilities.md: RequiresCapability Attribute
[RequiresCapability(Capability.Names.Docker)]
public class DockerBuildModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        // Only executes on workers that advertise "docker"
        await context.Tools.Docker.BuildAsync(new("."), cancellationToken: cancellationToken);
        return "built";
    }
}
