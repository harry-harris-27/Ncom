using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Ncom.Generators.Templates;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Ncom.Generators
{
    [Generator]
    public class StatusChannelGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() =>
                new AttributeSyntaxReceiver<StatusChannelAttribute>());
        }

        public void Execute(GeneratorExecutionContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                //Debugger.Launch();
            }
#endif

            if (context.SyntaxReceiver is not AttributeSyntaxReceiver<StatusChannelAttribute> syntaxReceiver)
            {
                return;
            }

            foreach (var classSyntax in syntaxReceiver.Classes)
            {
                // Converting the class to semantic model to access much more meaningful data.
                var model = context.Compilation.GetSemanticModel(classSyntax.SyntaxTree);

                // Parse to declared symbol, so you can access each part of code separately, such as interfaces, methods, members, contructor parameters etc.
                var symbol = model.GetDeclaredSymbol(classSyntax);

                // Finding the StatusChannelAttribute over it. I'm sure this attribute is placed, because my syntax receiver already checked before.
                // So, I can surely execute following query.
                var attribute = classSyntax.AttributeLists
                    .SelectMany(sm => sm.Attributes)
                    .First(x => x.Name.ToString()
                        .EnsureEndsWith("Attribute")
                        .Equals(typeof(StatusChannelAttribute).Name)
                    );

                // Getting constructor parameter of the attribute
                var attributeConstructorArguments = attribute.ArgumentList.Arguments.First();
                string statusChannelByte = attributeConstructorArguments.GetLastToken().ValueText;
                string generatedFile = "StatusChannel" + statusChannelByte;

                // Can't access embedded resource of main project.
                // So overridden template must be marked as Analyzer Additional File to be able to be accessed by an analyzer.
                var overridenTemplate = context.AdditionalFiles.FirstOrDefault(x =>
                {
                    return x.Path.EndsWith(generatedFile);
                })?.GetText().ToString();

                // Generate the real source code. Pass the template parameter if there is a overriden template.
                var sourceCode = GetSourceCodeFor(symbol, statusChannelByte, String.Empty);

                context.AddSource($"{generatedFile}.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
                Console.WriteLine(classSyntax);
            }
        }

        private string GetSourceCodeFor(ISymbol symbol, string statusChannelByte, string template = null)
        {
            // If template isn't provieded, use default one from embeded resources.
            if (string.IsNullOrWhiteSpace(template))
            {
                template = GetEmbededResource("Ncom.Generators.Templates.StatusChannel.txt");
            }

            // Can't use scriban at the moment, make it manually for now.
            return template
                .Replace("{{" + nameof(StatusChannelTemplateParameters.Namespace) + "}}", GetNamespaceRecursively(symbol.ContainingNamespace))
                .Replace("{{" + nameof(StatusChannelTemplateParameters.ClassName) + "}}", symbol.Name)
                .Replace("{{" + nameof(StatusChannelTemplateParameters.StatusChannelByte) + "}}", statusChannelByte);
        }

        private static string GetEmbededResource(string path)
        {
            using var stream = typeof(StatusChannelGenerator).Assembly.GetManifestResourceStream(path);

            using var streamReader = new StreamReader(stream);

            return streamReader.ReadToEnd();
        }

        private static string GetNamespaceRecursively(INamespaceSymbol symbol)
        {
            if (symbol.ContainingNamespace == null)
            {
                return symbol.Name;
            }

            return (GetNamespaceRecursively(symbol.ContainingNamespace) + "." + symbol.Name).Trim('.');
        }
    }
}
