using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "endpoint", "purge")]
public record AzAfdEndpointPurgeOptions(
[property: CommandSwitch("--content-paths")] string ContentPaths
) : AzOptions
{
    [CommandSwitch("--domains")]
    public string? Domains { get; set; }

    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}