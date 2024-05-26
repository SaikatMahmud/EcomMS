using ClosedXML.Excel;
using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.BLL.Services
{
    public class DataUploadService
    {
        public static List<Product> ParseProductData(Stream stream)
        {
            try
            {
                var products = new List<Product>();
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1);
                    foreach (var row in rows)
                    {
                        var product = new Product
                        {
                            Name = row.Cell(1).GetValue<string>(),
                            Price = row.Cell(2).GetValue<int>(),
                            Quantity = row.Cell(3).GetValue<int>(),
                            ReorderQuantity = row.Cell(4).GetValue<int>(),
                            Description = row.Cell(5).GetValue<string>(),
                            Specification = row.Cell(6).GetValue<string>(),
                            CategoryId = row.Cell(7).GetValue<int?>(),
                        };
                        products.Add(product);
                    }
                }
                return products;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<Category> ParseCategoryData(Stream stream)
        {
            try
            {
                var categories = new List<Category>();
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1);
                    foreach (var row in rows)
                    {
                        var catagory = new Category
                        {
                            Name = row.Cell(1).GetValue<string>(),
                            Description = row.Cell(2).GetValue<string>(),

                        };
                        categories.Add(catagory);
                    }
                }
                return categories;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
