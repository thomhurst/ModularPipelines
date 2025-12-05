using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migration-hub-refactor-spaces", "create-route")]
public record AwsMigrationHubRefactorSpacesCreateRouteOptions(
[property: CliOption("--application-identifier")] string ApplicationIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--route-type")] string RouteType,
[property: CliOption("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--default-route")]
    public string? DefaultRoute { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--uri-path-route")]
    public string? UriPathRoute { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}