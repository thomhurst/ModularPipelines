using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "create-activation")]
public record AwsSsmCreateActivationOptions(
[property: CliOption("--iam-role")] string IamRole
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--default-instance-name")]
    public string? DefaultInstanceName { get; set; }

    [CliOption("--registration-limit")]
    public int? RegistrationLimit { get; set; }

    [CliOption("--expiration-date")]
    public long? ExpirationDate { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--registration-metadata")]
    public string[]? RegistrationMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}