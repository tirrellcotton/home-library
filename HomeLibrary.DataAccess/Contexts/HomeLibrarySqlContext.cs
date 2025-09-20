using HomeLibrary.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.DataAccess.Contexts;

public class HomeLibrarySqlContext(DbContextOptions<HomeLibrarySqlContext> options) : DbContext(options)
{
    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookStatus> BookStatuses { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Author");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasOne(d => d.Author).WithMany(p => p.Books).HasConstraintName("FK_Books_Authors");

            entity.HasOne(d => d.BookStatus).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_BookStatus");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Genres");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books).HasConstraintName("FK_Books_Publishers");
        });
    }
}
