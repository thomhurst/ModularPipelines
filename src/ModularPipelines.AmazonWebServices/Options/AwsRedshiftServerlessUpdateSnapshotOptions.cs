using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "update-snapshot")]
public record AwsRedshiftServerlessUpdateSnapshotOptions(
[property: CommandSwitch("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CommandSwitch("--retention-period")]
    public int? RetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}