using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-mitigation-action")]
public record AwsIotCreateMitigationActionOptions(
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--action-params")] string ActionParams
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}