using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "batch-disassociate-resource")]
public record AwsFmsBatchDisassociateResourceOptions(
[property: CliOption("--resource-set-identifier")] string ResourceSetIdentifier,
[property: CliOption("--items")] string[] Items
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}