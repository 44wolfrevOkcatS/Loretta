﻿using System;
using Tsu;

namespace Loretta.CodeAnalysis.Syntax
{
    /// <summary>
    /// The base class for statements.
    /// </summary>
    public abstract class StatementSyntax : SyntaxNode
    {
        private protected StatementSyntax ( Option<SyntaxToken> semicolonToken )
        {
            this.SemicolonToken = semicolonToken;
        }

        /// <summary>
        /// The semicolon at the end of the statement (if any).
        /// </summary>
        public Option<SyntaxToken> SemicolonToken { get; }
    }
}