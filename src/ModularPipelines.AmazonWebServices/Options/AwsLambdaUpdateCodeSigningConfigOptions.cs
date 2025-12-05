using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "update-code-signing-config")]
public record AwsLambdaUpdateCodeSigningConfigOptions(
[property: CliOption("--code-signing-config-arn")] string CodeSigningConfigArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--allowed-publishers")]
    public string? AllowedPublishers { get; set; }

    [CliOption("--code-signing-policies")]
    public string? CodeSigningPolicies { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}