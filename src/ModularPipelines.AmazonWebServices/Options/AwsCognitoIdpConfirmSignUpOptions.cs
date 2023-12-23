using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "confirm-sign-up")]
public record AwsCognitoIdpConfirmSignUpOptions(
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--username")] string Username,
[property: CommandSwitch("--confirmation-code")] string ConfirmationCode
) : AwsOptions
{
    [CommandSwitch("--secret-hash")]
    public string? SecretHash { get; set; }

    [CommandSwitch("--analytics-metadata")]
    public string? AnalyticsMetadata { get; set; }

    [CommandSwitch("--user-context-data")]
    public string? UserContextData { get; set; }

    [CommandSwitch("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}