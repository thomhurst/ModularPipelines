using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "create-security-configuration")]
public record AwsEmrCreateSecurityConfigurationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--security-configuration")] string SecurityConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}