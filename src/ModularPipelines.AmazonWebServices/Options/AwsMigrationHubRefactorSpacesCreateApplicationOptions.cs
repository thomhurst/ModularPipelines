using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migration-hub-refactor-spaces", "create-application")]
public record AwsMigrationHubRefactorSpacesCreateApplicationOptions(
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--name")] string Name,
[property: CliOption("--proxy-type")] string ProxyType,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--api-gateway-proxy")]
    public string? ApiGatewayProxy { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}