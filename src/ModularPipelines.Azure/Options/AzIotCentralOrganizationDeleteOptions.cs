using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "organization", "delete")]
public record AzIotCentralOrganizationDeleteOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--org-id")] string OrgId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}