using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "configure")]
public record AzDevopsConfigureOptions : AzOptions
{
    [CliOption("--defaults")]
    public string? Defaults { get; set; }

    [CliFlag("--list")]
    public bool? List { get; set; }

    [CliFlag("--use-git-aliases")]
    public bool? UseGitAliases { get; set; }
}