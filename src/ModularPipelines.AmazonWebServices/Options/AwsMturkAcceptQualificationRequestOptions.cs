using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "accept-qualification-request")]
public record AwsMturkAcceptQualificationRequestOptions(
[property: CliOption("--qualification-request-id")] string QualificationRequestId
) : AwsOptions
{
    [CliOption("--integer-value")]
    public int? IntegerValue { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}