using AutoMapper;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Core.Entities.Implementation;

namespace Betterinarie_Back.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo de Usuario a UsuarioDto
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.ConsultasIds, opt => opt.MapFrom(src => src.Consultas.Select(c => c.Id).ToList()));

            // Mapeo de UsuarioDto a Usuario
            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.Consultas, opt => opt.Ignore());

            CreateMap<Rol, RolDto>()
             .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name));

            CreateMap<Mascota, MascotaDto>()
                 .ForMember(dest => dest.UrlImagen, opt => opt.MapFrom(src => src.URLImagen))
                 .ForMember(dest => dest.ConsultasIds, opt => opt.MapFrom(src => src.Consultas.Select(c => c.Id).ToList()))
                 .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro)); 

            CreateMap<MascotaDto, Mascota>()
                .ForMember(dest => dest.URLImagen, opt => opt.Ignore())
                .ForMember(dest => dest.PublicIdImagen, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore()); 

            CreateMap<Consulta, ConsultaDto>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estatus))
                .ForMember(dest => dest.VeterinarioNombre, opt => opt.MapFrom(src => src.Veterinario != null ? src.Veterinario.Nombre : null ))
                .ForMember(dest => dest.MedicamentosIds, opt => opt.MapFrom(src => src.Medicamentos.Select(m => m.Id).ToList()));

            CreateMap<ConsultaDto, Consulta>()
                .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estado));

            CreateMap<Medicamento, MedicamentoDto>()
                .ForMember(dest => dest.ConsultasIds, opt => opt.MapFrom(src => src.Consultas.Select(c => c.Id).ToList()));

            // Mapeo de MedicamentoDto a Medicamento
            CreateMap<MedicamentoDto, Medicamento>()
                .ForMember(dest => dest.Consultas, opt => opt.Ignore());

            // Mapeo de Cliente a ClienteDto
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.Mascotas, opt => opt.MapFrom(src => src.Mascotas));
            // Mapeo de ClienteDto a Cliente
            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.Mascotas, opt => opt.Ignore());
        }
    }
}