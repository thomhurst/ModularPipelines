using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "update-code-signing-config")]
public record AwsLambdaUpdateCodeSigningConfigOptions(
[property: CommandSwitch("--code-signing-config-arn")] string CodeSigningConfigArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--allowed-publishers")]
    public string? AllowedPublishers { get; set; }

    [CommandSwitch("--code-signing-policies")]
    public string? CodeSigningPolicies { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}