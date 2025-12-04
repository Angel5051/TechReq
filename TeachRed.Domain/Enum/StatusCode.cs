using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachRed.Domain.Enum
{

        public enum StatusCode
        {
            Ok = 200,
            Created = 201,
            BadRequest = 400,
            NotFound = 404, // Добавлено, чтобы устранить ошибку
            InternalServerError = 500,
        ValidationError = 501
    }
    }
