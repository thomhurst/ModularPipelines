using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "update-account-configuration")]
public record AwsMedialiveUpdateAccountConfigurationOptions : AwsOptions
{
    [CommandSwitch("--account-configuration")]
    public string? AccountConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}