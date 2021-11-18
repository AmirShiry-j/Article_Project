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
    class FollowConfig : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.Property(p => p.FollowTo).IsRequired();

        }
    }
}
