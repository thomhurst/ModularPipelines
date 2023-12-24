using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-service-specific-credential")]
public record AwsIamUpdateServiceSpecificCredentialOptions(
[property: CommandSwitch("--service-specific-credential-id")] string ServiceSpecificCredentialId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}