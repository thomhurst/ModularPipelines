using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "list")]
public record GcloudPubsubSchemasListOptions : GcloudOptions
{
    [CliOption("--view")]
    public string? View { get; set; }
}