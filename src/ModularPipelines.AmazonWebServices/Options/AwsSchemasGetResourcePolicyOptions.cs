using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "get-resource-policy")]
public record AwsSchemasGetResourcePolicyOptions : AwsOptions
{
    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}