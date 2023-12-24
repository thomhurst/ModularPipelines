using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudcontrol", "create-resource")]
public record AwsCloudcontrolCreateResourceOptions(
[property: CommandSwitch("--type-name")] string TypeName,
[property: CommandSwitch("--desired-state")] string DesiredState
) : AwsOptions
{
    [CommandSwitch("--type-version-id")]
    public string? TypeVersionId { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}