using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "assessment", "create")]
public record AzSecurityAssessmentCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--status-code")] string StatusCode
) : AzOptions
{
    [CliOption("--additional-data")]
    public string? AdditionalData { get; set; }

    [CliOption("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }

    [CliOption("--status-cause")]
    public string? StatusCause { get; set; }

    [CliOption("--status-description")]
    public string? StatusDescription { get; set; }
}