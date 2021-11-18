using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using Article_Project.Entities.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article_Project.DataLayer.Config
{
    class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(p => p.Text).IsRequired();
            builder.Property(p => p.Time).IsRequired();
        }
    }
}
