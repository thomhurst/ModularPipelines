using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "create-data-source")]
public record AwsAppsyncCreateDataSourceOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--service-role-arn")]
    public string? ServiceRoleArn { get; set; }

    [CommandSwitch("--dynamodb-config")]
    public string? DynamodbConfig { get; set; }

    [CommandSwitch("--lambda-config")]
    public string? LambdaConfig { get; set; }

    [CommandSwitch("--elasticsearch-config")]
    public string? ElasticsearchConfig { get; set; }

    [CommandSwitch("--open-search-service-config")]
    public string? OpenSearchServiceConfig { get; set; }

    [CommandSwitch("--http-config")]
    public string? HttpConfig { get; set; }

    [CommandSwitch("--relational-database-config")]
    public string? RelationalDatabaseConfig { get; set; }

    [CommandSwitch("--event-bridge-config")]
    public string? EventBridgeConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}