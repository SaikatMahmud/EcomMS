﻿using EcomMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Interfaces
{
    public interface ICustomer : IRepo<Customer>
    {
        //any extra functionality here if needed
        Customer Create(Customer obj);
    }
}
