using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "get-token")]
public record AwsEksGetTokenOptions : AwsOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--cluster-id")]
    public string? ClusterId { get; set; }
}