using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "delete-scheduled-action")]
public record AwsRedshiftServerlessDeleteScheduledActionOptions(
[property: CliOption("--scheduled-action-name")] string ScheduledActionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}