using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "update-capability")]
public record AwsB2biUpdateCapabilityOptions(
[property: CliOption("--capability-id")] string CapabilityId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--instructions-documents")]
    public string[]? InstructionsDocuments { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}