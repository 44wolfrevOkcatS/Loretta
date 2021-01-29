﻿namespace Loretta.CodeAnalysis.Syntax
{
    /// <summary>
    /// Represents a bad input where an expression was expected.
    /// </summary>
    public sealed partial class BadExpressionSyntax : PrefixExpressionSyntax
    {
        internal BadExpressionSyntax ( SyntaxTree syntaxTree, SyntaxToken token ) : base ( syntaxTree )
        {
            this.Token = token;
        }

        /// <inheritdoc/>
        public override SyntaxKind Kind => SyntaxKind.BadExpression;

        /// <summary>
        /// The token containing the bad input.
        /// </summary>
        public SyntaxToken Token { get; }
    }
}