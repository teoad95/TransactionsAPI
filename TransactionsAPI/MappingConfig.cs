using AutoMapper;
using TransactionsAPI.Models;
using TransactionsAPI.Models.DTO;

namespace TransactionsAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Transaction, TransactionUpdateDTO>().ReverseMap();
        }
    }
}