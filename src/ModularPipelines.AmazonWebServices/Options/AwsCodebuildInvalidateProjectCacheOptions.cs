using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "invalidate-project-cache")]
public record AwsCodebuildInvalidateProjectCacheOptions(
[property: CommandSwitch("--project-name")] string ProjectName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}