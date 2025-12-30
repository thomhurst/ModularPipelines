using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using static ModularPipelines.UnitTests.Helpers.SerializationTestModels;

namespace ModularPipelines.UnitTests.Helpers;

public class JsonTests : TestBase
{
    [Test]
    public async Task Can_Serialize()
    {
        var json = await GetService<IJson>();

        var result = json.ToJson(SerializationTestModel.CreateDefault());
        await Assert.That(result).IsEqualTo($@"{{""Foo"":""{TestValues.FooValue}"",""Hello"":""{TestValues.HelloValue}""}}");
    }

    [Test]
    public async Task Can_Serialize_With_Options()
    {
        var json = await GetService<IJson>();

        var result = json.ToJson(SerializationTestModel.CreateDefault(), new JsonSerializerOptions
        {
            WriteIndented = true,
        });
        // Normalize line endings to handle differences between platforms
        var expected = $"{{\n  \"Foo\": \"{TestValues.FooValue}\",\n  \"Hello\": \"{TestValues.HelloValue}\"\n}}";
        await Assert.That(result.ReplaceLineEndings("\n")).IsEqualTo(expected);
    }

    [Test]
    public async Task Can_Deserialize()
    {
        var json = await GetService<IJson>();

        var result = json.FromJson<SerializationTestModel>($@"{{""Foo"":""{TestValues.FooValue}"",""Hello"":""{TestValues.HelloValue}""}}");
        await Assert.That(result).IsEqualTo(SerializationTestModel.CreateDefault());
    }

    [Test]
    public async Task Can_Deserialize_With_Options()
    {
        var json = await GetService<IJson>();

        var result = json.FromJson<SerializationTestModel>($$"""
                                              {
                                                "foo": "{{TestValues.FooValue}}",
                                                "hello": "{{TestValues.HelloValue}}"
                                              }
                                              """, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        await Assert.That(result).IsEqualTo(SerializationTestModel.CreateDefault());
    }
}