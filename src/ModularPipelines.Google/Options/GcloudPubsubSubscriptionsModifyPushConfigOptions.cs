using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "modify-push-config")]
public record GcloudPubsubSubscriptionsModifyPushConfigOptions(
[property: PositionalArgument] string Subscription,
[property: CommandSwitch("--push-endpoint")] string PushEndpoint
) : GcloudOptions
{
    [CommandSwitch("--push-auth-service-account")]
    public string? PushAuthServiceAccount { get; set; }

    [CommandSwitch("--push-auth-token-audience")]
    public string? PushAuthTokenAudience { get; set; }

    [BooleanCommandSwitch("--push-no-wrapper")]
    public bool? PushNoWrapper { get; set; }

    [BooleanCommandSwitch("--push-no-wrapper-write-metadata")]
    public bool? PushNoWrapperWriteMetadata { get; set; }
}