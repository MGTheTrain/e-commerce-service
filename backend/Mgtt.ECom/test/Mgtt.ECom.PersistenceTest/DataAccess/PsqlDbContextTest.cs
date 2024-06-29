// <copyright file="PsqlDbContextTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.PersistenceTest.DataAccess
{
    using System;
    using System.Linq;
    using Mgtt.ECom.Domain.UserManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class PsqlDbContextTests
    {
        private DbContextOptions<PsqlDbContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<PsqlDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Fact]
        public void Can_Create_Read_Update_Delete_User()
        {
            // Arrange
            var options = this.GetInMemoryDbContextOptions();

            using (var context = new PsqlDbContext(options))
            {
                var user = new User
                {
                    UserName = "JohnDoe",
                    PasswordHash = "hashedpassword",
                    Email = "john.doe@example.com",
                    Role = "Admin",
                };

                // Act
                // Create
                context.Users.Add(user);
                context.SaveChanges();

                // Read
                var retrievedUser = context.Users.FirstOrDefault(u => u.UserName == "JohnDoe");
                Assert.NotNull(retrievedUser);
                Assert.Equal("JohnDoe", retrievedUser.UserName);
                Assert.Equal("hashedpassword", retrievedUser.PasswordHash);
                Assert.Equal("john.doe@example.com", retrievedUser.Email);
                Assert.Equal("Admin", retrievedUser.Role);

                // Update
                retrievedUser.Email = "john.doe@newemail.com";
                context.SaveChanges();

                var updatedUser = context.Users.FirstOrDefault(u => u.UserName == "JohnDoe");
                Assert.NotNull(updatedUser);
                Assert.Equal("john.doe@newemail.com", updatedUser.Email);

                // Delete
                context.Users.Remove(updatedUser);
                context.SaveChanges();

                var deletedUser = context.Users.FirstOrDefault(u => u.UserName == "JohnDoe");
                Assert.Null(deletedUser);
            }
        }
    }
}
