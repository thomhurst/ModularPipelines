using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "attachments", "delete")]
public record GcloudComputeInterconnectsAttachmentsDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}