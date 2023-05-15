using Application.Intefaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository:IRepository<User>
    {
        public string ComputeHash(string input);
    }
}
