using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthlake", "create-fhir-datastore")]
public record AwsHealthlakeCreateFhirDatastoreOptions(
[property: CliOption("--datastore-type-version")] string DatastoreTypeVersion
) : AwsOptions
{
    [CliOption("--datastore-name")]
    public string? DatastoreName { get; set; }

    [CliOption("--sse-configuration")]
    public string? SseConfiguration { get; set; }

    [CliOption("--preload-data-config")]
    public string? PreloadDataConfig { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--identity-provider-configuration")]
    public string? IdentityProviderConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}