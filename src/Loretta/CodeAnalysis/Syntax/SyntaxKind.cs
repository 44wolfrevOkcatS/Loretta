﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member (still not sure of how to document this)

namespace Loretta.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        BadToken,

        // Trivia
        [Trivia]
        ShebangTrivia,
        [Trivia]
        SingleLineCommentTrivia,
        [Trivia]
        MultiLineCommentTrivia,
        [Trivia]
        WhitespaceTrivia,
        [Trivia]
        LineBreakTrivia,
        [Trivia]
        SkippedTextTrivia,

        // Tokens
        [Token]
        EndOfFileToken,
        [Token]
        NumberToken,
        [Token]
        ShortStringToken,
        [Token]
        LongStringToken,
        [Token]
        IdentifierToken,
        [Token ( Text = "(" )]
        OpenParenthesisToken,
        [Token ( Text = ")" )]
        CloseParenthesisToken,
        [Token ( Text = "[" )]
        OpenBracketToken,
        [Token ( Text = "]" )]
        CloseBracketToken,
        [Token ( Text = "{" )]
        OpenBraceToken,
        [Token ( Text = "}" )]
        CloseBraceToken,
        [Token ( Text = ";" )]
        SemicolonToken,
        [Token ( Text = ":" )]
        ColonToken,
        [Token ( Text = "," )]
        CommaToken,
        [Token ( Text = "#" )]
        [UnaryOperator ( precedence: 7 )]
        HashToken,
        [Token ( Text = "+" )]
        [BinaryOperator ( precedence: 5 )]
        PlusToken,
        [Token ( Text = "+=" )]
        PlusEqualsToken,
        [Token ( Text = "-" )]
        [UnaryOperator ( precedence: 7 ), BinaryOperator ( precedence: 5 )]
        MinusToken,
        [Token ( Text = "-=" )]
        MinusEqualsToken,
        [Token ( Text = "*" )]
        [BinaryOperator ( precedence: 6 )]
        StarToken,
        [Token ( Text = "*=" )]
        StartEqualsToken,
        [Token ( Text = "/" )]
        [BinaryOperator ( precedence: 6 )]
        SlashToken,
        [Token ( Text = "/=" )]
        SlashEqualsToken,
        [Token ( Text = "^" )]
        [BinaryOperator ( precedence: 8 )]
        HatToken,
        [Token ( Text = "^=" )]
        HatEqualsToken,
        [Token ( Text = "%" )]
        [BinaryOperator ( precedence: 6 )]
        PercentToken,
        [Token ( Text = "%=" )]
        PercentEqualsToken,
        [Token ( Text = "." )]
        DotToken,
        [Token ( Text = ".." )]
        [BinaryOperator ( precedence: 4 )]
        DotDotToken,
        [Token ( Text = "..." )]
        DotDotDotToken,
        [Token ( Text = "..=" )]
        DotDotEqualsToken,
        [Token ( Text = "=" )]
        EqualsToken,
        [Token ( Text = "==" )]
        [BinaryOperator ( precedence: 3 )]
        EqualsEqualsToken,
        // TODO: Add tilde token and unary operator
        [Token ( Text = "~=" )]
        [BinaryOperator ( precedence: 3 )]
        TildeEqualsToken,
        [Token ( Text = "!" )]
        [UnaryOperator ( precedence: 7 )]
        BangToken,
        [Token ( Text = "!=" )]
        [BinaryOperator ( precedence: 3 )]
        BangEqualsToken,
        [Token ( Text = "<" )]
        [BinaryOperator ( precedence: 3 )]
        LessThanToken,
        [Token ( Text = "<=" )]
        [BinaryOperator ( precedence: 3 )]
        LessThanEqualsToken,
        [Token ( Text = "<<" )]
        // TODO: Add binary operator info
        LessThanLessThanToken,
        [Token ( Text = ">" )]
        [BinaryOperator ( precedence: 3 )]
        GreaterThanToken,
        [Token ( Text = ">=" )]
        [BinaryOperator ( precedence: 3 )]
        GreaterThanEqualsToken,
        [Token ( Text = ">>" )]
        // TODO: Add binary operator info
        GreaterThanGreaterThanToken,
        [Token ( Text = "&" )]
        // TODO: Add binary operator info
        AmpersandToken,
        [Token ( Text = "&&" )]
        [BinaryOperator ( precedence: 2 )]
        AmpersandAmpersandToken,
        [Token ( Text = "|" )]
        // TODO: Add binary operator info
        PipeToken,
        [Token ( Text = "||" )]
        [BinaryOperator ( precedence: 1 )]
        PipePipeToken,
        [Token ( Text = "::" )]
        ColonColonToken,

        // Keywords
        [Keyword ( "do" )]
        DoKeyword,
        [Keyword ( "end" )]
        EndKeyword,
        [Keyword ( "while" )]
        WhileKeyword,
        [Keyword ( "repeat" )]
        RepeatKeyword,
        [Keyword ( "until" )]
        UntilKeyword,
        [Keyword ( "if" )]
        IfKeyword,
        [Keyword ( "then" )]
        ThenKeyword,
        [Keyword ( "elseif" )]
        ElseIfKeyword,
        [Keyword ( "else" )]
        ElseKeyword,
        [Keyword ( "for" )]
        ForKeyword,
        [Keyword ( "in" )]
        InKeyword,
        [Keyword ( "function" )]
        FunctionKeyword,
        [Keyword ( "local" )]
        LocalKeyword,
        [Keyword ( "return" )]
        ReturnKeyword,
        [Keyword ( "break" )]
        BreakKeyword,
        [Keyword ( "goto" )]
        GotoKeyword,
        [Keyword ( "continue" )]
        ContinueKeyword,
        [Keyword ( "and" )]
        [BinaryOperator ( precedence: 2 )]
        AndKeyword,
        [Keyword ( "or" )]
        [BinaryOperator ( precedence: 1 )]
        OrKeyword,
        [Keyword ( "not" )]
        [UnaryOperator ( precedence: 7 )]
        NotKeyword,
        [Keyword ( "nil" )]
        NilKeyword,
        [Keyword ( "true" )]
        TrueKeyword,
        [Keyword ( "false" )]
        FalseKeyword,

        // Nodes
        [Node ( typeof ( CompilationUnitSyntax ) )]
        CompilationUnit,
        [Node ( typeof ( ParenthesizedExpressionSyntax ) )]
        ParenthesizedExpression,
        [Node ( typeof ( NumberLiteralExpressionSyntax ) )]
        NumberLiteralExpression,
        [Node ( typeof ( StringLiteralExpressionSyntax ) )]
        StringLiteralExpression,
        [Node ( typeof ( NilLiteralExpressionSyntax ) )]
        NilLiteralExpression,
        [Node ( typeof ( BooleanLiteralExpressionSyntax ) )]
        BooleanLiteralExpression,
        [Node ( typeof ( VarArgExpressionSyntax ) )]
        VarArgExpression,
        [Node ( typeof ( UnaryExpressionSyntax ) )]
        UnaryExpression,
        [Node ( typeof ( BinaryExpressionSyntax ) )]
        BinaryExpression,
        [Node ( typeof ( ParameterListSyntax ) )]
        ParameterList,
        [Node ( typeof ( VarArgParameterSyntax ) )]
        VarArgParameter,
        [Node ( typeof ( NameExpressionSyntax ) )]
        NameExpression,
        [Node ( typeof ( MemberAccessExpressionSyntax ) )]
        MemberAccessExpression,
        [Node ( typeof ( ElementAccessExpressionSyntax ) )]
        ElementAccessExpression,
        [Node ( typeof ( StringFunctionArgumentSyntax ) )]
        StringFunctionArgument,
        [Node ( typeof ( IdentifierKeyedTableFieldSyntax ) )]
        IdentifierKeyedTableField,
        [Node ( typeof ( ExpressionKeyedTableFieldSyntax ) )]
        ExpressionKeyedTableField,
        [Node ( typeof ( UnkeyedTableFieldSyntax ) )]
        UnkeyedTableField,
        [Node ( typeof ( TableConstructorExpressionSyntax ) )]
        TableConstructorExpression,
        [Node ( typeof ( TableConstructorFunctionArgumentSyntax ) )]
        TableConstructorFunctionArgument,
        [Node ( typeof ( FunctionArgumentListSyntax ) )]
        FunctionArgumentList,
        [Node ( typeof ( MethodCallExpressionSyntax ) )]
        MethodCallExpression,
        [Node ( typeof ( FunctionCallExpressionSyntax ) )]
        FunctionCallExpression,
        [Node ( typeof ( NamedParameterSyntax ) )]
        NamedParameter,
    }
}
