using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("xray", "get-sampling-targets")]
public record AwsXrayGetSamplingTargetsOptions(
[property: CommandSwitch("--sampling-statistics-documents")] string[] SamplingStatisticsDocuments
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}