using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delete-resources-by-external-id")]
public record AwsDeployDeleteResourcesByExternalIdOptions : AwsOptions
{
    [CliOption("--external-id")]
    public string? ExternalId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}