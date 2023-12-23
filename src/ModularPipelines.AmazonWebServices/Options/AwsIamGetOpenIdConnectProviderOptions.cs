using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "get-open-id-connect-provider")]
public record AwsIamGetOpenIdConnectProviderOptions(
[property: CommandSwitch("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}