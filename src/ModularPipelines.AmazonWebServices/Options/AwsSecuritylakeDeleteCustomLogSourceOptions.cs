using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "delete-custom-log-source")]
public record AwsSecuritylakeDeleteCustomLogSourceOptions(
[property: CommandSwitch("--source-name")] string SourceName
) : AwsOptions
{
    [CommandSwitch("--source-version")]
    public string? SourceVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}