﻿using BookStoreModels.Models;
using BookStoreModels.ViewModels;
using BookStoreRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookStoreApi.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        BookRepository _bookRepository = new BookRepository();

        [HttpGet]
        [Route("list")]
        public IActionResult GetBooks(string ?keyword, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var books = _bookRepository.GetBooks(pageIndex, pageSize, keyword);
                if (books == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Please provide correct information");

                ListResponse<BookModel> listResponse = new ListResponse<BookModel>()
                {
                    Records = books.Records.Select(x => new BookModel(x)).ToList(),
                    TotalRecords = books.TotalRecords
                };
                return StatusCode(HttpStatusCode.OK.GetHashCode(), listResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetBook(int id)
        {
            try
            {
                var response = _bookRepository.GetBook(id);
                if (response == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Please provide correct information");
                var book = new BookModel(response);
                return StatusCode(HttpStatusCode.OK.GetHashCode(), book);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("add")]
        [HttpPost]
        public IActionResult AddBook(BookModel bookModel)
        {
            try
            {
                Book book = new Book()
                {
                    Id = bookModel.Id,
                    Name = bookModel.Name,
                    Price = bookModel.Price,
                    Description = bookModel.Description,
                    Base64image = bookModel.Base64image,
                    Categoryid = bookModel.CategoryId,
                    Publisherid = bookModel.PublisherId,
                    Quantity = bookModel.Quantity,
                };
                var addedBook = _bookRepository.AddBook(book);
                BookModel bookModel1 = new BookModel(addedBook);
                if (addedBook == null)
                    return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Bad Request");
                //return Ok(user);
                return StatusCode(HttpStatusCode.OK.GetHashCode(), bookModel1);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("update")]
        [HttpPut]
        public IActionResult UpdateBook(BookModel bookModel)
        {
            try
            {
                if (bookModel != null)
                {
                    Book book = new Book()
                    {
                        Id = bookModel.Id,
                        Name = bookModel.Name,
                        Price = bookModel.Price,
                        Description = bookModel.Description,
                        Base64image = bookModel.Base64image,
                        Categoryid = bookModel.CategoryId,
                        Publisherid = bookModel.PublisherId,
                        Quantity = bookModel.Quantity,
                    };
                    var response = _bookRepository.UpdateBook(book);

                    if (response != null)
                        return StatusCode(HttpStatusCode.OK.GetHashCode(), new BookModel(response));
                }
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Please provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult DeleteBook(int id)
        {
            if (id == 0)
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "id is null");
            try
            {
                bool response = _bookRepository.DeleteBook(id);
                if (response == true)
                    return StatusCode(HttpStatusCode.OK.GetHashCode(), "Book Deleted Successfully");
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Please provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
    }
}
