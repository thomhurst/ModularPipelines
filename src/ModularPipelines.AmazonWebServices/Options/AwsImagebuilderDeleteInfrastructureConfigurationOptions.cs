using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "delete-infrastructure-configuration")]
public record AwsImagebuilderDeleteInfrastructureConfigurationOptions(
[property: CliOption("--infrastructure-configuration-arn")] string InfrastructureConfigurationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}