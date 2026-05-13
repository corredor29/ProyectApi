using System;

namespace Domain.Common;

public abstract class BaseEntity<TId>
{
    public TId Id {get; protected set;} = default!;
    public DateTime CreateAt {get; protected set;} = DateTime.Now;

    protected BaseEntity() {}
}
