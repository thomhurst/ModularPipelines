using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Documentation;

public partial class DocumentationSnippetTests
{
    [Test]
    public async Task Azure_Example_Dependencies_Match_Compiled_Local_Graph()
    {
        var documentation = await File.ReadAllTextAsync(Path.Combine(
            FindRepositoryRoot(), "docs/docs/examples/azure-example.md"));
        var fences = Regex.Matches(documentation, @"```csharp\r?\n(.*?)```", RegexOptions.Singleline)
            .Select(match => match.Groups[1].Value).ToArray();

        foreach (var module in CreateAzureExampleModules())
        {
            var type = module.GetType();
            var fence = fences.Single(code => code.Contains($"public class {type.Name} :", StringComparison.Ordinal));
            var dependencies = type.GetCustomAttributes(true).OfType<IModuleDependencyAttribute>().ToArray();
            await Assert.That(dependencies.All(dependency => !dependency.Optional)).IsTrue();
            await Assert.That(Regex.Matches(fence, @"\[DependsOn<[^\r\n]+\]").Select(match => match.Value))
                .IsEquivalentTo(dependencies.Select(dependency => $"[DependsOn<{dependency.Type.Name}>]"));
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Azure_Example_Function_Waits_For_Required_Role_Assignment(bool assignmentSucceeds)
    {
        using var scheduler = new ModuleScheduler(
            NullLogger.Instance,
            TimeProvider.System,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Mock.Of<IMetricsCollector>(),
            Mock.Of<IModuleConstraintEvaluator>(),
            Mock.Of<ISchedulerStatusReporter>());
        var modules = CreateAzureExampleModules();
        scheduler.InitializeModules(modules);
        scheduler.MarkModuleCompleted(typeof(ProvisionUserAssignedIdentityModule), success: true);
        scheduler.MarkModuleCompleted(typeof(ProvisionBlobStorageAccountModule), success: true);
        scheduler.MarkModuleCompleted(typeof(ProvisionBlobStorageContainerModule), success: true);

        var function = modules.OfType<ProvisionAzureFunction>().Single();
        var functionState = scheduler.GetModuleState(typeof(ProvisionAzureFunction))!;
        // Every other prerequisite is complete: the role assignment alone must block provisioning.
        await Assert.That(functionState.UnresolvedDependencies).IsEquivalentTo([typeof(AssignAccessToBlobStorageModule)]);
        await Assert.That(functionState.IsReadyToExecute).IsFalse();

        using var services = new ServiceCollection().BuildServiceProvider();
        using var cancellation = new CancellationTokenSource(TestHostSettings.DefaultTestTimeout);
        var provisioning = ProvisionAfterDependenciesAsync();
        await Assert.That(provisioning.IsCompleted).IsFalse();
        await Assert.That(function.Provisioned).IsFalse();

        var assignmentFailure = new InvalidOperationException("Role assignment failed");
        scheduler.MarkModuleCompleted(typeof(AssignAccessToBlobStorageModule), assignmentSucceeds,
            assignmentSucceeds ? null : assignmentFailure);

        if (assignmentSucceeds)
        {
            await provisioning.WaitAsync(cancellation.Token);
            await Assert.That(function.Provisioned).IsTrue();
        }
        else
        {
            var exception = await Assert.ThrowsAsync<DependencyFailedException>(
                () => provisioning.WaitAsync(cancellation.Token));
            await Assert.That(exception!.InnerException).IsSameReferenceAs(assignmentFailure);
            await Assert.That(function.Provisioned).IsFalse();
        }

        async Task ProvisionAfterDependenciesAsync()
        {
            await new DependencyWaiter().WaitForDependenciesAsync(functionState, scheduler, services, cancellation.Token);
            await function.ExecuteAsync(Mock.Of<IModuleContext>(), cancellation.Token);
        }
    }

    // These local modules compile the published dependency declarations without Azure SDKs or credentials.
    private static IModule[] CreateAzureExampleModules() =>
    [
        new ProvisionUserAssignedIdentityModule(),
        new ProvisionBlobStorageAccountModule(),
        new ProvisionBlobStorageContainerModule(),
        new AssignAccessToBlobStorageModule(),
        new ProvisionAzureFunction(),
    ];

    private class ProvisionUserAssignedIdentityModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private class ProvisionBlobStorageAccountModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [DependsOn<ProvisionBlobStorageAccountModule>]
    private class ProvisionBlobStorageContainerModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [DependsOn<ProvisionBlobStorageAccountModule>]
    [DependsOn<ProvisionUserAssignedIdentityModule>]
    private class AssignAccessToBlobStorageModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [DependsOn<ProvisionUserAssignedIdentityModule>]
    [DependsOn<ProvisionBlobStorageAccountModule>]
    [DependsOn<ProvisionBlobStorageContainerModule>]
    [DependsOn<AssignAccessToBlobStorageModule>]
    private class ProvisionAzureFunction : Module<bool>
    {
        public bool Provisioned { get; private set; }

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            Provisioned = true;
            return Task.FromResult(true);
        }
    }
}
