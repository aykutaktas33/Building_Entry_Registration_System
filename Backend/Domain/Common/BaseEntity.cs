using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public bool IsDelete { get; set; }
    }
}
