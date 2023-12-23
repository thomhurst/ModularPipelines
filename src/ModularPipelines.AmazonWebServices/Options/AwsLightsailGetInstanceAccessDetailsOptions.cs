using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-instance-access-details")]
public record AwsLightsailGetInstanceAccessDetailsOptions(
[property: CommandSwitch("--instance-name")] string InstanceName
) : AwsOptions
{
    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}