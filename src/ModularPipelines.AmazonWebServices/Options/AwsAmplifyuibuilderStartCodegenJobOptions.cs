using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifyuibuilder", "start-codegen-job")]
public record AwsAmplifyuibuilderStartCodegenJobOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--codegen-job-to-create")] string CodegenJobToCreate
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}