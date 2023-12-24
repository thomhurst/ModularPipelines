using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "test-authorization")]
public record AwsIotTestAuthorizationOptions(
[property: CommandSwitch("--auth-infos")] string[] AuthInfos
) : AwsOptions
{
    [CommandSwitch("--principal")]
    public string? Principal { get; set; }

    [CommandSwitch("--cognito-identity-pool-id")]
    public string? CognitoIdentityPoolId { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--policy-names-to-add")]
    public string[]? PolicyNamesToAdd { get; set; }

    [CommandSwitch("--policy-names-to-skip")]
    public string[]? PolicyNamesToSkip { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}