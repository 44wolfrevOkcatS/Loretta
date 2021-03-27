using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Loretta.Generators.SyntaxKindGenerators
{
    public sealed partial class SyntaxKindRelatedTypesGenerator
    {
        private static void GenerateSyntaxFacts(GeneratorExecutionContext context, INamedTypeSymbol syntaxKindType, KindList kinds)
        {
            SourceText sourceText;
            using (var writer = new SourceWriter())
            {
                writer.WriteLine("// <auto-generated />");
                writer.WriteLine();
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using System.Collections.Immutable;");
                writer.WriteLine("using System.Diagnostics.CodeAnalysis;");
                writer.WriteLine("using Tsu;");
                writer.WriteLine();
                writer.WriteLine("#nullable enable");
                writer.WriteLine();

                using (writer.CurlyIndenter("namespace Loretta.CodeAnalysis.Lua"))
                using (writer.CurlyIndenter("public static partial class SyntaxFacts"))
                {
                    GenerateMinMaxLength(kinds.Tokens.Concat(kinds.Keywords), writer, "Token");
                    GenerateMinMaxLength(kinds.Tokens, writer, "NonKeywordToken");
                    GenerateMinMaxLength(kinds.Keywords, writer, "Keyword");
                    GenerateMinMaxLength(kinds.UnaryOperators, writer, "UnaryOperator");
                    GenerateMinMaxLength(kinds.BinaryOperators, writer, "BinaryOperator");

                    writer.WriteLineNoTabs("");

                    GenerateGetUnaryOperatorPrecedence(kinds, writer);

                    writer.WriteLineNoTabs("");

                    GenerateGetUnaryExpression(kinds, writer);

                    writer.WriteLineNoTabs("");

                    GenerateGetBinaryOperatorPrecedence(kinds, writer);

                    writer.WriteLineNoTabs("");

                    GenerateGetBinaryExpression(kinds, writer);

                    writer.WriteLineNoTabs("");

                    GenerateGetKeywordKind(kinds, writer);

                    writer.WriteLineNoTabs("");

                    GenerateGetUnaryOperatorKinds(kinds, writer);

                    writer.WriteLineNoTabs("");

                    GenerateGetBinaryOperatorKinds(kinds, writer);

                    writer.WriteLineNoTabs("");

                    GenerateGetText(kinds, writer);

                    var properties =
                        kinds.SelectMany(kind => kind.Properties.Select(kv => (kind, key: kv.Key, value: kv.Value)))
                             .GroupBy(t => t.key, t => (t.kind, t.value));
                    foreach (var propertyGroup in properties)
                    {
                        var possibleTypes = propertyGroup.Select(t => t.value.Type).Where(t => t is not null).Distinct().ToImmutableArray();

                        string type;
                        if (possibleTypes.Length > 1)
                            type = context.Compilation.GetSpecialType(SpecialType.System_Object) + "?";
                        else
                            type = possibleTypes.Single()!.ToString();

                        writer.WriteLineNoTabs("");
                        using (new CurlyIndenter(writer, $"public static Option<{type}> Get{propertyGroup.Key}(SyntaxKind kind)"))
                        {
                            var values = propertyGroup.GroupBy(t => t.value, t => t.kind);
                            writer.WriteLine("return kind switch");
                            writer.WriteLine("{");
                            using (new Indenter(writer))
                            {
                                foreach (var value in values)
                                {
                                    writer.Write(string.Join(" or ", value.Select(k => $"SyntaxKind.{k.Field.Name}")));
                                    writer.Write(" => ");
                                    writer.Write(value.Key.ToCSharpString());
                                    writer.WriteLine(",");
                                }
                                writer.WriteLine("_ => default,");
                            }
                            writer.WriteLine("};");
                        }
                    }

                    writer.WriteLineNoTabs("");

                    // Generate IsTrivia
                    GenerateIsX(kinds, writer, "Trivia", kind => kind.IsTrivia);

                    writer.WriteLineNoTabs("");

                    // Generate IsKeyword
                    GenerateIsX(kinds, writer, "Keyword", kind => kind.TokenInfo?.IsKeyword is true);

                    writer.WriteLineNoTabs("");

                    // Generate IsToken
                    GenerateIsX(kinds, writer, "Token", kind => kind.TokenInfo is not null);

                    writer.WriteLineNoTabs("");

                    // Extra Categories
                    var extraCategories = kinds.SelectMany(kind => kind.ExtraCategories.Select(cat => (cat, kind))).GroupBy(t => t.cat, t => t.kind);
                    foreach (var group in extraCategories)
                    {
                        writer.WriteLineNoTabs("");

                        var groupKinds = new KindList(group.ToImmutableArray());
                        GenerateIsX(groupKinds, writer, group.Key, k => true);

                        writer.WriteLineNoTabs("");
                        writer.WriteLine("/// <summary>");
                        writer.WriteLine($"/// Returns all <see cref=\"SyntaxKind\"/>s that are in the {group.Key} category.");
                        writer.WriteLine("/// </summary>");
                        writer.WriteLine("/// <returns></returns>");
                        using (writer.CurlyIndenter($"public static IEnumerable<SyntaxKind> Get{group.Key}Kinds() => ImmutableArray.Create(new[]", ");"))
                        {
                            foreach (var kind in group)
                            {
                                writer.WriteLine($"SyntaxKind.{kind.Field.Name},");
                            }
                        }
                    }
                }

                sourceText = writer.GetText();
            }

            context.AddSource("SyntaxFacts.g.cs", sourceText);
            Utilities.DoVsCodeHack(syntaxKindType, "SyntaxFacts.g.cs", sourceText);
        }

        private static void GenerateMinMaxLength(IEnumerable<KindInfo> kinds, SourceWriter writer, string typeName)
        {
            var filteredKinds = kinds.ToImmutableArray();
            var min = filteredKinds.Min(kind => kind.TokenInfo!.Value.Text!.Length);
            var max = filteredKinds.Max(kind => kind.TokenInfo!.Value.Text!.Length);
            writer.WriteLine($"internal static readonly int Min{typeName}Length = {min};");
            writer.WriteLine($"internal static readonly int Max{typeName}Length = {max};");
        }

        private static void GenerateGetUnaryOperatorPrecedence(KindList kinds, SourceWriter writer)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Returns the precedence for a given unary operator or 0 if not a unary operator.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <param name=\"kind\"></param>");
            writer.WriteLine("/// <returns>");
            writer.WriteLine("/// A positive number indicating the binary operator precedence or 0 if the kind is not a binary operator.");
            writer.WriteLine("/// </returns>");
            using (writer.CurlyIndenter("public static int GetUnaryOperatorPrecedence(SyntaxKind kind)"))
            using (writer.CurlyIndenter("switch(kind)"))
            {
                var groups = kinds.UnaryOperators.GroupBy(kind => kind.UnaryOperatorInfo!.Value.Precedence);

                foreach (var group in groups.OrderByDescending(g => g.Key))
                {
                    foreach (var kind in group.OrderByDescending(info => info.Field.Name))
                    {
                        writer.WriteLine($"case SyntaxKind.{kind.Field.Name}:");
                    }
                    using (writer.Indenter())
                        writer.WriteLine($"return {group.Key};");
                }

                writer.WriteLine("default:");
                using (writer.Indenter())
                    writer.WriteLine("return 0;");
            }
        }

        private static void GenerateGetUnaryExpression(KindList kinds, SourceWriter writer)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Returns the expression kind for a given unary operator or None if not a unary operator.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <param name=\"kind\"></param>");
            writer.WriteLine("/// <returns>");
            writer.WriteLine("/// A positive number indicating the binary operator precedence or 0 if the kind is not a binary operator.");
            writer.WriteLine("/// </returns>");
            using (writer.Indenter("public static Option<SyntaxKind> GetUnaryExpression(SyntaxKind kind) =>"))
            using (writer.CurlyIndenter("kind switch", ";"))
            {
                foreach (var unaryOperator in kinds.UnaryOperators)
                {
                    writer.WriteLine($"SyntaxKind.{unaryOperator.Field.Name} => {unaryOperator.UnaryOperatorInfo!.Value.Expression.ToCSharpString()},");
                }
                writer.WriteLine("_ => default,");
            }
        }

        private static void GenerateGetBinaryOperatorPrecedence(KindList kinds, SourceWriter writer)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Returns the precedence for a given binary operator. Returns 0 if kind is not a binary operator.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <param name=\"kind\"></param>");
            writer.WriteLine("/// <returns>");
            writer.WriteLine("/// A positive number indicating the binary operator precedence or 0 if the kind is not a binary operator.");
            writer.WriteLine("/// </returns>");
            using (writer.CurlyIndenter("public static int GetBinaryOperatorPrecedence(SyntaxKind kind)"))
            using (writer.CurlyIndenter("switch(kind)"))
            {
                var groups = kinds.BinaryOperators.GroupBy(kind => kind.BinaryOperatorInfo!.Value.Precedence);

                foreach (var group in groups.OrderByDescending(g => g.Key))
                {
                    foreach (var kind in group.OrderByDescending(info => info.Field.Name))
                    {
                        writer.WriteLine($"case SyntaxKind.{kind.Field.Name}:");
                    }
                    using (new Indenter(writer))
                        writer.WriteLine($"return {group.Key};");

                    writer.WriteLineNoTabs("");
                }

                using (writer.Indenter("default:"))
                    writer.WriteLine("return 0;");
            }
        }

        private static void GenerateGetBinaryExpression(KindList kinds, SourceWriter writer)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Returns the expression kind for a given unary operator or None if not a unary operator.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <param name=\"kind\"></param>");
            writer.WriteLine("/// <returns>");
            writer.WriteLine("/// A positive number indicating the binary operator precedence or 0 if the kind is not a binary operator.");
            writer.WriteLine("/// </returns>");
            using (writer.Indenter("public static Option<SyntaxKind> GetBinaryExpression(SyntaxKind kind) =>"))
            using (writer.CurlyIndenter("kind switch", ";"))
            {
                foreach (var binaryOperator in kinds.BinaryOperators)
                {
                    writer.WriteLine($"SyntaxKind.{binaryOperator.Field.Name} => {binaryOperator.BinaryOperatorInfo!.Value.Expression.ToCSharpString()},");
                }
                writer.WriteLine("_ => default,");
            }
        }

        private static void GenerateGetKeywordKind(KindList kinds, SourceWriter writer)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Returns the <see cref=\"SyntaxKind\"/> for a given keyword.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <param name=\"text\"></param>");
            writer.WriteLine("/// <returns></returns>");
            using (writer.Indenter("public static SyntaxKind GetKeywordKind(String text) =>"))
            using (writer.CurlyIndenter("text switch", ";"))
            {
                foreach (var keyword in kinds.Keywords.OrderBy(kind => kind.Field.Name))
                {
                    writer.WriteLine($"\"{keyword.TokenInfo!.Value.Text}\" => SyntaxKind.{keyword.Field.Name},");
                }
                writer.WriteLine($"_ => SyntaxKind.IdentifierToken,");
            }
        }

        private static void GenerateGetUnaryOperatorKinds(KindList kinds, SourceWriter writer)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Returns all <see cref=\"SyntaxKind\"/>s that can be considered unary operators.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <returns></returns>");
            using (writer.CurlyIndenter("public static IEnumerable<SyntaxKind> GetUnaryOperatorKinds() => ImmutableArray.Create(new[]", ");"))
            {
                foreach (var unaryOperator in kinds.UnaryOperators.OrderBy(unaryOp => unaryOp.Field.Name))
                {
                    writer.WriteLine($"SyntaxKind.{unaryOperator.Field.Name},");
                }
            }
        }

        private static void GenerateGetBinaryOperatorKinds(KindList kinds, SourceWriter writer)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Returns all <see cref=\"SyntaxKind\"/>s that can be considered binary operators.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <returns></returns>");
            using (writer.CurlyIndenter("public static IEnumerable<SyntaxKind> GetBinaryOperatorKinds() => ImmutableArray.Create(new[]", ");"))
            {
                foreach (var binaryOperator in kinds.BinaryOperators.OrderBy(binaryOp => binaryOp.Field.Name))
                {
                    writer.WriteLine($"SyntaxKind.{binaryOperator.Field.Name},");
                }
            }
        }

        private static void GenerateGetText(KindList kinds, SourceWriter writer)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Gets the predefined text that corresponds to the provided syntax kind.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <param name=\"kind\">The kind to obtain the text for.</param>");
            writer.WriteLine("/// <returns>The text corresponding to the provided kind or <see cref=\"string.Emtpy\" /> if none.</returns>");
            using (writer.Indenter("public static string GetText (SyntaxKind kind) =>"))
            using (writer.CurlyIndenter("kind switch", ";"))
            {
                writer.WriteLine("#region Tokens");
                writer.WriteLineNoTabs("");
                foreach (var token in kinds.Tokens.OrderBy(tok => tok.Field.Name))
                {
                    writer.WriteLine($"SyntaxKind.{token.Field.Name} => \"{token.TokenInfo!.Value.Text}\",");
                }
                writer.WriteLineNoTabs("");
                writer.WriteLine("#endregion Tokens");

                writer.WriteLine("#region Keywords");
                writer.WriteLineNoTabs("");
                foreach (var keyword in kinds.Keywords.OrderBy(kw => kw.Field.Name))
                {
                    writer.WriteLine($"SyntaxKind.{keyword.Field.Name} => \"{keyword.TokenInfo!.Value.Text}\",");
                }
                writer.WriteLineNoTabs("");
                writer.WriteLine("#endregion Keywords");

                writer.WriteLine("_ => string.Empty,");
            }
        }

        private static void GenerateIsX(KindList kinds, SourceWriter writer, string typeName, Func<KindInfo, bool> filter)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine($"/// Checks whether the provided <see cref=\"SyntaxKind\"/> is a {typeName.ToLower()}'s.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine("/// <param name=\"kind\"></param>");
            writer.WriteLine("/// <returns></returns>");
            using (writer.CurlyIndenter($"public static bool Is{typeName}(SyntaxKind kind)"))
            using (writer.CurlyIndenter("switch(kind)"))
            {
                var filteredKinds = kinds.Where(filter);
                foreach (var keyword in filteredKinds.OrderBy(kw => kw.Field.Name))
                    writer.WriteLine($"case SyntaxKind.{keyword.Field.Name}:");
                using (writer.Indenter())
                    writer.WriteLine("return true;");
                writer.WriteLineNoTabs("");

                using (writer.Indenter("default:"))
                    writer.WriteLine("return false;");
            }
        }
    }
}
