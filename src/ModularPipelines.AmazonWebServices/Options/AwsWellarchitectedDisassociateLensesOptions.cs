using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "disassociate-lenses")]
public record AwsWellarchitectedDisassociateLensesOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-aliases")] string[] LensAliases
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}