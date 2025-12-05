using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migration-hub-refactor-spaces", "put-resource-policy")]
public record AwsMigrationHubRefactorSpacesPutResourcePolicyOptions(
[property: CliOption("--policy")] string Policy,
[property: CliOption("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}