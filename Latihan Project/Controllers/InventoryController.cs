using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using Latihan_Project.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Latihan_Project.Models;
using Microsoft.VisualBasic;

namespace Latihan_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryContext dbc;

        public InventoryController(InventoryContext context)
        {
            dbc = context;
        }

        // mengambil data catagory

        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategory()
        {
            var category = await dbc.Categories.Select(u => new
            {
                u.CategoryId,
                u.CategoryName,
                u.Description

            }).ToListAsync();

            return Ok(category);
        }

        // mengambil data category berupa id
        [HttpGet("Category/{id}")]
        public async Task<IActionResult> GetIdCategory([FromRoute] int id)
        {
            var category = await dbc.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category Not Found"
                });
            }

            return Ok(new
            {
                message = "Category Found",
                data = category
            });
        }

        // Create Category Endpoint

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CategoryRequest result)
        {
            var category = new Category
            {
                CategoryName = result.CategoryName,
                Description = result.Description
            };

            dbc.Categories.Add(category);

            await dbc.SaveChangesAsync();

            return Ok(new {
                message = "Create Succes",
                data = category
            });
        }

        // endpoint category update 
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCategory (int id, CategoryRequest result)
        {
            var category = await dbc.Categories.FindAsync(id);

            if(category == null)
            {
                return NotFound("Category Id");
            }

            category.CategoryName = result.CategoryName;
            category.Description = result.Description;

            await dbc.SaveChangesAsync();

            return Ok("Succes Update");
        }

        // EndPoint delete
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory (int id)
        {
            var category = await dbc.Categories.FindAsync(id);

            if(category == null)
            {
                return NotFound("Category Not Found");
            }

            dbc.Categories.Remove(category);

            await dbc.SaveChangesAsync();

            return Ok("Succes Delete category");
        }

        // Dto CategoryResquest 
        public class CategoryRequest
        {
            [Required] public string CategoryName { get; set; }
            [Required] public string Description { get; set; }
        }

        // mengambil semua product

        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProduck()
        {
            var product = await dbc.Products.Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.Price,
                p.Stock,
                p.CategoryId
            }).ToListAsync();

            return Ok(product);
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetIdProduct([FromRoute]int id)
        {
            var product = await dbc.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Id Product Not Found"
                });
            }
            return Ok(new 
            {
                message = "Get Id Succes",
                data = product
            });
        }

        // endpoint Create Product
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct (ProductRequest result)
        {
            var category = await dbc.Categories.FirstOrDefaultAsync(c => c.CategoryId == result.CategoryId);

            if(category == null)
            {
                return NotFound("Category Not Found");
            }

            var product = new Product 
            {
                ProductName = result.ProductName,
                Price = result.Price,
                Stock = result.Stock,
                CategoryId = result.CategoryId,
            };

            dbc.Products.Add(product);

            await dbc.SaveChangesAsync();

            return Ok(new 
            {
                message = "Succes Create Product",
                data = new
                {
                    product.ProductId,
                    product.ProductName,
                    product.Price,
                    product.Stock,
                     
                    CategoryName = category.CategoryName

                }
            });

        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductRequest result)
        {
            var product = await dbc.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if(product == null)
            {
                return NotFound("Id Product Not Found");
            }

            var category = await dbc.Categories.FirstOrDefaultAsync(c => c.CategoryId == result.CategoryId);

            if(category == null)
            {
                return NotFound("Id category Not Found");
            }

            product.ProductName = result.ProductName;
            product.Price = result.Price;
            product.Stock = result.Stock;
            product.CategoryId = result.CategoryId;

            await dbc.SaveChangesAsync();

            return Ok(new
            {
                message = "Update Succes",
                data = new
                {
                    product.ProductId,
                    product.ProductName,
                    product.Price,
                    product.Stock,
                    CategoryName = category.CategoryName
                }
            });
        }

        // EndPoint Delete Product
        [HttpDelete("Deleteproduct/{id}")]
        public async Task<IActionResult> DeleteProduct (int id)
        {
            var product = await dbc.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Id product NotFoudn");
            }

            dbc.Products.Remove(product);

            await dbc.SaveChangesAsync();

            return Ok("Delete Succes");
        }

        // Dto product requst
        public class ProductRequest
        {
            [Required] public string ProductName { get; set; }
            [Required] public decimal Price { get; set; }
            [Required] public int Stock { get; set; }
            [Required] public int CategoryId { get; set; }
        }
    }
}
