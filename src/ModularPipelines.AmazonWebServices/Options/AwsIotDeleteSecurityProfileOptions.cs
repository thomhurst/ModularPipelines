using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "delete-security-profile")]
public record AwsIotDeleteSecurityProfileOptions(
[property: CommandSwitch("--security-profile-name")] string SecurityProfileName
) : AwsOptions
{
    [CommandSwitch("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}