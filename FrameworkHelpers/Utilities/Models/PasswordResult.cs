using System.Collections.Generic;

namespace FrameworkHelpers.Utilities.Models
{
    public class PasswordResult
    {
        private static readonly PasswordResult _success = new PasswordResult(true);

        public PasswordResult(params string[] errors)
            : this((IEnumerable<string>)errors)
        {
        }

        public PasswordResult(IEnumerable<string> errors)
        {
            if (errors == null)
                errors = (IEnumerable<string>)new string[1]
                {
                        "DefaultError"
                };
            this.Succeeded = false;
            this.Errors = errors;
        }

        protected PasswordResult(bool success)
        {
            this.Succeeded = success;
            this.Errors = (IEnumerable<string>)new string[0];
        }

        public bool Succeeded { get; private set; }

        public IEnumerable<string> Errors { get; private set; }

        public static PasswordResult Success => PasswordResult._success;

        public static PasswordResult Failed(params string[] errors) => new PasswordResult(errors);
    }
}
