using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "delete-web-experience")]
public record AwsQbusinessDeleteWebExperienceOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--web-experience-id")] string WebExperienceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}