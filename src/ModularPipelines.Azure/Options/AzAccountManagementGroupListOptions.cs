using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "list")]
public record AzAccountManagementGroupListOptions : AzOptions
{
    [CliFlag("--no-register")]
    public bool? NoRegister { get; set; }
}