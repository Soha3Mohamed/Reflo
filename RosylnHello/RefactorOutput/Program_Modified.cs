using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RosylnHello
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region Parse a string of c# code to tree of nodes and printing it
            //    Console.WriteLine("Hello, World!");
            //    // 1) Some sample C# code as text
            //    string code = @"
            //public class Person 
            //{
            //    public string Name;
            //    public void SayHello() 
            //    {
            //        Console.WriteLine(""Hello "" + Name);
            //    }
            //}";

            //    // 2) Parse the code into a SyntaxTree
            //    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

            //    // 3) Get the root node (the top of the tree)
            //    SyntaxNode root = tree.GetRoot();

            //    // 4) Print the syntax tree
            //    Console.WriteLine(root.ToFullString());

            //    // Extra: Print the structure (tree format)
            //    Console.WriteLine("\n--- Tree Structure ---");
            //    Console.WriteLine(root);
            //    foreach (var node in root.DescendantNodes())
            //    {
            //        Console.WriteLine($"{node.Kind()} --> {node}");
            //    }

            #endregion

            #region Filtering the tree and focusing on classes only and printing its names and modifiers

            //    string code = @"
            //public class Person 
            //{
            //    public string Name;
            //    public void SayHello() 
            //    {
            //        Console.WriteLine(""Hello "" + Name);
            //    }
            //}

            //internal class Car
            //{
            //    public string Model;
            //}";

            //    // Parse the code into a tree
            //    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

            //    // Get the root node
            //    var root = tree.GetRoot();

            //    // Step 1: Find only the class declarations
            //    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            //    Console.WriteLine("Classes I found:");
            //    foreach (var cls in classes)
            //    {
            //        Console.WriteLine($"- {cls.Identifier.Text} (modifier: {string.Join(", ", cls.Modifiers)})");
            //    }

            #endregion

            //till now i was parsing pre-written string that is a c# code but the next region is parsing a real .cs file

            #region Parsing a Real .cs file

            //var filePath = @"C:\Users\soha mohamed\source\repos\RosylnHello\RosylnHello\Employee.cs";
            //var codeInFile = File.ReadAllText(filePath);

            //SyntaxTree tree = CSharpSyntaxTree.ParseText(codeInFile);

            //var root = tree.GetRoot();
            //var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            //Console.WriteLine("Classes I found:");

            ////this line filters only the internal classes by checking if any of the modifiers of the class is internal
            ////iskind is a method that checks if the syntax kind of the modifier is internal keyword
            //var InternalClasses = classes.Where(c => c.Modifiers.Any(m => m.IsKind(SyntaxKind.InternalKeyword))).ToList();
            //foreach (var cls in InternalClasses)
            //{

            //    Console.WriteLine($"- {cls.Identifier.Text} (modifier: {string.Join(", ", cls.Modifiers)})");
            //}


            #endregion

            #region A Whole Solution or Project

            //string projectPath = @"C:\Users\soha mohamed\source\repos\Inventra";

            //foreach (var file in Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories))
            //{
            //    string code = File.ReadAllText(file);
            //    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            //    var root = tree.GetRoot();

            //    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            //    foreach (var cls in classes)
            //    {
            //        Console.WriteLine($"{Path.GetFileName(file)} -> {cls.Identifier.Text} ({string.Join(", ", cls.Modifiers)})");
            //    }
            //}
            #endregion
                string projectPath = @"C:\Users\soha mohamed\source\repos\RosylnHello";

                foreach (var file in Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories))
                {
                    // Skip auto-generated or designer files
                    if (file.Contains("GlobalUsings.g.cs") || file.Contains(".Designer.cs"))
                        continue;

                    string code = File.ReadAllText(file);
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                    var root = tree.GetRoot();

                    // Get all class declarations
                    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                    var newRoot = root;
                    int count = 0;

                    foreach (var cls in classes)
                    {
                        // Check if class is internal
                        if (cls.Modifiers.Any(m => m.IsKind(SyntaxKind.InternalKeyword)))
                        {
                            // Create new modifier list: replace internal → public
                            var newModifiers = SyntaxFactory.TokenList(
                                cls.Modifiers.Select(m =>
                                    m.IsKind(SyntaxKind.InternalKeyword)
                                        ? SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTriviaFrom(m)
                                        : m
                                ));

                            // Create new class node with updated modifiers
                            var newClass = cls.WithModifiers(newModifiers);

                            // Replace old class node with new one
                            newRoot = newRoot.ReplaceNode(cls, newClass);
                            count++;

                            Console.WriteLine($"Changed class: {cls.Identifier.Text} in {Path.GetFileName(file)}");
                        }
                    }

                    // Only write a new file if any classes were changed
                    if (count > 0)
                    {
                        var newFilePath = Path.Combine(
                            Path.GetDirectoryName(file)! + "/RefactorOutput/",
                            Path.GetFileNameWithoutExtension(file) + "_Modified.cs"
                        );

                        File.WriteAllText(newFilePath, newRoot.ToFullString());
                        Console.WriteLine($"✔ Modified file saved: {Path.GetFileName(newFilePath)}");
                    }
                }

                Console.WriteLine("\nAll done! 🚀");
            }
        }
    }

