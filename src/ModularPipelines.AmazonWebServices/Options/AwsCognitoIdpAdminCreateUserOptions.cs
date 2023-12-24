using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "admin-create-user")]
public record AwsCognitoIdpAdminCreateUserOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--username")] string Username
) : AwsOptions
{
    [CommandSwitch("--user-attributes")]
    public string[]? UserAttributes { get; set; }

    [CommandSwitch("--validation-data")]
    public string[]? ValidationData { get; set; }

    [CommandSwitch("--temporary-password")]
    public string? TemporaryPassword { get; set; }

    [CommandSwitch("--message-action")]
    public string? MessageAction { get; set; }

    [CommandSwitch("--desired-delivery-mediums")]
    public string[]? DesiredDeliveryMediums { get; set; }

    [CommandSwitch("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}