using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "create-data-source-from-rds")]
public record AwsMachinelearningCreateDataSourceFromRdsOptions(
[property: CliOption("--data-source-id")] string DataSourceId,
[property: CliOption("--rds-data")] string RdsData,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--data-source-name")]
    public string? DataSourceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}