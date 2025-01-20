using Lab5Borowy.Data;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Linq;

namespace Lab5Borowy.Models;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new Lab5BorowyContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<Lab5BorowyContext>>() ))
        {
            // Look for any movies.
            if (context.Book.Any())
            {
                return;// DB has been already seeded
            }
            context.Book.AddRange(
                new Book
                {
                    Title = "Becoming",
                    Author = "Michelle Obama",
                    Description = "A deeply personal memoir by the former First Lady of the United States.",
                    Publishers = ["Crown"],
                    PublishDate = DateTime.Parse("2017-11-13"),
                    NumberOfPages = 448,
                    Weight = 1,
                    Price = 89,
                    Rating = "R"
                },
                new Book
                {
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    Description = "A fantasy adventure of Bilbo Baggins and his journey to reclaim a treasure.",
                    Publishers = ["George Allen & Unwin"],
                    PublishDate = DateTime.Parse("1937-09-21"),
                    NumberOfPages = 310,
                    Weight = 4,
                    Price = 59,
                    Rating = "A"
                },
                new Book
                {
                    Title = "Sapiens: A Brief History of Humankind",
                    Author = "Yuval Noah Harari",
                    Description = "An exploration of the history and impact of Homo sapiens.",
                    Publishers = ["Harper"],
                    PublishDate = DateTime.Parse("2014-09-04"),
                    NumberOfPages = 443,
                    Weight = 8,
                    Price = 79,
                    Rating = "C"
                }
            );
            context.SaveChanges();
        }
    }
}
