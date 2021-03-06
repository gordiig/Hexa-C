﻿using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MiniC.Generators;
using MiniC.Exceptions;
using MiniC.Scopes;
using static MiniC.MiniCParser;

namespace MiniC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Order matters
            SymbolType.AddTypeRange("void", "char", "int", "float");
            
            string filename = "../../../code.c";

            using (StreamReader file = new StreamReader(filename))
            {
                AntlrInputStream inputStream = new AntlrInputStream(file.ReadToEnd());

                MiniCLexer miniCLexer = new MiniCLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(miniCLexer);

                MiniCParser miniCParser = new MiniCParser(commonTokenStream);

                SyntaxErrorListener syntaxErrorListener = new SyntaxErrorListener();
                miniCParser.AddErrorListener(syntaxErrorListener);

                CompilationUnitContext tree = miniCParser.compilationUnit();
                if (miniCParser.NumberOfSyntaxErrors != 0)
                {
                    foreach (var error in syntaxErrorListener.ErrorMessages)
                    {
                        Console.WriteLine($"{filename} | Syntax error:  {error}");
                    }

                    Console.ReadKey();
                    return;
                }

                ParseTreeWalker walker = new ParseTreeWalker();
                SymbolTableSemanticListener semantic = new SymbolTableSemanticListener();
                try
                {
                    walker.Walk(semantic, tree);
                }
                catch (SemanticException e)
                {
                    Console.WriteLine($"{filename} | Semantic error:  {e.Message}");

                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("All ok!");
            Console.ReadKey();
        }
    }
}
