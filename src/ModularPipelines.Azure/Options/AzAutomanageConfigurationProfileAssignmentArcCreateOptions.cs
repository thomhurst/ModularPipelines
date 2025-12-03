using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "configuration-profile-assignment", "arc", "create")]
public record AzAutomanageConfigurationProfileAssignmentArcCreateOptions(
[property: CliOption("--configuration-profile-assignment-name")] string ConfigurationProfileAssignmentName,
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--configuration-profile")]
    public string? ConfigurationProfile { get; set; }
}