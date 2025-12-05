using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "delete-resource-policy")]
public record AwsSchemasDeleteResourcePolicyOptions : AwsOptions
{
    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}