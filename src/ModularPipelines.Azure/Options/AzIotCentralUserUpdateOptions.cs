using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "user", "update")]
public record AzIotCentralUserUpdateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--assignee")] string Assignee
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--object-id")]
    public string? ObjectId { get; set; }

    [CliOption("--roles")]
    public string? Roles { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}