// using System.Runtime.CompilerServices;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Pipeline.NET.Modules;
//
// namespace Pipeline.NET.SourceGenerator;
//
// public class DependencyTrackingSourceGenerator : IIncrementalGenerator
// {
//     public void Initialize(IncrementalGeneratorInitializationContext context)
//     {
//         var oneOfClasses = context.SyntaxProvider
//             .CreateSyntaxProvider(
//                 predicate: static (s, _) => IsGetModuleMethodCall(s), 
//                 transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
//             .Where(static m => m is not null)
//             .Collect();
//
//         context.RegisterSourceOutput(oneOfClasses, Execute);
//
//
//         static bool IsGetModuleMethodCall(SyntaxNode node)
//         {
//             return node is InvocationExpressionSyntax
//             {
//                 Expression: IdentifierNameSyntax
//                 {
//                     Identifier.ValueText: nameof(Module.GetModule) or nameof(Module.WaitForModule)
//                 }
//             };
//         }
//
//         static INamedTypeSymbol? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
//         {
//             ISymbol? symbol = context.SemanticModel.GetDeclaredSymbol(context.Node);
//
//             
//             
//             if (context.Node is AttributeSyntax attributeSyntax)
//             {
//                 attributeSyntax.ArgumentList.Arguments.First().
//                 return symbol!.ContainingType;
//             }
//             
//             
//             
//             if (symbol is not IAttribute namedTypeSymbol)
//             {
//                 return null;
//             }
//                 
//             var attributeData = namedTypeSymbol.GetAttributes().FirstOrDefault(ad =>
//                 string.Equals(ad.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), $"global::{AttributeNamespace}.{AttributeName}"));
//
//             return attributeData is null ? null : namedTypeSymbol;
//         }
// }