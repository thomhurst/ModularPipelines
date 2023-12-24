using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "delete-infrastructure-configuration")]
public record AwsImagebuilderDeleteInfrastructureConfigurationOptions(
[property: CommandSwitch("--infrastructure-configuration-arn")] string InfrastructureConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}