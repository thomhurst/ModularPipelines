using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "put-resource-policy")]
public record AwsGluePutResourcePolicyOptions(
[property: CliOption("--policy-in-json")] string PolicyInJson
) : AwsOptions
{
    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--policy-hash-condition")]
    public string? PolicyHashCondition { get; set; }

    [CliOption("--policy-exists-condition")]
    public string? PolicyExistsCondition { get; set; }

    [CliOption("--enable-hybrid")]
    public string? EnableHybrid { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}