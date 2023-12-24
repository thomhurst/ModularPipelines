using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "create-instance-profile")]
public record AwsIamCreateInstanceProfileOptions(
[property: CommandSwitch("--instance-profile-name")] string InstanceProfileName
) : AwsOptions
{
    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}