using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("entityresolution", "get-provider-service")]
public record AwsEntityresolutionGetProviderServiceOptions(
[property: CommandSwitch("--provider-name")] string ProviderName,
[property: CommandSwitch("--provider-service-name")] string ProviderServiceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}