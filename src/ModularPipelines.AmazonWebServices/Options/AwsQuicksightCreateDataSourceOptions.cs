using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-data-source")]
public record AwsQuicksightCreateDataSourceOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--data-source-id")] string DataSourceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--data-source-parameters")]
    public string? DataSourceParameters { get; set; }

    [CliOption("--credentials")]
    public string? Credentials { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--vpc-connection-properties")]
    public string? VpcConnectionProperties { get; set; }

    [CliOption("--ssl-properties")]
    public string? SslProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--folder-arns")]
    public string[]? FolderArns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}