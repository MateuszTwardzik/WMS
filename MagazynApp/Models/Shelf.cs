﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public int AlleyId { get; set; }
        public Alley Alley { get; set; }
        public string Name { get; set; }

        public double Capacity
        {
            get
            {
                return Sockets.Sum(s => s.Capacity);
            }
            set
            {
                Capacity = Sockets.Sum(s => s.Capacity);
            }
        }

        public double MaxCapacity { 
            get
            {
                return Sockets.Sum(s => s.MaxCapacity);
            }
            set
            {
                Capacity = Sockets.Sum(s => s.MaxCapacity);
            }
        }
        public IList<Socket> Sockets { get; set; }
    }
}