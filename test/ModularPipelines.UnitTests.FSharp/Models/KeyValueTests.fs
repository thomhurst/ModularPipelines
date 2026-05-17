namespace ModularPipelines.UnitTests.FSharp.Models

open System
open System.Collections.Generic
open ModularPipelines.Models
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type KeyValueTests() =
    [<Test>]
    member _.ImplicitOperator1() = async {
        let keyValue: KeyValue = ("one", "two")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(keyValue.Key), "one"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(keyValue.Value), "two"))
    }

    [<Test>]
    member _.ImplicitOperator2() = async {
        let keyValue: KeyValue = Tuple<string, string>("one", "two")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(keyValue.Key), "one"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(keyValue.Value), "two"))
    }

    [<Test>]
    member _.ImplicitOperator3() = async {
        let keyValue: KeyValue = KeyValuePair<string, string>("one", "two")
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(keyValue.Key), "one"))
        do! check(StringEqualsAssertionExtensions.IsEqualTo(Assert.That(keyValue.Value), "two"))
    }
