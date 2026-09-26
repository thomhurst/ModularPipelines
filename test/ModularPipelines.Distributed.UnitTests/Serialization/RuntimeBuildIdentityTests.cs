using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Modules;
using ModularPipelines.Serialization;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class RuntimeBuildIdentityTests
{
    [ModuleId("runtime-independent")]
    private abstract class ResultModule<T> : Module<T>;

    [Test]
    public async Task Framework_Boundary_Preserves_Application_Build_Validation()
    {
        // The host's trusted-platform list also contains application assemblies.
        await Assert.That(StableTypeName.IsFrameworkAssembly(typeof(ResultModule<>).Assembly)).IsFalse();
        await Assert.That(StableTypeName.IsFrameworkAssembly(typeof(Module<>).Assembly)).IsFalse();
    }

    [Test]
    [Arguments(typeof(string))]
    [Arguments(typeof(DateTime))]
    [Arguments(typeof(Uri))]
    [Arguments(typeof(List<string>))]
    public async Task Framework_Result_Fingerprint_Tolerates_Runtime_Servicing(Type type)
    {
        var first = new RuntimeRetargetedType(type, Guid.NewGuid());
        var second = new RuntimeRetargetedType(type, Guid.NewGuid());

        await Assert.That(first.Module.ModuleVersionId).IsNotEqualTo(second.Module.ModuleVersionId);
        await Assert.That(StableTypeName.GetBuildFingerprint(first))
            .IsEqualTo(StableTypeName.GetBuildFingerprint(second));
    }

    [Test]
    public async Task Schema_Tolerates_Runtime_Servicing_With_Unchanged_Pipeline_Binaries()
    {
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(new RuntimeRetargetedType(typeof(ResultModule<string>), Guid.NewGuid()));
        second.Register(new RuntimeRetargetedType(typeof(ResultModule<string>), Guid.NewGuid()));

        await Assert.That(first.GetPipelineSchemaVersion()).IsEqualTo(second.GetPipelineSchemaVersion());
    }

    // Model two hosts with identical application binaries and different BCL MVIDs.
    // TypeDelegator avoids loading a second, incompatible CoreLib into the test process.
    private sealed class RuntimeRetargetedType(Type type, Guid runtimeBuild) : TypeDelegator(type)
    {
        public override System.Reflection.Module Module =>
            typeImpl.Assembly == typeof(object).Assembly || typeImpl.Assembly == typeof(Uri).Assembly
                ? new RuntimeModule(runtimeBuild)
                : typeImpl.Module;

        public override Type? BaseType => typeImpl.BaseType is { } parent
            ? new RuntimeRetargetedType(parent, runtimeBuild)
            : null;

        public override bool IsGenericType => typeImpl.IsGenericType;

        public override Type GetGenericTypeDefinition() => typeImpl.GetGenericTypeDefinition();

        public override Type[] GetGenericArguments() =>
            [.. typeImpl.GetGenericArguments().Select(argument => new RuntimeRetargetedType(argument, runtimeBuild))];
    }

    private sealed class RuntimeModule(Guid buildId) : System.Reflection.Module
    {
        public override Guid ModuleVersionId => buildId;
    }
}
