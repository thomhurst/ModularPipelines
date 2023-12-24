using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "untag-open-id-connect-provider")]
public record AwsIamUntagOpenIdConnectProviderOptions(
[property: CommandSwitch("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn,
[property: CommandSwitch("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}