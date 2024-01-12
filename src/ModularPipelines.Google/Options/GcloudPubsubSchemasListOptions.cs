using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "schemas", "list")]
public record GcloudPubsubSchemasListOptions : GcloudOptions
{
    [CommandSwitch("--view")]
    public string? View { get; set; }
}