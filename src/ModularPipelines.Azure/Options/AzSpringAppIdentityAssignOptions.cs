using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "identity", "assign")]
public record AzSpringAppIdentityAssignOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliFlag("--system-assigned")]
    public bool? SystemAssigned { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }
}