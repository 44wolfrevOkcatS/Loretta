﻿// The code on this file is based on Roslyn which is distributed under the MIT license.

#nullable disable

namespace Loretta.Generators.SyntaxXml
{
    public class PredefinedNode : TreeType
    {
        public override T Accept<T>(TreeVisitor<T> visitor) => visitor.VisitPredefinedNode(this);
    }
}
