using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "put-draft-app-version-template")]
public record AwsResiliencehubPutDraftAppVersionTemplateOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--app-template-body")] string AppTemplateBody
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}