using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "update-analysis-template")]
public record AwsCleanroomsUpdateAnalysisTemplateOptions(
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier,
[property: CommandSwitch("--analysis-template-identifier")] string AnalysisTemplateIdentifier
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}