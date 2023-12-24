using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "start-policy-generation")]
public record AwsAccessanalyzerStartPolicyGenerationOptions(
[property: CommandSwitch("--policy-generation-details")] string PolicyGenerationDetails
) : AwsOptions
{
    [CommandSwitch("--cloud-trail-details")]
    public string? CloudTrailDetails { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}