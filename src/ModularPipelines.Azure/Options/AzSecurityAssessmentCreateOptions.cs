using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "assessment", "create")]
public record AzSecurityAssessmentCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--status-code")] string StatusCode
) : AzOptions
{
    [CommandSwitch("--additional-data")]
    public string? AdditionalData { get; set; }

    [CommandSwitch("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }

    [CommandSwitch("--status-cause")]
    public string? StatusCause { get; set; }

    [CommandSwitch("--status-description")]
    public string? StatusDescription { get; set; }
}