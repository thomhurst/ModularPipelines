using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "batch-delete-read-set")]
public record AwsOmicsBatchDeleteReadSetOptions(
[property: CommandSwitch("--ids")] string[] Ids,
[property: CommandSwitch("--sequence-store-id")] string SequenceStoreId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}