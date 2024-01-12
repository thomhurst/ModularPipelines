using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "images", "list")]
public record GcloudContainerImagesListOptions : GcloudOptions
{
    [CommandSwitch("--repository")]
    public string? Repository { get; set; }
}