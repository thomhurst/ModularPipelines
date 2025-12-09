using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "show")]
public record AzAccountManagementGroupShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--expand")]
    public bool? Expand { get; set; }

    [CliFlag("--no-register")]
    public bool? NoRegister { get; set; }

    [CliFlag("--recurse")]
    public bool? Recurse { get; set; }
}