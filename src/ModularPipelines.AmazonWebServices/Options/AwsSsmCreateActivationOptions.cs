using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "create-activation")]
public record AwsSsmCreateActivationOptions(
[property: CommandSwitch("--iam-role")] string IamRole
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--default-instance-name")]
    public string? DefaultInstanceName { get; set; }

    [CommandSwitch("--registration-limit")]
    public int? RegistrationLimit { get; set; }

    [CommandSwitch("--expiration-date")]
    public long? ExpirationDate { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--registration-metadata")]
    public string[]? RegistrationMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}