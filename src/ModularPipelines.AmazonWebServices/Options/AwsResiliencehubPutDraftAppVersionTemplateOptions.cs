using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "put-draft-app-version-template")]
public record AwsResiliencehubPutDraftAppVersionTemplateOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--app-template-body")] string AppTemplateBody
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}