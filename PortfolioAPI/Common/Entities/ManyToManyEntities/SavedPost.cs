using System;

namespace Common.Entities.ManyToManyEntities;

public class SavedPost
{
    public int UserId { get; set; }
    public int PostId { get; set; }

    public virtual User User { get; set; }
    public virtual Post Post { get; set; }

}
