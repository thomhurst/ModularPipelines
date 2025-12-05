using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "start-read-set-activation-job")]
public record AwsOmicsStartReadSetActivationJobOptions(
[property: CliOption("--sequence-store-id")] string SequenceStoreId,
[property: CliOption("--sources")] string[] Sources
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}