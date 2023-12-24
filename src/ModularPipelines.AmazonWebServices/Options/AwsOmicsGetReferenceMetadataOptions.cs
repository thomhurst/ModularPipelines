using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "get-reference-metadata")]
public record AwsOmicsGetReferenceMetadataOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--reference-store-id")] string ReferenceStoreId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}