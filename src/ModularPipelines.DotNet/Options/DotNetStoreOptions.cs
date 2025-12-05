using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetStoreOptions : DotNetOptions
{
    public DotNetStoreOptions(
        string pathToManifestFile,
        string frameworkVersion,
        string runtimeIdentifier
    )
    {
        CommandParts = ["store"];

        PathToManifestFile = pathToManifestFile;

        FrameworkVersion = frameworkVersion;

        RuntimeIdentifier = runtimeIdentifier;
    }

    [CliFlag("--manifest")]
    public virtual bool? Manifest { get; set; }

    [CliArgument(Name = "<PATH_TO_MANIFEST_FILE>")]
    public virtual string? PathToManifestFile { get; set; }

    [CliFlag("--framework")]
    public virtual bool? Framework { get; set; }

    [CliArgument(Name = "<FRAMEWORK_VERSION>")]
    public virtual string? FrameworkVersion { get; set; }

    [CliFlag("--runtime")]
    public virtual bool? Runtime { get; set; }

    [CliArgument(Name = "<RUNTIME_IDENTIFIER>")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CliOption("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CliFlag("--skip-optimization")]
    public virtual bool? SkipOptimization { get; set; }

    [CliFlag("--skip-symbols")]
    public virtual bool? SkipSymbols { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CliOption("--working-dir")]
    public virtual string? StoreWorkingDirectory { get; set; }
}
