using System.Text.Json;
using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Helpers;

public class JsonTests : TestBase
{
    [Test]
    public async Task Can_Serialize()
    {
        var json = await GetService<IJson>();

        var result = json.ToJson(new JsonModel { Foo = "Bar!", Hello = "World!" });

        Assert.That(result, Is.EqualTo("""
                                       {"Foo":"Bar!","Hello":"World!"}
                                       """));
    }

    [Test]
    public async Task Can_Serialize_With_Options()
    {
        var json = await GetService<IJson>();

        var result = json.ToJson(new JsonModel { Foo = "Bar!", Hello = "World!" }, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        Assert.That(result, Is.EqualTo("""
                                       {
                                         "Foo": "Bar!",
                                         "Hello": "World!"
                                       }
                                       """));
    }

    [Test]
    public async Task Can_Deserialize()
    {
        var json = await GetService<IJson>();

        var result = json.FromJson<JsonModel>("""
                                              {"Foo":"Bar!","Hello":"World!"}
                                              """);

        Assert.That(result, Is.EqualTo(new JsonModel { Foo = "Bar!", Hello = "World!" }));
    }

    [Test]
    public async Task Can_Deserialize_With_Options()
    {
        var json = await GetService<IJson>();

        var result = json.FromJson<JsonModel>("""
                                              {
                                                "foo": "Bar!",
                                                "hello": "World!"
                                              }
                                              """, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        Assert.That(result, Is.EqualTo(new JsonModel { Foo = "Bar!", Hello = "World!" }));
    }

    private record JsonModel
    {
        public string? Foo { get; set; }

        public string? Hello { get; set; }
    }
}