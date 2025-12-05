using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-export-image-tasks")]
public record AwsEc2DescribeExportImageTasksOptions : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--export-image-task-ids")]
    public string[]? ExportImageTaskIds { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}