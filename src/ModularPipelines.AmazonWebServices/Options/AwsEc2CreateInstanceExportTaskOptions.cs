using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-instance-export-task")]
public record AwsEc2CreateInstanceExportTaskOptions(
[property: CommandSwitch("--export-to-s3-task")] string ExportToS3Task,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--target-environment")] string TargetEnvironment
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}