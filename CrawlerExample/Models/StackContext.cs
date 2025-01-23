using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrawlerExample.Models
{
    public class StackContext:DbContext
    {
        public StackContext(DbContextOptions<StackContext> options) : base(options)
        {
        }

        public DbSet<Question> Question { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Tự tạo Primary Key cho column id Khi thực hiện SaveChanges, nếu không khai báo như dưới thì lúc SaveChanges sẽ báo lỗi HasNoKey
            base.OnModelCreating(builder);
            var keysProperties = builder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
            foreach (var property in keysProperties)
            {
                property.ValueGenerated = ValueGenerated.OnAdd;
            }
            builder.Entity<Question>().ToTable("Question");
        }


    }
}
