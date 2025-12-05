using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "delete-accessor")]
public record AwsManagedblockchainDeleteAccessorOptions(
[property: CliOption("--accessor-id")] string AccessorId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}