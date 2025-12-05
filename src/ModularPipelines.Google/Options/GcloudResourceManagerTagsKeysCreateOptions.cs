using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "keys", "create")]
public record GcloudResourceManagerTagsKeysCreateOptions(
[property: CliArgument] string ShortName,
[property: CliArgument] string Parent
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--purpose")]
    public string? Purpose { get; set; }

    [CliOption("--purpose-data")]
    public string[]? PurposeData { get; set; }
}