using System;

namespace FrameworkHelpers.Interfaces
{
    public interface ICreateEditHistory
    {
        Guid CreatedById { get; set; }
        DateTime CreateDate { get; set; }
        Guid EditedById { get; set; }
        DateTime EditDate { get; set; }
    }
}
