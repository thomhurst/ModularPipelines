using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("image", "show")]
public record AzImageShowOptions : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--image-name")]
    public string? ImageName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}