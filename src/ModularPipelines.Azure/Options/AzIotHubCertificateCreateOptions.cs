using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "certificate", "create")]
public record AzIotHubCertificateCreateOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--verified")]
    public bool? Verified { get; set; }
}