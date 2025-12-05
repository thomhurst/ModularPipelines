using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "addresses", "move")]
public record GcloudComputeAddressesMoveOptions(
[property: CliArgument] string Name,
[property: CliOption("--target-project")] string TargetProject
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--new-name")]
    public string? NewName { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}