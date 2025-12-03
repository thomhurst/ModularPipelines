using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "management-group", "create")]
public record AzAccountManagementGroupCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--no-register")]
    public bool? NoRegister { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }
}