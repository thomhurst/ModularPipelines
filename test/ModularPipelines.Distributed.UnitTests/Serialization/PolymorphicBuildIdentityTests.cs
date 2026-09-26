using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Serialization;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class PolymorphicBuildIdentityTests
{
    private abstract class ResultModule<T> : Module<T>;

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Nested_Polymorphism_Uses_Declared_Contract_Attributes(bool concreteContract)
    {
        var firstContext = new AssemblyLoadContext(null, isCollectible: true);
        var secondContext = new AssemblyLoadContext(null, isCollectible: true);
        try
        {
            var contractName = $"PolymorphicContract_{Guid.NewGuid():N}";
            var derivedName = $"PolymorphicDerived_{Guid.NewGuid():N}";
            var contractAssembly = new PersistedAssemblyBuilder(new AssemblyName(contractName), typeof(object).Assembly);
            var contractModule = contractAssembly.DefineDynamicModule(contractName);
            var baseType = contractModule.DefineType("Base", TypeAttributes.Public | TypeAttributes.Abstract);
            baseType.DefineDefaultConstructor(MethodAttributes.Public);
            var derivedAssembly = CreateDerivedAssembly(derivedName, baseType, "Original", out var derivedType);
            baseType.SetCustomAttribute(new CustomAttributeBuilder(
                typeof(JsonDerivedTypeAttribute).GetConstructor([typeof(Type), typeof(string)])!, [derivedType, "derived"]));
            baseType.CreateType();
            derivedType.CreateType();
            var fieldType = concreteContract
                ? contractModule.DefineType("Concrete", TypeAttributes.Public, baseType).CreateType()!
                : baseType;
            var envelope = contractModule.DefineType("Envelope", TypeAttributes.Public);
            var field = envelope.DefineField("Value", fieldType, FieldAttributes.Public);
            field.SetCustomAttribute(new CustomAttributeBuilder(typeof(JsonIncludeAttribute).GetConstructor(Type.EmptyTypes)!, []));
            envelope.CreateType();
            var contractImage = Save(contractAssembly);
            var firstContract = Load(firstContext, contractImage);
            var firstDerived = Load(firstContext, Save(derivedAssembly)).GetType("Derived")!;
            var secondContract = Load(secondContext, contractImage);
            var changedAssembly = CreateDerivedAssembly(derivedName, firstContract.GetType("Base")!, "Changed", out var changedType);
            changedType.CreateType();
            var secondDerived = Load(secondContext, Save(changedAssembly)).GetType("Derived")!;
            var firstEnvelope = firstContract.GetType("Envelope")!;
            var secondEnvelope = secondContract.GetType("Envelope")!;

            await Assert.That(firstEnvelope.Module.ModuleVersionId).IsEqualTo(secondEnvelope.Module.ModuleVersionId);
            await Assert.That(firstDerived.Module.ModuleVersionId).IsNotEqualTo(secondDerived.Module.ModuleVersionId);
            var firstRegistry = new ModuleTypeRegistry();
            var secondRegistry = new ModuleTypeRegistry();
            firstRegistry.Register(typeof(ResultModule<>).MakeGenericType(firstEnvelope));
            secondRegistry.Register(typeof(ResultModule<>).MakeGenericType(secondEnvelope));
            await Assert.That(firstRegistry.GetPipelineSchemaVersion() != secondRegistry.GetPipelineSchemaVersion()).IsEqualTo(!concreteContract);

            var localEnvelope = StableTypeName.Resolve(StableTypeName.Get(firstEnvelope))!;
            var firstValueType = concreteContract ? firstContract.GetType("Concrete")! : firstDerived;
            var secondValueType = concreteContract ? secondContract.GetType("Concrete")! : secondDerived;
            var localDerived = localEnvelope == firstEnvelope ? firstValueType : secondValueType;
            var remoteEnvelope = localEnvelope == firstEnvelope ? secondEnvelope : firstEnvelope;
            var remoteDerived = localEnvelope == firstEnvelope ? secondValueType : firstValueType;
            var local = JsonSerializer.Deserialize<ModuleResult<object>>(Serialize(localEnvelope, localDerived))!.Value;
            await Assert.That(localEnvelope.GetField("Value")!.GetValue(local)!.GetType()).IsEqualTo(localDerived);
            if (concreteContract)
            {
                var remote = JsonSerializer.Deserialize<ModuleResult<object>>(Serialize(remoteEnvelope, remoteDerived))!.Value;
                await Assert.That(localEnvelope.GetField("Value")!.GetValue(remote)!.GetType()).IsEqualTo(localDerived);
            }
            else
            {
                var error = Assert.Throws<JsonException>(() =>
                    JsonSerializer.Deserialize<ModuleResult<object>>(Serialize(remoteEnvelope, remoteDerived)));
                await Assert.That(error!.Message).Contains("build identity");
            }
        }
        finally
        {
            firstContext.Unload();
            secondContext.Unload();
        }
    }

    private static PersistedAssemblyBuilder CreateDerivedAssembly(string name, Type baseType, string propertyName, out TypeBuilder type)
    {
        var assembly = new PersistedAssemblyBuilder(new AssemblyName(name), typeof(object).Assembly);
        type = assembly.DefineDynamicModule(name).DefineType("Derived", TypeAttributes.Public, baseType);
        var property = type.DefineProperty(propertyName, PropertyAttributes.None, typeof(string), null);
        var getter = type.DefineMethod($"get_{propertyName}",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(string), Type.EmptyTypes);
        var il = getter.GetILGenerator();
        il.Emit(OpCodes.Ldstr, propertyName);
        il.Emit(OpCodes.Ret);
        property.SetGetMethod(getter);
        return assembly;
    }

    private static string Serialize(Type envelope, Type derived)
    {
        var value = Activator.CreateInstance(envelope)!;
        envelope.GetField("Value")!.SetValue(value, Activator.CreateInstance(derived));
        return JsonSerializer.Serialize(new ModuleResult<object>.Success(value)
        {
            Name = "PolymorphicResult",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        });
    }

    private static byte[] Save(PersistedAssemblyBuilder assembly)
    {
        using var stream = new MemoryStream();
        assembly.Save(stream);
        return stream.ToArray();
    }

    private static Assembly Load(AssemblyLoadContext context, byte[] image)
    {
        using var stream = new MemoryStream(image);
        return context.LoadFromStream(stream);
    }
}
