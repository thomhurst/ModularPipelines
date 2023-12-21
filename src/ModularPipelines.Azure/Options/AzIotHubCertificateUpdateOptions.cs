using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "certificate", "update")]
public record AzIotHubCertificateUpdateOptions(
[property: CommandSwitch("--etag")] string Etag,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [BooleanCommandSwitch("--verified")]
    public bool? Verified { get; set; }
}