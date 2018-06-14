using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GASPG.Models.ViewModel.GameViewModels;
using GASPG.Models.ViewModel.GenreViewModels;
using GASPG.Models.ViewModel.DeveloperViewModels;

namespace GASPG.Models.ViewModel.Mappings
{
    public class ModelViewMapping : Profile
    {
        public ModelViewMapping()
        {
            CreateMap<Developer, DeveloperViewModel>();
            CreateMap<Developer, CreateDeveloperViewModel>();
            CreateMap<Developer, DeleteDeveloperViewModel>();
            CreateMap<Developer, DetailsDeveloperViewModel>();
            CreateMap<Developer, EditDeveloperViewModel>();

            CreateMap<Genre, GenreViewModel>();
            CreateMap<Genre, CreateGenreViewModel>();
            CreateMap<Genre, DeleteGenreViewModel>();
            CreateMap<Genre, DetailsGenreViewModel>();
            CreateMap<Genre, EditGenreViewModel>();

            CreateMap<Game, GameViewModel>();
            CreateMap<Game, CreateGameViewModel>()
                .ForMember(dest => dest.GenreId,
                    opts => opts.MapFrom(src => src.Genre.GenreId))
                .ForMember(dest => dest.DeveloperId,
                    opts => opts.MapFrom(src => src.Developer.DeveloperId));
            CreateMap<Game, DeleteGameViewModel>();
            CreateMap<Game, DetailsGameViewModel>();
            CreateMap<Game, EditGameViewModel>()
                .ForMember(dest => dest.GenreId,
                    opts => opts.MapFrom(src => src.Genre.GenreId))
                .ForMember(dest => dest.DeveloperId,
                    opts => opts.MapFrom(src => src.Developer.DeveloperId));
        }
    }
}
