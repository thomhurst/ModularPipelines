using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;

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
    public async Task AddModule_ReturnsSamePipelineBuilder()
    {
        var builder = TestPipelineHostBuilder.Create();

        var result = builder.AddModule<TestModuleA>();

        await Assert.That(result).IsSameReferenceAs(builder);
    }

    [Test]
    public async Task RegistrationApi_UsesOnlyPipelineBuilder()
    {
        var addMethods = typeof(PipelineBuilderExtensions)
            .GetMethods()
            .Where(method => method.IsPublic && method.Name.StartsWith("Add", StringComparison.Ordinal))
            .ToList();

        await Assert.That(addMethods).IsNotEmpty();
        await Assert.That(addMethods.All(method => method.ReturnType == typeof(PipelineBuilder))).IsTrue();
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
        var builder = TestPipelineHostBuilder.Create();

        await Assert.That(builder.Options.LoadModularPipelineAssemblies).IsFalse();

        builder.ConfigurePipelineOptions(options => options.LoadModularPipelineAssemblies = true);

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
        var builder = TestPipelineHostBuilder.Create()
            .AddModules(typeof(TestModuleA), typeof(TestModuleB));

        var registeredTypes = ServiceCollectionExtensions.GetRegisteredModuleTypes(builder.Services);

        await Assert.That(registeredTypes).Contains(typeof(TestModuleA));
        await Assert.That(registeredTypes).Contains(typeof(TestModuleB));
    }

    [Test]
    public async Task AddModules_RejectsNonModuleTypes()
    {
        var builder = TestPipelineHostBuilder.Create();

        await Assert.That(() => builder.AddModules(typeof(string)))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task AddModule_ChainedCalls_RegisterAllModules()
    {
        var builder = TestPipelineHostBuilder.Create()
            .AddModule<TestModuleA>()
            .AddModule<TestModuleB>();

        var moduleDescriptors = builder.Services
            .Where(descriptor => descriptor.ServiceType == typeof(IModule))
            .ToList();

        await Assert.That(moduleDescriptors.Count).IsEqualTo(2);
    }

    [Test]
    public async Task WithTags_ConfiguresLatestModule()
    {
        var builder = TestPipelineHostBuilder.Create()
            .AddModule<TestModuleA>()
            .WithTags("tag1", "tag2");

        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ModuleRegistrationOptions>>().Value;

        await Assert.That(options.Tags[typeof(TestModuleA)]).IsEquivalentTo(["tag1", "tag2"]);
    }

    [Test]
    public async Task WithCategory_ConfiguresLatestModule()
    {
        var builder = TestPipelineHostBuilder.Create()
            .AddModule<TestModuleA>()
            .WithCategory("TestCategory");

        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ModuleRegistrationOptions>>().Value;

        await Assert.That(options.Categories[typeof(TestModuleA)]).IsEqualTo("TestCategory");
    }

    [Test]
    public async Task ModuleMetadata_CanChainAcrossModules()
    {
        var builder = TestPipelineHostBuilder.Create()
            .AddModule<TestModuleA>()
            .WithTags("tag1")
            .WithTags("tag2", "tag3")
            .WithCategory("Category1")
            .AddModule<TestModuleB>()
            .WithCategory("Category2");

        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ModuleRegistrationOptions>>().Value;

        await Assert.That(options.Tags[typeof(TestModuleA)]).IsEquivalentTo(["tag1", "tag2", "tag3"]);
        await Assert.That(options.Categories[typeof(TestModuleA)]).IsEqualTo("Category1");
        await Assert.That(options.Categories[typeof(TestModuleB)]).IsEqualTo("Category2");
    }

    [Test]
    public async Task ModuleMetadata_RequiresPriorModule()
    {
        var builder = TestPipelineHostBuilder.Create();

        await Assert.That(() => builder.WithTags("tag"))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Builder_CanAddRequirement()
    {
        var builder = TestPipelineHostBuilder.Create()
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
        var builder = TestPipelineHostBuilder.Create()
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
