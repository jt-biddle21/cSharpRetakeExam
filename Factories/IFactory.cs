using cSharpRetakeExam.Models;
using System.Collections.Generic;
namespace cSharpRetakeExam.Factory
{
    public interface IFactory<T> where T : BaseEntity
    {
    }
}