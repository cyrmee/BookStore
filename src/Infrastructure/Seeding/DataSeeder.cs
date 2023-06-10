using Domain.Repositories;
using Domain.Models;

namespace BookStore.Infrastructure.Seeding;

public static class DataSeeder
{
    public static async Task SeedBooks(IRepository _repository)
    {
        var books = new List<Book>()
        {
            new Book
            {
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                Publisher = "Grand Central Publishing",
                PublicationDate = new DateTime(1960, 7, 11).ToUniversalTime(),
                Isbn10 = "0061120081",
                Isbn13 = "9780061120084",
                Description = "A gripping portrayal of racial injustice and the loss of innocence in the Deep South.",
                Quantity = 5,
                Price = 9.99
            },
            new Book
            {
                Title = "1984",
                Author = "George Orwell",
                Publisher = "Signet Classics",
                PublicationDate = new DateTime(1949, 6, 8).ToUniversalTime(),
                Isbn10 = "0451524934",
                Isbn13 = "9780451524935",
                Description = "A dystopian novel depicting a totalitarian society and its impact on individuals' freedom.",
                Quantity = 8,
                Price = 12.99
            },
            new Book
            {
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                Publisher = "Scribner",
                PublicationDate = new DateTime(1925, 4, 10).ToUniversalTime(),
                Isbn10 = "0743273567",
                Isbn13 = "9780743273565",
                Description = "A story of wealth, love, and tragedy set in the backdrop of the Jazz Age.",
                Quantity = 4,
                Price = 11.99
            },
            new Book
            {
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                Publisher = "Penguin Classics",
                PublicationDate = new DateTime(1813, 1, 28).ToUniversalTime(),
                Isbn10 = "0141439513",
                Isbn13 = "9780141439518",
                Description = "A classic romance novel exploring themes of love, class, and societal expectations.",
                Quantity = 6,
                Price = 10.99
            },
            new Book
            {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                Publisher = "Mariner Books",
                PublicationDate = new DateTime(1937, 9, 21).ToUniversalTime(),
                Isbn10 = "0618260307",
                Isbn13 = "9780618260300",
                Description = "An adventure novel set in Middle-earth, preceding the events of The Lord of the Rings.",
                Quantity = 10,
                Price = 13.99
            },
            new Book
            {
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                Publisher = "Grand Central Publishing",
                PublicationDate = new DateTime(1960, 7, 11).ToUniversalTime(),
                Isbn10 = "0061120081",
                Isbn13 = "9780061120084",
                Description = "A gripping portrayal of racial injustice and the loss of innocence in the Deep South.",
                Quantity = 5,
                Price = 9.99
            },
            new Book
            {
                Title = "Nineteen Eighty-Four",
                Author = "George Orwell",
                Publisher = "Penguin Classics",
                PublicationDate = new DateTime(1949, 6, 8).ToUniversalTime(),
                Isbn10 = "0141393049",
                Isbn13 = "9780141393049",
                Description = "A dystopian novel depicting a totalitarian society and its impact on individuals' freedom.",
                Quantity = 8,
                Price = 12.99
            },
            new Book
            {
                Title = "The Catcher in the Rye",
                Author = "J.D. Salinger",
                Publisher = "Little, Brown and Company",
                PublicationDate = new DateTime(1951, 7, 16).ToUniversalTime(),
                Isbn10 = "0316769487",
                Isbn13 = "9780316769488",
                Description = "A coming-of-age novel exploring teenage rebellion, alienation, and identity.",
                Quantity = 7,
                Price = 11.99
            },
            new Book
            {
                Title = "The Alchemist",
                Author = "Paulo Coelho",
                Publisher = "HarperOne",
                PublicationDate = new DateTime(1988, 4, 25).ToUniversalTime(),
                Isbn10 = "0062315005",
                Isbn13 = "9780062315007",
                Description = "A philosophical novel about following one's dreams and finding one's true purpose.",
                Quantity = 9,
                Price = 10.99
            },
            new Book
            {
                Title = "Harry Potter and the Philosopher's Stone",
                Author = "J.K. Rowling",
                Publisher = "Bloomsbury Publishing",
                PublicationDate = new DateTime(1997, 6, 26).ToUniversalTime(),
                Isbn10 = "0747532699",
                Isbn13 = "9780747532699",
                Description = "The first book in the popular Harry Potter series, introducing the magical world of Hogwarts.",
                Quantity = 12,
                Price = 14.99
            }

        };

        foreach (var book in books)
        {
            if ((await _repository.Book!.GetAllAsync()).Count == 0)
            {
                _repository.Book.Add(book);
            }
        }

        await _repository.SaveAsync();
    }

    public static async Task SeedCategories(IRepository _repository)
    {
        var categories = new List<Category>()
        {
            new Category { Name = "Fiction" },
            new Category { Name = "Science Fiction" },
            new Category { Name = "Fantasy" },
            new Category { Name = "Mystery" },
            new Category { Name = "Thriller" },
            new Category { Name = "Romance" },
            new Category { Name = "Biography" },
            new Category { Name = "History" },
            new Category { Name = "Self-help" },
            new Category { Name = "Cooking" },
            new Category { Name = "Science" },
            new Category { Name = "Technology" },
            new Category { Name = "Business" },
            new Category { Name = "Finance" },
            new Category { Name = "Health" },
            new Category { Name = "Fitness" },
            new Category { Name = "Travel" },
            new Category { Name = "Art" },
            new Category { Name = "Music" },
            new Category { Name = "Sports" },
            new Category { Name = "Religion" },
            new Category { Name = "Philosophy" },
            new Category { Name = "Psychology" },
            new Category { Name = "Education" },
            new Category { Name = "Children" },
            new Category { Name = "Young Adult" },
            new Category { Name = "Reference" },
            new Category { Name = "Comics" },
            new Category { Name = "Graphic Novels" },
            new Category { Name = "Poetry" },
            new Category { Name = "Drama" },
            new Category { Name = "Classics" },
            new Category { Name = "Crime" },
            new Category { Name = "Horror" },
            new Category { Name = "Humor" },
            new Category { Name = "Parenting" },
            new Category { Name = "Cookbooks" },
            new Category { Name = "Self-improvement" },
            new Category { Name = "Motivational" },
            new Category { Name = "Biographies" },
            new Category { Name = "Autobiographies" },
            new Category { Name = "Historical Fiction" },
            new Category { Name = "Sci-Fi/Fantasy" },
            new Category { Name = "Memoirs" },
            new Category { Name = "True Crime" },
            new Category { Name = "Suspense" },
            new Category { Name = "Political" },
            new Category { Name = "Environmental" },
            new Category { Name = "Nature" },
            new Category { Name = "Parenting" },
            new Category { Name = "Relationships" },
            new Category { Name = "Crafts" },
            new Category { Name = "Hobbies" },
            new Category { Name = "Gardening" },
            new Category { Name = "Design" },
            new Category { Name = "Fashion" },
            new Category { Name = "Architecture" },
            new Category { Name = "Photography" }
        };

        foreach (var category in categories)
        {
            if ((await _repository.Category!.GetAllAsync()).Count == 0)
            {
                _repository.Category.Add(category);
            }
        }

        await _repository.SaveAsync();
    }
}