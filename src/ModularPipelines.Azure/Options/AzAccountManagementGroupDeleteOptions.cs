using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "delete")]
public record AzAccountManagementGroupDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--no-register")]
    public bool? NoRegister { get; set; }
}