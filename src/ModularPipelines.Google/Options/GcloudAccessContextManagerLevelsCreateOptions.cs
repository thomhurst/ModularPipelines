using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "levels", "create")]
public record GcloudAccessContextManagerLevelsCreateOptions(
[property: CliArgument] string Level,
[property: CliArgument] string Policy,
[property: CliOption("--title")] string Title,
[property: CliOption("--custom-level-spec")] string CustomLevelSpec,
[property: CliOption("--basic-level-spec")] string BasicLevelSpec,
[property: CliOption("--combine-function")] string CombineFunction
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }
}