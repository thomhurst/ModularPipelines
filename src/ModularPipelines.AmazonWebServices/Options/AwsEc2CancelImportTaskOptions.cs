using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "cancel-import-task")]
public record AwsEc2CancelImportTaskOptions : AwsOptions
{
    [CommandSwitch("--cancel-reason")]
    public string? CancelReason { get; set; }

    [CommandSwitch("--import-task-id")]
    public string? ImportTaskId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}