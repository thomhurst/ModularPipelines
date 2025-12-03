using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetMigrateOptions : DotNetOptions
{
    public DotNetMigrateOptions(
        string solutionFileProjectDir
    )
    {
        CommandParts = ["migrate", "[<SOLUTION_FILE|PROJECT_DIR>]"];

        SolutionFileProjectDir = solutionFileProjectDir;
    }

    public DotNetMigrateOptions()
    {
        CommandParts = ["migrate", "[<SOLUTION_FILE|PROJECT_DIR>]"];
    }

    [CliArgument(Name = "[<SOLUTION_FILE|PROJECT_DIR>]")]
    public string? SolutionFileProjectDir { get; set; }

    [CliOption("--format-report-file-json")]
    public virtual string? FormatReportFileJson { get; set; }

    [CliOption("--report-file")]
    public virtual string? ReportFile { get; set; }

    [CliFlag("--skip-project-references")]
    public virtual bool? SkipProjectReferences { get; set; }

    [CliFlag("--skip-backup")]
    public virtual bool? SkipBackup { get; set; }

    [CliOption("--template-file")]
    public virtual string? TemplateFile { get; set; }

    [CliFlag("--sdk-package-version")]
    public virtual bool? SdkPackageVersion { get; set; }

    [CliFlag("--xproj-file")]
    public virtual bool? XprojFile { get; set; }
}
