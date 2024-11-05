using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light_IDE.Other
{
    /// <summary>
    /// 智能提示预选词库
    /// </summary>
    public class CodeSuggestion
    {
        public List<string> javaCodeSug;
        public List<string> pyCodeSug;
        public List<string> CCodeSug;
        
        /// <summary>
        /// 想到就往里边加
        /// </summary>
        public CodeSuggestion() {
            javaCodeSug = new List<string> {
                "abstract",
                "boolean",
                "break",
                "byte",
                "case",
                "catch",
                "char",
                "class",
                "continue",
                "default",
                "do",
                "double",
                "else",
                "enum",
                "extends",
                "final",
                "finally",
                "float",
                "for",
                "if",
                "implements",
                "import",
                "int",
                "interface",
                "long",
                "native",
                "new",
                "package",
                "private",
                "protected",
                "public",
                "return",
                "short",
                "static",
                "super",
                "switch",
                "this",
                "throw",
                "try",
                "void",
                "while",

                "true",
                "false",
                "null",
            };
            pyCodeSug = new List<string>
            {
                "and",
                "or",
                "not",
                "if",
                "elif",
                "else",
                "for",
                "while",
                "True",
                "try",
                "return",
                "pass",
                "import",
                "from",
                "except",
                "finally",
                "in",
                "continue",
                "def",
                "break",
                "class",
                "del",
                "print()",
                "range()",
                "len()",
                "lower()",
                "upper()",
                "abs()",
                "max()",
                "min()",
                "input()",

            };
            CCodeSug = new List<string> {
            "void",
            "char",
            "int",
            "float",
            "double",
            "short",
            "long",
            "signed",
            "unsigned",
            "struct",
            "union",
            "enum",
            "typedef",
            "sizeof",
            "static",
            "const",
            "return",
            "continue",
            "break",
            "goto",
            "if",
            "else",
            "switch",
            "case",
            "default",
            "for",
            "do",
            "while",
            };
        }
    }
}
