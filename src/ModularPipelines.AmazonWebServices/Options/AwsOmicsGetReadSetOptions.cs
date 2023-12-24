using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "get-read-set")]
public record AwsOmicsGetReadSetOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--sequence-store-id")] string SequenceStoreId,
[property: CommandSwitch("--part-number")] int PartNumber
) : AwsOptions
{
    [CommandSwitch("--file")]
    public string? File { get; set; }
}