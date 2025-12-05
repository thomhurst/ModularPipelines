using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-data-source")]
public record AwsQuicksightUpdateDataSourceOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--data-source-id")] string DataSourceId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--data-source-parameters")]
    public string? DataSourceParameters { get; set; }

    [CliOption("--credentials")]
    public string? Credentials { get; set; }

    [CliOption("--vpc-connection-properties")]
    public string? VpcConnectionProperties { get; set; }

    [CliOption("--ssl-properties")]
    public string? SslProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}