using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "start-read-set-activation-job")]
public record AwsOmicsStartReadSetActivationJobOptions(
[property: CommandSwitch("--sequence-store-id")] string SequenceStoreId,
[property: CommandSwitch("--sources")] string[] Sources
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}