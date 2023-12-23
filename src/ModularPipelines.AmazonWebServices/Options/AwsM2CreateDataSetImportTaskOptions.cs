using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "create-data-set-import-task")]
public record AwsM2CreateDataSetImportTaskOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--import-config")] string ImportConfig
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}