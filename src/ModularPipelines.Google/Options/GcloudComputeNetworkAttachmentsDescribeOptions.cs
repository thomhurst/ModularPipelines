using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-attachments", "describe")]
public record GcloudComputeNetworkAttachmentsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}