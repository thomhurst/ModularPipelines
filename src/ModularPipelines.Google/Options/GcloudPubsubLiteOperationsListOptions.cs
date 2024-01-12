using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-operations", "list")]
public record GcloudPubsubLiteOperationsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--done")]
    public string? Done { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}