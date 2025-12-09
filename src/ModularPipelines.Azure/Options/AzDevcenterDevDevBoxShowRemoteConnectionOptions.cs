using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "dev", "dev-box", "show-remote-connection")]
public record AzDevcenterDevDevBoxShowRemoteConnectionOptions(
[property: CliOption("--dev-box-name")] string DevBoxName,
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }
}