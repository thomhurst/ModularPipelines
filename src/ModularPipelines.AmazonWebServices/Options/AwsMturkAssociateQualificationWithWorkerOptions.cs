using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "associate-qualification-with-worker")]
public record AwsMturkAssociateQualificationWithWorkerOptions(
[property: CliOption("--qualification-type-id")] string QualificationTypeId,
[property: CliOption("--worker-id")] string WorkerId
) : AwsOptions
{
    [CliOption("--integer-value")]
    public int? IntegerValue { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}