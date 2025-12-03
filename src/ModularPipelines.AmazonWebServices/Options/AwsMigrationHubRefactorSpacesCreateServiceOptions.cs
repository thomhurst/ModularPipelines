using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migration-hub-refactor-spaces", "create-service")]
public record AwsMigrationHubRefactorSpacesCreateServiceOptions(
[property: CliOption("--application-identifier")] string ApplicationIdentifier,
[property: CliOption("--endpoint-type")] string EndpointType,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--lambda-endpoint")]
    public string? LambdaEndpoint { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--url-endpoint")]
    public string? UrlEndpoint { get; set; }

    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}