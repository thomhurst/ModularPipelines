using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthlake", "create-fhir-datastore")]
public record AwsHealthlakeCreateFhirDatastoreOptions(
[property: CommandSwitch("--datastore-type-version")] string DatastoreTypeVersion
) : AwsOptions
{
    [CommandSwitch("--datastore-name")]
    public string? DatastoreName { get; set; }

    [CommandSwitch("--sse-configuration")]
    public string? SseConfiguration { get; set; }

    [CommandSwitch("--preload-data-config")]
    public string? PreloadDataConfig { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--identity-provider-configuration")]
    public string? IdentityProviderConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}