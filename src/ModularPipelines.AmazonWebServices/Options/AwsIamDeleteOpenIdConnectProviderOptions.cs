using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "delete-open-id-connect-provider")]
public record AwsIamDeleteOpenIdConnectProviderOptions(
[property: CommandSwitch("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}