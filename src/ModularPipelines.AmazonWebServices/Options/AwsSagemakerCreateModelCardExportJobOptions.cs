using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-model-card-export-job")]
public record AwsSagemakerCreateModelCardExportJobOptions(
[property: CommandSwitch("--model-card-name")] string ModelCardName,
[property: CommandSwitch("--model-card-export-job-name")] string ModelCardExportJobName,
[property: CommandSwitch("--output-config")] string OutputConfig
) : AwsOptions
{
    [CommandSwitch("--model-card-version")]
    public int? ModelCardVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}