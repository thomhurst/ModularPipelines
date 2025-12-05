using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "put-resource-policy")]
public record AwsSchemasPutResourcePolicyOptions(
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}