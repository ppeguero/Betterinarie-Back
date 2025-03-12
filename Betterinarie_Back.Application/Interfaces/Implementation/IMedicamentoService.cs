using Betterinarie_Back.Application.Dtos.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Application.Interfaces.Implementation
{
    public interface IMedicamentoService
    {
        Task<MedicamentoDto> GetMedicamentoById(int id);
        Task<IEnumerable<MedicamentoDto>> GetAllMedicamentos();
        Task<MedicamentoDto> CreateMedicamento(MedicamentoDto createDto);
        Task UpdateMedicamento(MedicamentoDto updateDto);
        Task DeleteMedicamento(int id);
    }
}
