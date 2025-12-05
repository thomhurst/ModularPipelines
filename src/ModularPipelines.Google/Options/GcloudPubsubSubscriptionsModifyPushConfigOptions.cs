using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "modify-push-config")]
public record GcloudPubsubSubscriptionsModifyPushConfigOptions(
[property: CliArgument] string Subscription,
[property: CliOption("--push-endpoint")] string PushEndpoint
) : GcloudOptions
{
    [CliOption("--push-auth-service-account")]
    public string? PushAuthServiceAccount { get; set; }

    [CliOption("--push-auth-token-audience")]
    public string? PushAuthTokenAudience { get; set; }

    [CliFlag("--push-no-wrapper")]
    public bool? PushNoWrapper { get; set; }

    [CliFlag("--push-no-wrapper-write-metadata")]
    public bool? PushNoWrapperWriteMetadata { get; set; }
}