using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "describe-addon-configuration")]
public record AwsEksDescribeAddonConfigurationOptions(
[property: CommandSwitch("--addon-name")] string AddonName,
[property: CommandSwitch("--addon-version")] string AddonVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}