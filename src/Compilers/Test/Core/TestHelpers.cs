﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Loretta.CodeAnalysis;
using Loretta.CodeAnalysis.Test.Utilities;
using Loretta.CodeAnalysis.Text;
using KeyValuePair = Loretta.Utilities.KeyValuePairUtil;

namespace Loretta.Test.Utilities
{
    public static class TestHelpers
    {
        public static ImmutableDictionary<K, V> CreateImmutableDictionary<K, V>(
            IEqualityComparer<K> comparer,
            params (K, V)[] entries)
            => ImmutableDictionary.CreateRange(comparer, entries.Select(KeyValuePair.ToKeyValuePair));

        public static ImmutableDictionary<K, V> CreateImmutableDictionary<K, V>(params (K, V)[] entries)
            => ImmutableDictionary.CreateRange(entries.Select(KeyValuePair.ToKeyValuePair));

        public static IEnumerable<Type> GetAllTypesWithStaticFieldsImplementingType(Assembly assembly, Type type)
        {
            return assembly.GetTypes().Where(t =>
            {
                return t.GetFields(BindingFlags.Public | BindingFlags.Static).Any(f => type.IsAssignableFrom(f.FieldType));
            }).ToList();
        }

        public static string GetCultureInvariantString(object value)
        {
            if (value == null)
                return null;

            var valueType = value.GetType();
            if (valueType == typeof(string))
            {
                return value as string;
            }

            if (valueType == typeof(DateTime))
            {
                return ((DateTime)value).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            }

            if (valueType == typeof(float))
            {
                return ((float)value).ToString(CultureInfo.InvariantCulture);
            }

            if (valueType == typeof(double))
            {
                return ((double)value).ToString(CultureInfo.InvariantCulture);
            }

            if (valueType == typeof(decimal))
            {
                return ((decimal)value).ToString(CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }

        /// <summary>
        /// <see cref="System.Xml.Linq.XComment.Value"/> is serialized with "--" replaced by "- -"
        /// </summary>
        public static string AsXmlCommentText(string text)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if ((c == '-') && (i > 0) && (text[i - 1] == '-'))
                {
                    builder.Append(' ');
                }
                builder.Append(c);
            }
            var result = builder.ToString();
            Debug.Assert(!result.Contains("--"));
            return result;
        }

        public static DiagnosticDescription Diagnostic(
            object code,
            string squiggledText = null,
            object[] arguments = null,
            LinePosition? startLocation = null,
            Func<SyntaxNode, bool> syntaxNodePredicate = null,
            bool argumentOrderDoesNotMatter = false,
            bool isSuppressed = false)
        {
            Debug.Assert(code is CodeAnalysis.Lua.ErrorCode or int or string);

            return new DiagnosticDescription(
                code as string ?? (object)(int)code,
                false,
                squiggledText,
                arguments,
                startLocation,
                syntaxNodePredicate,
                argumentOrderDoesNotMatter,
                code.GetType(),
                isSuppressed: isSuppressed);
        }

        internal static DiagnosticDescription Diagnostic(
           object code,
           XCData squiggledText,
           object[] arguments = null,
           LinePosition? startLocation = null,
           Func<SyntaxNode, bool> syntaxNodePredicate = null,
           bool argumentOrderDoesNotMatter = false,
           bool isSuppressed = false)
        {
            return Diagnostic(
                code,
                NormalizeNewLines(squiggledText),
                arguments,
                startLocation,
                syntaxNodePredicate,
                argumentOrderDoesNotMatter,
                isSuppressed: isSuppressed);
        }

        public static string NormalizeNewLines(XCData data)
        {
            if (ExecutionConditionUtil.IsWindows)
            {
                return data.Value.Replace("\n", "\r\n");
            }

            return data.Value;
        }
    }
}
