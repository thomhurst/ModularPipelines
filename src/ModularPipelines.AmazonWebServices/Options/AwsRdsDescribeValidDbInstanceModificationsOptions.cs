using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-valid-db-instance-modifications")]
public record AwsRdsDescribeValidDbInstanceModificationsOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}