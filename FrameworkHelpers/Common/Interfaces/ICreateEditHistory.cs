using System;

namespace FrameworkHelpers.Common.Interfaces
{
    public interface ICreateEditHistory
    {
        Guid CreatedById { get; set; }
        DateTime CreateDate { get; set; }
        Guid EditedById { get; set; }
        DateTime EditDate { get; set; }
    }
}
