using System;
using FrameworkHelpers.WebUserApi.Types;

namespace FrameworkHelpers.WebUserApi.Interfaces
{
    public interface ILoginDetails
    {
        int Id { get; }
        int? FailedLogins { get; set; }
        DateTime? LocoukDate { get; set; }
        LoginResult LoginResult { get; set; }
    }
}
