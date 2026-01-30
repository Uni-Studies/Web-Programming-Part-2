using System;

namespace Common.Entities.ManyToManyEntities;

public class SavedPost : BaseEntity
{
    public int UserId { get; set; }
    public int PostId { get; set; }

    public User User { get; set; }
    public Post Post { get; set; }

}
