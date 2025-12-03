using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "update-account-configuration")]
public record AwsMedialiveUpdateAccountConfigurationOptions : AwsOptions
{
    [CliOption("--account-configuration")]
    public string? AccountConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}