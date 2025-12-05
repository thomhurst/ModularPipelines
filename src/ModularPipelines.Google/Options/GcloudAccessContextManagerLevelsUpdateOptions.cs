using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "levels", "update")]
public record GcloudAccessContextManagerLevelsUpdateOptions(
[property: CliArgument] string Level,
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--custom-level-spec")]
    public string? CustomLevelSpec { get; set; }

    [CliOption("--basic-level-spec")]
    public string? BasicLevelSpec { get; set; }

    [CliOption("--combine-function")]
    public string? CombineFunction { get; set; }
}