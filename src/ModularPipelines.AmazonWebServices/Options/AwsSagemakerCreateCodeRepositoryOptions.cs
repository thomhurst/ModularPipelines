using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-code-repository")]
public record AwsSagemakerCreateCodeRepositoryOptions(
[property: CommandSwitch("--code-repository-name")] string CodeRepositoryName,
[property: CommandSwitch("--git-config")] string GitConfig
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}