using FlixedStarzProject.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlixedStarzProject.Core
{
    public interface IMovieRepository
    {
        Task<List<Data>> GetMovieData();
    }
}
