using System.Reflection;
using System.Reflection.Emit;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ModuleInterfaceBuildIdentityTests
{
    public interface IProcessor<T>;

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Schema_Detects_Interface_And_Argument_Builds(bool genericArgument, bool inherited)
    {
        Type BuildContract(int version)
        {
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("ExternalContract") { Version = new Version(version, 0, 0, 0) }, AssemblyBuilderAccess.RunAndCollect);
            var attributes = TypeAttributes.Public;
            if (!genericArgument)
            {
                attributes |= TypeAttributes.Interface | TypeAttributes.Abstract;
            }

            var type = assembly.DefineDynamicModule("ExternalContract").DefineType("ExternalType", attributes).CreateType()!;
            return genericArgument ? typeof(IProcessor<>).MakeGenericType(type) : type;
        }

        var module = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("InterfaceModuleBuild"), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("InterfaceModuleBuild");
        Type BuildModule(string name, Type contract)
        {
            const TypeAttributes attributes = TypeAttributes.Public | TypeAttributes.Abstract;
            var builder = module.DefineType(name, attributes, typeof(Module<string>));
            builder.AddInterfaceImplementation(contract);
            if (inherited)
            {
                builder = module.DefineType($"{name}Derived", attributes, builder.CreateType()!);
            }

            builder.SetCustomAttribute(new CustomAttributeBuilder(
                typeof(ModuleIdAttribute).GetConstructor([typeof(string)])!, ["interface-module-build"]));
            return builder.CreateType()!;
        }

        static string Schema(Type type)
        {
            var registry = new ModuleTypeRegistry();
            registry.Register(type);
            return registry.GetPipelineSchemaVersion();
        }

        var firstContract = BuildContract(1);
        var changedContract = BuildContract(2);
        var first = BuildModule("First", firstContract);
        var same = BuildModule("Same", firstContract);
        var changed = BuildModule("Changed", changedContract);
        await Assert.That(first.IsGenericType).IsFalse();
        await Assert.That(first.Module.ModuleVersionId).IsEqualTo(changed.Module.ModuleVersionId);
        await Assert.That(ModuleId.FromType(first)).IsEqualTo(ModuleId.FromType(changed));
        await Assert.That(first.GetInterfaces()).Contains(firstContract);
        await Assert.That(changed.GetInterfaces()).Contains(changedContract);
        await Assert.That(Schema(first)).IsEqualTo(Schema(same));
        await Assert.That(Schema(first)).IsNotEqualTo(Schema(changed));
    }
}
