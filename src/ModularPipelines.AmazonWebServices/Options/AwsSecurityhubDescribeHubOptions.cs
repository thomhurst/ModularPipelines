using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "describe-hub")]
public record AwsSecurityhubDescribeHubOptions : AwsOptions
{
    [CliOption("--hub-arn")]
    public string? HubArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}