using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "get-reference")]
public record AwsOmicsGetReferenceOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--reference-store-id")] string ReferenceStoreId,
[property: CommandSwitch("--part-number")] int PartNumber
) : AwsOptions
{
    [CommandSwitch("--range")]
    public string? Range { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }
}