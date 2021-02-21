﻿using GBD.Data.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace GBD.Data.Dto
{
    public partial class ReviewDto
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Body { get; set; }
        public int? Stars { get; set; }
        public int ProductId { get; set; }
        public DateTime InsertedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
