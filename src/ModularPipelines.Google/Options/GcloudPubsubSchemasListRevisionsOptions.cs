using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "list-revisions")]
public record GcloudPubsubSchemasListRevisionsOptions(
[property: CliArgument] string Schema
) : GcloudOptions
{
    [CliOption("--view")]
    public string? View { get; set; }
}