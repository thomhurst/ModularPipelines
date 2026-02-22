using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ModuleTypeRegistryTests
{
    private class TestResult
    {
        public string Value { get; set; } = string.Empty;
    }

    private class TestModule : Module<TestResult>
    {
        protected internal override Task<TestResult?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<TestResult?>(new TestResult { Value = "test" });
        }
    }

    private class AnotherModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            ModularPipelines.Context.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("hello");
        }
    }

    [Test]
    public async Task Register_And_Resolve_Returns_Correct_Types()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(TestModule));

        var resolved = registry.Resolve(typeof(TestModule).FullName!);

        await Assert.That(resolved).IsNotNull();
        await Assert.That(resolved!.Value.ModuleType).IsEqualTo(typeof(TestModule));
        await Assert.That(resolved!.Value.ResultType).IsEqualTo(typeof(TestResult));
    }

    [Test]
    public async Task Resolve_Unknown_Type_Returns_Null()
    {
        var registry = new ModuleTypeRegistry();

        var resolved = registry.Resolve("NonExistent.Module");

        await Assert.That(resolved).IsNull();
    }

    [Test]
    public async Task GetResultTypeName_Returns_FullName()
    {
        var registry = new ModuleTypeRegistry();

        var resultTypeName = ModuleTypeRegistry.GetResultTypeName(typeof(TestModule));

        await Assert.That(resultTypeName).IsEqualTo(typeof(TestResult).FullName);
    }

    [Test]
    public async Task Register_Multiple_Modules()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(TestModule));
        registry.Register(typeof(AnotherModule));

        var resolved1 = registry.Resolve(typeof(TestModule).FullName!);
        var resolved2 = registry.Resolve(typeof(AnotherModule).FullName!);

        await Assert.That(resolved1).IsNotNull();
        await Assert.That(resolved2).IsNotNull();
        await Assert.That(resolved2!.Value.ResultType).IsEqualTo(typeof(string));
    }
}
