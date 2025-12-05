using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "functions", "link")]
public record AzStaticwebappFunctionsLinkOptions(
[property: CliOption("--function-resource-id")] string FunctionResourceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}