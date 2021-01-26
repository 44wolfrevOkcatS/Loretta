﻿namespace Loretta.CodeAnalysis.Syntax
{
    /// <summary>
    /// Represents a table being passed as a function's arguments.
    /// </summary>
    public sealed partial class TableConstructorFunctionArgumentSyntax : FunctionArgumentSyntax
    {
        internal TableConstructorFunctionArgumentSyntax ( SyntaxTree syntaxTree, TableConstructorExpressionSyntax tableConstructor )
            : base ( syntaxTree )
        {
            this.TableConstructor = tableConstructor;
        }

        /// <inheritdoc/>
        public override SyntaxKind Kind => SyntaxKind.TableConstructorFunctionArgument;

        /// <summary>
        /// The actual table constructor.
        /// </summary>
        public TableConstructorExpressionSyntax TableConstructor { get; }
    }
}