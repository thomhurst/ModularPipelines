using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-topics", "list")]
public record GcloudPubsubLiteTopicsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}