using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "update-distribution-configuration")]
public record AwsImagebuilderUpdateDistributionConfigurationOptions(
[property: CommandSwitch("--distribution-configuration-arn")] string DistributionConfigurationArn,
[property: CommandSwitch("--distributions")] string[] Distributions
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}