using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "describe-global-table-settings")]
public record AwsDynamodbDescribeGlobalTableSettingsOptions(
[property: CliOption("--global-table-name")] string GlobalTableName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}