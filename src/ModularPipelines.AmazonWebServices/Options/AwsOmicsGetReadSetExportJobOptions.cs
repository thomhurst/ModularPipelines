using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "get-read-set-export-job")]
public record AwsOmicsGetReadSetExportJobOptions(
[property: CommandSwitch("--sequence-store-id")] string SequenceStoreId,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}