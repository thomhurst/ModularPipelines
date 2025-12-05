using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "update-application")]
public record AwsDeployUpdateApplicationOptions : AwsOptions
{
    [CliOption("--application-name")]
    public string? ApplicationName { get; set; }

    [CliOption("--new-application-name")]
    public string? NewApplicationName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}