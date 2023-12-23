using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicediscovery", "update-service")]
public record AwsServicediscoveryUpdateServiceOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--service")] string Service
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}