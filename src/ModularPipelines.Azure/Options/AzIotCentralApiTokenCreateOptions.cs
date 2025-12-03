using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "api-token", "create")]
public record AzIotCentralApiTokenCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--role")] string Role,
[property: CliOption("--tkid")] string Tkid
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--org-id")]
    public string? OrgId { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}