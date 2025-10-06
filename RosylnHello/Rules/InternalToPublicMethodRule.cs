using Microsoft.CodeAnalysis;
using RosylnHello.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosylnHello.Rules
{
    internal class InternalToPublicMethodRule : IRefactorRule
    {
        public string Description => throw new NotImplementedException();

        public SyntaxNode Apply(SyntaxNode root)
        {
            throw new NotImplementedException();
        }
    }
}
