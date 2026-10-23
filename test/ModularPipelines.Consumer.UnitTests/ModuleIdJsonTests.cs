using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Consumer.UnitTests;

public class ModuleIdJsonTests
{
    [Test]
    public async Task Reflection_Fallback_Is_Disabled()
    {
        await Assert.That(JsonSerializer.IsReflectionEnabledByDefault).IsFalse();
    }

    [Test]
    public async Task Identifier_RoundTrips_As_A_String()
    {
        var id = new ModuleId("configured-id:linux");
        var contract = ConsumerJsonContext.Default.ModuleId;

        var json = JsonSerializer.Serialize(id, contract);
        var result = JsonSerializer.Deserialize(json, contract);

        await Assert.That(json).IsEqualTo("\"configured-id:linux\"");
        await Assert.That(result).IsEqualTo(id);
    }

    [Test]
    public async Task Identifiers_RoundTrip_As_Dictionary_Keys_And_Values()
    {
        var id = new ModuleId("build:linux");
        var contract = ConsumerJsonContext.Default.ModuleIdDictionary;
        var values = new Dictionary<ModuleId, ModuleId> { [id] = id };

        var json = JsonSerializer.Serialize(values, contract);
        var result = JsonSerializer.Deserialize(json, contract);

        await Assert.That(json).IsEqualTo("{\"build:linux\":\"build:linux\"}");
        await Assert.That(result![id]).IsEqualTo(id);
    }

    [Test]
    [Arguments("\"\"")]
    [Arguments("\" \"")]
    [Arguments("\"\\t\"")]
    [Arguments("null")]
    [Arguments("1")]
    [Arguments("true")]
    [Arguments("{}")]
    [Arguments("[]")]
    public async Task Invalid_Identifier_Json_Is_Rejected(string json)
    {
        await Assert.That(() => JsonSerializer.Deserialize(json, ConsumerJsonContext.Default.ModuleId))
            .Throws<JsonException>();
    }

    [Test]
    [Arguments("{\"\":\"valid\"}")]
    [Arguments("{\" \":\"valid\"}")]
    [Arguments("{\"valid\":\"\"}")]
    [Arguments("{\"valid\":null}")]
    public async Task Invalid_Dictionary_Identifiers_Are_Rejected(string json)
    {
        await Assert.That(() => JsonSerializer.Deserialize(json, ConsumerJsonContext.Default.ModuleIdDictionary))
            .Throws<JsonException>();
    }

    [Test]
    public async Task Default_Identifier_Cannot_Be_Written()
    {
        await Assert.That(() => JsonSerializer.Serialize(default(ModuleId), ConsumerJsonContext.Default.ModuleId))
            .Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Serialize(
                new Dictionary<ModuleId, ModuleId> { [default] = new("valid") },
                ConsumerJsonContext.Default.ModuleIdDictionary))
            .Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Serialize(
                new Dictionary<ModuleId, ModuleId> { [new("valid")] = default },
                ConsumerJsonContext.Default.ModuleIdDictionary))
            .Throws<JsonException>();
    }
}

// This assembly deliberately has no InternalsVisibleTo access to ModularPipelines.
[JsonSerializable(typeof(ModuleId))]
[JsonSerializable(typeof(Dictionary<ModuleId, ModuleId>), TypeInfoPropertyName = "ModuleIdDictionary")]
internal partial class ConsumerJsonContext : JsonSerializerContext;
