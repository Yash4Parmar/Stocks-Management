using AutoMapper;
using Stocks_Management.Models;
using Stocks_Management.ViewModels;

namespace Stocks_Management.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Stock, VMCreateStock>().ReverseMap();
            CreateMap<Stock, VMGetStock>().ReverseMap();
        }
    }
}
