using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "execute-change-set")]
public record AwsCloudformationExecuteChangeSetOptions(
[property: CommandSwitch("--change-set-name")] string ChangeSetName
) : AwsOptions
{
    [CommandSwitch("--stack-name")]
    public string? StackName { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}