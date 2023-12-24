using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rbin", "update-rule")]
public record AwsRbinUpdateRuleOptions(
[property: CommandSwitch("--identifier")] string Identifier
) : AwsOptions
{
    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}