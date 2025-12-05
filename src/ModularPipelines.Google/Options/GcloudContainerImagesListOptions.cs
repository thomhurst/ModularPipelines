using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "images", "list")]
public record GcloudContainerImagesListOptions : GcloudOptions
{
    [CliOption("--repository")]
    public string? Repository { get; set; }
}