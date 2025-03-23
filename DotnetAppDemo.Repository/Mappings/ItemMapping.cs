using DotnetAppDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;
using System.Text;

namespace DotnetAppDemo.Repository.Mappings
{
    public class ItemMapping : BaseMapping<Item>
    {
        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasIndex(x => new { x.Name }).IsUnique();
            base.Configure(builder);
        }
    }
}
