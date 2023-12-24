using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-contact-method")]
public record AwsLightsailCreateContactMethodOptions(
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--contact-endpoint")] string ContactEndpoint
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}