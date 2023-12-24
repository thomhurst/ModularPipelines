using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "delete-template-share")]
public record AwsWellarchitectedDeleteTemplateShareOptions(
[property: CommandSwitch("--share-id")] string ShareId,
[property: CommandSwitch("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}