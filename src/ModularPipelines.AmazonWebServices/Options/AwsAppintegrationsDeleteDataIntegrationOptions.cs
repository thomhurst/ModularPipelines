using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appintegrations", "delete-data-integration")]
public record AwsAppintegrationsDeleteDataIntegrationOptions(
[property: CliOption("--data-integration-identifier")] string DataIntegrationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}