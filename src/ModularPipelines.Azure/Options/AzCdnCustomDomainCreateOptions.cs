using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "custom-domain", "create")]
public record AzCdnCustomDomainCreateOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--hostname")] string Hostname,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}