using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "organization", "update")]
public record AzIotCentralOrganizationUpdateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--org-id")] string OrgId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--org-name")]
    public string? OrgName { get; set; }

    [CliOption("--parent-id")]
    public string? ParentId { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}