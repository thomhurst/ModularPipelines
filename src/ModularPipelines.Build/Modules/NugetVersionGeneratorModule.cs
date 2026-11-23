using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

/// <summary>
/// Resolves the package version and validates publication version requirements before packing.
/// A published GitHub release supplies its tag's version; every other run uses GitVersion.
/// </summary>
[RequiresCapability("ci-master")]
public partial class NugetVersionGeneratorModule(IOptions<PublishSettings> publishSettings, IOptions<PipelineSettings> pipelineSettings) : Module<string>
{
    // Each version number feeds AssemblyVersion, whose parts cannot exceed 65534.
    private const int MaximumVersionNumber = 65534;

    private readonly IOptions<PublishSettings> _publishSettings = publishSettings;

    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var settings = _publishSettings.Value;
        ValidateReleaseVersion(settings.ReleaseVersion);

        if (settings.ShouldPublish && string.IsNullOrEmpty(settings.ReleaseVersion))
        {
            throw new InvalidOperationException("Publishing requires Publish:ReleaseVersion, the version of the published release's tag.");
        }

        var version = await ResolveVersionAsync(context, settings, cancellationToken).ConfigureAwait(false);

        context.Summary.KeyValue("Version", "Generated Version Number", version);

        return version;
    }

    private static void ValidateReleaseVersion(string? releaseVersion)
    {
        if (string.IsNullOrEmpty(releaseVersion))
        {
            return;
        }

        // SemVer 2.0 without build metadata; each version number must fit AssemblyVersion.
        // The release workflow's "Verify release" step applies the same grammar to the tag.
        var match = ReleaseVersionPattern().Match(releaseVersion);
        if (!match.Success || !Enumerable.Range(1, 3).All(group => match.Groups[group].Value.Length <= 5
            && int.Parse(match.Groups[group].Value, System.Globalization.CultureInfo.InvariantCulture) <= MaximumVersionNumber))
        {
            throw new InvalidOperationException("Publish:ReleaseVersion must be a major.minor.patch version with an optional prerelease suffix, such as 4.1.0 or 4.1.0-rc.1.");
        }
    }

    [GeneratedRegex(@"\A(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(?:-(?:0|[1-9][0-9]*|[0-9]*[A-Za-z-][0-9A-Za-z-]*)(?:\.(?:0|[1-9][0-9]*|[0-9]*[A-Za-z-][0-9A-Za-z-]*))*)?\z", RegexOptions.CultureInvariant)]
    private static partial Regex ReleaseVersionPattern();

    private async Task<string> ResolveVersionAsync(IModuleContext context, PublishSettings settings, CancellationToken cancellationToken)
    {
        var releaseVersion = settings.ReleaseVersion;
        var version = pipelineSettings.Value.PackageVersion;
        if (!string.IsNullOrWhiteSpace(version) && !string.IsNullOrEmpty(releaseVersion)
            && !string.Equals(version, releaseVersion, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The prebuilt Pipeline:PackageVersion does not match Publish:ReleaseVersion.");
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            if (pipelineSettings.Value.BuildAlreadyCompleted)
            {
                throw new InvalidOperationException("A prebuilt checkout requires Pipeline:PackageVersion to match its compiled assemblies.");
            }

            version = releaseVersion;
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
