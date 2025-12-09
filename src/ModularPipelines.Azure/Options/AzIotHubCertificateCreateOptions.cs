using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "certificate", "create")]
public record AzIotHubCertificateCreateOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--name")] string Name,
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--verified")]
    public bool? Verified { get; set; }
}