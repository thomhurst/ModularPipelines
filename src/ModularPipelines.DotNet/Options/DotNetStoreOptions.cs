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

    [BooleanCommandSwitch("--manifest")]
    public virtual bool? Manifest { get; set; }

    [PositionalArgument(PlaceholderName = "<PATH_TO_MANIFEST_FILE>")]
    public string? PathToManifestFile { get; set; }

    [BooleanCommandSwitch("--framework")]
    public virtual bool? Framework { get; set; }

    [PositionalArgument(PlaceholderName = "<FRAMEWORK_VERSION>")]
    public string? FrameworkVersion { get; set; }

    [BooleanCommandSwitch("--runtime")]
    public virtual bool? Runtime { get; set; }

    [PositionalArgument(PlaceholderName = "<RUNTIME_IDENTIFIER>")]
    public string? RuntimeIdentifier { get; set; }

    [CommandSwitch("--output")]
    public virtual string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--skip-optimization")]
    public virtual bool? SkipOptimization { get; set; }

    [BooleanCommandSwitch("--skip-symbols")]
    public virtual bool? SkipSymbols { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CommandSwitch("--working-dir")]
    public virtual string? StoreWorkingDirectory { get; set; }
}
