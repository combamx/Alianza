﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Alianza.Models
{
    public partial class ExclusiveData
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Authorize { get; set; }
        public string EmailNew { get; set; }
        public int? IdRequest { get; set; }

        public virtual User EmailNewNavigation { get; set; }
    }
}