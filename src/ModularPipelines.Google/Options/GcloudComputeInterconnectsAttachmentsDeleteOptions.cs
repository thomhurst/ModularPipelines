using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "attachments", "delete")]
public record GcloudComputeInterconnectsAttachmentsDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}