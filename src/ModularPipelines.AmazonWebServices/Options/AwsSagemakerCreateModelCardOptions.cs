using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-model-card")]
public record AwsSagemakerCreateModelCardOptions(
[property: CliOption("--model-card-name")] string ModelCardName,
[property: CliOption("--content")] string Content,
[property: CliOption("--model-card-status")] string ModelCardStatus
) : AwsOptions
{
    [CliOption("--security-config")]
    public string? SecurityConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}