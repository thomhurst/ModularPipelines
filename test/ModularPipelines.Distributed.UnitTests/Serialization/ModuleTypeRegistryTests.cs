using ModularPipelines.Attributes;
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
        protected internal override Task<TestResult> ExecuteAsync(
            ModularPipelines.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<TestResult>(new TestResult { Value = "test" });
        }
    }

    private class AnotherModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("hello");
        }
    }

    [ModuleId("stable-module")]
    private class StableModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult("stable");
    }

    [ModuleId("stable-module")]
    private class ConflictingStableModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            ModularPipelines.IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(1);
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
    public async Task ModuleId_Defaults_To_FullName_And_Honors_Override()
    {
        await Assert.That(ModuleId.FromType(typeof(TestModule)).Value)
            .IsEqualTo(typeof(TestModule).FullName);
        await Assert.That(ModuleId.FromType(typeof(StableModule)).Value)
            .IsEqualTo("stable-module");
    }

    [Test]
    public async Task Duplicate_ModuleId_Is_Rejected()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(StableModule));

        var exception = Assert.Throws<InvalidOperationException>(
            () => registry.Register(typeof(ConflictingStableModule)));

        await Assert.That(exception!.Message).Contains("stable-module");
    }

    [Test]
    public async Task SchemaVersion_Is_Stable_Regardless_Of_Registration_Order()
    {
        var first = new ModuleTypeRegistry();
        first.Register(typeof(TestModule));
        first.Register(typeof(AnotherModule));
        var second = new ModuleTypeRegistry();
        second.Register(typeof(AnotherModule));
        second.Register(typeof(TestModule));

        await Assert.That(first.GetPipelineSchemaVersion())
            .IsEqualTo(second.GetPipelineSchemaVersion());
    }

    [Test]
    public async Task SchemaVersion_Changes_When_Module_Set_Changes()
    {
        var first = new ModuleTypeRegistry();
        first.Register(typeof(TestModule));
        var second = new ModuleTypeRegistry();
        second.Register(typeof(TestModule));
        second.Register(typeof(AnotherModule));

        await Assert.That(first.GetPipelineSchemaVersion())
            .IsNotEqualTo(second.GetPipelineSchemaVersion());
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
    [Test]
    public async Task Registration_Invalidates_Cached_Schema()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(TestModule));
        var original = registry.GetPipelineSchemaVersion();
        registry.Register(typeof(AnotherModule));
        await Assert.That(registry.GetPipelineSchemaVersion()).IsNotEqualTo(original);
        var updated = registry.GetPipelineSchemaVersion();
        registry.Register(typeof(AnotherModule));
        await Assert.That(registry.GetPipelineSchemaVersion()).IsEqualTo(updated);
    }

    [ModuleId("result-build-module")]
    private abstract class ResultBuildModule<T> : Module<T>;

    [ModuleId("generic-module-build")]
    private abstract class GenericBuildModule<T> : Module<string>;

    [Test]
    public async Task Schema_Detects_Different_Generic_Module_Argument_Builds()
    {
        static Type BuildArgument()
        {
            var assembly = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(
                new System.Reflection.AssemblyName("ModuleArgumentBuild"),
                System.Reflection.Emit.AssemblyBuilderAccess.RunAndCollect);
            return assembly.DefineDynamicModule("ModuleArgumentBuild")
                .DefineType("Argument", System.Reflection.TypeAttributes.Public).CreateType()!;
        }

        var firstType = typeof(GenericBuildModule<>).MakeGenericType(BuildArgument());
        var secondType = typeof(GenericBuildModule<>).MakeGenericType(BuildArgument());
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(firstType);
        second.Register(secondType);

        await Assert.That(ModuleId.FromType(firstType)).IsEqualTo(ModuleId.FromType(secondType));
        await Assert.That(firstType.Module.ModuleVersionId).IsEqualTo(secondType.Module.ModuleVersionId);
        await Assert.That(first.GetPipelineSchemaVersion()).IsNotEqualTo(second.GetPipelineSchemaVersion());
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Schema_Detects_Different_Result_Builds_With_Unchanged_Module_Binary(int shape)
    {
        static Type BuildResult(int shape)
        {
            var assembly = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(
                new System.Reflection.AssemblyName("ResultBuild"),
                System.Reflection.Emit.AssemblyBuilderAccess.RunAndCollect);
            var resultType = assembly.DefineDynamicModule("ResultBuild")
                .DefineType("Result", System.Reflection.TypeAttributes.Public).CreateType()!;
            return shape switch
            {
                1 => resultType.MakeArrayType(),
                2 => typeof(List<>).MakeGenericType(resultType.MakeArrayType()),
                _ => resultType,
            };
        }

        var firstType = typeof(ResultBuildModule<>).MakeGenericType(BuildResult(shape));
        var secondType = typeof(ResultBuildModule<>).MakeGenericType(BuildResult(shape));
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(firstType);
        second.Register(secondType);

        await Assert.That(ModuleId.FromType(firstType)).IsEqualTo(ModuleId.FromType(secondType));
        await Assert.That(firstType.Module.ModuleVersionId).IsEqualTo(secondType.Module.ModuleVersionId);
        await Assert.That(first.GetPipelineSchemaVersion()).IsNotEqualTo(second.GetPipelineSchemaVersion());
    }

    [Test]
    public async Task Schema_Detects_Different_Builds_With_Identical_Type_Names()
    {
        static Type BuildModule()
        {
            var assembly = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(
                new System.Reflection.AssemblyName("PipelineBuild"),
                System.Reflection.Emit.AssemblyBuilderAccess.RunAndCollect);
            return assembly.DefineDynamicModule("PipelineBuild").DefineType("BuildModule",
                System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Abstract,
                typeof(Module<string>)).CreateType()!;
        }

        var firstType = BuildModule();
        var secondType = BuildModule();
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(firstType);
        second.Register(secondType);
        await Assert.That(ModuleId.FromType(firstType)).IsEqualTo(ModuleId.FromType(secondType));
        await Assert.That(first.GetPipelineSchemaVersion()).IsNotEqualTo(second.GetPipelineSchemaVersion());
    }
}
