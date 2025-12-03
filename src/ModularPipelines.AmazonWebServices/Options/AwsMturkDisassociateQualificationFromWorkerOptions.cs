using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "disassociate-qualification-from-worker")]
public record AwsMturkDisassociateQualificationFromWorkerOptions(
[property: CliOption("--worker-id")] string WorkerId,
[property: CliOption("--qualification-type-id")] string QualificationTypeId
) : AwsOptions
{
    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}