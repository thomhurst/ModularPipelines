using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "create-code-signing-config")]
public record AwsLambdaCreateCodeSigningConfigOptions(
[property: CommandSwitch("--allowed-publishers")] string AllowedPublishers
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--code-signing-policies")]
    public string? CodeSigningPolicies { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}