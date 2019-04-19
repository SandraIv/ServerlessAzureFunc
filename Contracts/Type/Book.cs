using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Type
{
   public class Book
    {
        public string Id { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Desription { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class BookCreteModel
    {
        public string Desription { get; set; }
        public string Id { get; set; }
    }
    public class BookTableEntity: TableEntity
    {
        public DateTime CreatedTime { get; set; }
        public string Desription { get; set; }
        public bool IsCompleted { get; set; }
    }
    public static class Mapping
    {
        public static BookTableEntity ToTableEntity(this Book book)
        {
            return new BookTableEntity()
            {
                CreatedTime = book.CreatedTime,
                Desription = book.Desription,
                IsCompleted = book.IsCompleted,
                RowKey =book.Id,
                PartitionKey = "TODO"
            };
        }
        public static Book ToBook(this BookTableEntity entity)
        {
            return new Book()
            {
                Id = entity.RowKey,
                CreatedTime = entity.CreatedTime,
                Desription = entity.Desription,
                IsCompleted = entity.IsCompleted

            };
        }
    }
}
