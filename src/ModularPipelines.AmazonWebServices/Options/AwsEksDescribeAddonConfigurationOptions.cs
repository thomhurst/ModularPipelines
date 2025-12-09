using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "describe-addon-configuration")]
public record AwsEksDescribeAddonConfigurationOptions(
[property: CliOption("--addon-name")] string AddonName,
[property: CliOption("--addon-version")] string AddonVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}