using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "describe-user-stack-associations")]
public record AwsAppstreamDescribeUserStackAssociationsOptions : AwsOptions
{
    [CliOption("--stack-name")]
    public string? StackName { get; set; }

    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}