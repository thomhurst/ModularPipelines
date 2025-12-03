using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "dev", "dev-box", "list")]
public record AzDevcenterDevDevBoxListOptions : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }
}