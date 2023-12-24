using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "set-security-token-service-preferences")]
public record AwsIamSetSecurityTokenServicePreferencesOptions(
[property: CommandSwitch("--global-endpoint-token-version")] string GlobalEndpointTokenVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}