using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "create-data-set-import-task")]
public record AwsM2CreateDataSetImportTaskOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--import-config")] string ImportConfig
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}