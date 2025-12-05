using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "image-definition", "wait")]
public record AzSigImageDefinitionWaitOptions : AzOptions
{
    [CliFlag("--created")]
    public bool? Created { get; set; }

    [CliOption("--custom")]
    public string? Custom { get; set; }

    [CliFlag("--deleted")]
    public bool? Deleted { get; set; }

    [CliFlag("--exists")]
    public bool? Exists { get; set; }

    [CliOption("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CliOption("--gallery-name")]
    public string? GalleryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--updated")]
    public bool? Updated { get; set; }
}