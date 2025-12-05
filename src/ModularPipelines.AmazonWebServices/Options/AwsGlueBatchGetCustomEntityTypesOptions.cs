using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "batch-get-custom-entity-types")]
public record AwsGlueBatchGetCustomEntityTypesOptions(
[property: CliOption("--names")] string[] Names
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}