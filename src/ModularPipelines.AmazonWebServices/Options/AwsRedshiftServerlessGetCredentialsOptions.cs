using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "get-credentials")]
public record AwsRedshiftServerlessGetCredentialsOptions : AwsOptions
{
    [CliOption("--custom-domain-name")]
    public string? CustomDomainName { get; set; }

    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--workgroup-name")]
    public string? WorkgroupName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}