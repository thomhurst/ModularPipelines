using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "delete-asset-type")]
public record AwsDatazoneDeleteAssetTypeOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}