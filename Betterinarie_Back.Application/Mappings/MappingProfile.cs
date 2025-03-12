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

            // Mapeo de Mascota a MascotaDto
            CreateMap<Mascota, MascotaDto>()
                .ForMember(dest => dest.ConsultasIds, opt => opt.MapFrom(src => src.Consultas.Select(c => c.Id).ToList()));

            // Mapeo de MascotaDto a Mascota
            CreateMap<MascotaDto, Mascota>()
                .ForMember(dest => dest.Consultas, opt => opt.Ignore());

            // Mapeo de Consulta a ConsultaDto
            CreateMap<Consulta, ConsultaDto>()
                .ForMember(dest => dest.MedicamentosIds, opt => opt.MapFrom(src => src.Medicamentos.Select(m => m.Id).ToList()));

            // Mapeo de ConsultaDto a Consulta
            CreateMap<ConsultaDto, Consulta>()
                .ForMember(dest => dest.Medicamentos, opt => opt.Ignore());

            // Mapeo de Medicamento a MedicamentoDto
            CreateMap<Medicamento, MedicamentoDto>()
                .ForMember(dest => dest.ConsultasIds, opt => opt.MapFrom(src => src.Consultas.Select(c => c.Id).ToList()));

            // Mapeo de MedicamentoDto a Medicamento
            CreateMap<MedicamentoDto, Medicamento>()
                .ForMember(dest => dest.Consultas, opt => opt.Ignore());

            // Mapeo de Cliente a ClienteDto
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.MascotasIds, opt => opt.MapFrom(src => src.Mascotas.Select(m => m.Id).ToList()));

            // Mapeo de ClienteDto a Cliente
            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.Mascotas, opt => opt.Ignore());
        }
    }
}