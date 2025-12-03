using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-dev-endpoint")]
public record AwsGlueUpdateDevEndpointOptions(
[property: CliOption("--endpoint-name")] string EndpointName
) : AwsOptions
{
    [CliOption("--public-key")]
    public string? PublicKey { get; set; }

    [CliOption("--add-public-keys")]
    public string[]? AddPublicKeys { get; set; }

    [CliOption("--delete-public-keys")]
    public string[]? DeletePublicKeys { get; set; }

    [CliOption("--custom-libraries")]
    public string? CustomLibraries { get; set; }

    [CliOption("--delete-arguments")]
    public string[]? DeleteArguments { get; set; }

    [CliOption("--add-arguments")]
    public IEnumerable<KeyValue>? AddArguments { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}