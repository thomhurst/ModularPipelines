using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "reset-service-specific-credential")]
public record AwsIamResetServiceSpecificCredentialOptions(
[property: CommandSwitch("--service-specific-credential-id")] string ServiceSpecificCredentialId
) : AwsOptions
{
    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}