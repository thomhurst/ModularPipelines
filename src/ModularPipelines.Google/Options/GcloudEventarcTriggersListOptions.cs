using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "triggers", "list")]
public record GcloudEventarcTriggersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}