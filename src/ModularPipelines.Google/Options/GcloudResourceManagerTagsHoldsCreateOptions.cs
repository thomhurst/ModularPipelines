using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "holds", "create")]
public record GcloudResourceManagerTagsHoldsCreateOptions(
[property: CliArgument] string Parent,
[property: CliOption("--holder")] string Holder
) : GcloudOptions
{
    [CliOption("--help-link")]
    public string? HelpLink { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--origin")]
    public string? Origin { get; set; }
}