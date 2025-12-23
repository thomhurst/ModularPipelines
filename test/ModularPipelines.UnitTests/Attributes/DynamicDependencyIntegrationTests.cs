using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

[NotInParallel(nameof(DynamicDependencyIntegrationTests))]
public class DynamicDependencyIntegrationTests : TestBase
{
    private static readonly List<string> ExecutionOrder = new();

    public class AddDependencyAttribute : Attribute, IModuleRegistrationEventReceiver
    {
        private readonly Type _dependencyType;

        public AddDependencyAttribute(Type dependencyType)
        {
            _dependencyType = dependencyType;
        }

        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            context.AddDependency(_dependencyType);
            return Task.CompletedTask;
        }
    }

    public class ModuleA : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Add("A");
            await Task.Yield();
            return "A";
        }
    }

    [AddDependency(typeof(ModuleA))]
    public class ModuleB : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Add("B");
            await Task.Yield();
            return "B";
        }
    }

    [Before(Test)]
    public void ClearExecutionOrder()
    {
        ExecutionOrder.Clear();
    }

    [Test]
    public async Task DynamicDependency_ModuleBWaitsForModuleA()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleA>()
            .AddModule<ModuleB>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);
        await Assert.That(ExecutionOrder).IsEquivalentTo(new[] { "A", "B" });
    }
}
