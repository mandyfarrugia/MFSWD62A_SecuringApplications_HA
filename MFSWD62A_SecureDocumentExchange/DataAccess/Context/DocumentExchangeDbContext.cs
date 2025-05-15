using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    /// <summary>
    /// This class is the gateway to the database. Therefore, all connections and management of the database are done here.
    /// The instanation of an object of this class establishes a connection to the database.
    /// </summary>
    /// <param name="options">An instance of DbContextOptions stores information pertaining to configuration.
    /// For this assignment, it will set up the database provider targeting Microsoft SQL Server and the connection string pointing to the database.
    /// This instance can be used to configure Entity Framework through an ASP.NET Core application, unit testing with in-memory databases, as well applying Dependency Injection via said instance to inject the instance of DbContext, in this case DocumentExchangeDbContext.</param>
    public class DocumentExchangeDbContext(DbContextOptions<DocumentExchangeDbContext> options) : IdentityDbContext<UploadingUser>(options)
    {
        /// <summary>
        /// This property is used to access the UploadingUser table in the database to perform CRUD operations.
        /// </summary>
        public DbSet<UploadingUser> UploadingUsers { get; set; }

        /// <summary>
        /// OnModelCreating is used to handle the mapping model classes in C# .NET to database tables, in this case in SQL Server.
        /// It will also configure the relationships between the tables (such as one-to-many, as well as tables (name of tables, column names and types, as well constraints).
        /// Case in point, this method can be used to set unique instead, as well generating a NEWID() for a UNIQUEIDENTIFIER column (tantamount to C# .NET's Guid type).
        /// </summary>
        /// <param name="builder">This method uses the parameter of type ModelBuilder to configure entity types (tables), properties (columns), relationships, keys and indexes, constraints, as well as seeding of initial data.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); //Helps ensure that tables related to authentication (Identity) are created properly.
        }
    }
}