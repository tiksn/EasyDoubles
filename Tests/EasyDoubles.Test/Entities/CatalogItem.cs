namespace EasyDoubles.Test.Entities;

using System.ComponentModel.DataAnnotations;
using TIKSN.Data;

public class CatalogItem : IEntity<int>
{
    public int AvailableStock { get; set; }

    public CatalogBrand? CatalogBrand { get; set; }

    public int CatalogBrandId { get; set; }

    public CatalogType? CatalogType { get; set; }

    public int CatalogTypeId { get; set; }

    public string? Description { get; set; }

    public int ID { get; set; }

    public int MaxStockThreshold { get; set; }

    [Required]
    public string? Name { get; set; }

    public bool OnReorder { get; set; }

    public string? PictureFileName { get; set; }

    public decimal Price { get; set; }

    public int RestockThreshold { get; set; }
}
