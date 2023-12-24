using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "update-account-settings")]
public record AwsOpensearchserverlessUpdateAccountSettingsOptions : AwsOptions
{
    [CommandSwitch("--capacity-limits")]
    public string? CapacityLimits { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}