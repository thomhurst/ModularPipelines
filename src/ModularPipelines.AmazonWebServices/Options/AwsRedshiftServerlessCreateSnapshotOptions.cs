using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "create-snapshot")]
public record AwsRedshiftServerlessCreateSnapshotOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CliOption("--retention-period")]
    public int? RetentionPeriod { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}