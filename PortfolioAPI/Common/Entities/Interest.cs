using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Common.Entities.ManyToManyEntities;

namespace Common.Entities;

public class Interest : BaseEntity
{
    public string Name { get; set; }
    
    [JsonIgnore]
    public virtual List<User> Users { get; set; }
    
}
