using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkflowInstanceValue
    {
        public int Id { get; set; }
        public int WorkflowInstanceId { get; set; }
        public int ItemAttributeId { get; set; }
        public string ItemAttributeValueString { get; set; } = string.Empty;
        public double ItemAttributeValueNumber { get; set; }
    }
}