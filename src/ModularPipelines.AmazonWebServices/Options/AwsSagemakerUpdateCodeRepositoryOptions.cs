using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-code-repository")]
public record AwsSagemakerUpdateCodeRepositoryOptions(
[property: CommandSwitch("--code-repository-name")] string CodeRepositoryName
) : AwsOptions
{
    [CommandSwitch("--git-config")]
    public string? GitConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}