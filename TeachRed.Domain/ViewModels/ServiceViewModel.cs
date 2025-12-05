using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TeachRed.Domain.ViewModels
{
    public class ServiceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Добавляем новые поля
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsExpress { get; set; }
    }
}