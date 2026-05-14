namespace ModularPipelines.UnitTests.FSharp.Dependencies

open System
open System.Collections.Generic
open System.Reflection
open ModularPipelines.Context

/// <summary>
/// A mock implementation of <see cref="IDependencyContext"/> for unit testing dependency attributes.
/// </summary>
type MockDependencyContext() =
    let tags = Dictionary<Type, HashSet<string>>()
    let categories = Dictionary<Type, string>()

    /// <summary>
    /// Configures tags for a specific module type.
    /// </summary>
    /// <param name="type">The module type to configure tags for.</param>
    /// <param name="tagArray">The tags to associate with the module type.</param>
    /// <returns>This instance for method chaining.</returns>
    member this.WithTags(moduleType: Type, [<ParamArray>] tagArray: string[]) =
        tags[moduleType] <- HashSet<string>(tagArray, StringComparer.OrdinalIgnoreCase)
        this

    /// <summary>
    /// Configures the category for a specific module type.
    /// </summary>
    /// <param name="type">The module type to configure the category for.</param>
    /// <param name="category">The category to associate with the module type.</param>
    /// <returns>This instance for method chaining.</returns>
    member this.WithCategory(moduleType: Type, category: string) =
        categories[moduleType] <- category
        this

    interface IDependencyContext with
        member _.GetTags(moduleType: Type) =
            match tags.TryGetValue(moduleType) with
            | true, t -> t :> IReadOnlySet<string>
            | false, _ -> HashSet<string>() :> IReadOnlySet<string>

        member _.GetCategory(moduleType: Type) =
            match categories.TryGetValue(moduleType) with
            | true, cat -> cat
            | false, _ -> null

        member _.HasAttribute<'TAttribute when 'TAttribute :> Attribute>(moduleType: Type) =
            moduleType.GetCustomAttributes(typeof<'TAttribute>, true).Length > 0

        member _.GetAttribute<'TAttribute when 'TAttribute :> Attribute>(moduleType: Type) =
            moduleType.GetCustomAttribute<'TAttribute>()

        member _.GetAttributes<'TAttribute when 'TAttribute :> Attribute>(moduleType: Type) =
            moduleType.GetCustomAttributes<'TAttribute>()
