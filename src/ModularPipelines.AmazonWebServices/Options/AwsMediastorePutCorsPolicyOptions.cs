using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediastore", "put-cors-policy")]
public record AwsMediastorePutCorsPolicyOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--cors-policy")] string[] CorsPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}