using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "schemas", "list-revisions")]
public record GcloudPubsubSchemasListRevisionsOptions(
[property: PositionalArgument] string Schema
) : GcloudOptions
{
    [CommandSwitch("--view")]
    public string? View { get; set; }
}