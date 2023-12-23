using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "get-authorization-token")]
public record AwsEcrGetAuthorizationTokenOptions : AwsOptions
{
    [CommandSwitch("--registry-ids")]
    public string[]? RegistryIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}