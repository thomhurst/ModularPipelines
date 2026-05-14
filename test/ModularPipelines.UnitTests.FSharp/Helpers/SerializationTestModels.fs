namespace ModularPipelines.UnitTests.FSharp.Helpers

open System.Collections.Generic
open System.Text.Json.Serialization

/// <summary>
/// Shared test models for serialization tests (JSON, XML, YAML).
/// Reduces duplication of similar model definitions across test files.
/// </summary>
module SerializationTestModels =
    /// <summary>
    /// Standard test values used across serialization tests.
    /// </summary>
    module TestValues =
        let FooValue = "Bar!"
        let HelloValue = "World!"
        let ItemsValue = [| "One"; "Two"; "3" |]

    /// <summary>
    /// Base test model with common properties for serialization testing.
    /// </summary>
    [<CLIMutable>]
    type SerializationTestModel =
        {
            Foo: string
            Hello: string
            [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)>]
            Items: List<string>
        }
        
        /// <summary>
        /// Creates a new model with default test values.
        /// </summary>
        static member CreateDefault() =
            { Foo = TestValues.FooValue
              Hello = TestValues.HelloValue
              Items = null }

        /// <summary>
        /// Creates a new model with default test values including items.
        /// </summary>
        static member CreateWithItems() =
            { Foo = TestValues.FooValue
              Hello = TestValues.HelloValue
              Items = List<string>(TestValues.ItemsValue) }
