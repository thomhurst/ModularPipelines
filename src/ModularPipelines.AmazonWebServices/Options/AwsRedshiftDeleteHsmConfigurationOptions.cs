using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-hsm-configuration")]
public record AwsRedshiftDeleteHsmConfigurationOptions(
[property: CommandSwitch("--hsm-configuration-identifier")] string HsmConfigurationIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}