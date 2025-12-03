using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "dev", "dev-box", "create")]
public record AzDevcenterDevDevBoxCreateOptions(
[property: CliOption("--dev-box-name")] string DevBoxName,
[property: CliOption("--pool")] string Pool,
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }
}