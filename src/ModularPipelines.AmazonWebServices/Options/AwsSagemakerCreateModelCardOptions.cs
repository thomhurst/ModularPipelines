using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-model-card")]
public record AwsSagemakerCreateModelCardOptions(
[property: CommandSwitch("--model-card-name")] string ModelCardName,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--model-card-status")] string ModelCardStatus
) : AwsOptions
{
    [CommandSwitch("--security-config")]
    public string? SecurityConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}