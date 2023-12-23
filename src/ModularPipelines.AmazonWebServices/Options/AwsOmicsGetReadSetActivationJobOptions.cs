using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "get-read-set-activation-job")]
public record AwsOmicsGetReadSetActivationJobOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--sequence-store-id")] string SequenceStoreId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}