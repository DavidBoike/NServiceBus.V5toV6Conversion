﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Messages.Events
{
    public class OrderShipped
    {
        public string OrderId { get; set; }
    }
}
