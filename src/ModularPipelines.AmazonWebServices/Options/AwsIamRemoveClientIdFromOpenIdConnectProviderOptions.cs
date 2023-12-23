using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "remove-client-id-from-open-id-connect-provider")]
public record AwsIamRemoveClientIdFromOpenIdConnectProviderOptions(
[property: CommandSwitch("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn,
[property: CommandSwitch("--client-id")] string ClientId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}