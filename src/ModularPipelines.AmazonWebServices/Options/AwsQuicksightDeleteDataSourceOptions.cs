using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-data-source")]
public record AwsQuicksightDeleteDataSourceOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--data-source-id")] string DataSourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}