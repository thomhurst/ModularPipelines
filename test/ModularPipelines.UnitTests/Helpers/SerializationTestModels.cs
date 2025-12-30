using System.Text.Json.Serialization;

namespace ModularPipelines.UnitTests.Helpers;

/// <summary>
/// Shared test models for serialization tests (JSON, XML, YAML).
/// Reduces duplication of similar model definitions across test files.
/// </summary>
public static class SerializationTestModels
{
    /// <summary>
    /// Standard test values used across serialization tests.
    /// </summary>
    public static class TestValues
    {
        public const string FooValue = "Bar!";
        public const string HelloValue = "World!";
        public static readonly string[] ItemsValue = ["One", "Two", "3"];
    }

    /// <summary>
    /// Base test model with common properties for serialization testing.
    /// </summary>
    public record SerializationTestModel
    {
        public string? Foo { get; set; }
        public string? Hello { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Items { get; set; }

        /// <summary>
        /// Creates a new model with default test values.
        /// </summary>
        public static SerializationTestModel CreateDefault() =>
            new() { Foo = TestValues.FooValue, Hello = TestValues.HelloValue };

        /// <summary>
        /// Creates a new model with default test values including items.
        /// </summary>
        public static SerializationTestModel CreateWithItems() =>
            new()
            {
                Foo = TestValues.FooValue,
                Hello = TestValues.HelloValue,
                Items = [..TestValues.ItemsValue],
            };
    }
}
