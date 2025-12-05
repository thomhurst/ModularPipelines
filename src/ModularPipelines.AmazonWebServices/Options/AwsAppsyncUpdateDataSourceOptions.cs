using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "update-data-source")]
public record AwsAppsyncUpdateDataSourceOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--service-role-arn")]
    public string? ServiceRoleArn { get; set; }

    [CliOption("--dynamodb-config")]
    public string? DynamodbConfig { get; set; }

    [CliOption("--lambda-config")]
    public string? LambdaConfig { get; set; }

    [CliOption("--elasticsearch-config")]
    public string? ElasticsearchConfig { get; set; }

    [CliOption("--open-search-service-config")]
    public string? OpenSearchServiceConfig { get; set; }

    [CliOption("--http-config")]
    public string? HttpConfig { get; set; }

    [CliOption("--relational-database-config")]
    public string? RelationalDatabaseConfig { get; set; }

    [CliOption("--event-bridge-config")]
    public string? EventBridgeConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}