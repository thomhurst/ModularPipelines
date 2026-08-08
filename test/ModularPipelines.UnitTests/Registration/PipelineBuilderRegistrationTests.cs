using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Registration;

public class PipelineBuilderRegistrationTests
{
    private class TestModuleA : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class TestModuleB : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private abstract class TestModuleBase : SimpleTestModule<bool>
    {
    }

    private sealed class DerivedTestModule : TestModuleBase
    {
        protected override bool Result => true;
    }

    private sealed class TestAssembly(
        AssemblyName assemblyName,
        params AssemblyName[] referencedAssemblies) : Assembly
    {
        public override bool IsDynamic => false;

        public override AssemblyName GetName() => assemblyName;

        public override AssemblyName GetName(bool copiedName) => assemblyName;

        public override AssemblyName[] GetReferencedAssemblies() => referencedAssemblies;
    }

    [Test]
    public async Task AddModule_ReturnsTypedRegistrationConvertibleToBuilder()
    {
        var builder = TestPipelineBuilder.Create();

        var result = builder.AddModule<TestModuleA>();
        PipelineBuilder convertedBuilder = result;

        await Assert.That(result).IsTypeOf<ModuleRegistration<TestModuleA>>();
        await Assert.That(convertedBuilder).IsSameReferenceAs(builder);
    }

    [Test]
    public async Task TestPipelineBuilder_DisablesPersistentRunReports()
    {
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<TestModuleA>()
            .BuildAsync();
        var runReportOptions = pipeline.Services
            .GetRequiredService<IOptions<PipelineOptions>>()
            .Value
            .RunReport;

        using (Assert.Multiple())
        {
            await Assert.That(runReportOptions.AutoWriteInCi).IsFalse();
            await Assert.That(runReportOptions.HistoryRetention).IsEqualTo(0);
        }
    }

    [Test]
    public async Task RegistrationApi_UsesTypedHandlesOnlyForAddModule()
    {
        var addMethods = typeof(PipelineBuilderExtensions)
            .GetMethods()
            .Where(method => method.IsPublic && method.Name.StartsWith("Add", StringComparison.Ordinal))
            .ToList();
        var addModuleMethods = addMethods
            .Where(method => method.Name == nameof(PipelineBuilderExtensions.AddModule))
            .ToList();
        var otherAddMethods = addMethods.Except(addModuleMethods).ToList();

        await Assert.That(addModuleMethods.Count).IsEqualTo(3);
        await Assert.That(addModuleMethods.All(method =>
            method.ReturnType.IsGenericType
            && method.ReturnType.GetGenericTypeDefinition() == typeof(ModuleRegistration<>))).IsTrue();
        await Assert.That(otherAddMethods.All(method => method.ReturnType == typeof(PipelineBuilder))).IsTrue();
        await Assert.That(typeof(PipelineBuilderExtensions).GetMethods()
            .Any(method => method.Name is "WithTags" or "WithCategory")).IsFalse();
        await Assert.That(typeof(ServiceCollectionExtensions).IsPublic).IsFalse();
    }

