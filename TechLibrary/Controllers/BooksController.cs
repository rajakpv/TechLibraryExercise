using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;
using System;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    _logger.LogInformation("Get all books test");

        //    var books = await _bookService.GetBooksAsync();

        //    var bookResponse = _mapper.Map<List<BookResponse>>(books);

        //    return Ok(bookResponse);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Get book by id {id}");

                var book = await _bookService.GetBookByIdAsync(id);

                var bookResponse = _mapper.Map<BookResponse>(book);

                return Ok(bookResponse);
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error occured while Getting book : " + ex.Message);
                return NotFound();
            }
            
        }

        
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] BookParameters bookParameters)
        {
            try
            {
                _logger.LogInformation($"Get book with Parameters {bookParameters}");
                var books = await _bookService.GetPagenatedBooksAsync(bookParameters);
                var bookResponse = _mapper.Map<List<BookResponse>>(books);

                _logger.LogInformation($"Returned {bookResponse.Count} books from database.");

                return Ok(bookResponse);
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error occured while Getting books : " + ex.Message);
                return NotFound();
            }           
        }

        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> AddNewBook([FromBody] Book book)
        {
            try
            {
                _logger.LogInformation($"Add book with details {book}");
                var bookid = await _bookService.AddNewBook(book);

                if (bookid > 0)
                {
                    return Ok(bookid);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error occured while addding a book : "+ ex.Message);
                return BadRequest(ex);
            }            
        }

        [HttpPost]
        [Route("UpdateBook")]
        public async Task<IActionResult> UpdateBook([FromBody] Book book)
        {
            try
            {
                _logger.LogInformation($"update book with details {book}");
                var bookid = await _bookService.UpdateBook(book);
                if (bookid > 0)
                {
                    return Ok(bookid);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error occured while updating book with bookId : " + book.BookId + " " + ex.Message);
                return BadRequest(ex);
            }
        }

        //[HttpGet("SearchBook/{sarchString}")]
        //[Route("SearchBook/{sarchString}")]
        //public async Task<IActionResult> SearchBook(string sarchString)
        //{
        //    try
        //    {
        //        _logger.LogInformation($"Search book with the search string:  {sarchString}");
        //        var books = await _bookService.SearchBook(sarchString);                
        //        var bookResponse = _mapper.Map<List<BookResponse>>(books);

        //        _logger.LogInformation($"Returned {bookResponse.Count} books from database.");
                
        //        if (bookResponse.Count > 0)
        //        {
        //            return Ok(bookResponse);
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation($"Error occured while getting books with search string : " + sarchString + " " + ex.Message);
        //        return BadRequest(ex);
        //    }
        //}

        [HttpGet("SearchBookISBN/{sarchString}")]
        [Route("SearchBookISBN/{sarchString}")]
        public async Task<IActionResult> SearchBookISBN(string sarchString)
        {
            try
            {
                _logger.LogInformation($"Search book with the search string:  {sarchString}");
                var books = await _bookService.SearchBookISBN(sarchString);
                var bookResponse = _mapper.Map<List<BookResponse>>(books);

                _logger.LogInformation($"Returned {bookResponse.Count} books from database.");

                if (bookResponse.Count > 0)
                {
                    return Ok(bookResponse);
                }
                else
                {
                    return Ok(bookResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error occured while getting books with search string : " + sarchString + " " + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("SearchBook/")]
        public async Task<IActionResult> SearchBook([FromQuery] BookParameters bookParameters)
        {
            try
            {
                _logger.LogInformation($"Get book with Parameters {bookParameters}");
                var books = await _bookService.GetPagenatedSearchBook(bookParameters);
                var bookResponse = _mapper.Map<List<BookResponse>>(books);

                _logger.LogInformation($"Returned {bookResponse.Count} books from database.");

                return Ok(bookResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error occured while Getting books : " + ex.Message);
                return NotFound();
            }
        }

    }
}
