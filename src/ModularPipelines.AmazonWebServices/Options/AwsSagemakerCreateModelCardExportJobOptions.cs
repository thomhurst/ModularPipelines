using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-model-card-export-job")]
public record AwsSagemakerCreateModelCardExportJobOptions(
[property: CliOption("--model-card-name")] string ModelCardName,
[property: CliOption("--model-card-export-job-name")] string ModelCardExportJobName,
[property: CliOption("--output-config")] string OutputConfig
) : AwsOptions
{
    [CliOption("--model-card-version")]
    public int? ModelCardVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}