    [Test]
    public async Task BuildApi_IsAsyncOnly()
    {
        var synchronousBuild = typeof(PipelineBuilder).GetMethod(
            "Build",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        await Assert.That(synchronousBuild).IsNull();
    }

    [Test]
    public async Task ModularPipelineAssemblyLoading_IsOptIn()
    {
        var builder = TestPipelineBuilder.Create();

        await Assert.That(builder.Options.LoadModularPipelineAssemblies).IsFalse();

        builder.ConfigurePipelineOptions(options => options with
        {
            LoadModularPipelineAssemblies = true,
        });

        await Assert.That(builder.Options.LoadModularPipelineAssemblies).IsTrue();
    }

    [Test]
    public async Task ReferencedIntegrationLoading_TraversesNonModularAssemblies()
    {
        var integrationName = new AssemblyName("ModularPipelines.FakeIntegration");
        var integrationAssembly = new TestAssembly(integrationName);
        var intermediateAssembly = new TestAssembly(
            new AssemblyName("Acme.Pipeline.Modules"),
            integrationName);
        var loadedAssemblyNames = new List<AssemblyName>();

        ReferencedAssemblyTraversal.LoadModularPipelinesAssemblies(
            [intermediateAssembly],
            assemblyName =>
            {
                loadedAssemblyNames.Add(assemblyName);
                return integrationAssembly;
            });

        await Assert.That(loadedAssemblyNames).Contains(integrationName);
    }

    [Test]
    public async Task Environment_IsCachedAndHonorsBuilderOptions()
    {
        var contentRoot = Path.GetTempPath();
        var fileName = $"pipeline-environment-{Guid.NewGuid():N}.txt";
        var filePath = Path.Combine(contentRoot, fileName);
        await File.WriteAllTextAsync(filePath, "content");

        try
        {
            using var builder = Pipeline.CreateBuilder(new PipelineBuilderOptions
            {
                ApplicationName = "ConfiguredApp",
                EnvironmentName = "ConfiguredEnvironment",
                ContentRootPath = contentRoot,
            });

            var environment = builder.Environment;

            await Assert.That(builder.Environment).IsSameReferenceAs(environment);
            await Assert.That(environment.ApplicationName).IsEqualTo("ConfiguredApp");
            await Assert.That(environment.EnvironmentName).IsEqualTo("ConfiguredEnvironment");
            await Assert.That(environment.ContentRootPath).IsEqualTo(Path.GetFullPath(contentRoot));
            await Assert.That(environment.ContentRootFileProvider.GetFileInfo(fileName).Exists).IsTrue();
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Test]
    public async Task Environment_HonorsCommandLineHostConfiguration()
    {
        var contentRoot = Path.GetTempPath();
        using var builder = Pipeline.CreateBuilder(
        [
            "--applicationName", "CommandLineApp",
            "--environment", "CommandLineEnvironment",
            "--contentRoot", contentRoot,
        ]);

        await Assert.That(builder.Environment.ApplicationName).IsEqualTo("CommandLineApp");
        await Assert.That(builder.Environment.EnvironmentName).IsEqualTo("CommandLineEnvironment");
        await Assert.That(builder.Environment.ContentRootPath).IsEqualTo(Path.GetFullPath(contentRoot));
    }

    [Test]
    public async Task ContentRootFileProvider_IsOwnedByBuiltPipeline()
    {
        var fileProvider = new Mock<IFileProvider>();
        var disposable = fileProvider.As<IDisposable>();
        var builder = Pipeline.CreateBuilder();
        builder.Environment.ContentRootFileProvider = fileProvider.Object;
        builder.AddModule<TestModuleA>();

        builder.Dispose();
        disposable.Verify(x => x.Dispose(), Times.Never);

        var pipeline = await builder.BuildAsync();
        disposable.Verify(x => x.Dispose(), Times.Never);

        await pipeline.DisposeAsync();
        disposable.Verify(x => x.Dispose(), Times.Once);
    }

    [Test]
    public async Task ContentRootFileProvider_IsDisposed_WhenPipelineBuildFails()
    {
        var fileProvider = new Mock<IFileProvider>();
        var disposable = fileProvider.As<IDisposable>();
        var builder = Pipeline.CreateBuilder();
        builder.Environment.ContentRootFileProvider = fileProvider.Object;

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<PipelineValidationException>();

        disposable.Verify(x => x.Dispose(), Times.Once);
    }

    [Test]
    public async Task ExecutePipelineAsync_ValidatesBeforeRunning()
    {
        var builder = Pipeline.CreateBuilder();

        await Assert.That(async () => await builder.ExecutePipelineAsync())
            .Throws<PipelineValidationException>();
    }

    [Test]
    public async Task AddModules_UsesRuntimeTypeArray()
    {
        var addModulesMethods = typeof(PipelineBuilderExtensions)
            .GetMethods()
            .Where(method => method.IsPublic && method.Name == nameof(PipelineBuilderExtensions.AddModules))
            .ToList();

        await Assert.That(addModulesMethods.Count).IsEqualTo(1);
        await Assert.That(addModulesMethods[0].IsGenericMethod).IsFalse();
        await Assert.That(addModulesMethods[0].GetParameters()[1].ParameterType).IsEqualTo(typeof(Type[]));
    }

    [Test]
    public async Task AddModules_RegistersRuntimeTypes()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModules(typeof(TestModuleA), typeof(TestModuleB));

        var registeredTypes = ServiceCollectionExtensions.GetRegisteredModuleTypes(builder.Services);

        await Assert.That(registeredTypes).Contains(typeof(TestModuleA));
        await Assert.That(registeredTypes).Contains(typeof(TestModuleB));
    }

    [Test]
    public async Task AddModules_RejectsNonModuleTypes()
    {
        var builder = TestPipelineBuilder.Create();

        await Assert.That(() => builder.AddModules(typeof(string)))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task AddModule_ChainedCalls_RegisterAllModules()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<TestModuleA>()
            .AddModule<TestModuleB>();

        var moduleDescriptors = builder.Services
            .Where(descriptor => descriptor.ServiceType == typeof(IModule))
            .ToList();

        await Assert.That(moduleDescriptors.Count).IsEqualTo(2);
    }

    [Test]
    public async Task Builder_CanAddRequirement()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<TestModuleA>()
            .AddRequirement<TestRequirement>();

        var requirements = builder.Services
            .Where(descriptor => descriptor.ServiceType == typeof(IPipelineRequirement))
            .ToList();

        await Assert.That(requirements.Count).IsEqualTo(1);
    }

    [Test]
    public async Task Builder_CanConfigureOptions()
    {
        var configuredValue = false;
        var builder = TestPipelineBuilder.Create()
            .AddModule<TestModuleA>()
            .Configure<TestOptions>(_ => configuredValue = true);

        using var provider = builder.Services.BuildServiceProvider();
        _ = provider.GetRequiredService<IOptions<TestOptions>>().Value;

        await Assert.That(configuredValue).IsTrue();
    }

    private class TestRequirement : IPipelineRequirement
    {
        public Task<RequirementDecision> MustAsync(IPipelineContext context)
            => Task.FromResult(RequirementDecision.Passed);
    }

    private class TestOptions
    {
        public string? Value { get; set; }
    }
}
