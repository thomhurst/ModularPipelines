using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "user", "create")]
public record AzIotCentralUserCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--assignee")] string Assignee,
[property: CliOption("--role")] string Role
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--object-id")]
    public string? ObjectId { get; set; }

    [CliOption("--org-id")]
    public string? OrgId { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}