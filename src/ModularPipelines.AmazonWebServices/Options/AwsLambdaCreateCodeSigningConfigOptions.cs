using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "create-code-signing-config")]
public record AwsLambdaCreateCodeSigningConfigOptions(
[property: CliOption("--allowed-publishers")] string AllowedPublishers
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--code-signing-policies")]
    public string? CodeSigningPolicies { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}