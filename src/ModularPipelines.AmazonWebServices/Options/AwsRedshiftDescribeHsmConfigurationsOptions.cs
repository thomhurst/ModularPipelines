using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-hsm-configurations")]
public record AwsRedshiftDescribeHsmConfigurationsOptions : AwsOptions
{
    [CliOption("--hsm-configuration-identifier")]
    public string? HsmConfigurationIdentifier { get; set; }

    [CliOption("--tag-keys")]
    public string[]? TagKeys { get; set; }

    [CliOption("--tag-values")]
    public string[]? TagValues { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}