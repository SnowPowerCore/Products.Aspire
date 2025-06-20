using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Backend.Core.Base;

public abstract record BaseEntity()
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id", Order = 1)]
    public long Id { get; init; }
}