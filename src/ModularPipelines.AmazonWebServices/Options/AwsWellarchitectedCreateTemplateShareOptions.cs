using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "create-template-share")]
public record AwsWellarchitectedCreateTemplateShareOptions(
[property: CommandSwitch("--template-arn")] string TemplateArn,
[property: CommandSwitch("--shared-with")] string SharedWith
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}