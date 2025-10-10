using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosylnHello.Interfaces
{
    public interface IRefactorRule
    {
        SyntaxNode Apply(SyntaxNode root);
        string Description { get; }
    }
}
