using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("schemas", "delete-resource-policy")]
public record AwsSchemasDeleteResourcePolicyOptions : AwsOptions
{
    [CommandSwitch("--registry-name")]
    public string? RegistryName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}