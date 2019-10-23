using System;

namespace FrameworkHelpers.Utilities.Models
{
    internal class AppConfigSource
    {
        public Func<string, bool> CanHandle { get; set; }
        public Func<string, string> Value { get; set; }    
    }
}
