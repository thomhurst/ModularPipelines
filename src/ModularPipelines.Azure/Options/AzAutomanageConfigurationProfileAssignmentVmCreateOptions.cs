using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automanage", "configuration-profile-assignment", "vm", "create")]
public record AzAutomanageConfigurationProfileAssignmentVmCreateOptions(
[property: CliOption("--configuration-profile-assignment-name")] string ConfigurationProfileAssignmentName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--configuration-profile")]
    public string? ConfigurationProfile { get; set; }
}