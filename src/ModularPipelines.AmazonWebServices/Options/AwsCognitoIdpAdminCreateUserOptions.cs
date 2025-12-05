using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-create-user")]
public record AwsCognitoIdpAdminCreateUserOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username
) : AwsOptions
{
    [CliOption("--user-attributes")]
    public string[]? UserAttributes { get; set; }

    [CliOption("--validation-data")]
    public string[]? ValidationData { get; set; }

    [CliOption("--temporary-password")]
    public string? TemporaryPassword { get; set; }

    [CliOption("--message-action")]
    public string? MessageAction { get; set; }

    [CliOption("--desired-delivery-mediums")]
    public string[]? DesiredDeliveryMediums { get; set; }

    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}