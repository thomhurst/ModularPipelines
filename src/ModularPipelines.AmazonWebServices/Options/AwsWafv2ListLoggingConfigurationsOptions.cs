using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "list-logging-configurations")]
public record AwsWafv2ListLoggingConfigurationsOptions(
[property: CommandSwitch("--scope")] string Scope
) : AwsOptions
{
    [CommandSwitch("--next-marker")]
    public string? NextMarker { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}