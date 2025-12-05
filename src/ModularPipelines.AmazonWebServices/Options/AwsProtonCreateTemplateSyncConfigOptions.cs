using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "create-template-sync-config")]
public record AwsProtonCreateTemplateSyncConfigOptions(
[property: CliOption("--branch")] string Branch,
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--repository-provider")] string RepositoryProvider,
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-type")] string TemplateType
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}