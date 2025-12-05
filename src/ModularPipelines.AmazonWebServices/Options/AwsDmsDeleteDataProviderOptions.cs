using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "delete-data-provider")]
public record AwsDmsDeleteDataProviderOptions(
[property: CliOption("--data-provider-identifier")] string DataProviderIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}