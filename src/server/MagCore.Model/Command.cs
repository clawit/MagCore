using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public class Command
    {
        public Action Action { get; set; }

        public string Sender { get; set; }

        public Position Target { get; set; }
    }
}
