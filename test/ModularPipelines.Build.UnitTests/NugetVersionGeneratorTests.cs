using Microsoft.Extensions.Options;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Settings;
using ModularPipelines.Git;
using ModularPipelines.Git.Models;
using Moq;

namespace ModularPipelines.Build.UnitTests;

public class NugetVersionGeneratorTests
{
    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public async Task Publishing_Requires_An_Expected_Version(string? expectedVersion)
    {
        var (module, context) = CreateModule(true, false, expectedVersion, "4.0.0");

        await Assert.That(async () => { await module.Execute(context); })
            .Throws<InvalidOperationException>().And.HasMessageContaining("expected-version");
    }

    [Test]
    [Arguments(false, "4.0.1", "4.0.0")]
    [Arguments(false, "4.0.0", "4.0.1")]
    [Arguments(false, "4.0.0", "v4.0.0")]
    [Arguments(true, "4.0.0", "4.0.0")]
    public async Task Publishing_Rejects_An_Unexpected_Version(bool isAlpha, string computedVersion, string expectedVersion)
    {
        var (module, context) = CreateModule(true, isAlpha, expectedVersion, computedVersion);

        await Assert.That(async () => { await module.Execute(context); })
            .Throws<InvalidOperationException>().And.HasMessageContaining("does not match expected version");
    }

    [Test]
    [Arguments(false, "4.0.0", "4.0.0")]
    [Arguments(true, "4.0.0-alpha0002", "4.0.0-alpha0002")]
    public async Task Publishing_Returns_The_Matching_Version(bool isAlpha, string expectedVersion, string packageVersion)
    {
        var (module, context) = CreateModule(true, isAlpha, expectedVersion, "4.0.0");

        await Assert.That(await module.Execute(context)).IsEqualTo(packageVersion);
    }

    [Test]
    [Arguments(false, null, "4.0.0")]
    [Arguments(true, null, "4.0.0-alpha0002")]
    [Arguments(false, "different-version", "4.0.0")]
    public async Task Validation_And_Tagging_Do_Not_Require_An_Expected_Version(bool isAlpha, string? expectedVersion, string packageVersion)
    {
        var (module, context) = CreateModule(false, isAlpha, expectedVersion, "4.0.0");

        await Assert.That(await module.Execute(context)).IsEqualTo(packageVersion);
    }

    [Test]
    [Arguments(false, "4.2.3")]
    [Arguments(true, "4.2.3-alpha0012")]
    public async Task Packing_Reuses_The_Exact_Prebuild_Version(bool shouldPublish, string version)
    {
        var (module, context) = CreateModule(shouldPublish, false, version, "different-git-version",
            new PipelineSettings { BuildAlreadyCompleted = true, PackageVersion = version });

        await Assert.That(await module.Execute(context)).IsEqualTo(version);
        Mock.Get(context).Verify(x => x.Tools.Get<IGit>(), Times.Never);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    public async Task Rejects_Prebuild_Without_Its_Version(string? version)
    {
        var (module, context) = CreateModule(false, false, null, "4.2.3",
            new PipelineSettings { BuildAlreadyCompleted = true, PackageVersion = version });

        await Assert.That(async () => { await module.Execute(context); })
            .Throws<InvalidOperationException>().And.HasMessageContaining("Pipeline:PackageVersion");
        Mock.Get(context).Verify(x => x.Tools.Get<IGit>(), Times.Never);
    }

    [Test]
    [Arguments(null)]
    [Arguments("4.2.4")]
    public async Task Publishing_Validates_The_Prebuild_Version(string? expectedVersion)
    {
        var (module, context) = CreateModule(true, false, expectedVersion, "4.2.3",
            new PipelineSettings { BuildAlreadyCompleted = true, PackageVersion = "4.2.3" });

        await Assert.That(async () => { await module.Execute(context); }).Throws<InvalidOperationException>();
        Mock.Get(context).Verify(x => x.Tools.Get<IGit>(), Times.Never);
    }

    private static (ExposedVersionModule Module, IModuleContext Context) CreateModule(
        bool shouldPublish, bool isAlpha, string? expectedVersion, string computedVersion, PipelineSettings? pipelineSettings = null)
    {
        var git = new Mock<IGit> { DefaultValue = DefaultValue.Mock };
        git.Setup(x => x.Versioning.GetVersioningInformationAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GitVersionInformation
            {
                FullSemVer = computedVersion,
                CommitsSinceVersionSource = 2,
            });
        var context = new Mock<IModuleContext> { DefaultValue = DefaultValue.Mock };
        context.Setup(x => x.Tools.Get<IGit>()).Returns(git.Object);
        var module = new ExposedVersionModule(Microsoft.Extensions.Options.Options.Create(new PublishSettings
        {
            ShouldPublish = shouldPublish,
            IsAlpha = isAlpha,
            ExpectedVersion = expectedVersion,
        }), Microsoft.Extensions.Options.Options.Create(pipelineSettings ?? new PipelineSettings()));
        return (module, context.Object);
    }

    // Exercise the module body independently of its CI capability requirement.
    private class ExposedVersionModule(IOptions<PublishSettings> settings, IOptions<PipelineSettings> pipelineSettings) : NugetVersionGeneratorModule(settings, pipelineSettings)
    {
        public Task<string> Execute(IModuleContext context) => base.ExecuteAsync(context, CancellationToken.None);
    }
}
