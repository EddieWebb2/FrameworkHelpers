using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FrameworkHelpers.Utilities.Models;

namespace FrameworkHelpers.Utilities
{
    public class PasswordOptions
    {
        // Minimum required length
        public int RequiredLength { get; set; }

        // Require a non letter or digit character
        public bool RequireNonLetterOrDigit { get; set; }

        // Require a lower case letter ('a' - 'z')
        public bool RequireLowercase { get; set; }

        // Require an upper case letter ('A' - 'Z')
        public bool RequireUppercase { get; set; }

        // Require a digit ('0' - '9')</summary>
        public bool RequireDigit { get; set; }

        public virtual Task<PasswordResult> ValidateAsync(string item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            List<string> stringList = new List<string>();
            if (string.IsNullOrWhiteSpace(item) || item.Length < this.RequiredLength)
                stringList.Add(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "PasswordTooShort", (object)this.RequiredLength));
            if (this.RequireNonLetterOrDigit && item.All<char>(new Func<char, bool>(this.IsLetterOrDigit)))
                stringList.Add("PasswordRequireNonLetterOrDigit");
            if (this.RequireDigit && item.All<char>((Func<char, bool>)(c => !this.IsDigit(c))))
                stringList.Add("PasswordRequireDigit");
            if (this.RequireLowercase && item.All<char>((Func<char, bool>)(c => !this.IsLower(c))))
                stringList.Add("PasswordRequireLower");
            if (this.RequireUppercase && item.All<char>((Func<char, bool>)(c => !this.IsUpper(c))))
                stringList.Add("PasswordRequireUpper");
            if (stringList.Count == 0)
                return Task.FromResult<PasswordResult>(PasswordResult.Success);
            return Task.FromResult<PasswordResult>(PasswordResult.Failed(string.Join(" ", (IEnumerable<string>)stringList)));
        }

        public virtual bool IsDigit(char c)
        {
            if (c >= '0')
                return c <= '9';
            return false;
        }

        public virtual bool IsLower(char c)
        {
            if (c >= 'a')
                return c <= 'z';
            return false;
        }

        public virtual bool IsUpper(char c)
        {
            if (c >= 'A')
                return c <= 'Z';
            return false;
        }

        public virtual bool IsLetterOrDigit(char c)
        {
            if (!this.IsUpper(c) && !this.IsLower(c))
                return this.IsDigit(c);
            return true;
        }
    }
}
