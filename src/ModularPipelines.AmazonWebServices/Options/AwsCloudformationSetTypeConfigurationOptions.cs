using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "set-type-configuration")]
public record AwsCloudformationSetTypeConfigurationOptions(
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--type-arn")]
    public string? TypeArn { get; set; }

    [CliOption("--configuration-alias")]
    public string? ConfigurationAlias { get; set; }

    [CliOption("--type-name")]
    public string? TypeName { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}