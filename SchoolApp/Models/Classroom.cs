﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultConnection.Models
{
    public class Classroom
    {
        public int ClassroomID { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Comments { get; set; }
    }
}