using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "describe-configuration-set")]
public record AwsSesDescribeConfigurationSetOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CommandSwitch("--configuration-set-attribute-names")]
    public string[]? ConfigurationSetAttributeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}