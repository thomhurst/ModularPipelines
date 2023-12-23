using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "update-scaling-parameters")]
public record AwsCloudsearchUpdateScalingParametersOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--scaling-parameters")] string ScalingParameters
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}