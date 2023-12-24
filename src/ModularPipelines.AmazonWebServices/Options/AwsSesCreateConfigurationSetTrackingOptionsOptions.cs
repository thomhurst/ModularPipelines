using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "create-configuration-set-tracking-options")]
public record AwsSesCreateConfigurationSetTrackingOptionsOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName,
[property: CommandSwitch("--tracking-options")] string TrackingOptions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}