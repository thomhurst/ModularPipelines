using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[ModuleCategory("VersionTag")]
public class NugetVersionGeneratorModule(IOptions<PublishSettings> publishSettings, IOptions<PipelineSettings> pipelineSettings) : Module<string>
{
    private readonly IOptions<PublishSettings> _publishSettings = publishSettings;

    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var settings = _publishSettings.Value;
        if (settings.ShouldPublish && string.IsNullOrWhiteSpace(settings.ExpectedVersion))
        {
            throw new InvalidOperationException("Publishing requires Publish:ExpectedVersion (the expected-version dispatch input).");
        }

        var version = pipelineSettings.Value.PackageVersion;
        if (string.IsNullOrWhiteSpace(version))
        {
            if (pipelineSettings.Value.BuildAlreadyCompleted)
            {
                throw new InvalidOperationException("A prebuilt checkout requires Pipeline:PackageVersion to match its compiled assemblies.");
            }

            var gitVersionInformation = await context.Tools.Git.Versioning.GetVersioningInformationAsync(cancellationToken).ConfigureAwait(false);
            version = settings.IsAlpha
                ? $"{gitVersionInformation.FullSemVer}-alpha{gitVersionInformation.CommitsSinceVersionSourcePadded!}"
                : gitVersionInformation.FullSemVer!;
        }

        if (settings.ShouldPublish && !string.Equals(version, settings.ExpectedVersion, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Computed package version '{version}' does not match expected version '{settings.ExpectedVersion}'. Publication stopped before packing; select the intended release commit and version.");
        }

        context.Summary.KeyValue("Version", "Generated Version Number", version);

        return version;
    }
}
