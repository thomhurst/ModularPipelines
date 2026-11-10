using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

/// <summary>
/// Resolves the package version and validates publication version requirements before packing.
/// </summary>
[RequiresCapability("ci-master")]
[ModuleCategory("VersionTag")]
public partial class NugetVersionGeneratorModule(IOptions<PublishSettings> publishSettings, IOptions<PipelineSettings> pipelineSettings) : Module<string>
{
    private readonly IOptions<PublishSettings> _publishSettings = publishSettings;

    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var settings = _publishSettings.Value;
        ValidatePrereleaseVersion(settings.PrereleaseVersion);

        if (settings.ShouldPublish && string.IsNullOrWhiteSpace(settings.ExpectedVersion))
        {
            throw new InvalidOperationException("Publishing requires Publish:ExpectedVersion (the expected-version dispatch input).");
        }

        var version = await ResolveVersionAsync(context, settings, cancellationToken).ConfigureAwait(false);
        if (settings.ShouldPublish && !string.Equals(version, settings.ExpectedVersion, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Computed package version '{version}' does not match expected version '{settings.ExpectedVersion}'. Publication stopped before packing; select the intended release commit and version.");
        }

        context.Summary.KeyValue("Version", "Generated Version Number", version);

        return version;
    }

    private static void ValidatePrereleaseVersion(string? prereleaseVersion)
    {
        if (!string.IsNullOrEmpty(prereleaseVersion) && !PrereleaseVersionPattern().IsMatch(prereleaseVersion))
        {
            throw new InvalidOperationException("Publish:PrereleaseVersion (the prerelease-version dispatch input) must be a canonical major.minor.patch-rc.N version.");
        }
    }

    [GeneratedRegex(@"\A(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)-rc\.(0|[1-9][0-9]*)\z", RegexOptions.CultureInvariant)]
    private static partial Regex PrereleaseVersionPattern();

    private async Task<string> ResolveVersionAsync(IModuleContext context, PublishSettings settings, CancellationToken cancellationToken)
    {
        var prereleaseVersion = settings.PrereleaseVersion;
        var version = pipelineSettings.Value.PackageVersion;
        if (!string.IsNullOrWhiteSpace(version) && !string.IsNullOrEmpty(prereleaseVersion)
            && !string.Equals(version, prereleaseVersion, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The prebuilt Pipeline:PackageVersion does not match Publish:PrereleaseVersion.");
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            if (pipelineSettings.Value.BuildAlreadyCompleted)
            {
                throw new InvalidOperationException("A prebuilt checkout requires Pipeline:PackageVersion to match its compiled assemblies.");
            }

            version = prereleaseVersion;
            if (string.IsNullOrEmpty(version))
            {
                var gitVersionInformation = await context.Tools.Git.Versioning.GetVersioningInformationAsync(cancellationToken).ConfigureAwait(false);
                version = settings.IsAlpha
                    ? $"{gitVersionInformation.FullSemVer}-alpha{gitVersionInformation.CommitsSinceVersionSourcePadded!}"
                    : gitVersionInformation.FullSemVer!;
            }
        }

        return version;
    }
}
