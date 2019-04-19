using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Contracts.Type;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;

namespace FirstAzureFunc
{
    public static class BookFuctions
    {
        static List<MyBook> list = new List<MyBook>();

        [FunctionName("CreateBook")]
        public static async Task<IActionResult> CreateBook(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "book")] HttpRequest req,
            [Table("todos", Connection ="AzureWebJobsStorage")] IAsyncCollector<BookTableEntity> bookTable,
            ILogger log)
        {
            log.LogInformation("create book");



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<BookCreteModel>(requestBody);
            
                var todo = new Book()
                {
                    Id = data.Id,
                    Desription = data.Desription

                };
            
            
            await bookTable.AddAsync(todo.ToTableEntity());
    //        var myBook = new MyBook()
    //        {
                
    //            Name = data.Name,
    //            Description= data.Description
    //};
    //        list.Add(myBook);
           // await bookTable.AddAsync(myBook);
            return new OkObjectResult(todo);

        }
        [FunctionName("GetBooks")]
        public static async Task<IActionResult> GetBooks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books")] HttpRequest req,
            [Table("todos",Connection ="AzureWebJobsStorage")] CloudTable table,
            ILogger log)
        {
            log.LogInformation("get all books");

            var query = new TableQuery<BookTableEntity>();
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);
            return new OkObjectResult(segment.Select(Mapping.ToBook));

        }

    }
}
