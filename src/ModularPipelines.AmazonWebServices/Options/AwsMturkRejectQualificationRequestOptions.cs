using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "reject-qualification-request")]
public record AwsMturkRejectQualificationRequestOptions(
[property: CliOption("--qualification-request-id")] string QualificationRequestId
) : AwsOptions
{
    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}