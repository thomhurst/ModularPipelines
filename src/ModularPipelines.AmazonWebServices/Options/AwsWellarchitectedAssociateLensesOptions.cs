using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "associate-lenses")]
public record AwsWellarchitectedAssociateLensesOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-aliases")] string[] LensAliases
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}