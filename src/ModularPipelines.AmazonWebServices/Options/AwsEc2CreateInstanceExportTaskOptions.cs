using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-instance-export-task")]
public record AwsEc2CreateInstanceExportTaskOptions(
[property: CliOption("--export-to-s3-task")] string ExportToS3Task,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--target-environment")] string TargetEnvironment
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}