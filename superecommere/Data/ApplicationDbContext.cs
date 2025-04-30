
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using superecommere.Data.Config;
using superecommere.Models.Categories;
using superecommere.Models.Domain;
using superecommere.Models.Products;
using superecommere.Models.Store;
using superecommere.Models.UploadFile;
using System.Reflection;
using System.Reflection.Metadata;

namespace superecommere.Data
{
    public class ApplicationDbContext : IdentityDbContext<TblUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public DbSet<TblUser> Users { get; set; }
        public DbSet<TblStore> Stores { get; set; }
        public DbSet<TblProducts> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<StoreCategories> StoreCategories { get; set; }
        public DbSet<SubStoreCategory> SubStoreCategories { get; set; }
        public DbSet<StoreCategoryContainer> StoreCategoryContainer { get; set; }
        public DbSet<UploadImgProducts> UploadImgProducts { get; set; }
        public DbSet<ProductTranslation> ProductTranslation { get; set; }
        public DbSet<CreateStoreRequest> CreateStoreRequest { get; set; }
        public DbSet<CustomDomainRequest> CustomDomainRequest { get; set; }









#pragma warning restore CS0114 // Member hides inherited member; missing override keyword

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<TblStore>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Id).ValueGeneratedOnAdd();
            //    //entity.HasOne(e => e.User)
            //    //  .WithMany(u => u.Stores);// Assuming a user can have multiple stores
            //      //.HasForeignKey(e => e.User.Id); //Ensure the foreign key property exists


            //});


            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
            modelBuilder.Entity<TblStore>()
                .HasIndex(s => s.Subdomain)
                .IsUnique();

            // Create the unique index on CustomDomain
            modelBuilder.Entity<TblStore>()
                .HasIndex(s => s.CustomDomain)
                .IsUnique()
                .HasFilter("[CustomDomain] IS NOT NULL");

            //modelBuilder.Entity<TblUser>()
            //   .HasMany(e => e.Stores)
            //   .WithOne(e => e.User)
            //   .HasForeignKey(e => e.UserId)
            //    .IsRequired(false);

            //modelBuilder.Entity<TblStore>()
            //    .HasOne<TblUser>()
            //    .WithMany(e => e.Stores)
            //    .HasForeignKey(e => e.UserId)
            //    .IsRequired();

            //modelBuilder.Entity<TblStore>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Id).ValueGeneratedOnAdd();
            //    entity.HasOne(e => e.User)
            //          .WithMany(u => u.Stores)
            //          .HasForeignKey(e => e.UserId);
            //});

            // Other configurations...




            //modelBuilder.Entity<TblUser>(entity =>
            //{
            //    modelBuilder.Entity<TblUser>()
            //    .HasMany(e => e.Stores)
            //    .WithOne(e => e.User)
            //    .HasForeignKey(e => e.UserId);
            //    //.IsRequired();
            //    //entity.HasKey(e => e.Id);
            //});


        }


    }
}
