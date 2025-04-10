using API.DAL.Entities;
using AutoMapper;
using SharedBlocks.Dtos.Books;

namespace API.SERVICE.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDTO>();
        CreateMap<CreateBookDTO, Book>();
        CreateMap<UpdateBookDTO, Book>();

        CreateMap<Review, ReviewDTO>();
    }
}