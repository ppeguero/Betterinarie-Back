using AutoMapper;
using Betterinarie_Back.Application.Dtos.Implementation;
using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Entities.Implementation.Enum;

namespace Betterinarie_Back.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.ConsultasIds, opt => opt.MapFrom(src => src.Consultas.Select(c => c.Id).ToList()));
            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.Consultas, opt => opt.Ignore());

            CreateMap<Usuario, UsuarioEditDto>();
            CreateMap<UsuarioEditDto, Usuario>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Consultas, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

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
                .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estatus))
                .ForMember(dest => dest.VeterinarioNombre, opt => opt.MapFrom(src => src.Veterinario != null ? src.Veterinario.Nombre : null))
                .ForMember(dest => dest.MedicamentosIds, opt => opt.MapFrom(src => src.Medicamentos.Select(m => m.Id).ToList()));

            CreateMap<ConsultaDto, Consulta>()
                .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => (EstatusConsulta)src.Estatus))
                .ForMember(dest => dest.Hora, opt => opt.MapFrom(src => src.Hora));

            CreateMap<ConsultaPostDto, Consulta>()
                .ForMember(dest => dest.Medicamentos, opt => opt.Ignore());

            CreateMap<ConsultaUpdateDto, Consulta>()
                .ForMember(dest => dest.Medicamentos, opt => opt.Ignore());

            CreateMap<Medicamento, MedicamentoDto>()
                .ForMember(dest => dest.ConsultasIds, opt => opt.MapFrom(src => src.Consultas.Select(c => c.Id).ToList()));

            CreateMap<MedicamentoDto, Medicamento>()
                .ForMember(dest => dest.Consultas, opt => opt.Ignore());

            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.Mascotas, opt => opt.MapFrom(src => src.Mascotas));

            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.Mascotas, opt => opt.Ignore());
        }
    }
}