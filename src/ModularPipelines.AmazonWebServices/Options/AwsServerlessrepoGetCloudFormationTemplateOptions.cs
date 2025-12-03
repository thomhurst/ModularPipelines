using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("serverlessrepo", "get-cloud-formation-template")]
public record AwsServerlessrepoGetCloudFormationTemplateOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--template-id")] string TemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}