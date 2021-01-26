﻿namespace Loretta.CodeAnalysis.Syntax
{
    /// <summary>
    /// Represents a list of expressions being passed as a function's arguments.
    /// </summary>
    public sealed partial class FunctionArgumentListSyntax : FunctionArgumentSyntax
    {
        internal FunctionArgumentListSyntax (
            SyntaxTree syntaxTree,
            SyntaxToken openParenthesisToken,
            SeparatedSyntaxList<ExpressionSyntax> expressions,
            SyntaxToken closeParenthesisToken )
            : base ( syntaxTree )

        {
            this.OpenParenthesisToken = openParenthesisToken;
            this.Expressions = expressions;
            this.CloseParenthesisToken = closeParenthesisToken;
        }

        /// <inheritdoc/>
        public override SyntaxKind Kind => SyntaxKind.FunctionArgumentList;

        /// <summary>
        /// The argument list's opening parenthesis token.
        /// </summary>
        public SyntaxToken OpenParenthesisToken { get; }

        /// <summary>
        /// The list of expressions.
        /// </summary>
        public SeparatedSyntaxList<ExpressionSyntax> Expressions { get; }

        /// <summary>
        /// The argument list's closing parenthesis token.
        /// </summary>
        public SyntaxToken CloseParenthesisToken { get; }
    }
}