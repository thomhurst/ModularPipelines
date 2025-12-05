using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "holds", "list")]
public record GcloudResourceManagerTagsHoldsListOptions(
[property: CliArgument] string Parent
) : GcloudOptions
{
    [CliOption("--holder")]
    public string? Holder { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--origin")]
    public string? Origin { get; set; }
}