using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "create-custom-log-source")]
public record AwsSecuritylakeCreateCustomLogSourceOptions(
[property: CommandSwitch("--source-name")] string SourceName
) : AwsOptions
{
    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--event-classes")]
    public string[]? EventClasses { get; set; }

    [CommandSwitch("--source-version")]
    public string? SourceVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}