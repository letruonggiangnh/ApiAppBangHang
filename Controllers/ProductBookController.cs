using ApiAppBangHang.Models;
using ApiAppBangHang.Models.ViewModel;
using ApiAppBanSach.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiAppBangHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBookController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        public ProductBookController(AppBanSachDbContext context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        [Route("trending")]
        public IActionResult Get()
        {
            try
            {
                List<BookViewModel> bookViewModels = new List<BookViewModel>();
                var books = unitOfWork.ProductBookRepository.Get(filter: p => p.TagId == 2);
                
                foreach (var book in books)
                {
                    BookViewModel bookViewModel = new BookViewModel();
                    bookViewModel.BookId = book.BookId;
                    bookViewModel.BookName = book.BookName;
                    bookViewModel.BookPrice = book.BookPrice;
                    bookViewModel.Discount = book.Discount;
                    bookViewModel.ImageUrl = book.ImageUrl;
                    bookViewModel.BookDiscountedPrice = book.BookPrice - book.BookPrice * book.Discount / 100;
                    bookViewModels.Add(bookViewModel);
                }

                return Ok(bookViewModels);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("bookList")]
        public IActionResult GetBookList([FromQuery] int bookParentId)
        {
            try
            {
                List<BookViewModel> bookViewModels = new List<BookViewModel>();
                var categoryChildrens = unitOfWork.BookCategoryChildRepository.Get(filter: p => p.CategoryParentId == bookParentId);

                foreach (var categoryChildren in categoryChildrens)
                {
                    var books = unitOfWork.ProductBookRepository.Get(filter: p => p.CategoryChildId == categoryChildren.CategoryChildId); 
                    foreach (var book in books)
                    {
                        BookViewModel bookViewModel = new BookViewModel();
                        bookViewModel.BookId = book.BookId;
                        bookViewModel.BookName = book.BookName;
                        bookViewModel.BookPrice = book.BookPrice;
                        bookViewModel.Discount = book.Discount;
                        bookViewModel.ImageUrl = book.ImageUrl;
                        bookViewModel.BookDiscountedPrice = book.BookPrice - book.BookPrice * book.Discount / 100;
                        bookViewModels.Add(bookViewModel);
                    }
                }

                return Ok(bookViewModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("bookdescription")]
        public IActionResult GetBookDescriptionById([FromQuery] int id)
        {
            try
            {
                List<BookDescription> bookDescriptions = unitOfWork.BookDescriptionRepository.Get(filter: p => p.BookId == id).ToList();
                return Ok(bookDescriptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("book")]
        public IActionResult GetBookById([FromQuery] int id)
        {
            try
            {
                ProductBook book = unitOfWork.ProductBookRepository.GetByID(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
