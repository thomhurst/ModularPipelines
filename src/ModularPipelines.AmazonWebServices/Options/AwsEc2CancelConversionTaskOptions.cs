using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "cancel-conversion-task")]
public record AwsEc2CancelConversionTaskOptions(
[property: CliOption("--conversion-task-id")] string ConversionTaskId
) : AwsOptions
{
    [CliOption("--reason-message")]
    public string? ReasonMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}