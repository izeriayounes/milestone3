using AutoMapper;
using milestone3.Models;
using milestone3.DTO;

namespace milestone3.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<CartItemDTO, CartItem>().ReverseMap();
            CreateMap<CustomerDTO, Customer>().ReverseMap();
        }
    }
}
