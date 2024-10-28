using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models;

public partial class BookStoreContext : DbContext
{
    public BookStoreContext()
    {
    }

    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorBook> AuthorBooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<BookCollection> BookCollections { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostCategory> PostCategories { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Slider> Sliders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }
    public virtual DbSet<Ads> Ads { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.UseCollation("utf8mb4_vietnamese_ci");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__author__3213E83F2377359E");

            entity.ToTable("author");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AuthorBook>(entity =>
        {
            entity.HasKey(ab =>
            new { ab.AuthorId, ab.BookId })
            .HasName("PK_AuthorBook");
            entity.ToTable("author_book");

            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");

            entity.HasOne(d => d.Author).WithMany()
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKg7j6ud9d32ll232o9mgo90s57");

            entity.HasOne(d => d.Book).WithMany()
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKn8665s8lv781v4eojs8jo3jao");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__book__3213E83FEB58FE4D");

            entity.ToTable("book");

            entity.HasIndex(e => e.Isbn, "UK_ehpdfjpu1jm3hijhj4mm0hx9h").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Cover)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("cover");
            entity.Property(e => e.Description)
                .IsUnicode(true)
                .HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Isbn)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("isbn");
            entity.Property(e => e.Page).HasColumnName("page");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PublicationDate).HasColumnName("publication_date");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Size)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("size");
            entity.Property(e => e.Sold).HasColumnName("sold");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("title");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK5jgwecmfn1vyn9jtld3o64v4x");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FKgtvt7p649s4x80y6f4842pnfq");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__book_cat__3213E83F882BB774");

            entity.ToTable("book_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("name");
        });

        modelBuilder.Entity<BookCollection>(entity =>
        {
            entity.HasKey(e => new { e.CollectionId, e.BookId })
          .HasName("PK_BookCollection");
            entity.ToTable("book_collection");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CollectionId).HasColumnName("collection_id");

            entity.HasOne(d => d.Book).WithMany()
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKhrhume0ucplaek9m8pb6ild7s");

            entity.HasOne(d => d.Collection).WithMany()
                .HasForeignKey(d => d.CollectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKka9jqqmcu25by7m32gihxb4rr");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__collecti__3213E83F3B0A3343");

            entity.ToTable("collection");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDisplay).HasColumnName("is_display");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feedback__3213E83F9E4B9185");

            entity.ToTable("feedback");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Comment)
                .IsUnicode(true)
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(6)
                .HasColumnName("created_at");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FKgclyi456gw0lcd6xcfj2l7r6s");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK6xn6ah744nvjpnblisld6i3o1");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__image__3213E83F81F83E75");

            entity.ToTable("image");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("description");
            entity.Property(e => e.Link)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("link");

            entity.HasOne(d => d.Book).WithMany(p => p.Images)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK56boxkje8rys2n78amvgkk913");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order__3213E83F68E13795");

            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("address");
            entity.Property(e => e.Created)
                .HasPrecision(6)
                .HasColumnName("created");
            entity.Property(e => e.CustomerNote)
                .IsUnicode(true)
                .HasColumnName("customer_note");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("full_name");
            entity.Property(e => e.PaymentState)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("payment_state");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.ShippingPrice).HasColumnName("shipping_price");
            entity.Property(e => e.ShippingState)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("shipping_state");
            entity.Property(e => e.ShopNote)
                .IsUnicode(true)
                .HasColumnName("shop_note");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FKrcaf946w0bh6qj0ljiw3pwpnu");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_de__3213E83F6D552A2D");

            entity.ToTable("order_detail");

            entity.HasIndex(e => new { e.BookId, e.OrderId }, "UKl5hghjlnpjnocpkyqlln3a6ca").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OriginalPrice).HasColumnName("original_price");
            entity.Property(e => e.SalePrice).HasColumnName("sale_price");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK3aceepmpjwpo8pdn7gmjdfckq");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FKlb8mofup9mi791hraxt9wlj5u");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__post__3213E83FCF3FECD1");

            entity.ToTable("post");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brief)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("brief");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Content)
                .IsUnicode(true)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(6)
                .HasColumnName("created_at");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("thumbnail");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FKi4fb9yu9ap3j0g42j0qja9b4a");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK51aeb5le008k8klgnyfaalmn");
        });

        modelBuilder.Entity<PostCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__post_cat__3213E83F1A4E5776");

            entity.ToTable("post_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__publishe__3213E83F22F7E1EA");

            entity.ToTable("publisher");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("name");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("website");
        });

        modelBuilder.Entity<Slider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__slider__3213E83FACC147B7");

            entity.ToTable("slider");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BackLink)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("back_link");
            entity.Property(e => e.Description)
                .IsUnicode(true)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83FEE17F8A6");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "UK_ob8kqyqqgmefl0aco34akdtpe").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(true)
                .HasColumnName("address");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.Created)
                 .IsRequired();
            entity.Property(e => e.Gender)
                .HasMaxLength(10)  // Thêm giới hạn độ dài cho trường Gender
                .IsUnicode(true)  // Chỉ lưu ký tự ASCII
                .HasColumnName("gender");  // Thêm trường gender
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__wishlist__3213E83FC6B72597");

            entity.ToTable("wishlist");

            entity.HasIndex(e => new { e.BookId, e.UserId }, "UK23j0w0ls8ramaftxntclngekj").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK94k0l1f4gpde7nw2scncp8pp4");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK6e4b6ubvjarad3f5g8wqhec");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refreshtoken");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Token)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(e => e.CreatedDate)
                  .IsRequired();

            entity.Property(e => e.ExpirationDate)
                  .IsRequired();

            entity.Property(e => e.Revoked)
                  .HasDefaultValue(false);

            entity.Property(e => e.Used)
                  .HasDefaultValue(false);

            entity.HasOne(d => d.User)
                  .WithMany(p => p.RefreshTokens)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Ads>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ads__3213E83F2377359E");

            entity.ToTable("ads");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(true)
                .HasColumnName("title");
            entity.Property(e => e.Description)
                .HasMaxLength(500)  // Giả sử bạn muốn giới hạn chiều dài mô tả
                .IsUnicode(true)
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(255) // Giả sử đây là đường dẫn hình ảnh
                .IsUnicode(true)
                .HasColumnName("image");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
