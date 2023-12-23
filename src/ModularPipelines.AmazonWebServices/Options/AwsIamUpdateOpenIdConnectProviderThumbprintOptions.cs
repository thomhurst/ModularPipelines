using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-open-id-connect-provider-thumbprint")]
public record AwsIamUpdateOpenIdConnectProviderThumbprintOptions(
[property: CommandSwitch("--open-id-connect-provider-arn")] string OpenIdConnectProviderArn,
[property: CommandSwitch("--thumbprint-list")] string[] ThumbprintList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}