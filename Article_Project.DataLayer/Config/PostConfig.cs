using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Article_Project.Entities.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article_Project.DataLayer.Config
{
    class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.Texts).IsRequired();
            builder.Property(p => p.Orders).IsRequired();
            builder.Property(p => p.ImageNames).IsRequired();
            builder.Property(p => p.Subject).IsRequired();
            builder.Property(p => p.TimeCreate).IsRequired();
        }
    }
}
