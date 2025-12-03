using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "stop-stack-set-operation")]
public record AwsCloudformationStopStackSetOperationOptions(
[property: CliOption("--stack-set-name")] string StackSetName,
[property: CliOption("--operation-id")] string OperationId
) : AwsOptions
{
    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}