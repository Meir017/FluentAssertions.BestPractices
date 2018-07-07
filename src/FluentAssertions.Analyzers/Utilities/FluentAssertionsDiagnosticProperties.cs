using System.Collections.Immutable;

namespace FluentAssertions.Analyzers
{

    public class FluentAssertionsDiagnosticProperties
    {
        public ImmutableDictionary<string, string> Properties { get; }

        public FluentAssertionsDiagnosticProperties(ImmutableDictionary<string, string> properties)
        {
            Properties = properties;
        }
        
        public string VisitorName => GetPropertyOrDefault(Constants.DiagnosticProperties.VisitorName);
        
        private string GetPropertyOrDefault(string key) => Properties.TryGetValue(key, out var value) ? value : string.Empty;
    }
}
