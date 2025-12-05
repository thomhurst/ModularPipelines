using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "cancel-import-task")]
public record AwsEc2CancelImportTaskOptions : AwsOptions
{
    [CliOption("--cancel-reason")]
    public string? CancelReason { get; set; }

    [CliOption("--import-task-id")]
    public string? ImportTaskId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}