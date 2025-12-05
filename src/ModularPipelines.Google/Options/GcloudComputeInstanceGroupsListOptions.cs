using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "list")]
public record GcloudComputeInstanceGroupsListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliFlag("--only-managed")]
    public bool? OnlyManaged { get; set; }

    [CliFlag("--only-unmanaged")]
    public bool? OnlyUnmanaged { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }

    [CliOption("--zones")]
    public string[]? Zones { get; set; }
}