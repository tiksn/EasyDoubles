namespace EasyDoubles.Test.Entities;

using System.ComponentModel.DataAnnotations;
using TIKSN.Data;

public class CatalogType : IEntity<int>
{
    public int ID { get; set; }

    [Required]
    public string? Type { get; set; }
}
