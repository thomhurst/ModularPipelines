using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "assessment-metadata", "create")]
public record AzSecurityAssessmentMetadataCreateOptions(
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--severity")] string Severity
) : AzOptions
{
    [CommandSwitch("--remediation-description")]
    public string? RemediationDescription { get; set; }
}