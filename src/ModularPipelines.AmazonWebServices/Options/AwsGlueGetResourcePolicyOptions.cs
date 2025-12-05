using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-resource-policy")]
public record AwsGlueGetResourcePolicyOptions : AwsOptions
{
    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}