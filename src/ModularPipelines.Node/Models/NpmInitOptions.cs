using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("init", "(same", "as", "`npx")]
public record NpmInitOptions
(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Value
) : NpmOptions
{
    [CliOption("--init-author-name")]
    public virtual string? InitAuthorName { get; set; }

    [CliOption("--init-author-url")]
    public virtual string? InitAuthorUrl { get; set; }

    [CliOption("--init-license")]
    public virtual string? InitLicense { get; set; }

    [CliOption("--init-module")]
    public virtual string? InitModule { get; set; }

    [CliOption("--init-version")]
    public virtual string? InitVersion { get; set; }

    [CliFlag("--yes")]
    public virtual bool? Yes { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--workspaces-update")]
    public virtual bool? WorkspacesUpdate { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }
}