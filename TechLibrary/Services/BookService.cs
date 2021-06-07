using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookid);
        Task<IEnumerable<Book>> GetPagenatedBooksAsync(BookParameters bookParameters);
        Task<int> AddNewBook(Book book);
        Task<int> UpdateBook(Book book);
        Task<IEnumerable<Book>> SearchBook(string searchString);
        Task<IEnumerable<Book>> SearchBookISBN(string searchString);
        Task<IEnumerable<Book>> GetPagenatedSearchBook(BookParameters bookParameters);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public async Task<IEnumerable<Book>> GetPagenatedBooksAsync(BookParameters bookParameters)
        {
            return await _dataContext.Set<Book>().OrderBy(b => b.BookId)
                .Skip((bookParameters.PageNumber - 1) * bookParameters.PageSize)
                .Take(bookParameters.PageSize)
                .ToListAsync();
        }

        public async Task<int> AddNewBook(Book book)
        {
            _dataContext.Books.Add(book);
            return await _dataContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(book), new { id = book.BookId }, book);
        }

        public async Task<int> UpdateBook(Book book)
        {
            var dbBook = await _dataContext.Books.SingleOrDefaultAsync(b => b.BookId == book.BookId);
            if (dbBook.BookId > 0)
            {
                _dataContext.Entry(dbBook).CurrentValues.SetValues(book);                
                _dataContext.SaveChanges();
                return dbBook.BookId;                
            }
            else
            {
                return 0;
            }
        }

        public async Task<IEnumerable<Book>> SearchBook(string searchString)
        {           
            return await _dataContext.Books.Where(b => b.Title.Contains(searchString)).ToListAsync();          
        }

         public async Task<IEnumerable<Book>> SearchBookISBN(string searchString)
        {           
            return await _dataContext.Books.Where(b => b.ISBN.Equals(searchString)).ToListAsync();          
        }

        public async Task<IEnumerable<Book>> GetPagenatedSearchBook(BookParameters bookParameters)
        {
            return await _dataContext.Set<Book>()
                .Where(b => b.Title.Contains(bookParameters.SearchString))
                .OrderBy(b => b.BookId)
                .Skip((bookParameters.PageNumber - 1) * bookParameters.PageSize)
                .Take(bookParameters.PageSize)
                .ToListAsync();
        }
    }

    public class BookParameters
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public string SearchString { get; set; }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
