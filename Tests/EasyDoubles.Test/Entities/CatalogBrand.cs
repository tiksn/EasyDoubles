namespace EasyDoubles.Test.Entities;

using System.ComponentModel.DataAnnotations;
using TIKSN.Data;

public class CatalogBrand : IEntity<int>
{
    [Required]
    public string? Brand { get; set; }

    public int ID { get; set; }
}
