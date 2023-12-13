using BosonHiggsApi.DL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BosonHiggsApi.BL.Enums;
using BosonHiggsApi.BL.Helpers;

namespace BosonHiggsApi.DL
{
    public class Seed
    {
        public static async Task SeedDatabaseAsync(IServiceProvider appServiceProvider)
        {
            await using var scope = appServiceProvider.CreateAsyncScope();
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<Seed>>();
            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Migration error");
            }

            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                //await SeedLevels(context); //TODO: re-comment after changing connectionString
                //await SeedAdmin(context);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "DB seed error");
            }
        }

        public static async Task SeedLevels(ApplicationDbContext context)
        {
            if (!context.Levels.Any())
            {
                var levels = new List<Level>
                {
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "1-test-level-name",
                        Description = "1-test-level-description",
                        Hint = "1-test-level-hint",
                        Link = "1-test-level-link",
                        Type = LevelType.First,
                        Token = RandomString.AlphaNumeric(64),
                        CreatedDateTime = DateTime.Now,
                        NextLevel = new Level()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "2-test-level-name",
                            Description = "2-test-level-description",
                            Hint = "2-test-level-hint",
                            Link = "2-test-level-link",
                            Type = LevelType.Second,
                            Token = RandomString.AlphaNumeric(64),
                            CreatedDateTime = DateTime.Now,
                            NextLevel = new Level()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "3-test-level-name",
                                Description = "3-test-level-description",
                                Hint = "3-test-level-hint",
                                Link = "3-test-level-link",
                                Type = LevelType.Third,
                                Token = RandomString.AlphaNumeric(64),
                                CreatedDateTime = DateTime.Now,
                                NextLevel = new Level()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Name = "4-test-level-name",
                                    Description = "4-test-level-description",
                                    Hint = "4-test-level-hint",
                                    Link = "4-test-level-link",
                                    Type = LevelType.Fourth,
                                    Token = RandomString.AlphaNumeric(64),
                                    CreatedDateTime = DateTime.Now,
                                    NextLevel = new Level()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Name = "5-test-level-name",
                                        Description = "5-test-level-description",
                                        Hint = "5-test-level-hint",
                                        Link = "5-test-level-link",
                                        Type = LevelType.Fifth,
                                        Token = RandomString.AlphaNumeric(64),
                                        CreatedDateTime = DateTime.Now,
                                        NextLevel = new Level()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Name = "6-test-level-name",
                                            Description = "6-test-level-description",
                                            Hint = "6-test-level-hint",
                                            Link = "6-test-level-link",
                                            Type = LevelType.Sixth,
                                            Token = RandomString.AlphaNumeric(64),
                                            CreatedDateTime = DateTime.Now,
                                            NextLevel = new Level()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Name = "7-test-level-name",
                                                Description = "7-test-level-description",
                                                Hint = "7-test-level-hint",
                                                Link = "7-test-level-link",
                                                Type = LevelType.Seventh,
                                                Token = RandomString.AlphaNumeric(64),
                                                CreatedDateTime = DateTime.Now,
                                                NextLevel = new Level()
                                                {
                                                    Id = Guid.NewGuid().ToString(),
                                                    Name = "finish",
                                                    Description = "finish",
                                                    Hint = "finish",
                                                    Link = "finish",
                                                    Type = LevelType.Finish,
                                                    Token = RandomString.AlphaNumeric(64),
                                                    CreatedDateTime = DateTime.Now
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                await context.Levels.AddRangeAsync(levels);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedAdmin(ApplicationDbContext context)
        {
            if (!context.Users.Any(x => x.NickName == "Admin"))
            {
                await context.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Token = AppConstants.AdminToken,
                    Email = "abakirova.elizat@gmail.com",
                    NickName = "Admin",
                    CreatedDateTime = DateTime.Now,
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
